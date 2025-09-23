using System.Collections.Generic;
using System.Linq;
using TechShop.ViewModels;

namespace TechShop.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice => Items.Sum(item => item.Gia * item.SoLuong);
    }
}