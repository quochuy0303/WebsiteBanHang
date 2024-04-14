using WebsiteBanHang.Models;

namespace WebsiteBanHang.ViewModels
{
    public class ProductViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Product> Prods { get; set; }
        public string cateName { get; set; }
        public int TotalPages { get; set; } // Tổng số trang
        public int CurrentPage { get; set; } // Trang hiện tại
    }
}
