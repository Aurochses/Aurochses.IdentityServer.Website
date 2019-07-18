using System;
using System.Linq;
using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.Website.Extensions;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.IdentityServer.Website.Models.Login;
using Aurochses.IdentityServer.Website.Models.Shared;
using Aurochses.IdentityServer.Website.Options;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.Website.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILogger _logger;
        private readonly AccountOptions _accountOptions;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
        private readonly IClientStore _clientStore;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventService _eventService;

        public LoginController(
            ILogger<LoginController> logger,
            IOptions<AccountOptions> accountOptions,
            IIdentityServerInteractionService identityServerInteractionService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IClientStore clientStore,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEventService eventService
        )
        {
            _logger = logger;
            _accountOptions = accountOptions.Value;
            _identityServerInteractionService = identityServerInteractionService;
            _authenticationSchemeProvider = authenticationSchemeProvider;
            _clientStore = clientStore;
            _signInManager = signInManager;
            _userManager = userManager;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var viewModel = await BuildLoginViewModel(returnUrl);

            if (viewModel.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                _logger.LogInformation("We only have one option for logging in and it's an external provider.");
                return RedirectToAction("Challenge", "External", new { Provider = viewModel.ExternalLoginScheme, ReturnUrl = returnUrl });
            }

            return View(viewModel);
        }

        private async Task<LoginViewModel> BuildLoginViewModel(string returnUrl)
        {
            var context = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                // this is meant to short circuit the UI and only trigger the one external IdP
                return new LoginViewModel
                {
                    UserName = context.LoginHint,
                    ReturnUrl = returnUrl,

                    EnableLocalLogin = false,

                    ExternalProviders = new []
                    {
                        new ExternalProvider
                        {
                            AuthenticationScheme = context.IdP
                        }
                    }
                };
            }

            var schemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(
                    x => x.DisplayName != null
                         || x.Name.Equals(_accountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase)
                )
                .Select(
                    x => new ExternalProvider
                    {
                        DisplayName = x.DisplayName,
                        AuthenticationScheme = x.Name
                    }
                )
                .ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                UserName = context?.LoginHint,
                ReturnUrl = returnUrl,

                AllowRememberLogin = _accountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && _accountOptions.AllowLocalLogin,

                ExternalProviders = providers.ToArray()
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _identityServerInteractionService.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _identityServerInteractionService.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await _clientStore.IsPkceClient(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return RedirectToAction("Index", "Home");
                }
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberLogin, _accountOptions.LockoutOnFailure);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (_accountOptions.RequireEmailConfirmation && !await _userManager.IsEmailConfirmedAsync(user))
                    {
                        await _signInManager.SignOutAsync();

                        return RedirectToAction("Index", "EmailConfirmation");
                    }

                    await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClient(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("Invalid return URL");
                    }
                }
                if (_accountOptions.AllowTwoFactorAuthentication && result.RequiresTwoFactor)
                {
                    return RedirectToAction("SendCode", "TwoFactorLogin", new { model.ReturnUrl, model.RememberLogin });
                }
                if (_accountOptions.LockoutOnFailure && result.IsLockedOut)
                {
                    return RedirectToAction("Index", "LockedOut");
                }

                await _eventService.RaiseAsync(new UserLoginFailureEvent(model.UserName, "Invalid credentials"));

                ModelState.AddModelError("InvalidCredentials", string.Empty);
            }

            // something went wrong, show form with error
            var viewModel = await BuildLoginViewModel(model);

            return View(viewModel);
        }

        private async Task<LoginViewModel> BuildLoginViewModel(LoginInputModel model)
        {
            var viewModel = await BuildLoginViewModel(model.ReturnUrl);

            viewModel.UserName = model.UserName;
            viewModel.RememberLogin = model.RememberLogin;

            return viewModel;
        }
    }
}
