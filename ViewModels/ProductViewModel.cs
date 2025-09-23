namespace TechShop.ViewModels
{
    public class ProductViewModel
    {
        public string? Id { get; set; } // Đánh dấu nullable
        public string? Ten { get; set; } // Đánh dấu nullable
        public int Gia { get; set; }
        public string? Loai { get; set; } // Đánh dấu nullable
        public int Soluongton { get; set; }
        public string? MoTa { get; set; } // Đánh dấu nullable
        public int SoLuongBan { get; set; }
        public List<string> HinhAnh { get; set; } = new List<string>();
        public string? MaNcc { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayXoa { get; set; }
        // Thêm thuộc tính FormattedPrice để định dạng giá
        public string FormattedPrice => Gia.ToString("N0") + " VNĐ";
    }
}