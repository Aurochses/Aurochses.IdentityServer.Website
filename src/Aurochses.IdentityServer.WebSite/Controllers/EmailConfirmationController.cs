using System.Threading.Tasks;
using Aurochses.Identity;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.EmailConfirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class EmailConfirmationController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class EmailConfirmationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailConfirmationController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="emailService">The email service.</param>
        public EmailConfirmationController(UserManager<ApplicationUser> userManager, IEmailService emailService)
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
        public async Task<IActionResult> Index(EmailConfirmationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null) return RedirectToAction("UserNotFound");
                if (await _userManager.IsEmailConfirmedAsync(user)) return RedirectToAction("IsAlreadyConfirmed");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("Confirm", "EmailConfirmation", new { UserId = user.Id, Token = token }, HttpContext.Request.Scheme);

                await _emailService.SendEmailConfirmationTokenAsync(user, callbackUrl);

                return RedirectToAction("EmailSent");
            }

            return View(model);
        }

        /// <summary>
        /// User not found.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult UserNotFound()
        {
            return View();
        }

        /// <summary>
        /// Is already confirmed.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult IsAlreadyConfirmed()
        {
            return View();
        }

        /// <summary>
        /// Email sent.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult EmailSent()
        {
            return View();
        }

        /// <summary>
        /// Confirms the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Confirm(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Success");
            }

            return RedirectToAction("Error");
        }

        /// <summary>
        /// Successes.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Success()
        {
            return View();
        }

        /// <summary>
        /// Error.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}
