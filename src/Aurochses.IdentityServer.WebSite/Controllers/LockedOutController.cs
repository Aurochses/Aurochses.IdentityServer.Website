using Aurochses.IdentityServer.WebSite.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Class LockedOutController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class LockedOutController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
