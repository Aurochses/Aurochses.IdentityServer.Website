using Aurochses.IdentityServer.WebSite.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class HomeController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    /// <remarks>
    /// https://github.com/IdentityServer/IdentityServer4.Samples/tree/f189416024a15820b1f474dccdc48c73185559f4/Quickstarts/6_AspNetIdentity/src/IdentityServerWithAspNetIdentity
    /// </remarks>
    [SecurityHeaders]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>IActionResult.</returns>
        public IActionResult Index(string returnUrl = null)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                return View();
            }

            return RedirectToAction("Index", "SignIn", new { ReturnUrl = returnUrl });
        }
    }
}
