using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;
using TechShop.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TechShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = await _context.Accounts
                .Include(a => a.Khachhangs)
                .FirstOrDefaultAsync(a => a.UsernameTk == username);

            if (account != null)
            {

                if (account.RoleTk == 1)
                {
                    if (account.PasswordTk == password)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, account.UsernameTk),
                            new Claim(ClaimTypes.Role, "Admin")
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties();

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "AdminProduct");
                    }
                }
                else
                {
                    if (account.PasswordTk == HashPassword(password))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, account.UsernameTk),
                            new Claim(ClaimTypes.Role, "User")
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties();

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            return View();
        }

        // GET: Đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Accounts.AnyAsync(a => a.UsernameTk == model.UsernameTk))
                {
                    ModelState.AddModelError("UsernameTk", "Tên đăng nhập đã tồn tại");
                    return View(model);
                }

                if (await _context.Accounts.AnyAsync(a => a.Gmail == model.Gmail))
                {
                    ModelState.AddModelError("Gmail", "Email đã tồn tại");
                    return View(model);
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Tạo tài khoản mới với mật khẩu đã hash
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
                            MaKh = "KH" + account.MaTk.ToString("D4"),
                            FulltenKh = model.FullName,
                            SdtKh = model.PhoneNumber,
                            DiachiKh = model.Address,
                            MaTk = account.MaTk
                        };

                        _context.Khachhangs.Add(khachhang);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                        return RedirectToAction(nameof(Login));
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

            return View(model);
        }

        // GET: Quên mật khẩu
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Quên mật khẩu
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.UsernameTk == model.Username && a.Gmail == model.Email);

                if (account != null)
                {
                    // Chuyển hướng đến ResetPassword
                    var resetModel = new ResetPasswordViewModel
                    {
                        Username = model.Username,
                        Email = model.Email
                    };
                    return View("ResetPassword", resetModel);
                }

                ModelState.AddModelError("", "Tài khoản hoặc email không đúng");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.UsernameTk == model.Username && a.Gmail == model.Email);

                if (account != null)
                {
                    // Hash mật khẩu mới trước khi lưu
                    account.PasswordTk = HashPassword(model.NewPassword);
                    _context.Accounts.Update(account);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("", "Có lỗi xảy ra, vui lòng thử lại");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}