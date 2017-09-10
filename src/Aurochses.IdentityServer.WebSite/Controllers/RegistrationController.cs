using System.Threading.Tasks;
using Aurochses.Identity;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Aurochses.IdentityServer.WebSite.Models.Registration;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class RegistrationController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class RegistrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="emailService">The email service.</param>
        public RegistrationController(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

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

        /// <summary>
        /// Validates the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>Task&lt;JsonResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ValidateEmail(string email)
        {
            var user = await _userManager.FindByNameAsync(email);

            return Json(user == null);
        }

        /// <summary>
        /// Indexes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateRecaptcha]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegistrationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("Confirm", "EmailConfirmation", new { UserId = user.Id, Token = token }, HttpContext.Request.Scheme);

                    await _emailService.SendEmailConfirmationTokenAsync(user, callbackUrl);

                    return RedirectToAction("EmailSent", "Registration", new { ReturnUrl = returnUrl });
                }

                this.AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        /// <summary>
        /// Emails the sent.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>IActionResult.</returns>
        public IActionResult EmailSent(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
    }
}
