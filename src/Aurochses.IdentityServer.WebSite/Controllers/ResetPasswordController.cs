using System.Threading.Tasks;
using Aurochses.Identity.EntityFramework;
using Aurochses.Identity.Mvc;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.ResetPassword;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class ResetPasswordController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class ResetPasswordController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public ResetPasswordController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Indexes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>IActionResult.</returns>
        public IActionResult Index(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return RedirectToAction("Error");

            return View();
        }

        /// <summary>
        /// Indexes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateRecaptcha]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Error");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Success");
            }

            this.AddErrors(result);
            return View();
        }

        /// <summary>
        /// Successes this instance.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Success()
        {
            return View();
        }

        /// <summary>
        /// Errors this instance.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}