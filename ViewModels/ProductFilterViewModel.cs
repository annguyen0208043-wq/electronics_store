using System.Collections.Generic;

namespace TechShop.ViewModels
{
    public class ProductFilterViewModel
    {
        public List<string> SelectedCategories { get; set; } = new List<string>();
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 5000000;
    }
} 