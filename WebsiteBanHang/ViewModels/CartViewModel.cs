using WebsiteBanHang.Models;

namespace WebsiteBanHang.ViewModels
{
    public class CartViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
