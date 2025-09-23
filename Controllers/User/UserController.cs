using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;
using TechShop.ViewModels;
using System.Security.Claims;

namespace TechShop.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Sản phẩm mới: lấy theo ngày tạo mới nhất
                var newProducts = await _context.Products
                    .OrderByDescending(p => p.NgayTao) // Giả sử có trường NgayTao
                    .Take(8)
                    .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Ten = p.Ten,
                        Gia = p.Gia,
                        HinhAnh = p.HinhAnh,
                    }).ToListAsync();

                // Sản phẩm thịnh hành: lấy theo số lượt bán nhiều nhất
                var popularProducts = await _context.Products
                    .OrderByDescending(p => p.SoLuongBan) // Giả sử có trường SoLuongBan
                    .Take(8)
                    .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Ten = p.Ten,
                        Gia = p.Gia,
                        HinhAnh = p.HinhAnh,
                    }).ToListAsync();

                var model = new HomeViewModel
                {
                    NewProducts = newProducts,
                    PopularProducts = popularProducts
                };

                return View("~/Views/User/Home/Index.cshtml", model);
            }
            catch (Exception)
            {
                return View(new HomeViewModel());
            }
        }

        public async Task<IActionResult> Product()
        {
            var products = await _context.Products
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Ten = p.Ten,
                    Gia = p.Gia,
                    Loai = p.Loai,
                    HinhAnh = p.HinhAnh
                })
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Category(string category)
        {
            var products = await _context.Products
                .Where(p => p.Loai == category)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id, // Use Id instead of MaHh
                    Ten = p.Ten,
                    Gia = p.Gia,
                    Loai = p.Loai,
                    MoTa = p.MoTa,
                    HinhAnh = _context.HinhAnhSanPhams
                        .Where(img => img.MaHh == p.Id)
                        .Select(img => img.DuongDan)
                        .ToList()
                })
                .ToListAsync();

            ViewData["CategoryName"] = category;
            return View("Category/Index", products);
        }

        public async Task<IActionResult> OrderHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> Warranty()
        {
            var userEmail = User.FindFirst("Email")?.Value;
            var khachhang = await _context.Khachhangs
                .Include(k => k.Hoadons)
                .ThenInclude(h => h.ChitietHds)
                .ThenInclude(c => c.MaHhNavigation)
                .FirstOrDefaultAsync(k => k.MaTkNavigation != null && k.MaTkNavigation.Gmail == userEmail);

            if (khachhang == null)
            {
                return View(new List<Hoadon>());
            }

            return View(khachhang.Hoadons.ToList());
        }

        public async Task<IActionResult> Profile()
        {
            var userEmail = User.FindFirst("Email")?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs
                .Include(k => k.MaTkNavigation)
                .FirstOrDefaultAsync(k => k.MaTkNavigation != null && k.MaTkNavigation.Gmail == userEmail);

            if (khachhang == null)
            {
                return NotFound();
            }

            var profileViewModel = new ProfileViewModel
            {
                FullName = khachhang.FulltenKh,
                Email = khachhang.MaTkNavigation.Gmail,
                PhoneNumber = khachhang.SdtKh,
                Address = khachhang.DiachiKh
            };

            return View(profileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var userEmail = User.FindFirst("Email")?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs
                .Include(k => k.MaTkNavigation)
                .FirstOrDefaultAsync(k => k.MaTkNavigation != null && k.MaTkNavigation.Gmail == userEmail);

            if (khachhang == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin khách hàng
            khachhang.FulltenKh = model.FullName;
            khachhang.SdtKh = model.PhoneNumber;
            khachhang.DiachiKh = model.Address;

            // Cập nhật email trong bảng tài khoản
            if (khachhang.MaTkNavigation != null)
            {
                khachhang.MaTkNavigation.Gmail = model.Email;
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin.");
                return View("Profile", model);
            }
        }
    }
}