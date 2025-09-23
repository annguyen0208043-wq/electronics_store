using System;
using System.Collections.Generic;

namespace TechShop.ViewModels
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        
        // Thông tin khách hàng
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        
        // Chi tiết sản phẩm
        public List<OrderItemViewModel> OrderDetails { get; set; }
    }

    public class OrderItemViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
} 