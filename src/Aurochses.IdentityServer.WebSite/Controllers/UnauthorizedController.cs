using Aurochses.IdentityServer.WebSite.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class UnauthorizedController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class UnauthorizedController : Controller
    {
        /// <summary>
        /// Indexes the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>IActionResult.</returns>
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
    }
}