using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
