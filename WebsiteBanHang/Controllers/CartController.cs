using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebsiteBanHang.Models;
using WebsiteBanHang.ViewModels;
namespace WebsiteBanHang.Controllers
{
    public class CartController : Controller
    {
        private readonly WebsiteBanHangContext _context;
        private const string CartSession = "CartSession";
        public CartController(WebsiteBanHangContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
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
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "User");
            }
            var product = _context.Products.Find(ProductId);
            var cart = HttpContext.Session.GetString(CartSession);
            if (!string.IsNullOrEmpty(cart))
            {
                var list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
                if (list.Exists(x => x.Product.IdPro == ProductId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.IdPro == ProductId)
                        {
                            item.Quantity += Quantity;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = Quantity;
                    list.Add(item);
                }
                HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(list));
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = Quantity;
                var list = new List<CartItem>();
                list.Add(item);
                HttpContext.Session.SetString(CartSession, JsonConvert.SerializeObject(list));
            }
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
                // Perform payment logic here (calculate total, save order to database, etc.)

                // Assume payment logic is successful
                // Clear cart after successful payment
                HttpContext.Session.Remove(CartSession);

                // Return JSON response indicating success
                return Json(new { status = true, message = "Payment successful. Your order has been placed." });
            }
            catch (Exception ex)
            {
                // Return JSON response indicating failure with error message
                return Json(new { status = false, message = "Error processing payment: " + ex.Message });
            }
        }

    }
}