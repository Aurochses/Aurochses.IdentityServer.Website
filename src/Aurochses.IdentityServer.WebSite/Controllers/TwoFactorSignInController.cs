using System.Linq;
using System.Threading.Tasks;
using Aurochses.Identity;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.TwoFactorSignIn;
using Aurochses.Mvc.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class TwoFactorSignInController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class TwoFactorSignInController : Controller
    {
        private readonly IStringLocalizer<TwoFactorSignInController> _controllerLocalization;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoFactorSignInController" /> class.
        /// </summary>
        /// <param name="controllerLocalization">The controller localization.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="smsService">The SMS service.</param>
        public TwoFactorSignInController(IStringLocalizer<TwoFactorSignInController> controllerLocalization, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService, ISmsService smsService)
        {
            _controllerLocalization = controllerLocalization;

            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _smsService = smsService;
        }

        /// <summary>
        /// Sends the code.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction("Error");
            }

            var twoFactorProviders = await _userManager.GetValidTwoFactorProvidersAsync(user);

            var twoFactorProvidersList = twoFactorProviders
                .Select(
                    twoFactorProvider => new SelectListItem
                    {
                        Value = twoFactorProvider,
                        Text = _controllerLocalization[twoFactorProvider]
                    }
                )
                .ToList();

            return View(new SendCodeViewModel { Providers = twoFactorProvidersList, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// Sends the code.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction("Error");
            }

            // Generate the token and send it
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Error");
            }

            switch (model.SelectedProvider)
            {
                case "Email":
                    await _emailService.SendTwoFactorTokenAsync(user, token);
                    break;
                case "Phone":
                    await _smsService.SendTwoFactorTokenAsync(user, token);
                    break;
                default:
                    return RedirectToAction("Error");
            }

            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        /// <summary>
        /// Verifies the code.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction("Error");
            }

            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// Verifies the code.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Token, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return this.RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Index", "LockedOut");
            }

            ModelState.AddModelError("InvalidCode", string.Empty);

            return View(model);
        }

        /// <summary>
        /// Errors this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}
