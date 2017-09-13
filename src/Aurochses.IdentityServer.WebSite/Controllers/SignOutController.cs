using System;
using System.Threading.Tasks;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.SignOut;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4;
using Microsoft.AspNetCore.Http.Authentication;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Class SignOutController.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class SignOutController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignOutController"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="identityServerInteractionService">The identity server interaction service.</param>
        public SignOutController(SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService identityServerInteractionService)
        {
            _signInManager = signInManager;
            _identityServerInteractionService = identityServerInteractionService;
        }

        /// <summary>
        /// Indexes the specified logout identifier.
        /// </summary>
        /// <param name="logoutId">The logout identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Index(string logoutId)
        {
            var logoutRequest = await _identityServerInteractionService.GetLogoutContextAsync(logoutId);

            var model = new SignedOutViewModel
            {
                PostLogoutRedirectUri = logoutRequest?.PostLogoutRedirectUri,
                ClientName = logoutRequest?.ClientId,
                SignOutIframeUrl = logoutRequest?.SignOutIFrameUrl,
                AutomaticRedirectAfterSignOut = true,
                LogoutId = logoutId
            };

            var user = await HttpContext.GetIdentityServerUserAsync();

            var identityProvider = user?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (identityProvider != null && identityProvider != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    model.LogoutId = await _identityServerInteractionService.CreateLogoutContextAsync();
                }

                model.ExternalAuthenticationScheme = identityProvider;
            }

            if (model.TriggerExternalSignout)
            {
                try
                {
                    // hack: try/catch to handle social providers that throw
                    await HttpContext.Authentication.SignOutAsync(
                        model.ExternalAuthenticationScheme,
                        new AuthenticationProperties
                        {
                            // ReSharper disable once RedundantAnonymousTypePropertyName
                            RedirectUri = Url.Action("Index", "SignOut", new {LogoutId = model.LogoutId})
                        }
                    );
                }
                catch (NotSupportedException) // this is for the external providers that don't have signout
                {

                }
                catch (InvalidOperationException) // this is for Windows/Negotiate
                {

                }
            }

            // delete authentication cookie
            await _signInManager.SignOutAsync();

            return View("SignedOut", model);
        }
    }
}