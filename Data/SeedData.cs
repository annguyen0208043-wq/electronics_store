using Microsoft.EntityFrameworkCore;
using TechShop.Models;
using Microsoft.Extensions.Logging;

namespace TechShop.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, AppDbContext context, int desiredCount = 10)
        {
            // Lấy ILoggerFactory và tạo logger
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("SeedData");

            logger.LogInformation("Bắt đầu kiểm tra dữ liệu...");

            // 1. Seed PhanloaiSp
            AdjustTableCount<PhanloaiSp>(context, desiredCount, logger, () =>
            {
                var currentCount = context.PhanloaiSps.Count();
                var newItems = new List<PhanloaiSp>
                {
                    new PhanloaiSp { MaPlsp = "PL001", TenPlsp = "Bàn phím" },
                    new PhanloaiSp { MaPlsp = "PL002", TenPlsp = "Loa" },
                    new PhanloaiSp { MaPlsp = "PL003", TenPlsp = "Màn hình" },
                    new PhanloaiSp { MaPlsp = "PL004", TenPlsp = "Tai nghe" },
                    new PhanloaiSp { MaPlsp = "PL005", TenPlsp = "Laptop" }
                };
                // Thêm dữ liệu mới nếu cần
                for (int i = currentCount + 1; i <= desiredCount; i++)
                {
                    newItems.Add(new PhanloaiSp { MaPlsp = $"PL{i:D3}", TenPlsp = $"Phân loại {i}" });
                }
                return newItems.Take(desiredCount - currentCount).ToList();
            }, entities => context.PhanloaiSps.RemoveRange(entities));

            // 2. Seed Nhacungcap
            AdjustTableCount<Nhacungcap>(context, desiredCount, logger, () =>
            {
                var currentCount = context.Nhacungcaps.Count();
                var newItems = new List<Nhacungcap>
                {
                    new Nhacungcap { MaNcc = "NCC001", TenNcc = "Logitech VN", EmailNcc = "contact@logitech.vn", SdtNcc = "0901234567", DiachiNcc = "Hà Nội", Nguoilienlac = "Nguyễn Văn A" },
                    new Nhacungcap { MaNcc = "NCC002", TenNcc = "Razer VN", EmailNcc = "contact@razer.vn", SdtNcc = "0902345678", DiachiNcc = "TP.HCM", Nguoilienlac = "Trần Thị B" },
                    new Nhacungcap { MaNcc = "NCC003", TenNcc = "JBL VN", EmailNcc = "contact@jbl.vn", SdtNcc = "0903456789", DiachiNcc = "Đà Nẵng", Nguoilienlac = "Lê Văn C" },
                    new Nhacungcap { MaNcc = "NCC004", TenNcc = "Dell VN", EmailNcc = "contact@dell.vn", SdtNcc = "0904567890", DiachiNcc = "Hà Nội", Nguoilienlac = "Phạm Thị D" },
                    new Nhacungcap { MaNcc = "NCC005", TenNcc = "Asus VN", EmailNcc = "contact@asus.vn", SdtNcc = "0905678901", DiachiNcc = "TP.HCM", Nguoilienlac = "Hoàng Văn E" }
                };
                for (int i = currentCount + 1; i <= desiredCount; i++)
                {
                    newItems.Add(new Nhacungcap
                    {
                        MaNcc = $"NCC{i:D3}",
                        TenNcc = $"Nhà cung cấp {i}",
                        EmailNcc = $"contact_ncc{i:D3}@example.com",
                        SdtNcc = $"090{i:D7}",
                        DiachiNcc = "Việt Nam",
                        Nguoilienlac = $"Người {i}"
                    });
                }
                return newItems.Take(desiredCount - currentCount).ToList();
            }, entities => context.Nhacungcaps.RemoveRange(entities));

            // 3. Seed Hanghoa
            AdjustTableCount<Hanghoa>(context, desiredCount, logger, () =>
            {
                var currentCount = context.Hanghoas.Count();
                var newItems = new List<Hanghoa>
                {
                    new Hanghoa { MaHh = "HH001", TenHh = "Bàn phím Logitech G Pro X", MaNcc = "NCC001", LoaiHh = "Bàn phím", GiaHh = 2500000, Soluongton = 100, MoTa = "Bàn phím cơ cao cấp" },
                    new Hanghoa { MaHh = "HH002", TenHh = "Bàn phím Razer Huntsman", MaNcc = "NCC002", LoaiHh = "Bàn phím", GiaHh = 3200000, Soluongton = 80, MoTa = "Bàn phím cơ RGB" },
                    new Hanghoa { MaHh = "HH003", TenHh = "Bàn phím Corsair K70", MaNcc = "NCC001", LoaiHh = "Bàn phím", GiaHh = 2800000, Soluongton = 90, MoTa = "Bàn phím cơ bền bỉ" },
                    new Hanghoa { MaHh = "HH004", TenHh = "Bàn phím SteelSeries Apex", MaNcc = "NCC002", LoaiHh = "Bàn phím", GiaHh = 2700000, Soluongton = 70, MoTa = "Bàn phím cơ đa năng" },
                    new Hanghoa { MaHh = "HH005", TenHh = "Bàn phím HyperX Alloy", MaNcc = "NCC001", LoaiHh = "Bàn phím", GiaHh = 2600000, Soluongton = 60, MoTa = "Bàn phím cơ giá rẻ" },
                    new Hanghoa { MaHh = "HH006", TenHh = "Bàn phím Ducky One 3", MaNcc = "NCC002", LoaiHh = "Bàn phím", GiaHh = 2900000, Soluongton = 50, MoTa = "Bàn phím cơ chất lượng cao" },
                    new Hanghoa { MaHh = "HH007", TenHh = "Bàn phím Keychron K8", MaNcc = "NCC001", LoaiHh = "Bàn phím", GiaHh = 2400000, Soluongton = 100, MoTa = "Bàn phím cơ không dây" },
                    new Hanghoa { MaHh = "HH008", TenHh = "Bàn phím Anne Pro 2", MaNcc = "NCC002", LoaiHh = "Bàn phím", GiaHh = 2300000, Soluongton = 80, MoTa = "Bàn phím cơ nhỏ gọn" }
                    // Thêm các bản ghi khác nếu cần
                };
                for (int i = currentCount + 1; i <= desiredCount; i++)
                {
                    newItems.Add(new Hanghoa
                    {
                        MaHh = $"HH{i:D3}",
                        TenHh = $"Hàng hóa {i}",
                        MaNcc = "NCC001",
                        LoaiHh = "Khác",
                        GiaHh = 1000000 * i,
                        Soluongton = 50,
                        MoTa = $"Mô tả hàng hóa {i}"
                    });
                }
                return newItems.Take(desiredCount - currentCount).ToList();
            }, entities => context.Hanghoas.RemoveRange(entities));

            // 4. Seed Banphim
            AdjustTableCount<Banphim>(context, desiredCount, logger, () =>
            {
                var currentCount = context.Banphims.Count();
                var newItems = new List<Banphim>
                {
                    new Banphim { MaBp = "BP001", TenBp = "Logitech G Pro X", HangBp = "Logitech", GiaBp = 2500000, LoaiBp = "Cơ", KieuketnoiBp = "USB", DenledBp = true, XuatxuBp = "Trung Quốc", NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangBp = "Mới", MaNcc = "NCC001", MaPlsp = "PL001", MaHh = "HH001" },
                    new Banphim { MaBp = "BP002", TenBp = "Razer Huntsman", HangBp = "Razer", GiaBp = 3200000, LoaiBp = "Cơ", KieuketnoiBp = "USB", DenledBp = true, XuatxuBp = "Hàn Quốc", NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangBp = "Mới", MaNcc = "NCC002", MaPlsp = "PL001", MaHh = "HH002" },
                    new Banphim { MaBp = "BP003", TenBp = "Corsair K70", HangBp = "Corsair", GiaBp = 2800000, LoaiBp = "Cơ", KieuketnoiBp = "USB", DenledBp = true, XuatxuBp = "Mỹ", NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)), TinhtrangBp = "Mới", MaNcc = "NCC001", MaPlsp = "PL001", MaHh = "HH003" },
                    new Banphim { MaBp = "BP004", TenBp = "SteelSeries Apex", HangBp = "SteelSeries", GiaBp = 2700000, LoaiBp = "Cơ", KieuketnoiBp = "USB", DenledBp = true, XuatxuBp = "Trung Quốc", NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangBp = "Mới", MaNcc = "NCC002", MaPlsp = "PL001", MaHh = "HH004" },
                    new Banphim { MaBp = "BP005", TenBp = "HyperX Alloy", HangBp = "HyperX", GiaBp = 2600000, LoaiBp = "Cơ", KieuketnoiBp = "USB", DenledBp = true, XuatxuBp = "Mỹ", NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangBp = "Mới", MaNcc = "NCC001", MaPlsp = "PL001", MaHh = "HH005" },
                    new Banphim { MaBp = "BP006", TenBp = "Ducky One 3", HangBp = "Ducky", GiaBp = 2900000, LoaiBp = "Cơ", KieuketnoiBp = "USB", DenledBp = true, XuatxuBp = "Đài Loan", NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangBp = "Mới", MaNcc = "NCC002", MaPlsp = "PL001", MaHh = "HH006" },
                    new Banphim { MaBp = "BP007", TenBp = "Keychron K8", HangBp = "Keychron", GiaBp = 2400000, LoaiBp = "Cơ", KieuketnoiBp = "Bluetooth", DenledBp = true, XuatxuBp = "Trung Quốc", NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangBp = "Mới", MaNcc = "NCC001", MaPlsp = "PL001", MaHh = "HH007" },
                    new Banphim { MaBp = "BP008", TenBp = "Anne Pro 2", HangBp = "Anne", GiaBp = 2300000, LoaiBp = "Cơ", KieuketnoiBp = "Bluetooth", DenledBp = true, XuatxuBp = "Trung Quốc", NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangBp = "Mới", MaNcc = "NCC002", MaPlsp = "PL001", MaHh = "HH008" }
                };
                for (int i = currentCount + 1; i <= desiredCount; i++)
                {
                    newItems.Add(new Banphim
                    {
                        MaBp = $"BP{i:D3}",
                        TenBp = $"Bàn phím {i}",
                        HangBp = "Generic",
                        GiaBp = 1000000 * i,
                        LoaiBp = "Cơ",
                        KieuketnoiBp = "USB",
                        DenledBp = true,
                        XuatxuBp = "Trung Quốc",
                        NgaysanxuatBp = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)),
                        TinhtrangBp = "Mới",
                        MaNcc = "NCC001",
                        MaPlsp = "PL001",
                        MaHh = $"HH{i:D3}"
                    });
                }
                return newItems.Take(desiredCount - currentCount).ToList();
            }, entities => context.Banphims.RemoveRange(entities));

            // 5. Seed Loa
            AdjustTableCount<Loa>(context, desiredCount, logger, () =>
            {
                var currentCount = context.Loas.Count();
                var newItems = new List<Loa>
                {
                    new Loa { MaLoa = "LOA001", TenLoa = "JBL Flip 5", HangLoa = "JBL", GiaLoa = 2800000, CongsuatLoa = "20W", KieuketnoiLoa = "Bluetooth", XuatxuLoa = "Mỹ", NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLoa = "Mới", MaNcc = "NCC003", MaPlsp = "PL002", MaHh = "HH009" },
                    new Loa { MaLoa = "LOA002", TenLoa = "Bose SoundLink", HangLoa = "Bose", GiaLoa = 4500000, CongsuatLoa = "30W", KieuketnoiLoa = "Bluetooth", XuatxuLoa = "Mỹ", NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLoa = "Mới", MaNcc = "NCC003", MaPlsp = "PL002", MaHh = "HH010" },
                    new Loa { MaLoa = "LOA003", TenLoa = "Sony SRS-XB43", HangLoa = "Sony", GiaLoa = 3500000, CongsuatLoa = "25W", KieuketnoiLoa = "Bluetooth", XuatxuLoa = "Nhật Bản", NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLoa = "Mới", MaNcc = "NCC003", MaPlsp = "PL002", MaHh = "HH011" },
                    new Loa { MaLoa = "LOA004", TenLoa = "Harman Kardon Onyx", HangLoa = "Harman Kardon", GiaLoa = 5000000, CongsuatLoa = "40W", KieuketnoiLoa = "Bluetooth", XuatxuLoa = "Mỹ", NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLoa = "Mới", MaNcc = "NCC003", MaPlsp = "PL002", MaHh = "HH012" },
                    new Loa { MaLoa = "LOA005", TenLoa = "Anker Soundcore", HangLoa = "Anker", GiaLoa = 2000000, CongsuatLoa = "15W", KieuketnoiLoa = "Bluetooth", XuatxuLoa = "Trung Quốc", NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLoa = "Mới", MaNcc = "NCC003", MaPlsp = "PL002", MaHh = "HH013" },
                    new Loa { MaLoa = "LOA006", TenLoa = "Ultimate Ears Boom", HangLoa = "UE", GiaLoa = 3000000, CongsuatLoa = "20W", KieuketnoiLoa = "Bluetooth", XuatxuLoa = "Mỹ", NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLoa = "Mới", MaNcc = "NCC003", MaPlsp = "PL002", MaHh = "HH014" },
                    new Loa { MaLoa = "LOA007", TenLoa = "JBL Charge 5", HangLoa = "JBL", GiaLoa = 3200000, CongsuatLoa = "30W", KieuketnoiLoa = "Bluetooth", XuatxuLoa = "Mỹ", NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLoa = "Mới", MaNcc = "NCC003", MaPlsp = "PL002", MaHh = "HH015" },
                    new Loa { MaLoa = "LOA008", TenLoa = "Bose Home Speaker", HangLoa = "Bose", GiaLoa = 6000000, CongsuatLoa = "50W", KieuketnoiLoa = "Wi-Fi", XuatxuLoa = "Mỹ", NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLoa = "Mới", MaNcc = "NCC003", MaPlsp = "PL002", MaHh = "HH016" }
                };
                for (int i = currentCount + 1; i <= desiredCount; i++)
                {
                    newItems.Add(new Loa
                    {
                        MaLoa = $"LOA{i:D3}",
                        TenLoa = $"Loa {i}",
                        HangLoa = "Generic",
                        GiaLoa = 1000000 * i,
                        CongsuatLoa = "20W",
                        KieuketnoiLoa = "Bluetooth",
                        XuatxuLoa = "Trung Quốc",
                        NgaysanxuatLoa = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)),
                        TinhtrangLoa = "Mới",
                        MaNcc = "NCC003",
                        MaPlsp = "PL002",
                        MaHh = $"HH{i:D3}"
                    });
                }
                return newItems.Take(desiredCount - currentCount).ToList();
            }, entities => context.Loas.RemoveRange(entities));

            // 6. Seed Manhinh
            AdjustTableCount<Manhinh>(context, desiredCount, logger, () =>
            {
                var currentCount = context.Manhinhs.Count();
                var newItems = new List<Manhinh>
                {
                    new Manhinh { MaMh = "MH001", TenMh = "Dell UltraSharp 27", HangMh = "Dell", GiaMh = 8500000, KichthuocMh = "27 inch", TansoMh = "60Hz", DoPhanGiaiMh = "2560x1440", XuatxuMh = "Trung Quốc", NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangMh = "Mới", MaNcc = "NCC004", MaPlsp = "PL003", MaHh = "HH017" },
                    new Manhinh { MaMh = "MH002", TenMh = "LG UltraGear 32", HangMh = "LG", GiaMh = 9000000, KichthuocMh = "32 inch", TansoMh = "144Hz", DoPhanGiaiMh = "2560x1440", XuatxuMh = "Hàn Quốc", NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangMh = "Mới", MaNcc = "NCC004", MaPlsp = "PL003", MaHh = "HH018" },
                    new Manhinh { MaMh = "MH003", TenMh = "Samsung Odyssey G7", HangMh = "Samsung", GiaMh = 9500000, KichthuocMh = "27 inch", TansoMh = "240Hz", DoPhanGiaiMh = "2560x1440", XuatxuMh = "Hàn Quốc", NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangMh = "Mới", MaNcc = "NCC004", MaPlsp = "PL003", MaHh = "HH019" },
                    new Manhinh { MaMh = "MH004", TenMh = "Asus ROG Swift", HangMh = "Asus", GiaMh = 12000000, KichthuocMh = "27 inch", TansoMh = "165Hz", DoPhanGiaiMh = "2560x1440", XuatxuMh = "Đài Loan", NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangMh = "Mới", MaNcc = "NCC005", MaPlsp = "PL003", MaHh = "HH020" },
                    new Manhinh { MaMh = "MH005", TenMh = "Acer Predator", HangMh = "Acer", GiaMh = 8800000, KichthuocMh = "27 inch", TansoMh = "144Hz", DoPhanGiaiMh = "1920x1080", XuatxuMh = "Trung Quốc", NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangMh = "Mới", MaNcc = "NCC004", MaPlsp = "PL003", MaHh = "HH021" },
                    new Manhinh { MaMh = "MH006", TenMh = "ViewSonic Elite", HangMh = "ViewSonic", GiaMh = 8700000, KichthuocMh = "27 inch", TansoMh = "144Hz", DoPhanGiaiMh = "2560x1440", XuatxuMh = "Trung Quốc", NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangMh = "Mới", MaNcc = "NCC004", MaPlsp = "PL003", MaHh = "HH022" },
                    new Manhinh { MaMh = "MH007", TenMh = "HP Omen", HangMh = "HP", GiaMh = 8600000, KichthuocMh = "27 inch", TansoMh = "165Hz", DoPhanGiaiMh = "2560x1440", XuatxuMh = "Trung Quốc", NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangMh = "Mới", MaNcc = "NCC004", MaPlsp = "PL003", MaHh = "HH023" },
                    new Manhinh { MaMh = "MH008", TenMh = "BenQ Zowie", HangMh = "BenQ", GiaMh = 8400000, KichthuocMh = "24 inch", TansoMh = "144Hz", DoPhanGiaiMh = "1920x1080", XuatxuMh = "Trung Quốc", NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangMh = "Mới", MaNcc = "NCC004", MaPlsp = "PL003", MaHh = "HH024" }
                };
                for (int i = currentCount + 1; i <= desiredCount; i++)
                {
                    newItems.Add(new Manhinh
                    {
                        MaMh = $"MH{i:D3}",
                        TenMh = $"Màn hình {i}",
                        HangMh = "Generic",
                        GiaMh = 5000000 * i,
                        KichthuocMh = "27 inch",
                        TansoMh = "60Hz",
                        DoPhanGiaiMh = "1920x1080",
                        XuatxuMh = "Trung Quốc",
                        NgaysanxuatMh = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)),
                        TinhtrangMh = "Mới",
                        MaNcc = "NCC004",
                        MaPlsp = "PL003",
                        MaHh = $"HH{i:D3}"
                    });
                }
                return newItems.Take(desiredCount - currentCount).ToList();
            }, entities => context.Manhinhs.RemoveRange(entities));

            // 7. Seed Tainghe
            AdjustTableCount<Tainghe>(context, desiredCount, logger, () =>
            {
                var currentCount = context.Tainghes.Count();
                var newItems = new List<Tainghe>
                {
                    new Tainghe { MaTn = "TN001", TenTn = "Sony WH-1000XM5", HangTn = "Sony", GiaTn = 7500000, LoaiTn = "Over-ear", KieuketnoiTn = "Bluetooth", PinTn = "30 giờ", TrongluongTn = "250g", XuatxuTn = "Nhật Bản", NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangTn = "Mới", MaNcc = "NCC003", MaPlsp = "PL004", MaHh = "HH025" },
                    new Tainghe { MaTn = "TN002", TenTn = "Bose QuietComfort", HangTn = "Bose", GiaTn = 7000000, LoaiTn = "Over-ear", KieuketnoiTn = "Bluetooth", PinTn = "24 giờ", TrongluongTn = "240g", XuatxuTn = "Mỹ", NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangTn = "Mới", MaNcc = "NCC003", MaPlsp = "PL004", MaHh = "HH026" },
                    new Tainghe { MaTn = "TN003", TenTn = "JBL Live 660NC", HangTn = "JBL", GiaTn = 3500000, LoaiTn = "Over-ear", KieuketnoiTn = "Bluetooth", PinTn = "40 giờ", TrongluongTn = "265g", XuatxuTn = "Mỹ", NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangTn = "Mới", MaNcc = "NCC003", MaPlsp = "PL004", MaHh = "HH027" },
                    new Tainghe { MaTn = "TN004", TenTn = "Sennheiser HD 450BT", HangTn = "Sennheiser", GiaTn = 4000000, LoaiTn = "Over-ear", KieuketnoiTn = "Bluetooth", PinTn = "30 giờ", TrongluongTn = "238g", XuatxuTn = "Đức", NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangTn = "Mới", MaNcc = "NCC003", MaPlsp = "PL004", MaHh = "HH028" },
                    new Tainghe { MaTn = "TN005", TenTn = "Anker Soundcore Q30", HangTn = "Anker", GiaTn = 2000000, LoaiTn = "Over-ear", KieuketnoiTn = "Bluetooth", PinTn = "40 giờ", TrongluongTn = "260g", XuatxuTn = "Trung Quốc", NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangTn = "Mới", MaNcc = "NCC003", MaPlsp = "PL004", MaHh = "HH029" },
                    new Tainghe { MaTn = "TN006", TenTn = "Razer Kraken", HangTn = "Razer", GiaTn = 2500000, LoaiTn = "Over-ear", KieuketnoiTn = "3.5mm", PinTn = "Không", TrongluongTn = "322g", XuatxuTn = "Trung Quốc", NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangTn = "Mới", MaNcc = "NCC002", MaPlsp = "PL004", MaHh = "HH030" },
                    new Tainghe { MaTn = "TN007", TenTn = "HyperX Cloud II", HangTn = "HyperX", GiaTn = 2700000, LoaiTn = "Over-ear", KieuketnoiTn = "3.5mm", PinTn = "Không", TrongluongTn = "275g", XuatxuTn = "Mỹ", NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangTn = "Mới", MaNcc = "NCC001", MaPlsp = "PL004", MaHh = "HH031" },
                    new Tainghe { MaTn = "TN008", TenTn = "SteelSeries Arctis", HangTn = "SteelSeries", GiaTn = 3000000, LoaiTn = "Over-ear", KieuketnoiTn = "Bluetooth", PinTn = "28 giờ", TrongluongTn = "280g", XuatxuTn = "Trung Quốc", NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangTn = "Mới", MaNcc = "NCC002", MaPlsp = "PL004", MaHh = "HH032" }
                };
                for (int i = currentCount + 1; i <= desiredCount; i++)
                {
                    newItems.Add(new Tainghe
                    {
                        MaTn = $"TN{i:D3}",
                        TenTn = $"Tai nghe {i}",
                        HangTn = "Generic",
                        GiaTn = 1000000 * i,
                        LoaiTn = "Over-ear",
                        KieuketnoiTn = "Bluetooth",
                        PinTn = "20 giờ",
                        TrongluongTn = "250g",
                        XuatxuTn = "Trung Quốc",
                        NgaysanxuatTn = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)),
                        TinhtrangTn = "Mới",
                        MaNcc = "NCC003",
                        MaPlsp = "PL004",
                        MaHh = $"HH{i:D3}"
                    });
                }
                return newItems.Take(desiredCount - currentCount).ToList();
            }, entities => context.Tainghes.RemoveRange(entities));

            // 8. Seed Laptop
            AdjustTableCount<Laptop>(context, desiredCount, logger, () =>
            {
                var currentCount = context.Laptops.Count();
                var newItems = new List<Laptop>
                {
                    new Laptop { MaLt = "LT001", TenLt = "Dell XPS 13", HangLt = "Dell", GiaLt = 25000000, KichthuocLt = "13.4 inch", RamLt = "16GB", OcungLt = "512GB SSD", XuatxuLt = "Mỹ", NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLt = "Mới", MaNcc = "NCC004", MaPlsp = "PL005", MaHh = "HH033" },
                    new Laptop { MaLt = "LT002", TenLt = "Asus ZenBook 14", HangLt = "Asus", GiaLt = 22000000, KichthuocLt = "14 inch", RamLt = "16GB", OcungLt = "1TB SSD", XuatxuLt = "Đài Loan", NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLt = "Mới", MaNcc = "NCC005", MaPlsp = "PL005", MaHh = "HH034" },
                    new Laptop { MaLt = "LT003", TenLt = "Lenovo ThinkPad X1 Carbon", HangLt = "Lenovo", GiaLt = 30000000, KichthuocLt = "14 inch", RamLt = "32GB", OcungLt = "1TB SSD", XuatxuLt = "Trung Quốc", NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLt = "Mới", MaNcc = "NCC004", MaPlsp = "PL005", MaHh = "HH035" },
                    new Laptop { MaLt = "LT004", TenLt = "HP Spectre x360", HangLt = "HP", GiaLt = 28000000, KichthuocLt = "13.5 inch", RamLt = "16GB", OcungLt = "512GB SSD", XuatxuLt = "Mỹ", NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLt = "Mới", MaNcc = "NCC004", MaPlsp = "PL005", MaHh = "HH036" },
                    new Laptop { MaLt = "LT005", TenLt = "Acer Swift 3", HangLt = "Acer", GiaLt = 18000000, KichthuocLt = "14 inch", RamLt = "8GB", OcungLt = "512GB SSD", XuatxuLt = "Trung Quốc", NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLt = "Mới", MaNcc = "NCC004", MaPlsp = "PL005", MaHh = "HH037" },
                    new Laptop { MaLt = "LT006", TenLt = "MSI Stealth 15M", HangLt = "MSI", GiaLt = 32000000, KichthuocLt = "15.6 inch", RamLt = "16GB", OcungLt = "1TB SSD", XuatxuLt = "Trung Quốc", NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLt = "Mới", MaNcc = "NCC004", MaPlsp = "PL005", MaHh = "HH038" },
                    new Laptop { MaLt = "LT007", TenLt = "Razer Blade 14", HangLt = "Razer", GiaLt = 35000000, KichthuocLt = "14 inch", RamLt = "16GB", OcungLt = "1TB SSD", XuatxuLt = "Mỹ", NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLt = "Mới", MaNcc = "NCC002", MaPlsp = "PL005", MaHh = "HH039" },
                    new Laptop { MaLt = "LT008", TenLt = "Lenovo Legion 5", HangLt = "Lenovo", GiaLt = 27000000, KichthuocLt = "15.6 inch", RamLt = "16GB", OcungLt = "512GB SSD", XuatxuLt = "Trung Quốc", NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)), TinhtrangLt = "Mới", MaNcc = "NCC004", MaPlsp = "PL005", MaHh = "HH040" }
                };
                for (int i = currentCount + 1; i <= desiredCount; i++)
                {
                    newItems.Add(new Laptop
                    {
                        MaLt = $"LT{i:D3}",
                        TenLt = $"Laptop {i}",
                        HangLt = "Generic",
                        GiaLt = 10000000 * i,
                        KichthuocLt = "15 inch",
                        RamLt = "8GB",
                        OcungLt = "512GB SSD",
                        XuatxuLt = "Trung Quốc",
                        NgaysanxuatLt = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)),
                        TinhtrangLt = "Mới",
                        MaNcc = "NCC004",
                        MaPlsp = "PL005",
                        MaHh = $"HH{i:D3}"
                    });
                }
                return newItems.Take(desiredCount - currentCount).ToList();
            }, entities => context.Laptops.RemoveRange(entities));

            logger.LogInformation("Hoàn tất seed dữ liệu.");
        }

        private static void AdjustTableCount<T>(AppDbContext context, int desiredCount, ILogger logger, Func<List<T>> getNewItems, Action<List<T>> removeEntities) where T : class
        {
            var set = context.Set<T>();
            int currentCount = set.Count();

            if (currentCount < desiredCount)
            {
                logger.LogInformation($"Bắt đầu seed dữ liệu cho {typeof(T).Name}...");
                var newItems = getNewItems();
                set.AddRange(newItems);
                context.SaveChanges();
                logger.LogInformation($"Đã thêm {newItems.Count} bản ghi cho {typeof(T).Name}. Tổng: {set.Count()}");
            }
            else if (currentCount > desiredCount)
            {
                logger.LogInformation($"Bắt đầu xóa dữ liệu dư thừa từ {typeof(T).Name}...");
                var toRemove = set.Take(currentCount - desiredCount).ToList();
                removeEntities(toRemove);
                context.SaveChanges();
                logger.LogInformation($"Đã xóa {toRemove.Count} bản ghi từ {typeof(T).Name}. Tổng: {set.Count()}");
            }
        }
    }
}