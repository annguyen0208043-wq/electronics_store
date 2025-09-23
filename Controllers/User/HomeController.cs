using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TechShop.Controllers.User
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult About()
        {
            return View("~/Views/User/Home/About.cshtml");
        }

        public IActionResult Contact()
        {
            return View("~/Views/User/Home/Contact.cshtml");
        }
    }
} 