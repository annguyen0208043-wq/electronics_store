namespace TechShop.ViewModels

{
    public class HomeViewModel
    {
        public List<ProductViewModel> NewProducts { get; set; } = new List<ProductViewModel>();
        public List<ProductViewModel> PopularProducts { get; set; } = new List<ProductViewModel>();
        public List<TopProductViewModel> TopProducts { get; set; } = new List<TopProductViewModel>();
    }
}
