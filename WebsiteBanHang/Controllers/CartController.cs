using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebsiteBanHang.Models;
using WebsiteBanHang.ViewModels;
using WebsiteBanHang.Helpers;
using WebsiteBanHang.Services;

namespace WebsiteBanHang.Controllers
{
    public class CartController : Controller
    {
        private readonly WebsiteBanHangContext _context;
        private readonly IVnPayService _vnPayService;
        private const string CartSession = "CartSession";

        public CartController(WebsiteBanHangContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var cart = HttpContext.Session.GetString(CartSession);
            var list = new List<CartItem>();

            if (!string.IsNullOrEmpty(cart))
            {
                list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
            }

            var cartViewModel = new CartViewModel
            {
                Menus = menus,
                Blogs = blogs,
                CartItems = list
            };

            return View(cartViewModel);
        }

        public IActionResult AddItem(int ProductId, int Quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            var product = _context.Products.Find(ProductId);
            var cart = HttpContext.Session.GetString(CartSession);
            var list = new List<CartItem>();

            if (!string.IsNullOrEmpty(cart))
            {
                list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                var existingItem = list.FirstOrDefault(x => x.Product.IdPro == ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += Quantity;
                }
                else
                {
                    list.Add(new CartItem { Product = product, Quantity = Quantity });
                }
            }
            else
            {
                list.Add(new CartItem { Product = product, Quantity = Quantity });
            }

            HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(list));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteAll()
        {
            try
            {
                HttpContext.Session.Remove(CartSession);
                return Json(new { status = true, message = "Cart cleared successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error clearing cart: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var cart = HttpContext.Session.GetString(CartSession);
                var list = JsonConvert.DeserializeObject<List<CartItem>>(cart);

                var itemToRemove = list.FirstOrDefault(x => x.Product.IdPro == id);
                if (itemToRemove != null)
                {
                    list.Remove(itemToRemove);
                    HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(list));
                    return Json(new { status = true, message = "Item deleted successfully" });
                }

                return Json(new { status = false, message = "Item not found in cart" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error deleting item: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Payment()
        {
            try
            {
                var cart = HttpContext.Session.GetString(CartSession);
                if (string.IsNullOrEmpty(cart))
                {
                    return Json(new { status = false, message = "Cart is empty" });
                }

                var list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                var totalAmount = list.Sum(item => item.Product.Price.GetValueOrDefault(0) * item.Quantity);

                double amount = (double)totalAmount;

                var paymentRequest = new VnPaymentRequestModel
                {
                    OrderId = new Random().Next(1000, 9999),
                    FullName = User.Identity.Name,
                    Description = "Payment for order",
                    Amount = amount,
                    CreatedDate = DateTime.Now
                };

                var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest);

                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error processing payment: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response.Success)
            {
                var cart = HttpContext.Session.GetString(CartSession);
                if (string.IsNullOrEmpty(cart))
                {
                    // Xử lý trường hợp giỏ hàng rỗng
                    return RedirectToAction("Index");
                }

                var list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                if (list == null || !list.Any())
                {
                    // Xử lý trường hợp giỏ hàng rỗng
                    return RedirectToAction("Index");
                }

                var userIdClaim = User.FindFirst("CustomerID");
                if (userIdClaim == null)
                {
                    // Xử lý trường hợp thiếu CustomerID claim
                    return RedirectToAction("Login", "User");
                }

                int userId;
                if (!int.TryParse(userIdClaim.Value, out userId))
                {
                    // Xử lý trường hợp CustomerID claim không hợp lệ
                    return RedirectToAction("Login", "User");
                }

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = list.Sum(item => item.Product.Price.GetValueOrDefault(0) * item.Quantity),
                    OrderDetails = list.Select(item => new OrderDetail
                    {
                        ProductId = item.Product.IdPro,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price.GetValueOrDefault(0)
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove(CartSession);
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("PaymentFail");
            }
        }


        public IActionResult Success()
        {
            return View();
        }

        public IActionResult PaymentFail()
        {
            return View();
        }
    }
}
