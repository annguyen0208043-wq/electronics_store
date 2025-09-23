using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechShop.Data;
using TechShop.Models;
using TechShop.ViewModels;
using System.Diagnostics;

namespace TechShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminStatisticsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminStatisticsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Statistics")]
        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate)
        {
            var model = new StatisticsViewModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            // Lấy thống kê doanh thu và số đơn hàng trong khoảng thời gian
            if (startDate.HasValue && endDate.HasValue)
            {
                var orders = await _context.Orders
                    .Where(o => o.OrderDate >= startDate.Value && 
                               o.OrderDate <= endDate.Value)
                    .ToListAsync();

                // Tính tổng doanh thu và số đơn hàng
                var completedOrders = orders.Where(o => o.Status == "Delivered" || o.Status == "Shipped").ToList();
                model.TotalRevenue = completedOrders.Sum(o => o.TotalAmount);
                model.CompletedOrders = completedOrders.Count();
                model.DeliveredOrdersList = completedOrders;
            }

            // Lấy top 5 sản phẩm bán chạy nhất (không phụ thuộc vào thời gian)
            var topProducts = await _context.OrderDetails
                .Include(od => od.Order)
                .Where(od => od.Order.Status == "Delivered" || od.Order.Status == "Shipped")
                .GroupBy(od => new { od.ProductId, od.ProductName })
                .Select(g => new TopProductViewModel
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalSold = g.Sum(od => od.Quantity),
                    TotalRevenue = g.Sum(od => od.Price * od.Quantity)
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(5)
                .ToListAsync();

            model.TopProducts = topProducts;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    totalRevenue = model.TotalRevenue,
                    completedOrders = model.CompletedOrders,
                    deliveredOrdersList = model.DeliveredOrdersList.Select(o => new
                    {
                        orderId = o.OrderId,
                        orderDate = o.OrderDate,
                        userId = o.UserId,
                        totalAmount = o.TotalAmount,
                        status = o.Status
                    }),
                    topProducts = model.TopProducts.Select(p => new
                    {
                        productName = p.ProductName,
                        totalSold = p.TotalSold,
                        totalRevenue = p.TotalRevenue
                    })
                });
            }

            return View("~/Views/Admin/Statistics/Statistics.cshtml", model);
        }

        [HttpGet("GetOrderDetails")]
        public async Task<IActionResult> GetOrderDetails(string orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return PartialView("~/Views/Admin/Statistics/_OrderDetails.cshtml", order);
        }
    }
}
