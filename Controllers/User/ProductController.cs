using Microsoft.AspNetCore.Mvc;
using TechShop.Models;
using TechShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TechShop.Data;

namespace TechShop.Controllers.User
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string[] categories, int? minPrice, int? maxPrice)
        {
            // Lấy tất cả sản phẩm ban đầu
            var query = _context.Hanghoas
                .Include(h => h.HinhAnhSanPhams)
                .AsQueryable();

            // Nếu có lọc theo danh mục
            if (categories != null && categories.Length > 0)
            {
                query = query.Where(h => categories.Contains(h.LoaiHh));
            }

            // Nếu có lọc theo giá
            if (minPrice.HasValue || maxPrice.HasValue)
            {
                if (minPrice.HasValue)
                {
                    query = query.Where(h => h.GiaHh >= minPrice.Value);
                }
                if (maxPrice.HasValue)
                {
                    query = query.Where(h => h.GiaHh <= maxPrice.Value);
                }
            }

            var products = await query.ToListAsync();

            // Chuyển đổi sang ProductViewModel
            var viewModel = products.Select(p => new ProductViewModel
            {
                Id = p.MaHh,
                Ten = p.TenHh,
                Gia = p.GiaHh,
                Loai = p.LoaiHh,
                HinhAnh = p.HinhAnhSanPhams.Select(h => h.DuongDan).ToList()
            }).ToList();

            // Lưu trạng thái bộ lọc vào ViewBag
            ViewBag.SelectedCategories = categories?.ToList() ?? new List<string>();
            ViewBag.MinPrice = minPrice ?? 0;
            ViewBag.MaxPrice = maxPrice ?? 50000000;

            return View("~/Views/User/Product/Index.cshtml", viewModel);
        }

        public IActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return RedirectToAction(nameof(Index));
            }

            var products = _context.Hanghoas
                .Where(p => p.TenHh.Contains(searchString) || p.LoaiHh.Contains(searchString))
                .Select(p => new ProductViewModel
                {
                    Id = p.MaHh,
                    Ten = p.TenHh,
                    Gia = p.GiaHh,
                    Loai = p.LoaiHh,
                    HinhAnh = _context.HinhAnhSanPhams
                        .Where(img => img.MaHh == p.MaHh)
                        .Select(img => img.DuongDan)
                        .ToList()
                })
                .ToList();

            ViewBag.SearchString = searchString;
            return View("~/Views/User/Product/Index.cshtml", products);
        }

        // Hiển thị sản phẩm theo loại (category)
        public async Task<IActionResult> Category(string category)
        {
            var products = await _context.Hanghoas
                .Where(h => h.LoaiHh == category)
                .Select(h => new ProductViewModel
                {
                    Id = h.MaHh,
                    Ten = h.TenHh,
                    Gia = h.GiaHh,
                    Loai = h.LoaiHh,
                    HinhAnh = _context.HinhAnhSanPhams
                        .Where(img => img.MaHh == h.MaHh)
                        .Select(img => img.DuongDan)
                        .ToList()
                })
                .ToListAsync();

            ViewBag.Category = category;
            return View("~/Views/User/Product/Index.cshtml", products);
        }

        // Trang chi tiết sản phẩm
        public async Task<IActionResult> Details(string id)
        {
            var product = await _context.Hanghoas
                .Include(h => h.HinhAnhSanPhams)
                .FirstOrDefaultAsync(h => h.MaHh == id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                Id = product.MaHh,
                Ten = product.TenHh,
                Gia = product.GiaHh,
                Loai = product.LoaiHh,
                Soluongton = product.Soluongton,
                MoTa = product.MoTa,
                HinhAnh = product.HinhAnhSanPhams
                    .Select(img => img.DuongDan)
                    .ToList() ?? new List<string> { "default.jpg" }
            };

            // Lấy thông tin chi tiết dựa vào loại sản phẩm
            switch (product.LoaiHh?.ToLower())
            {
                case "laptop":
                    var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.MaHh == id);
                    if (laptop != null)
                    {
                        ViewBag.HangLt = laptop.HangLt;
                        ViewBag.KichthuocLt = laptop.KichthuocLt;
                        ViewBag.RamLt = laptop.RamLt;
                        ViewBag.OcungLt = laptop.OcungLt;
                        ViewBag.XuatxuLt = laptop.XuatxuLt;
                        ViewBag.NgaysanxuatLt = laptop.NgaysanxuatLt;
                        ViewBag.TinhtrangLt = laptop.TinhtrangLt;
                    }
                    break;
                case "loa":
                    var loa = await _context.Loas.FirstOrDefaultAsync(l => l.MaHh == id);
                    if (loa != null)
                    {
                        ViewBag.HangLoa = loa.HangLoa;
                        ViewBag.CongsuatLoa = loa.CongsuatLoa;
                        ViewBag.KieuketnoiLoa = loa.KieuketnoiLoa;
                        ViewBag.XuatxuLoa = loa.XuatxuLoa;
                        ViewBag.NgaysanxuatLoa = loa.NgaysanxuatLoa;
                        ViewBag.TinhtrangLoa = loa.TinhtrangLoa;
                    }
                    break;
                case "màn hình":
                case "manhinh":
                    var manhinh = await _context.Manhinhs.FirstOrDefaultAsync(m => m.MaHh == id);
                    if (manhinh != null)
                    {
                        ViewBag.HangMh = manhinh.HangMh;
                        ViewBag.KichthuocMh = manhinh.KichthuocMh;
                        ViewBag.TansoMh = manhinh.TansoMh;
                        ViewBag.DoPhanGiaiMh = manhinh.DoPhanGiaiMh;
                        ViewBag.XuatxuMh = manhinh.XuatxuMh;
                        ViewBag.NgaysanxuatMh = manhinh.NgaysanxuatMh;
                        ViewBag.TinhtrangMh = manhinh.TinhtrangMh;
                    }
                    break;
                case "tai nghe":
                case "tainghe":
                    var tainghe = await _context.Tainghes.FirstOrDefaultAsync(t => t.MaHh == id);
                    if (tainghe != null)
                    {
                        ViewBag.HangTn = tainghe.HangTn;
                        ViewBag.LoaiTn = tainghe.LoaiTn;
                        ViewBag.KieuketnoiTn = tainghe.KieuketnoiTn;
                        ViewBag.PinTn = tainghe.PinTn;
                        ViewBag.TrongluongTn = tainghe.TrongluongTn;
                        ViewBag.XuatxuTn = tainghe.XuatxuTn;
                        ViewBag.NgaysanxuatTn = tainghe.NgaysanxuatTn;
                        ViewBag.TinhtrangTn = tainghe.TinhtrangTn;
                    }
                    break;
                case "bàn phím":
                case "banphim":
                    var banphim = await _context.Banphims.FirstOrDefaultAsync(b => b.MaHh == id);
                    if (banphim != null)
                    {
                        ViewBag.HangBp = banphim.HangBp;
                        ViewBag.LoaiBp = banphim.LoaiBp;
                        ViewBag.KieuketnoiBp = banphim.KieuketnoiBp;
                        ViewBag.DenledBp = banphim.DenledBp;
                        ViewBag.XuatxuBp = banphim.XuatxuBp;
                        ViewBag.NgaysanxuatBp = banphim.NgaysanxuatBp;
                        ViewBag.TinhtrangBp = banphim.TinhtrangBp;
                    }
                    break;
            }

            return View("~/Views/User/Product/Details.cshtml", viewModel);
        }
    }
}