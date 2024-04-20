using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Interface;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repository;
using WebsiteBanHang.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly WebsiteBanHangContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(WebsiteBanHangContext context ,IProductRepository productRepository,
        ICategoryRepository categoryRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            // Tính chỉ số bắt đầu và số lượng sản phẩm cần lấy
            int startIndex = (page - 1) * pageSize;
            var prods = await _context.Products
                .Where(m => m.Hide == 0)
                .OrderBy(m => m.Order)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            // Tính tổng số trang dựa trên số lượng sản phẩm và pageSize
            int totalItems = await _context.Products.Where(m => m.Hide == 0).CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
                TotalPages = totalPages,
                CurrentPage = page
            };

            return View(viewModel);
        }


        public async Task<IActionResult> _MenuPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _BlogPartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> CateProd(string slug, long id)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var cateProds = await _context.Catologies
            .Where(cp => cp.IdCat == id && cp.Link == slug).FirstOrDefaultAsync();
            if (cateProds == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "CateProd Error",
                };
                return View("Error", errorViewModel);
            }
            var prods = await _context.Products.Where(m => m.Hide == 0 && m.IdCat == cateProds.IdCat)
.OrderBy(m => m.Order).ToListAsync();
            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
                cateName = cateProds.NameCat,
            };
            return View(viewModel);
        }

        public async Task<IActionResult> ProdDetail(string slug, long id)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var prods = await _context.Products.Where(m => m.Link == slug && m.IdPro ==
            id).ToListAsync();
            if (prods == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "Product Error",
                };
                return View("Error", errorViewModel);
            }
            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
            };
            return View(viewModel);
        }


        // GET: /Product/Add
        public async Task<IActionResult> Add()
        {
            // Kiểm tra xem tài khoản có claim "Permission" không

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "IdCat", "NameCat");
            return View();
        }

        // POST: /Product/Add
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid)
            {
                // Thực hiện thêm sản phẩm vào cơ sở dữ liệu
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, hiển thị lại form với dữ liệu đã nhập
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "IdCat", "NameCat");
            return View(product);
        }

        // GET: /Product/Update/1
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Trả về trang 404 nếu không tìm thấy sản phẩm
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "IdCat", "NameCat");

            return View(product);
        }

        // POST: /Product/Update/1
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product updatedProduct)
        {
            if (id != updatedProduct.IdPro)
            {
                return NotFound(); // Trả về trang 404 nếu id không khớp
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateAsync(updatedProduct);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Xử lý ngoại lệ khi xảy ra lỗi trong quá trình cập nhật
                    return RedirectToAction(nameof(Error));
                }
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "IdCat", "NameCat");

            return View(updatedProduct);
        }

        // Action hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Action xác nhận xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
