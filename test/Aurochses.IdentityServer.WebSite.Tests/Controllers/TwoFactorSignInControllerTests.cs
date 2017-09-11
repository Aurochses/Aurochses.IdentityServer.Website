using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurochses.Identity.EntityFramework;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.TwoFactorSignIn;
using Aurochses.Testing;
using Aurochses.Testing.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class TwoFactorSignInControllerTests
    {
        private readonly Mock<IStringLocalizer<TwoFactorSignInController>> _mockControllerLocalization;

        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ISmsService> _mockSmsService;

        private readonly TwoFactorSignInController _controller;

        public TwoFactorSignInControllerTests()
        {
            _mockControllerLocalization = new Mock<IStringLocalizer<TwoFactorSignInController>>(MockBehavior.Strict);

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(_mockUserManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<ApplicationUser>>>().Object);
            _mockEmailService = new Mock<IEmailService>(MockBehavior.Strict);
            _mockSmsService = new Mock<ISmsService>(MockBehavior.Strict);

            _controller = new TwoFactorSignInController(_mockControllerLocalization.Object, _mockUserManager.Object, _mockSignInManager.Object, _mockEmailService.Object, _mockSmsService.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<TwoFactorSignInController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Fact]
        public async Task SendCode_RedirectToActionResult_WhenUserIsNull()
        {
            // Arrange
            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await _controller.SendCode();

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("http://www.example.com", true)]
        public async Task SendCode_ReturnViewResultWithSendCodeViewModel_WhenUserIsNotNull(string returnUrl, bool rememberMe)
        {
            // Arrange
            var user = new ApplicationUser();
            var twoFactorProviders = new Dictionary<string, string>()
            {
                { "Email", "EmailLocalizedValue" },
                { "Phone", "PhoneLocalizedValue" }
            };

            foreach (var twoFactorProvider in twoFactorProviders)
            {
                _mockControllerLocalization
                    .Setup(x => x[twoFactorProvider.Key])
                    .Returns(() => new LocalizedString(twoFactorProvider.Key, twoFactorProvider.Value));
            }

            var viewModel = new SendCodeViewModel
            {
                Providers = twoFactorProviders
                    .Select(
                        twoFactorProvider => new SelectListItem
                        {
                            Value = twoFactorProvider.Key,
                            Text = _mockControllerLocalization.Object[twoFactorProvider.Key]
                        }
                    )
                    .ToList(),
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            };

            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => user)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.GetValidTwoFactorProvidersAsync(user))
                .ReturnsAsync(() => twoFactorProviders.Select(x => x.Key).ToList())
                .Verifiable();

            // Act
            var actionResult = await _controller.SendCode(returnUrl, rememberMe);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();

            MvcAssert.ViewResult(actionResult, null, viewModel);
        }

        [Theory]
        [InlineData(typeof(HttpPostAttribute))]
        [InlineData(typeof(ValidateAntiForgeryTokenAttribute))]
        public void SendCodePost_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.MethodHasAttribute<TwoFactorSignInController>("SendCode", new[] { typeof(SendCodeViewModel) }, attributeType);
        }

        private static SendCodeViewModel GetSendCodeViewModel(string selectedProvider, string returnUrl, bool rememberMe)
        {
            return new SendCodeViewModel
            {
                Providers = new List<SelectListItem>
                {
                    new SelectListItem {Value = "Email", Text = "Email"},
                    new SelectListItem {Value = "Phone", Text = "Phone"},
                },
                SelectedProvider = selectedProvider,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            };
        }

        [Fact]
        public async Task SendCodePost_ReturnViewResult_WhenModelStateIsNotValid()
        {
            // Arrange
            const string modelStateKey = "SelectedProvider";
            const string modelStateErrorMessage = "Required";

            var viewModel = GetSendCodeViewModel(null, null, false);

            _controller.ModelState.AddModelError(modelStateKey, modelStateErrorMessage);

            // Act
            var actionResult = await _controller.SendCode(viewModel);

            // Assert
            var viewResult = MvcAssert.ViewResult(actionResult);

            MvcAssert.ModelState(viewResult, modelStateKey, modelStateErrorMessage);
        }

        [Fact]
        public async Task SendCodePost_ReturnRedirectToActionResult_WhenUserIsNull()
        {
            // Arrange
            var viewModel = GetSendCodeViewModel(null, null, false);

            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await _controller.SendCode(viewModel);

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Fact]
        public async Task SendCodePost_ReturnRedirectToActionResult_WhenTokenIsNull()
        {
            // Arrange
            var user = new ApplicationUser();

            var viewModel = GetSendCodeViewModel("Email", null, false);

            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => user)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.GenerateTwoFactorTokenAsync(user, viewModel.SelectedProvider))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await _controller.SendCode(viewModel);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Fact]
        public async Task SendCodePost_ReturnRedirectToActionResult_WhenSelectedProviderIsIncorrect()
        {
            // Arrange
            var user = new ApplicationUser();

            var viewModel = GetSendCodeViewModel("IncorrectSelectedProvider", null, false);

            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => user)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.GenerateTwoFactorTokenAsync(user, viewModel.SelectedProvider))
                .ReturnsAsync(() => "token")
                .Verifiable();

            // Act
            var actionResult = await _controller.SendCode(viewModel);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("http://www.example.com", true)]
        public async Task SendCodePost_ReturnRedirectToActionResult_WhenSelectedProviderIsEmail(string returnUrl, bool rememberMe)
        {
            // Arrange
            var user = new ApplicationUser();
            var token = "token";

            var viewModel = GetSendCodeViewModel("Email", returnUrl, rememberMe);

            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => user)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.GenerateTwoFactorTokenAsync(user, viewModel.SelectedProvider))
                .ReturnsAsync(() => token)
                .Verifiable();

            _mockEmailService
                .Setup(x => x.SendTwoFactorTokenAsync(user, token))
                .ReturnsAsync(() => SendResult.Success)
                .Verifiable();

            // Act
            var actionResult = await _controller.SendCode(viewModel);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();
            _mockEmailService.Verify();

            MvcAssert.RedirectToActionResult(
                actionResult,
                "VerifyCode",
                null,
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("Provider", viewModel.SelectedProvider),
                    new KeyValuePair<string, object>("ReturnUrl", viewModel.ReturnUrl),
                    new KeyValuePair<string, object>("RememberMe", viewModel.RememberMe)
                }
            );
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("http://www.example.com", true)]
        public async Task SendCodePost_ReturnRedirectToActionResult_WhenSelectedProviderIsPhone(string returnUrl, bool rememberMe)
        {
            // Arrange
            var user = new ApplicationUser();
            var token = "token";

            var viewModel = GetSendCodeViewModel("Phone", returnUrl, rememberMe);

            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => user)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.GenerateTwoFactorTokenAsync(user, viewModel.SelectedProvider))
                .ReturnsAsync(() => token)
                .Verifiable();

            _mockSmsService
                .Setup(x => x.SendTwoFactorTokenAsync(user, token))
                .ReturnsAsync(() => SendResult.Success)
                .Verifiable();

            // Act
            var actionResult = await _controller.SendCode(viewModel);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();
            _mockSmsService.Verify();

            MvcAssert.RedirectToActionResult(
                actionResult,
                "VerifyCode",
                null,
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("Provider", viewModel.SelectedProvider),
                    new KeyValuePair<string, object>("ReturnUrl", viewModel.ReturnUrl),
                    new KeyValuePair<string, object>("RememberMe", viewModel.RememberMe)
                }
            );
        }

        [Fact]
        public async Task VerifyCode_ReturnRedirectToActionResult_WhenUserIsNull()
        {
            // Arrange
            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await _controller.VerifyCode(null, false);

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Theory]
        [InlineData(null, false, null)]
        [InlineData("Email", true, "http://www.example.com")]
        public async Task VerifyCode_ReturnViewResultWithVerifyCodeViewModel_WhenUserIsNotNull(string provider, bool rememberMe, string returnUrl)
        {
            // Arrange
            _mockSignInManager
                .Setup(x => x.GetTwoFactorAuthenticationUserAsync())
                .ReturnsAsync(() => new ApplicationUser())
                .Verifiable();

            // Act
            var actionResult = await _controller.VerifyCode(provider, rememberMe, returnUrl);

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.ViewResult(
                actionResult,
                null,
                new VerifyCodeViewModel
                {
                    Provider = provider,
                    ReturnUrl = returnUrl,
                    RememberMe = rememberMe
                }
            );
        }

        [Theory]
        [InlineData(typeof(HttpPostAttribute))]
        [InlineData(typeof(ValidateAntiForgeryTokenAttribute))]
        public void VerifyCodePost_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.MethodHasAttribute<TwoFactorSignInController>("VerifyCode", new[] { typeof(VerifyCodeViewModel) }, attributeType);
        }

        private static VerifyCodeViewModel GetVerifyCodeViewModel(string provider, string token, string returnUrl, bool rememberBrowser, bool rememberMe)
        {
            return new VerifyCodeViewModel
            {
                Provider = provider,
                ReturnUrl = returnUrl,
                Token = token,
                RememberBrowser = rememberBrowser,
                RememberMe = rememberMe
            };
        }

        [Fact]
        public async Task VerifyCodePost_ReturnViewResultWithVerifyCodeViewModel_WhenModelStateIsNotValid()
        {
            // Arrange
            const string modelStateKey = "Provider";
            const string modelStateErrorMessage = "Required";

            var viewModel = GetVerifyCodeViewModel(null, null, null, false, false);

            _controller.ModelState.AddModelError(modelStateKey, modelStateErrorMessage);

            // Act
            var actionResult = await _controller.VerifyCode(viewModel);

            // Assert
            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ModelState(viewResult, modelStateKey, modelStateErrorMessage);
        }

        [Fact]
        public async Task VerifyCodePost_ReturnViewResultWithVerifyCodeViewModel_WhenTwoFactorSignInAsyncFailed()
        {
            // Arrange
            var viewModel = GetVerifyCodeViewModel("Email", "Token", null, false, false);

            _mockSignInManager
                .Setup(x => x.TwoFactorSignInAsync(viewModel.Provider, viewModel.Token, viewModel.RememberMe, viewModel.RememberBrowser))
                .ReturnsAsync(() => SignInResult.Failed)
                .Verifiable();

            // Act
            var actionResult = await _controller.VerifyCode(viewModel);

            // Assert
            _mockSignInManager.Verify();

            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ModelState(viewResult, "InvalidCode", string.Empty);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("/SignIn", true)]
        [InlineData("http://www.example.com", false)]
        public async Task VerifyCodePost_ReturnRedirectResult_WhenTwoFactorSignInAsyncSucceeded(string returnUrl, bool isLocalUrl)
        {
            // Arrange
            var viewModel = GetVerifyCodeViewModel("Email", "Token", returnUrl, false, false);

            _mockSignInManager
                .Setup(x => x.TwoFactorSignInAsync(viewModel.Provider, viewModel.Token, viewModel.RememberMe, viewModel.RememberBrowser))
                .ReturnsAsync(() => SignInResult.Success)
                .Verifiable();

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(returnUrl))
                .Returns(isLocalUrl)
                .Verifiable();

            _controller.Url = mockUrlHelper.Object;

            // Act
            var actionResult = await _controller.VerifyCode(viewModel);

            // Assert
            _mockSignInManager.Verify();
            mockUrlHelper.Verify();

            if (_controller.Url.IsLocalUrl(returnUrl))
            {
                MvcAssert.RedirectResult(actionResult, returnUrl);
            }
            else
            {
                MvcAssert.RedirectToActionResult(actionResult, "Index", "Home");
            }
        }

        [Fact]
        public async Task VerifyCodePost_RedirectToActionResult_WhenTwoFactorSignInAsyncIsLockedOut()
        {
            // Arrange
            var viewModel = GetVerifyCodeViewModel("Email", "Token", null, false, false);

            _mockSignInManager
                .Setup(x => x.TwoFactorSignInAsync(viewModel.Provider, viewModel.Token, viewModel.RememberMe, viewModel.RememberBrowser))
                .ReturnsAsync(() => SignInResult.LockedOut)
                .Verifiable();

            // Act
            var actionResult = await _controller.VerifyCode(viewModel);

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "LockedOut");
        }

        [Fact]
        public void Error_ReturnViewResult()
        {
            // Arrange & Act
            var actionResult = _controller.Error();

            // Assert
            MvcAssert.ViewResult(actionResult);
        }
    }
}