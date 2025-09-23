using Microsoft.AspNetCore.Mvc;
using TechShop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using TechShop.Data;
using Microsoft.EntityFrameworkCore;
using TechShop.Models;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace TechShop.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ admin mới truy cập được
    public class AdminProductController : Controller
    {
        private readonly AppDbContext _context;

        public AdminProductController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index(string category = null, bool showDeleted = false)
        {
            // Lấy danh sách các loại sản phẩm cho dropdown
            ViewBag.Categories = await _context.PhanloaiSps
                .Select(p => p.TenPlsp)
                .ToListAsync();

            // Xử lý logic lọc sản phẩm
            IQueryable<Hanghoa> query = _context.Hanghoas
                .Include(h => h.HinhAnhSanPhams);

            // Lọc theo trạng thái xóa
            if (!showDeleted)
            {
                query = query.Where(h => h.NgayXoa == null);
            }
            else
            {
                query = query.Where(h => h.NgayXoa != null);
            }

            // Lọc theo loại sản phẩm
            if (!string.IsNullOrEmpty(category))
            {
                var categoryNormalized = char.ToUpper(category.Trim()[0]) + category.Trim().Substring(1).ToLower();
                query = query.Where(h => h.LoaiHh.Trim().ToLower() == categoryNormalized.ToLower()


                );


                ViewData["CategoryName"] = categoryNormalized;
                ViewData["Title"] = $"Danh sách sản phẩm - {categoryNormalized}";
            }
            else
            {
                ViewData["CategoryName"] = null;
                ViewData["Title"] = "Danh sách sản phẩm";
            }

            // Lấy danh sách sản phẩm
            var products = await query.ToListAsync();
            var viewModels = products.Select(p => new ProductViewModel
            {
                Id = p.MaHh,
                Ten = p.TenHh,
                Gia = p.GiaHh,
                Loai = p.LoaiHh,
                Soluongton = p.Soluongton,
                MoTa = p.MoTa,
                HinhAnh = p.HinhAnhSanPhams.Select(h => h.DuongDan).ToList(),
                NgayTao = p.NgayTao,
                NgayXoa = p.NgayXoa
            }).ToList();

            ViewBag.ShowDeleted = showDeleted;
            return View("~/Views/Admin/Product/Index.cshtml", viewModels);
        }
        // Chi tiết sản phẩm
        public async Task<IActionResult> Details(string id)
        {
            var hanghoa = await _context.Hanghoas
                .Include(h => h.HinhAnhSanPhams)
                .FirstOrDefaultAsync(h => h.MaHh == id);
            if (hanghoa == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                Id = hanghoa.MaHh,
                Ten = hanghoa.TenHh,
                Gia = hanghoa.GiaHh,
                Loai = hanghoa.LoaiHh,
                Soluongton = hanghoa.Soluongton,
                MoTa = hanghoa.MoTa,
                HinhAnh = hanghoa.HinhAnhSanPhams.Select(img => img.DuongDan).ToList() ?? new List<string> { "default.jpg" },
                NgayTao = hanghoa.NgayTao,
                NgayXoa = hanghoa.NgayXoa
            };

            // Lấy thông tin chi tiết dựa vào loại sản phẩm
            switch (hanghoa.LoaiHh?.ToLower())
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

            return View("~/Views/Admin/Product/Details.cshtml", viewModel);
        }

        // Tạo sản phẩm mới
        public IActionResult Create()
        {
            return View("~/Views/Admin/Product/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Ten)) { ModelState.AddModelError("Ten", "Tên sản phẩm là bắt buộc."); return View("~/Views/Admin/Product/Create.cshtml", model); }
                if (model.Gia < 0) { ModelState.AddModelError("Gia", "Giá không được âm."); return View("~/Views/Admin/Product/Create.cshtml", model); }
                if (model.Soluongton < 0) { ModelState.AddModelError("Soluongton", "Số lượng tồn không được âm."); return View("~/Views/Admin/Product/Create.cshtml", model); }
                if (string.IsNullOrEmpty(model.MaNcc)) { ModelState.AddModelError("MaNcc", "Mã nhà cung cấp là bắt buộc."); return View("~/Views/Admin/Product/Create.cshtml", model); }

                if (!await _context.Nhacungcaps.AnyAsync(n => n.MaNcc == model.MaNcc))
                {
                    ModelState.AddModelError("MaNcc", "Mã nhà cung cấp không tồn tại.");
                    return View("~/Views/Admin/Product/Create.cshtml", model);
                }

                // Tự động tạo Id mới
                string newId = GenerateNewProductId();
                while (_context.Hanghoas.Any(h => h.MaHh == newId)) // Đảm bảo không trùng Id
                {
                    newId = GenerateNewProductId(); // Nếu trùng thì tạo Id mới
                }

                // Khi tạo sản phẩm mới
                string loaiHhNormalized = string.Empty;
                switch ((model.Loai ?? "").Trim().ToLower())
                {
                    case "banphim":
                        loaiHhNormalized = "Bàn phím";
                        break;
                    case "manhinh":
                        loaiHhNormalized = "Màn hình";
                        break;
                    case "tainghe":
                        loaiHhNormalized = "Tai nghe";
                        break;
                    case "loa":
                        loaiHhNormalized = "Loa";
                        break;
                    case "laptop":
                        loaiHhNormalized = "Laptop";
                        break;
                    default:
                        loaiHhNormalized = string.IsNullOrWhiteSpace(model.Loai) ? string.Empty : char.ToUpper(model.Loai.Trim()[0]) + model.Loai.Trim().Substring(1).ToLower();
                        break;
                }
                var hanghoa = new Hanghoa
                {
                    MaHh = newId, // Gán Id tự động
                    TenHh = model.Ten,
                    GiaHh = model.Gia,
                    LoaiHh = loaiHhNormalized,
                    Soluongton = model.Soluongton,
                    MoTa = model.MoTa ?? string.Empty,
                    MaNcc = model.MaNcc,
                    NgayTao = DateTime.Now,
                    NgayXoa = null
                };

                _context.Add(hanghoa);
                await _context.SaveChangesAsync();

                // Lưu vào bảng sản phẩm chi tiết tương ứng
                switch ((model.Loai ?? "").Trim().ToLower())
                {
                    case "laptop":
                        var laptop = new Laptop
                        {
                            MaLt = hanghoa.MaHh,
                            MaHh = hanghoa.MaHh,
                            TenLt = model.Ten,
                            GiaLt = model.Gia,
                            HangLt = Request.Form["HangLt"],
                            KichthuocLt = Request.Form["KichthuocLt"],
                            RamLt = Request.Form["RamLt"],
                            OcungLt = Request.Form["OcungLt"],
                            XuatxuLt = Request.Form["XuatxuLt"],
                            NgaysanxuatLt = string.IsNullOrEmpty(Request.Form["NgaysanxuatLt"]) ? (DateOnly?)null : DateOnly.FromDateTime(DateTime.Parse(Request.Form["NgaysanxuatLt"])),
                            TinhtrangLt = Request.Form["TinhtrangLt"],
                            MaNcc = model.MaNcc, // Lưu NCC
                            MaPlsp = "PL005" // Laptop
                        };
                        _context.Laptops.Add(laptop);
                        break;
                    case "loa":
                        var loa = new Loa
                        {
                            MaLoa = hanghoa.MaHh,
                            MaHh = hanghoa.MaHh,
                            TenLoa = model.Ten,
                            GiaLoa = model.Gia,
                            HangLoa = Request.Form["HangLoa"],
                            CongsuatLoa = Request.Form["CongsuatLoa"],
                            KieuketnoiLoa = Request.Form["KieuketnoiLoa"],
                            XuatxuLoa = Request.Form["XuatxuLoa"],
                            NgaysanxuatLoa = string.IsNullOrEmpty(Request.Form["NgaysanxuatLoa"]) ? (DateOnly?)null : DateOnly.FromDateTime(DateTime.Parse(Request.Form["NgaysanxuatLoa"])),
                            TinhtrangLoa = Request.Form["TinhtrangLoa"],
                            MaNcc = model.MaNcc, // Lưu NCC
                            MaPlsp = "PL002" // Loa
                        };
                        _context.Loas.Add(loa);
                        break;
                    case "manhinh":
                        var manhinh = new Manhinh
                        {
                            MaMh = hanghoa.MaHh,
                            MaHh = hanghoa.MaHh,
                            TenMh = model.Ten,
                            GiaMh = model.Gia,
                            HangMh = Request.Form["HangMh"],
                            KichthuocMh = Request.Form["KichthuocMh"],
                            TansoMh = Request.Form["TansoMh"],
                            DoPhanGiaiMh = Request.Form["DoPhanGiaiMh"],
                            XuatxuMh = Request.Form["XuatxuMh"],
                            NgaysanxuatMh = string.IsNullOrEmpty(Request.Form["NgaysanxuatMh"]) ? (DateOnly?)null : DateOnly.FromDateTime(DateTime.Parse(Request.Form["NgaysanxuatMh"])),
                            TinhtrangMh = Request.Form["TinhtrangMh"],
                            MaNcc = model.MaNcc, // Lưu NCC
                            MaPlsp = "PL003" // Màn hình
                        };
                        _context.Manhinhs.Add(manhinh);
                        break;
                    case "tainghe":
                        var tainghe = new Tainghe
                        {
                            MaTn = hanghoa.MaHh,
                            MaHh = hanghoa.MaHh,
                            TenTn = model.Ten,
                            GiaTn = model.Gia,
                            HangTn = Request.Form["HangTn"],
                            LoaiTn = Request.Form["LoaiTn"],
                            KieuketnoiTn = Request.Form["KieuketnoiTn"],
                            PinTn = Request.Form["PinTn"],
                            TrongluongTn = Request.Form["TrongluongTn"],
                            XuatxuTn = Request.Form["XuatxuTn"],
                            NgaysanxuatTn = string.IsNullOrEmpty(Request.Form["NgaysanxuatTn"]) ? (DateOnly?)null : DateOnly.FromDateTime(DateTime.Parse(Request.Form["NgaysanxuatTn"])),
                            TinhtrangTn = Request.Form["TinhtrangTn"],
                            MaNcc = model.MaNcc, // Lưu NCC
                            MaPlsp = "PL004" // Tai nghe
                        };
                        _context.Tainghes.Add(tainghe);
                        break;
                    case "banphim":
                        var banphim = new Banphim
                        {
                            MaBp = hanghoa.MaHh,
                            MaHh = hanghoa.MaHh,
                            TenBp = model.Ten,
                            GiaBp = model.Gia,
                            HangBp = Request.Form["HangBp"],
                            LoaiBp = Request.Form["LoaiBp"],
                            KieuketnoiBp = Request.Form["KieuketnoiBp"],
                            DenledBp = Request.Form["DenledBp"] == "true",
                            XuatxuBp = Request.Form["XuatxuBp"],
                            NgaysanxuatBp = string.IsNullOrEmpty(Request.Form["NgaysanxuatBp"]) ? (DateOnly?)null : DateOnly.FromDateTime(DateTime.Parse(Request.Form["NgaysanxuatBp"])),
                            TinhtrangBp = Request.Form["TinhtrangBp"],
                            MaNcc = model.MaNcc, // Lưu NCC
                            MaPlsp = "PL001" // Bàn phím
                        };
                        _context.Banphims.Add(banphim);
                        break;
                }
                await _context.SaveChangesAsync();

                model.HinhAnh = new List<string>();
                if (images != null && images.Count > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", hanghoa.MaHh);
                    Directory.CreateDirectory(uploadsFolder);

                    int imageIndex = 1;
                    foreach (var image in images)
                    {
                        if (image.Length > 0)
                        {
                            var extension = Path.GetExtension(image.FileName).ToLower();
                            if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(extension))
                            {
                                ModelState.AddModelError("images", "Chỉ chấp nhận file ảnh định dạng .jpg, .jpeg hoặc .png.");
                                return View("~/Views/Admin/Product/Create.cshtml", model);
                            }
                            if (image.Length > 5 * 1024 * 1024)
                            {
                                ModelState.AddModelError("images", "Kích thước file không được vượt quá 5MB.");
                                return View("~/Views/Admin/Product/Create.cshtml", model);
                            }

                            var fileName = $"{hanghoa.MaHh}_{imageIndex}{extension}";
                            var filePath = Path.Combine(uploadsFolder, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }
                            var imagePath = $"{hanghoa.MaHh}/{fileName}";
                            model.HinhAnh.Add(imagePath);

                            _context.HinhAnhSanPhams.Add(new HinhAnhSanPham { MaHh = hanghoa.MaHh, DuongDan = imagePath });
                            imageIndex++;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    model.HinhAnh = new List<string> { "default.jpg" };
                }

                TempData["Success"] = "Sản phẩm đã được tạo thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/Product/Create.cshtml", model);
        }
        // GET: Hiển thị form chỉnh sửa sản phẩm
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hanghoa = await _context.Hanghoas
                .Include(h => h.HinhAnhSanPhams)
                .FirstOrDefaultAsync(h => h.MaHh == id);
            if (hanghoa == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                Id = hanghoa.MaHh,
                Ten = hanghoa.TenHh,
                Gia = hanghoa.GiaHh,
                Loai = hanghoa.LoaiHh?.ToLower(),
                Soluongton = hanghoa.Soluongton,
                MoTa = hanghoa.MoTa,
                MaNcc = hanghoa.MaNcc,
                HinhAnh = hanghoa.HinhAnhSanPhams?.Select(img => img.DuongDan).ToList() ?? new List<string> { "default.jpg" },
                NgayTao = hanghoa.NgayTao,
                NgayXoa = hanghoa.NgayXoa
            };

            // Lấy thông tin chi tiết dựa vào loại sản phẩm
            switch (hanghoa.LoaiHh?.ToLower())
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
                        ViewBag.NgaysanxuatLt = laptop.NgaysanxuatLt?.ToDateTime(TimeOnly.MinValue);
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
                        ViewBag.NgaysanxuatLoa = loa.NgaysanxuatLoa?.ToDateTime(TimeOnly.MinValue);
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
                        ViewBag.NgaysanxuatMh = manhinh.NgaysanxuatMh?.ToDateTime(TimeOnly.MinValue);
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
                        ViewBag.NgaysanxuatTn = tainghe.NgaysanxuatTn?.ToDateTime(TimeOnly.MinValue);
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
                        ViewBag.NgaysanxuatBp = banphim.NgaysanxuatBp?.ToDateTime(TimeOnly.MinValue);
                        ViewBag.TinhtrangBp = banphim.TinhtrangBp;
                    }
                    break;
            }

            return View("~/Views/Admin/Product/Edit.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProductViewModel model, List<IFormFile> images)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var hanghoa = await _context.Hanghoas
                        .Include(h => h.HinhAnhSanPhams)
                        .FirstOrDefaultAsync(h => h.MaHh == id);
                    if (hanghoa == null)
                    {
                        TempData["Error"] = "Sản phẩm không tồn tại.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Kiểm tra dữ liệu đầu vào
                    if (string.IsNullOrEmpty(model.Ten))
                    {
                        ModelState.AddModelError("Ten", "Tên sản phẩm là bắt buộc.");
                        return View("~/Views/Admin/Product/Edit.cshtml", model);
                    }
                    if (model.Gia < 0)
                    {
                        ModelState.AddModelError("Gia", "Giá không được âm.");
                        return View("~/Views/Admin/Product/Edit.cshtml", model);
                    }
                    if (model.Soluongton < 0)
                    {
                        ModelState.AddModelError("Soluongton", "Số lượng tồn không được âm.");
                        return View("~/Views/Admin/Product/Edit.cshtml", model);
                    }
                    if (string.IsNullOrEmpty(model.MaNcc))
                    {
                        ModelState.AddModelError("MaNcc", "Mã nhà cung cấp là bắt buộc.");
                        return View("~/Views/Admin/Product/Edit.cshtml", model);
                    }
                    if (!await _context.Nhacungcaps.AnyAsync(n => n.MaNcc == model.MaNcc))
                    {
                        ModelState.AddModelError("MaNcc", "Mã nhà cung cấp không tồn tại.");
                        return View("~/Views/Admin/Product/Edit.cshtml", model);
                    }
                    if (string.IsNullOrEmpty(model.Loai))
                    {
                        ModelState.AddModelError("Loai", "Loại sản phẩm là bắt buộc.");
                        return View("~/Views/Admin/Product/Edit.cshtml", model);
                    }

                    // Chuẩn hóa LoaiHh
                    string loaiHhNormalized = string.Empty;
                    switch ((model.Loai ?? "").Trim().ToLower())
                    {
                        case "banphim":
                            loaiHhNormalized = "Bàn phím";
                            break;
                        case "manhinh":
                            loaiHhNormalized = "Màn hình";
                            break;
                        case "tainghe":
                            loaiHhNormalized = "Tai nghe";
                            break;
                        case "loa":
                            loaiHhNormalized = "Loa";
                            break;
                        case "laptop":
                            loaiHhNormalized = "Laptop";
                            break;
                        default:
                            loaiHhNormalized = char.ToUpper(model.Loai.Trim()[0]) + model.Loai.Trim().Substring(1).ToLower();
                            TempData["Warning"] = $"Loại sản phẩm không chuẩn: {model.Loai}. Đã chuẩn hóa thành {loaiHhNormalized}";
                            break;
                    }

                    // Cập nhật thông tin cơ bản của sản phẩm
                    hanghoa.TenHh = model.Ten;
                    hanghoa.GiaHh = model.Gia;
                    hanghoa.LoaiHh = loaiHhNormalized;
                    hanghoa.Soluongton = model.Soluongton;
                    hanghoa.MoTa = model.MoTa ?? string.Empty;
                    hanghoa.MaNcc = model.MaNcc;

                    _context.Update(hanghoa);

                    // Cập nhật thông tin chi tiết dựa vào loại sản phẩm
                    try
                    {
                        switch (loaiHhNormalized)
                        {
                            case "Laptop":
                                var laptop = await _context.Laptops.FirstOrDefaultAsync(l => l.MaHh == id);
                                if (laptop != null)
                                {
                                    laptop.TenLt = model.Ten;
                                    laptop.GiaLt = model.Gia;
                                    laptop.HangLt = Request.Form["HangLt"];
                                    laptop.KichthuocLt = Request.Form["KichthuocLt"];
                                    laptop.RamLt = Request.Form["RamLt"];
                                    laptop.OcungLt = Request.Form["OcungLt"];
                                    laptop.XuatxuLt = Request.Form["XuatxuLt"];
                                    laptop.NgaysanxuatLt = string.IsNullOrEmpty(Request.Form["NgaysanxuatLt"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatLt"]);
                                    laptop.TinhtrangLt = Request.Form["TinhtrangLt"];
                                    laptop.MaNcc = model.MaNcc;
                                    _context.Update(laptop);
                                }
                                else
                                {
                                    laptop = new Laptop
                                    {
                                        MaLt = hanghoa.MaHh,
                                        MaHh = hanghoa.MaHh,
                                        TenLt = model.Ten,
                                        GiaLt = model.Gia,
                                        HangLt = Request.Form["HangLt"],
                                        KichthuocLt = Request.Form["KichthuocLt"],
                                        RamLt = Request.Form["RamLt"],
                                        OcungLt = Request.Form["OcungLt"],
                                        XuatxuLt = Request.Form["XuatxuLt"],
                                        NgaysanxuatLt = string.IsNullOrEmpty(Request.Form["NgaysanxuatLt"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatLt"]),
                                        TinhtrangLt = Request.Form["TinhtrangLt"],
                                        MaNcc = model.MaNcc,
                                        MaPlsp = "PL005"
                                    };
                                    _context.Laptops.Add(laptop);
                                }
                                break;

                            case "Loa":
                                var loa = await _context.Loas.FirstOrDefaultAsync(l => l.MaHh == id);
                                if (loa != null)
                                {
                                    loa.TenLoa = model.Ten;
                                    loa.GiaLoa = model.Gia;
                                    loa.HangLoa = Request.Form["HangLoa"];
                                    loa.CongsuatLoa = Request.Form["CongsuatLoa"];
                                    loa.KieuketnoiLoa = Request.Form["KieuketnoiLoa"];
                                    loa.XuatxuLoa = Request.Form["XuatxuLoa"];
                                    loa.NgaysanxuatLoa = string.IsNullOrEmpty(Request.Form["NgaysanxuatLoa"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatLoa"]);
                                    loa.TinhtrangLoa = Request.Form["TinhtrangLoa"];
                                    loa.MaNcc = model.MaNcc;
                                    _context.Update(loa);
                                }
                                else
                                {
                                    loa = new Loa
                                    {
                                        MaLoa = hanghoa.MaHh,
                                        MaHh = hanghoa.MaHh,
                                        TenLoa = model.Ten,
                                        GiaLoa = model.Gia,
                                        HangLoa = Request.Form["HangLoa"],
                                        CongsuatLoa = Request.Form["CongsuatLoa"],
                                        KieuketnoiLoa = Request.Form["KieuketnoiLoa"],
                                        XuatxuLoa = Request.Form["XuatxuLoa"],
                                        NgaysanxuatLoa = string.IsNullOrEmpty(Request.Form["NgaysanxuatLoa"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatLoa"]),
                                        TinhtrangLoa = Request.Form["TinhtrangLoa"],
                                        MaNcc = model.MaNcc,
                                        MaPlsp = "PL002"
                                    };
                                    _context.Loas.Add(loa);
                                }
                                break;

                            case "Màn hình":
                                var manhinh = await _context.Manhinhs.FirstOrDefaultAsync(m => m.MaHh == id);
                                if (manhinh != null)
                                {
                                    manhinh.TenMh = model.Ten;
                                    manhinh.GiaMh = model.Gia;
                                    manhinh.HangMh = Request.Form["HangMh"];
                                    manhinh.KichthuocMh = Request.Form["KichthuocMh"];
                                    manhinh.TansoMh = Request.Form["TansoMh"];
                                    manhinh.DoPhanGiaiMh = Request.Form["DoPhanGiaiMh"];
                                    manhinh.XuatxuMh = Request.Form["XuatxuMh"];
                                    manhinh.NgaysanxuatMh = string.IsNullOrEmpty(Request.Form["NgaysanxuatMh"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatMh"]);
                                    manhinh.TinhtrangMh = Request.Form["TinhtrangMh"];
                                    manhinh.MaNcc = model.MaNcc;
                                    _context.Update(manhinh);
                                }
                                else
                                {
                                    manhinh = new Manhinh
                                    {
                                        MaMh = hanghoa.MaHh,
                                        MaHh = hanghoa.MaHh,
                                        TenMh = model.Ten,
                                        GiaMh = model.Gia,
                                        HangMh = Request.Form["HangMh"],
                                        KichthuocMh = Request.Form["KichthuocMh"],
                                        TansoMh = Request.Form["TansoMh"],
                                        DoPhanGiaiMh = Request.Form["DoPhanGiaiMh"],
                                        XuatxuMh = Request.Form["XuatxuMh"],
                                        NgaysanxuatMh = string.IsNullOrEmpty(Request.Form["NgaysanxuatMh"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatMh"]),
                                        TinhtrangMh = Request.Form["TinhtrangMh"],
                                        MaNcc = model.MaNcc,
                                        MaPlsp = "PL003"
                                    };
                                    _context.Manhinhs.Add(manhinh);
                                }
                                break;

                            case "Tai nghe":
                                var tainghe = await _context.Tainghes.FirstOrDefaultAsync(t => t.MaHh == id);
                                if (tainghe != null)
                                {
                                    tainghe.TenTn = model.Ten;
                                    tainghe.GiaTn = model.Gia;
                                    tainghe.HangTn = Request.Form["HangTn"];
                                    tainghe.LoaiTn = Request.Form["LoaiTn"];
                                    tainghe.KieuketnoiTn = Request.Form["KieuketnoiTn"];
                                    tainghe.PinTn = Request.Form["PinTn"];
                                    tainghe.TrongluongTn = Request.Form["TrongluongTn"];
                                    tainghe.XuatxuTn = Request.Form["XuatxuTn"];
                                    tainghe.NgaysanxuatTn = string.IsNullOrEmpty(Request.Form["NgaysanxuatTn"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatTn"]);
                                    tainghe.TinhtrangTn = Request.Form["TinhtrangTn"];
                                    tainghe.MaNcc = model.MaNcc;
                                    _context.Update(tainghe);
                                }
                                else
                                {
                                    tainghe = new Tainghe
                                    {
                                        MaTn = hanghoa.MaHh,
                                        MaHh = hanghoa.MaHh,
                                        TenTn = model.Ten,
                                        GiaTn = model.Gia,
                                        HangTn = Request.Form["HangTn"],
                                        LoaiTn = Request.Form["LoaiTn"],
                                        KieuketnoiTn = Request.Form["KieuketnoiTn"],
                                        PinTn = Request.Form["PinTn"],
                                        TrongluongTn = Request.Form["TrongluongTn"],
                                        XuatxuTn = Request.Form["XuatxuTn"],
                                        NgaysanxuatTn = string.IsNullOrEmpty(Request.Form["NgaysanxuatTn"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatTn"]),
                                        TinhtrangTn = Request.Form["TinhtrangTn"],
                                        MaNcc = model.MaNcc,
                                        MaPlsp = "PL004"
                                    };
                                    _context.Tainghes.Add(tainghe);
                                }
                                break;

                            case "Bàn phím":
                                var banphim = await _context.Banphims.FirstOrDefaultAsync(b => b.MaHh == id);
                                if (banphim != null)
                                {
                                    banphim.TenBp = model.Ten;
                                    banphim.GiaBp = model.Gia;
                                    banphim.HangBp = Request.Form["HangBp"];
                                    banphim.LoaiBp = Request.Form["LoaiBp"];
                                    banphim.KieuketnoiBp = Request.Form["KieuketnoiBp"];
                                    banphim.DenledBp = Request.Form["DenledBp"] == "true";
                                    banphim.XuatxuBp = Request.Form["XuatxuBp"];
                                    banphim.NgaysanxuatBp = string.IsNullOrEmpty(Request.Form["NgaysanxuatBp"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatBp"]);
                                    banphim.TinhtrangBp = Request.Form["TinhtrangBp"];
                                    banphim.MaNcc = model.MaNcc;
                                    _context.Update(banphim);
                                }
                                else
                                {
                                    banphim = new Banphim
                                    {
                                        MaBp = hanghoa.MaHh,
                                        MaHh = hanghoa.MaHh,
                                        TenBp = model.Ten,
                                        GiaBp = model.Gia,
                                        HangBp = Request.Form["HangBp"],
                                        LoaiBp = Request.Form["LoaiBp"],
                                        KieuketnoiBp = Request.Form["KieuketnoiBp"],
                                        DenledBp = Request.Form["DenledBp"] == "true",
                                        XuatxuBp = Request.Form["XuatxuBp"],
                                        NgaysanxuatBp = string.IsNullOrEmpty(Request.Form["NgaysanxuatBp"]) ? (DateOnly?)null : DateOnly.Parse(Request.Form["NgaysanxuatBp"]),
                                        TinhtrangBp = Request.Form["TinhtrangBp"],
                                        MaNcc = model.MaNcc,
                                        MaPlsp = "PL001"
                                    };
                                    _context.Banphims.Add(banphim);
                                }
                                break;

                            default:
                                TempData["Error"] = $"Loại sản phẩm không được hỗ trợ: {loaiHhNormalized}";
                                return View("~/Views/Admin/Product/Edit.cshtml", model);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Lỗi khi lưu chi tiết sản phẩm ({loaiHhNormalized}): {ex.Message}";
                        return View("~/Views/Admin/Product/Edit.cshtml", model);
                    }

                    // Xử lý hình ảnh
                    if (images != null && images.Count > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", hanghoa.MaHh);
                        Directory.CreateDirectory(uploadsFolder);

                        int imageIndex = _context.HinhAnhSanPhams.Count(img => img.MaHh == hanghoa.MaHh) + 1;
                        foreach (var image in images)
                        {
                            if (image.Length > 0)
                            {
                                var extension = Path.GetExtension(image.FileName).ToLower();
                                if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(extension))
                                {
                                    ModelState.AddModelError("images", "Chỉ chấp nhận file ảnh định dạng .jpg, .jpeg hoặc .png.");
                                    return View("~/Views/Admin/Product/Edit.cshtml", model);
                                }
                                if (image.Length > 5 * 1024 * 1024)
                                {
                                    ModelState.AddModelError("images", "Kích thước file không được vượt quá 5MB.");
                                    return View("~/Views/Admin/Product/Edit.cshtml", model);
                                }

                                var fileName = $"{hanghoa.MaHh}_{imageIndex}{extension}";
                                var filePath = Path.Combine(uploadsFolder, fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await image.CopyToAsync(stream);
                                }
                                var imagePath = $"{hanghoa.MaHh}/{fileName}";
                                _context.HinhAnhSanPhams.Add(new HinhAnhSanPham { MaHh = hanghoa.MaHh, DuongDan = imagePath });
                                imageIndex++;
                            }
                        }
                    }

                    await _context.SaveChangesAsync();

                    // Cập nhật danh sách hình ảnh trong model
                    model.HinhAnh = _context.HinhAnhSanPhams
                        .Where(img => img.MaHh == hanghoa.MaHh)
                        .Select(img => img.DuongDan)
                        .ToList() ?? new List<string> { "default.jpg" };

                    TempData["Success"] = "Sản phẩm đã được chỉnh sửa thành công!";
                    return RedirectToAction(nameof(Index), new { category = loaiHhNormalized.ToLower() });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HanghoaExists(model.Id))
                    {
                        TempData["Error"] = "Sản phẩm không tồn tại.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Sản phẩm đã bị chỉnh sửa bởi người khác. Vui lòng thử lại.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi chỉnh sửa sản phẩm: {ex.Message}";
                    return View("~/Views/Admin/Product/Edit.cshtml", model);
                }
            }

            // Nếu ModelState không hợp lệ, điền lại dữ liệu ViewBag
            switch ((model.Loai ?? "").ToLower())
            {
                case "laptop":
                    ViewBag.HangLt = Request.Form["HangLt"];
                    ViewBag.KichthuocLt = Request.Form["KichthuocLt"];
                    ViewBag.RamLt = Request.Form["RamLt"];
                    ViewBag.OcungLt = Request.Form["OcungLt"];
                    ViewBag.XuatxuLt = Request.Form["XuatxuLt"];
                    ViewBag.NgaysanxuatLt = Request.Form["NgaysanxuatLt"];
                    ViewBag.TinhtrangLt = Request.Form["TinhtrangLt"];
                    break;

                case "loa":
                    ViewBag.HangLoa = Request.Form["HangLoa"];
                    ViewBag.CongsuatLoa = Request.Form["CongsuatLoa"];
                    ViewBag.KieuketnoiLoa = Request.Form["KieuketnoiLoa"];
                    ViewBag.XuatxuLoa = Request.Form["XuatxuLoa"];
                    ViewBag.NgaysanxuatLoa = Request.Form["NgaysanxuatLoa"];
                    ViewBag.TinhtrangLoa = Request.Form["TinhtrangLoa"];
                    break;

                case "manhinh":
                    ViewBag.HangMh = Request.Form["HangMh"];
                    ViewBag.KichthuocMh = Request.Form["KichthuocMh"];
                    ViewBag.TansoMh = Request.Form["TansoMh"];
                    ViewBag.DoPhanGiaiMh = Request.Form["DoPhanGiaiMh"];
                    ViewBag.XuatxuMh = Request.Form["XuatxuMh"];
                    ViewBag.NgaysanxuatMh = Request.Form["NgaysanxuatMh"];
                    ViewBag.TinhtrangMh = Request.Form["TinhtrangMh"];
                    break;

                case "tainghe":
                    ViewBag.HangTn = Request.Form["HangTn"];
                    ViewBag.LoaiTn = Request.Form["LoaiTn"];
                    ViewBag.KieuketnoiTn = Request.Form["KieuketnoiTn"];
                    ViewBag.PinTn = Request.Form["PinTn"];
                    ViewBag.TrongluongTn = Request.Form["TrongluongTn"];
                    ViewBag.XuatxuTn = Request.Form["XuatxuTn"];
                    ViewBag.NgaysanxuatTn = Request.Form["NgaysanxuatTn"];
                    ViewBag.TinhtrangTn = Request.Form["TinhtrangTn"];
                    break;

                case "banphim":
                    ViewBag.HangBp = Request.Form["HangBp"];
                    ViewBag.LoaiBp = Request.Form["LoaiBp"];
                    ViewBag.KieuketnoiBp = Request.Form["KieuketnoiBp"];
                    ViewBag.DenledBp = Request.Form["DenledBp"] == "true";
                    ViewBag.XuatxuBp = Request.Form["XuatxuBp"];
                    ViewBag.NgaysanxuatBp = Request.Form["NgaysanxuatBp"];
                    ViewBag.TinhtrangBp = Request.Form["TinhtrangBp"];
                    break;
            }

            return View("~/Views/Admin/Product/Edit.cshtml", model);
        }
        // Khôi phục sản phẩm đã xóa
        public async Task<IActionResult> Restore(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hanghoa = await _context.Hanghoas
                .FirstOrDefaultAsync(m => m.MaHh == id);

            if (hanghoa == null)
            {
                return NotFound();
            }

            // Khôi phục sản phẩm bằng cách xóa NgayXoa
            hanghoa.NgayXoa = null;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Sản phẩm đã được khôi phục thành công!";
            return RedirectToAction(nameof(Index), new { showDeleted = true });
        }

        // Hiển thị trang xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hanghoa = await _context.Hanghoas
                .Include(h => h.HinhAnhSanPhams)
                .FirstOrDefaultAsync(h => h.MaHh == id);
            if (hanghoa == null)
            {
                return NotFound();
            }

            var viewModel = new ProductViewModel
            {
                Id = hanghoa.MaHh,
                Ten = hanghoa.TenHh,
                Gia = hanghoa.GiaHh,
                Loai = hanghoa.LoaiHh,
                Soluongton = hanghoa.Soluongton,
                MoTa = hanghoa.MoTa,
                HinhAnh = hanghoa.HinhAnhSanPhams
                    .Select(img => img.DuongDan)
                    .ToList() ?? new List<string> { "default.jpg" }
            };

            return View("~/Views/Admin/Product/Delete.cshtml", viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var hanghoa = await _context.Hanghoas.FindAsync(id);
            if (hanghoa == null)
            {
                return NotFound();
            }

            // Soft delete by setting NgayXoa
            hanghoa.NgayXoa = DateTime.Now;
            _context.Update(hanghoa);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Sản phẩm đã được xóa thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("Admin/Product/DeleteImage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(string productId, string imageName)
        {
            if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(imageName))
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ" });
            }

            try
            {
                // Tìm bản ghi hình ảnh trong database
                var image = await _context.HinhAnhSanPhams
                    .FirstOrDefaultAsync(h => h.MaHh == productId && h.DuongDan == imageName);

                if (image != null)
                {
                    // Xóa khỏi database
                    _context.HinhAnhSanPhams.Remove(image);
                    await _context.SaveChangesAsync();

                    // Xóa file hình ảnh từ thư mục wwwroot/images
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageName); // imageName đã bao gồm MaHh/TenFile.jpg
                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        catch (IOException)
                        {
                            // Nếu không xóa được file, vẫn trả về thành công vì đã xóa được trong DB
                            return Json(new { success = true, message = "Đã xóa khỏi database nhưng không thể xóa file ảnh" });
                        }
                    }

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Không tìm thấy hình ảnh" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool HanghoaExists(string id)
        {
            return _context.Hanghoas.Any(e => e.MaHh == id);
        }

        private string GenerateNewProductId()
        {
            var lastProduct = _context.Hanghoas.OrderByDescending(h => h.MaHh).FirstOrDefault();
            if (lastProduct == null)
            {
                return "HH001";
            }

            int number = int.Parse(lastProduct.MaHh.Replace("HH", "")) + 1;
            return $"HH{number:D3}";
        }
    }
}