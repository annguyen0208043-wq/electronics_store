namespace TechShop.Models
{
    public class HinhAnhSanPham
    {
        public int Id { get; set; }
        public string? MaHh { get; set; }
        public string? DuongDan { get; set; }
        public virtual Hanghoa Hanghoa { get; set; }
    }
}