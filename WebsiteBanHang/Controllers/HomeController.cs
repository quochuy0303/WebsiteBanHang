using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebsiteBanHang.Models;
using WebsiteBanHang.ViewModels;


namespace WebsiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebsiteBanHangContext _context;
        
        public HomeController(WebsiteBanHangContext context)
        {
            _context = context;           
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();          
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>m.Order).Take(2).ToListAsync();
            var slides = await _context.Sliders.Where(m => m.Hide == 0).OrderBy(m =>
m.Order).ToListAsync();
            var cat_prods = await _context.Products.Where(m => m.Hide == 0 && m.IdCat ==
2).OrderBy(m => m.Order).Take(3).ToListAsync();
            var cat_cate_prods = await _context.Catologies.Where(m => m.IdCat ==
            2).FirstOrDefaultAsync();
            var dog_prods = await _context.Products.Where(m => m.Hide == 0 && m.IdCat ==
1).OrderBy(m => m.Order).Take(3).ToListAsync();
            var dog_cate_prods = await _context.Catologies.Where(m => m.IdCat ==
            1).FirstOrDefaultAsync();
            var viewModel = new HomeViewModel
            {
                Menus = menus,         
                Blogs = blogs,
                Sliders = slides,
                CatProds = cat_prods,
                DogProds = dog_prods,
                CatCateProds = cat_cate_prods,
                DogCateProds = dog_cate_prods,
            };
            return View(viewModel);
        }
        
        public async Task<IActionResult> _BlogPartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> _MenuPartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> _SlidePartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _ProductPartial()
        {
            return PartialView();
        }     
    }
}
