using System;
using System.Linq;
using System.Threading.Tasks;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.IdentityServer.Website.Models.Login;
using Aurochses.IdentityServer.Website.Options;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        public LoginController(
            ILogger<LoginController> logger,
            IOptions<AccountOptions> accountOptions,
            IIdentityServerInteractionService identityServerInteractionService,
            IAuthenticationSchemeProvider authenticationSchemeProvider,
            IClientStore clientStore
        )
        {
            _logger = logger;
            _accountOptions = accountOptions.Value;
            _identityServerInteractionService = identityServerInteractionService;
            _authenticationSchemeProvider = authenticationSchemeProvider;
            _clientStore = clientStore;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginInputModel model, string button)
        {
            throw new NotImplementedException();
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
    }
}
