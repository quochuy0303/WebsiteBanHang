using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebsiteBanHang.Controllers
{
    public class AdminController : Controller
    {
        public async Task<IActionResult> Admin()
        {
            if (User.Identity.IsAuthenticated)
            {
                string permission = User.FindFirst(ClaimTypes.Role)?.Value;
                if (permission == "1")
                {
                    return View("Admin");
                }
            }

            // Người dùng không có quyền hoặc chưa xác thực, hiển thị view thông báo không có quyền truy cập
            return View("AccessDenied");
        }

        public IActionResult _MenuPartial()
        {
            return PartialView();
        }

        public IActionResult _BlogPartial()
        {
            return PartialView();
        }
    }
}
