using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.SignOut;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
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

            var user = HttpContext.User;

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
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                // ReSharper disable once RedundantAnonymousTypePropertyName
                var url = Url.Action("Index", "SignOut", new { LogoutId = model.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                // hack: try/catch to handle social providers that throw
                return SignOut(new AuthenticationProperties { RedirectUri = url }, model.ExternalAuthenticationScheme);
            }

            // delete authentication cookie
            await _signInManager.SignOutAsync();

            return View("SignedOut", model);
        }
    }
}