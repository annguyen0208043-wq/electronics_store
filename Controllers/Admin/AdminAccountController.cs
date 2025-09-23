using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;
using TechShop.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace TechShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminAccountController : Controller
    {
        private readonly AppDbContext _context;

        public AdminAccountController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts
                .Include(a => a.Khachhangs)
                .Select(a => new AccountViewModel
                {
                    MaTk = a.MaTk,
                    UsernameTk = a.UsernameTk,
                    Gmail = a.Gmail,
                    RoleTk = a.RoleTk,
                    FullName = a.Khachhangs.FirstOrDefault() != null ? a.Khachhangs.FirstOrDefault().FulltenKh : null,
                    PhoneNumber = a.Khachhangs.FirstOrDefault() != null ? a.Khachhangs.FirstOrDefault().SdtKh : null,
                    Address = a.Khachhangs.FirstOrDefault() != null ? a.Khachhangs.FirstOrDefault().DiachiKh : null
                })
                .ToListAsync();

            return View("~/Views/Admin/Account/Index.cshtml", accounts);
        }

        public IActionResult Create()
        {
            return View("~/Views/Admin/Account/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra username đã tồn tại chưa
                    if (await _context.Accounts.AnyAsync(a => a.UsernameTk == model.UsernameTk))
                    {
                        ModelState.AddModelError("UsernameTk", "Tên đăng nhập đã tồn tại");
                        return View("~/Views/Admin/Account/Create.cshtml", model);
                    }

                    // Kiểm tra email đã tồn tại chưa
                    if (await _context.Accounts.AnyAsync(a => a.Gmail == model.Gmail))
                    {
                        ModelState.AddModelError("Gmail", "Email đã tồn tại");
                        return View("~/Views/Admin/Account/Create.cshtml", model);
                    }

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // Tạo tài khoản mới
                            var account = new Account
                            {
                                UsernameTk = model.UsernameTk,
                                PasswordTk = HashPassword(model.PasswordTk),
                                Gmail = model.Gmail,
                                RoleTk = 2
                            };

                            _context.Accounts.Add(account);
                            await _context.SaveChangesAsync();

                            // Tạo thông tin khách hàng
                            var khachhang = new Khachhang
                            {
                                MaKh = GenerateCustomerId(),
                                FulltenKh = model.FullName,
                                SdtKh = model.PhoneNumber,
                                DiachiKh = model.Address,
                                MaTk = account.MaTk
                            };

                            _context.Khachhangs.Add(khachhang);
                            await _context.SaveChangesAsync();

                            await transaction.CommitAsync();
                            TempData["SuccessMessage"] = "Tạo tài khoản thành công!";
                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException;
                    var errorMessage = "Có lỗi xảy ra khi tạo tài khoản: ";
                    
                    if (innerException != null)
                    {
                        errorMessage += innerException.Message;
                        Console.WriteLine($"Inner Exception: {innerException.Message}");
                        Console.WriteLine($"Stack Trace: {innerException.StackTrace}");
                    }
                    else
                    {
                        errorMessage += ex.Message;
                        Console.WriteLine($"Exception: {ex.Message}");
                        Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    }

                    ModelState.AddModelError("", errorMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    ModelState.AddModelError("", "Có lỗi không mong muốn xảy ra: " + ex.Message);
                }
            }

            return View("~/Views/Admin/Account/Create.cshtml", model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Khachhangs)
                .FirstOrDefaultAsync(a => a.MaTk == id);

            if (account == null)
            {
                return NotFound();
            }

            var model = new AccountViewModel
            {
                MaTk = account.MaTk,
                UsernameTk = account.UsernameTk,
                Gmail = account.Gmail,
                RoleTk = account.RoleTk,
                FullName = account.Khachhangs.FirstOrDefault() != null ? account.Khachhangs.FirstOrDefault().FulltenKh : null,
                PhoneNumber = account.Khachhangs.FirstOrDefault() != null ? account.Khachhangs.FirstOrDefault().SdtKh : null,
                Address = account.Khachhangs.FirstOrDefault() != null ? account.Khachhangs.FirstOrDefault().DiachiKh : null
            };

            return View("~/Views/Admin/Account/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AccountViewModel model)
        {
            if (id != model.MaTk)
            {
                return NotFound();
            }

            // Bỏ qua validation cho PasswordTk
            ModelState.Remove("PasswordTk");

            if (ModelState.IsValid)
            {
                try
                {
                    var account = await _context.Accounts
                        .Include(a => a.Khachhangs)
                        .FirstOrDefaultAsync(a => a.MaTk == id);

                    if (account == null)
                    {
                        return NotFound();
                    }

                    // Kiểm tra username đã tồn tại chưa (trừ tài khoản hiện tại)
                    if (await _context.Accounts.AnyAsync(a => a.UsernameTk == model.UsernameTk && a.MaTk != id))
                    {
                        ModelState.AddModelError("UsernameTk", "Tên đăng nhập đã tồn tại");
                        return View("~/Views/Admin/Account/Edit.cshtml", model);
                    }

                    // Kiểm tra email đã tồn tại chưa (trừ tài khoản hiện tại)
                    if (await _context.Accounts.AnyAsync(a => a.Gmail == model.Gmail && a.MaTk != id))
                    {
                        ModelState.AddModelError("Gmail", "Email đã tồn tại");
                        return View("~/Views/Admin/Account/Edit.cshtml", model);
                    }

                    // Cập nhật thông tin tài khoản
                    account.UsernameTk = model.UsernameTk;
                    account.Gmail = model.Gmail;
                    account.RoleTk = 2; // Luôn là User

                    // Cập nhật thông tin khách hàng
                    var khachhang = account.Khachhangs.FirstOrDefault();
                    if (khachhang != null)
                    {
                        khachhang.FulltenKh = model.FullName;
                        khachhang.SdtKh = model.PhoneNumber;
                        khachhang.DiachiKh = model.Address;
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(model.MaTk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật: " + ex.Message);
                }
            }

            return View("~/Views/Admin/Account/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Khachhangs)
                .FirstOrDefaultAsync(a => a.MaTk == id);

            if (account == null)
            {
                return NotFound();
            }

            // Xóa thông tin khách hàng
            foreach (var khachhang in account.Khachhangs)
            {
                _context.Khachhangs.Remove(khachhang);
            }

            // Xóa tài khoản
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.MaTk == id);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private string GenerateCustomerId()
        {
 
            var lastAccount = _context.Accounts
                .OrderByDescending(a => a.MaTk)
                .FirstOrDefault();

            int nextNumber = 1;
            if (lastAccount != null)
            {
                nextNumber = lastAccount.MaTk + 1;
            }
            return "KH" + nextNumber.ToString("D4");
        }
    }
}