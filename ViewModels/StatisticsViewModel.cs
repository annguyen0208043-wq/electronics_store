using System;
using System.Collections.Generic;
using TechShop.ViewModels;
using TechShop.Models;

namespace TechShop.ViewModels
{
    public class StatisticsViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int CompletedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public List<TopProductViewModel> TopProducts { get; set; } = new List<TopProductViewModel>();
        public List<Order> DeliveredOrdersList { get; set; } = new List<Order>();
    }

    public class TopProductViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}