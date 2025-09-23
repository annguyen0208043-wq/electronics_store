using TechShop.Models;

namespace TechShop.ViewModels
{
    public class CartItem
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string HinhAnh { get; set; }
        public Hanghoa Product { get; set; }
    }
}
