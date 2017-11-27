using System.Security.Claims;
using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.AspNetCore.Mvc.Helpers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Helpers;
using Aurochses.IdentityServer.WebSite.Models.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class ExternalLoginController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class ExternalLoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalLoginController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="emailService">The email service.</param>
        public ExternalLoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        /// <summary>
        /// Indexes the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("Callback", "ExternalLogin", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        /// <summary>
        /// Callbacks the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="remoteError">The remote error.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return RedirectToAction("Index", "SignIn");
            }

            // Get the information about the user from the external login provider
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return RedirectToAction("Index", "SignIn");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    await _signInManager.SignOutAsync();

                    return RedirectToAction("Index", "EmailConfirmation");
                }

                // Update any authentication tokens if login succeeded
                await _signInManager.UpdateExternalAuthenticationTokensAsync(externalLoginInfo);

                return this.RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("SendCode", "TwoFactorSignIn", new { ReturnUrl = returnUrl, RememberMe = false });
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Index", "LockedOut");
            }

            // If the user does not have an account, then ask the user to create an account.
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = externalLoginInfo.LoginProvider;

            var registrationViewModel = new RegistrationViewModel
            {
                Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                FirstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname)
            };

            return View("Registration", registrationViewModel);
        }

        /// <summary>
        /// Registrations the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateRecaptcha]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    return RedirectToAction("Index", "SignIn");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, externalLoginInfo);

                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("Confirm", "EmailConfirmation", new { UserId = user.Id, Token = token }, HttpContext.Request.Scheme);

                        await _emailService.SendEmailConfirmationTokenAsync(user, callbackUrl);

                        // Update any authentication tokens as well
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(externalLoginInfo);

                        return RedirectToAction("EmailSent", "Registration", new { ReturnUrl = returnUrl });
                    }
                }

                this.AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }
    }
}