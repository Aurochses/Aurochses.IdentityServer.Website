using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Aurochses.Identity;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.Registration;
using Aurochses.Runtime;
using Aurochses.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Xunit;
using Aurochses.Testing.Mvc;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class ExternalLoginControllerTests
    {
        private readonly ExternalLoginInfo _externalLoginInfo;

        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<IEmailService> _mockEmailService;

        private readonly ExternalLoginController _controller;

        public ExternalLoginControllerTests()
        {
            _externalLoginInfo = new ExternalLoginInfo(ClaimsPrincipal.Current, "TestLoginProvider", "TestProviderKey", "TestDisplayName");

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(_mockUserManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<ApplicationUser>>>().Object);
            _mockEmailService = new Mock<IEmailService>(MockBehavior.Strict);

            _controller = new ExternalLoginController(_mockUserManager.Object, _mockSignInManager.Object, _mockEmailService.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<ExternalLoginController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Theory]
        [InlineData(typeof(HttpPostAttribute))]
        [InlineData(typeof(ValidateAntiForgeryTokenAttribute))]
        public void IndexPost_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.MethodHasAttribute<ExternalLoginController>("Index", new[] { typeof(string), typeof(string) }, attributeType);
        }

        [Theory]
        [InlineData("TestProvider", null)]
        [InlineData("TestProvider", "http://www.example.com")]
        public void IndexPost_ReturnChallengeResult(string provider, string returnUrl)
        {
            // Arrange
            const string redirectUrl = "redirectUrl";

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup
                (
                    x => x.Action
                    (
                        It.Is<UrlActionContext>
                        (
                            context => context.Action == "Callback"
                                       && context.Controller == "ExternalLogin"
                                       && context.Values.ValueEquals(new {ReturnUrl = returnUrl})
                        )
                    )
                )
                .Returns(redirectUrl)
                .Verifiable();

            _controller.Url = mockUrlHelper.Object;

            var authenticationProperties = new AuthenticationProperties(new AttributeDictionary {{"key", "value"}});

            _mockSignInManager
                .Setup(x => x.ConfigureExternalAuthenticationProperties(provider, redirectUrl, null))
                .Returns(() => authenticationProperties)
                .Verifiable();

            // Act
            var actionResult = _controller.Index(provider, returnUrl);

            // Assert
            mockUrlHelper.Verify();
            _mockSignInManager.Verify();

            MvcAssert.ChallengeResult(actionResult, authenticationProperties, provider);
        }

        [Fact]
        public async Task Callback_ReturnRedirectToActionResult_WhenRemoteErrorIsNotNull()
        {
            // Arrange & Act
            var actionResult = await _controller.Callback(null, "remoteError");

            // Assert
            MvcAssert.RedirectToActionResult(actionResult, "Index", "SignIn");
        }

        [Fact]
        public async Task Callback_ReturnRedirectToActionResult_WhenExternalLoginInfoIsNull()
        {
            // Arrange
            _mockSignInManager
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await _controller.Callback();

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "SignIn");
        }

        private void SetupCallback(Microsoft.AspNetCore.Identity.SignInResult signInResult)
        {
            _mockSignInManager
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(_externalLoginInfo)
                .Verifiable();

            _mockSignInManager
                .Setup(x => x.ExternalLoginSignInAsync(_externalLoginInfo.LoginProvider, _externalLoginInfo.ProviderKey, false))
                .ReturnsAsync(signInResult)
                .Verifiable();
        }

        [Fact]
        public async Task Callback_ReturnRedirectToActionResult_WhenSignInResultSucceededAndEmailIsNotConfirmed()
        {
            // Arrange
            var user = new ApplicationUser { EmailConfirmed = false };

            SetupCallback(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockUserManager
                .Setup(x => x.FindByLoginAsync(_externalLoginInfo.LoginProvider, _externalLoginInfo.ProviderKey))
                .ReturnsAsync(user)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.IsEmailConfirmedAsync(user))
                .ReturnsAsync(user.EmailConfirmed)
                .Verifiable();

            _mockSignInManager
                .Setup(x => x.SignOutAsync())
                .Returns(() => Task.FromResult(0))
                .Verifiable();

            // Act
            var actionResult = await _controller.Callback();

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "EmailConfirmation");
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("/SignIn", true)]
        [InlineData("http://www.example.com", false)]
        public async Task Callback_ReturnRedirect_WhenSignInResultSucceededAndEmailIsConfirmed(string returnUrl, bool isLocalUrl)
        {
            // Arrange
            var user = new ApplicationUser { EmailConfirmed = true };

            SetupCallback(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockUserManager
                .Setup(x => x.FindByLoginAsync(_externalLoginInfo.LoginProvider, _externalLoginInfo.ProviderKey))
                .ReturnsAsync(user)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.IsEmailConfirmedAsync(user))
                .ReturnsAsync(user.EmailConfirmed)
                .Verifiable();

            _mockSignInManager
                .Setup(x => x.UpdateExternalAuthenticationTokensAsync(_externalLoginInfo))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(returnUrl))
                .Returns(isLocalUrl)
                .Verifiable();

            _controller.Url = mockUrlHelper.Object;

            // Act
            var actionResult = await _controller.Callback(returnUrl);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();
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

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task Callback_ReturnRedirectToActionResult_WhenSignInResultRequiresTwoFactor(string returnUrl)
        {
            // Arrange
            SetupCallback(Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired);

            // Act
            var actionResult = await _controller.Callback(returnUrl);

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(
                actionResult,
                "SendCode",
                "TwoFactorSignIn",
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("ReturnUrl", returnUrl),
                    new KeyValuePair<string, object>("RememberMe", false)
                }
            );
        }

        [Fact]
        public async Task Callback_ReturnRedirectToActionResult_WhenSignInResultIsLockedOut()
        {
            // Arrange
            SetupCallback(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            // Act
            var actionResult = await _controller.Callback();

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "LockedOut");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task Callback_ReturnViewResultWithRegistrationViewModel_WhenSignInResultFailed(string returnUrl)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, GetType().GenerateEmail(nameof(Callback_ReturnViewResultWithRegistrationViewModel_WhenSignInResultFailed))));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, "John"));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, "Black"));
            _externalLoginInfo.Principal = new ClaimsPrincipal(claimsIdentity);

            SetupCallback(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var actionResult = await _controller.Callback(returnUrl);

            // Assert
            _mockSignInManager.Verify();

            var registrationViewModel = new RegistrationViewModel
            {
                Email = _externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                FirstName = _externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = _externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname)
            };

            var viewResult = MvcAssert.ViewResult(actionResult, "Registration", registrationViewModel);

            MvcAssert.ViewData(
                viewResult,
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("ReturnUrl", returnUrl),
                    new KeyValuePair<string, object>("LoginProvider", _externalLoginInfo.LoginProvider)
                }
            );
        }

        [Theory]
        [InlineData(typeof(HttpPostAttribute))]
        [InlineData(typeof(ValidateRecaptchaAttribute))]
        [InlineData(typeof(ValidateAntiForgeryTokenAttribute))]
        public void RegistrationPost_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.MethodHasAttribute<ExternalLoginController>("Registration", new[] { typeof(RegistrationViewModel), typeof(string) }, attributeType);
        }

        private static RegistrationViewModel GetRegistrationViewModel(string methodName)
        {
            return new RegistrationViewModel
            {
                Email = typeof(ExternalLoginControllerTests).GenerateEmail(methodName),
                Password = "TestPassword",
                ConfirmPassword = "TestConfirmPassword",
                FirstName = "John",
                LastName = "Black"
            };
        }

        [Theory]
        [InlineData("Email", "Required", null)]
        [InlineData("Email", "Required", "http://www.example.com")]
        public async Task RegistrationPost_ReturnViewResultWithRegistrationViewModel_WhenModelStateIsNotValid(string modelStateKey, string modelStateErrorMessage, string returnUrl)
        {
            // Arrange
            _controller.ModelState.AddModelError(modelStateKey, modelStateErrorMessage);

            var viewModel = GetRegistrationViewModel(nameof(RegistrationPost_ReturnViewResultWithRegistrationViewModel_WhenModelStateIsNotValid));

            // Act
            var actionResult = await _controller.Registration(viewModel, returnUrl);

            // Assert
            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);

            MvcAssert.ModelState(viewResult, modelStateKey, modelStateErrorMessage);
        }

        [Fact]
        public async Task RegistrationPost_ReturnRedirectToActionResult_WhenExternalLoginInfoIsNull()
        {
            // Arrange
            var viewModel = GetRegistrationViewModel(nameof(RegistrationPost_ReturnRedirectToActionResult_WhenExternalLoginInfoIsNull));

            _mockSignInManager
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await _controller.Registration(viewModel);

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "SignIn");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task RegistrationPost_ReturnViewResult_WhenCreateAsyncIdentityResultFailed(string returnUrl)
        {
            // Arrange
            var viewModel = GetRegistrationViewModel(nameof(RegistrationPost_ReturnViewResult_WhenCreateAsyncIdentityResultFailed));

            _mockSignInManager
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(_externalLoginInfo)
                .Verifiable();

            var identityError = new IdentityError
            {
                Code = "IdentityErrorCode",
                Description = "IdentityErrorDescription"
            };

            _mockUserManager
                .Setup
                (
                    x => x.CreateAsync
                    (
                        It.Is<ApplicationUser>
                        (
                            applicationUser => applicationUser.UserName == viewModel.Email
                                               && applicationUser.Email == viewModel.Email
                                               && applicationUser.FirstName == viewModel.FirstName
                                               && applicationUser.LastName == viewModel.LastName
                        ),
                        viewModel.Password
                    )
                )
                .ReturnsAsync(IdentityResult.Failed(identityError))
                .Verifiable();

            // Act
            var actionResult = await _controller.Registration(viewModel, returnUrl);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();

            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);

            MvcAssert.ModelState(viewResult, string.Empty, identityError.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task RegistrationPost_ReturnViewResult_WhenAddLoginAsyncIdentityResultFailed(string returnUrl)
        {
            // Arrange
            var userId = Guid.Empty;

            var viewModel = GetRegistrationViewModel(nameof(RegistrationPost_ReturnViewResult_WhenAddLoginAsyncIdentityResultFailed));

            _mockSignInManager
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(_externalLoginInfo)
                .Verifiable();

            var identityError = new IdentityError
            {
                Code = "IdentityErrorCode",
                Description = "IdentityErrorDescription"
            };

            _mockUserManager
                .Setup
                (
                    x => x.CreateAsync
                    (
                        It.Is<ApplicationUser>
                        (
                            applicationUser => applicationUser.UserName == viewModel.Email
                                               && applicationUser.Email == viewModel.Email
                                               && applicationUser.FirstName == viewModel.FirstName
                                               && applicationUser.LastName == viewModel.LastName
                        ),
                        viewModel.Password
                    )
                )
                .Callback((ApplicationUser applicationUser, string password) => applicationUser.Id = userId)
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            _mockUserManager
                .Setup
                (
                    x => x.AddLoginAsync
                    (
                        It.Is<ApplicationUser>
                        (
                            applicationUser => applicationUser.Id == userId
                                               && applicationUser.UserName == viewModel.Email
                                               && applicationUser.Email == viewModel.Email
                                               && applicationUser.FirstName == viewModel.FirstName
                                               && applicationUser.LastName == viewModel.LastName
                        ),
                        _externalLoginInfo
                    )
                )
                .ReturnsAsync(IdentityResult.Failed(identityError))
                .Verifiable();

            // Act
            var actionResult = await _controller.Registration(viewModel, returnUrl);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();

            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);

            MvcAssert.ModelState(viewResult, string.Empty, identityError.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task RegistrationPost_ReturnRedirectToActionResult_WhenIdentityResultSucceeded(string returnUrl)
        {
            // Arrange
            var userId = Guid.Empty;
            const string token = "token";
            const string scheme = "http";
            const string callbackUrl = "callbackUrl";

            var viewModel = GetRegistrationViewModel(nameof(RegistrationPost_ReturnRedirectToActionResult_WhenIdentityResultSucceeded));

            _mockSignInManager
                .Setup(x => x.GetExternalLoginInfoAsync(null))
                .ReturnsAsync(_externalLoginInfo)
                .Verifiable();

            _mockUserManager
                .Setup
                (
                    x => x.CreateAsync
                    (
                        It.Is<ApplicationUser>
                        (
                            applicationUser => applicationUser.UserName == viewModel.Email
                                               && applicationUser.Email == viewModel.Email
                                               && applicationUser.FirstName == viewModel.FirstName
                                               && applicationUser.LastName == viewModel.LastName
                        ),
                        viewModel.Password
                    )
                )
                .Callback((ApplicationUser applicationUser, string password) => applicationUser.Id = userId)
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            _mockUserManager
                .Setup
                (
                    x => x.AddLoginAsync
                    (
                        It.Is<ApplicationUser>
                        (
                            applicationUser => applicationUser.Id == userId
                                               && applicationUser.UserName == viewModel.Email
                                               && applicationUser.Email == viewModel.Email
                                               && applicationUser.FirstName == viewModel.FirstName
                                               && applicationUser.LastName == viewModel.LastName
                        ),
                        _externalLoginInfo
                    )
                )
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            _mockUserManager
                .Setup
                (
                    x => x.GenerateEmailConfirmationTokenAsync
                    (
                        It.Is<ApplicationUser>
                        (
                            applicationUser => applicationUser.Id == userId
                                               && applicationUser.UserName == viewModel.Email
                                               && applicationUser.Email == viewModel.Email
                                               && applicationUser.FirstName == viewModel.FirstName
                                               && applicationUser.LastName == viewModel.LastName
                        )
                    )
                )
                .ReturnsAsync(token)
                .Verifiable();

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup
                (
                    x => x.Action
                    (
                        It.Is<UrlActionContext>
                        (
                            context => context.Action == "Confirm"
                                       && context.Controller == "EmailConfirmation"
                                       && context.Values.ValueEquals(new {UserId = userId, Token = token})
                                       && context.Protocol == scheme
                        )
                    )
                )
                .Returns(callbackUrl)
                .Verifiable();

            _controller.Url = mockUrlHelper.Object;
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Scheme = scheme;

            _mockEmailService
                .Setup
                (
                    x => x.SendEmailConfirmationTokenAsync
                    (
                        It.Is<ApplicationUser>
                        (
                            applicationUser => applicationUser.Id == userId
                                               && applicationUser.UserName == viewModel.Email
                                               && applicationUser.Email == viewModel.Email
                                               && applicationUser.FirstName == viewModel.FirstName
                                               && applicationUser.LastName == viewModel.LastName
                        ),
                        callbackUrl
                    )
                )
                .ReturnsAsync(() => SendResult.Success)
                .Verifiable();

            _mockSignInManager
                .Setup(x => x.UpdateExternalAuthenticationTokensAsync(_externalLoginInfo))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            // Act
            var actionResult = await _controller.Registration(viewModel, returnUrl);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();
            mockUrlHelper.Verify();
            _mockEmailService.Verify();

            MvcAssert.RedirectToActionResult(
                actionResult,
                "EmailSent",
                "Registration",
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("ReturnUrl", returnUrl)
                }
            );
        }
    }
}