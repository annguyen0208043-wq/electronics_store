using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TechShop.Data;
using TechShop.Models;
using TechShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TechShop.Controllers.User
{
    [Authorize]
    [Route("User")]
    public class OrderTrackingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderTrackingController> _logger;

        public OrderTrackingController(AppDbContext context, ILogger<OrderTrackingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("OrderTracking")]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.UserId == username)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    OrderDetails = o.OrderDetails.Select(od => new OrderItemViewModel
                    {
                        ProductId = od.ProductId,
                        ProductName = od.ProductName,
                        Price = od.Price,
                        Quantity = od.Quantity,
                        ImageUrl = od.ImageUrl
                    }).ToList()
                })
                .ToListAsync();

            return View("~/Views/User/OrderTracking/Index.cshtml", orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddToOrderTracking([FromBody] OrderTrackingRequest request)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để theo dõi đơn hàng" });
                }

                var username = User.Identity.Name;
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.OrderId == request.OrderId && o.UserId == username);

                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                // Cập nhật thông tin đơn hàng nếu cần
                order.Status = "Pending";
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã thêm đơn hàng vào theo dõi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm đơn hàng vào OrderTracking");
                return Json(new { success = false, message = "Có lỗi xảy ra khi thêm đơn hàng vào theo dõi" });
            }
        }

        public class OrderTrackingRequest
        {
            public string OrderId { get; set; }
            public List<OrderDetailRequest> OrderDetails { get; set; }
            public decimal TotalAmount { get; set; }
        }

        public class OrderDetailRequest
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string ImageUrl { get; set; }
        }
    }
} 