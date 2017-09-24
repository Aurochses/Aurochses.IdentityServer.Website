using System.Threading.Tasks;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.App.Project;
using Aurochses.IdentityServer.WebSite.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class UserController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    [SecurityHeaders]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProjectOptions _projectOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="projectOptions">The project options.</param>
        public UserController(UserManager<ApplicationUser> userManager, IOptions<ProjectOptions> projectOptions)
        {
            _userManager = userManager;
            _projectOptions = projectOptions.Value;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            if (!string.IsNullOrWhiteSpace(_projectOptions.AccountUrl))
            {
                return Redirect(_projectOptions.AccountUrl);
            }

            return View(user);
        }
    }
}