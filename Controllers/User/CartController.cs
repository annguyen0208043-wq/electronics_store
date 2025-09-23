using Microsoft.AspNetCore.Mvc;
using TechShop.Models;
using TechShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using TechShop.Data;
using System;
using System.Security.Claims;

namespace TechShop.Controllers.User
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(AppDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Lấy giỏ hàng từ Session
        private CartViewModel GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson)
                ? new CartViewModel()
                : JsonConvert.DeserializeObject<CartViewModel>(cartJson);
        }

        // Lưu giỏ hàng vào Session
        private void SaveCart(CartViewModel cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult AddToCart(string id, int quantity = 1)
        {
            var product = _context.Hanghoas
                .Include(h => h.HinhAnhSanPhams)
                .FirstOrDefault(h => h.MaHh == id);

            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }

            if (product.Soluongton <= 0)
            {
                return Json(new { success = false, message = "Sản phẩm đã hết hàng!" });
            }

            var cart = GetCart();
            var cartItem = cart.Items.FirstOrDefault(item => item.Id == id);

            if (cartItem != null)
            {
                if (cartItem.SoLuong + quantity <= product.Soluongton)
                {
                    cartItem.SoLuong += quantity;
                }
                else
                {
                    return Json(new { success = false, message = $"Số lượng vượt quá tồn kho ({product.Soluongton})!" });
                }
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    Id = product.MaHh,
                    Ten = product.TenHh,
                    Gia = product.GiaHh,
                    SoLuong = quantity,
                    HinhAnh = product.HinhAnhSanPhams?.FirstOrDefault()?.DuongDan ?? "default.jpg"
                });
            }

            SaveCart(cart);
            return Json(new
            {
                success = true,
                message = "Đã thêm sản phẩm vào giỏ hàng!",
                itemCount = cart.Items.Sum(item => item.SoLuong)
            });
        }

        // Xem giỏ hàng
        public IActionResult Index()
        {
            var cart = GetCart();
            return View("~/Views/User/Cart/Index.cshtml", cart);
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public IActionResult UpdateQuantity(string id, int quantity)
        {
            try
            {
                var cart = GetCart();
                var cartItem = cart.Items.FirstOrDefault(item => item.Id == id);
                var product = _context.Hanghoas.FirstOrDefault(h => h.MaHh == id);

                if (cartItem == null || product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
                }

                if (quantity <= 0)
                {
                    return Json(new { success = false, message = "Số lượng phải lớn hơn 0!" });
                }

                if (quantity <= product.Soluongton)
                {
                    cartItem.SoLuong = quantity;
                    SaveCart(cart);

                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật số lượng!",
                        subtotal = (cartItem.Gia * quantity).ToString("N0") + " đ",
                        total = cart.TotalPrice.ToString("N0") + " đ",
                        itemCount = cart.Items.Sum(item => item.SoLuong)
                    });
                }
                else
                {
                    return Json(new { success = false, message = $"Số lượng vượt quá tồn kho ({product.Soluongton})!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật giỏ hàng!" });
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public IActionResult RemoveItem(string id)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.Id == id);
            
            if (item != null)
            {
                cart.Items.Remove(item);
                SaveCart(cart);
                return Json(new { 
                    success = true, 
                    message = "Đã xóa sản phẩm khỏi giỏ hàng!",
                    total = cart.TotalPrice.ToString("N0") + " đ",
                    itemCount = cart.Items.Sum(i => i.SoLuong)
                });
            }

            return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng!" });
        }

        // Đặt hàng
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để đặt hàng" });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy Username từ claim
                var username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin người dùng" });
                }
                
                if (request?.Items == null || !request.Items.Any())
                {
                    return Json(new { success = false, message = "Giỏ hàng trống" });
                }

                var totalAmount = 0m;
                var order = new Order
                {
                    OrderId = Guid.NewGuid().ToString("N"),
                    UserId = username,
                    OrderDate = DateTime.Now,
                    TotalAmount = request.Items.Sum(item => item.Quantity * _context.Hanghoas.FirstOrDefault(h => h.MaHh == item.Id)?.GiaHh ?? 0),
                    Status = "Pending"
                };

                var orderDetails = new List<object>();

                // Kiểm tra và tạo chi tiết đơn hàng
                foreach (var item in request.Items)
                {
                    var product = await _context.Hanghoas
                        .Include(h => h.HinhAnhSanPhams)
                        .FirstOrDefaultAsync(h => h.MaHh == item.Id);

                    if (product == null)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = $"Không tìm thấy sản phẩm với mã {item.Id}" });
                    }

                    if (item.Quantity <= 0)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = "Số lượng sản phẩm phải lớn hơn 0" });
                    }

                    // Kiểm tra số lượng tồn kho
                    if (product.Soluongton < item.Quantity)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = $"Sản phẩm {product.TenHh} chỉ còn {product.Soluongton} sản phẩm" });
                    }

                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = product.MaHh,
                        ProductName = product.TenHh,
                        Price = product.GiaHh,
                        Quantity = item.Quantity,
                        ImageUrl = product.HinhAnhSanPhams?.FirstOrDefault()?.DuongDan ?? "default.jpg"
                    };

                    order.OrderDetails.Add(orderDetail);
                    totalAmount += product.GiaHh * item.Quantity;

                    // Thêm thông tin chi tiết vào danh sách để trả về
                    orderDetails.Add(new
                    {
                        productId = product.MaHh,
                        productName = product.TenHh,
                        price = product.GiaHh,
                        quantity = item.Quantity,
                        imageUrl = orderDetail.ImageUrl
                    });

                    // Cập nhật số lượng tồn kho
                    product.Soluongton -= item.Quantity;
                }

                order.TotalAmount = totalAmount;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Xóa giỏ hàng sau khi đặt hàng thành công
                HttpContext.Session.Remove("Cart");

                return Json(new 
                { 
                    success = true, 
                    message = "Đặt hàng thành công", 
                    orderId = order.OrderId,
                    orderDetails = orderDetails,
                    totalAmount = totalAmount,
                    redirectUrl = Url.Action("Index", "OrderTracking", new { area = "User" })
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi khi tạo đơn hàng");
                return Json(new { success = false, message = "Có lỗi xảy ra khi đặt hàng" });
            }
        }

        // Lấy số lượng sản phẩm trong giỏ hàng
        public IActionResult GetCount()
        {
            var cart = GetCart();
            return Json(cart.Items.Sum(item => item.SoLuong));
        }
    }

    public class CreateOrderRequest
    {
        public List<OrderItemRequest> Items { get; set; }
    }

    public class OrderItemRequest
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}