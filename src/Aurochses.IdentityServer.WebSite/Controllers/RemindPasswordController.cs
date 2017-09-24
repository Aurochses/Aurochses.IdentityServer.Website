using System.Threading.Tasks;
using Aurochses.Identity;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.RemindPassword;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class RemindPasswordController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class RemindPasswordController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemindPasswordController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="emailService">The email service.</param>
        public RemindPasswordController(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Index()
        {
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
        public async Task<IActionResult> Index(RemindPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null) return RedirectToAction("UserNotFound");

                if (await _userManager.IsEmailConfirmedAsync(user) == false)
                {
                    return RedirectToAction("Index", "EmailConfirmation");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("Index", "ResetPassword", new { UserId = user.Id, Token = token }, HttpContext.Request.Scheme);

                await _emailService.SendPasswordResetTokenAsync(user, callbackUrl);

                return RedirectToAction("EmailSent");
            }

            return View(model);
        }

        /// <summary>
        /// Users the not found.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult UserNotFound()
        {
            return View();
        }

        /// <summary>
        /// Emails the sent.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult EmailSent()
        {
            return View();
        }
    }
}
