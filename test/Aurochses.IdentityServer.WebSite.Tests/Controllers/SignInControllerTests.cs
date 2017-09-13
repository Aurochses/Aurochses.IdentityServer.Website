using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.SignIn;
using Aurochses.Testing;
using Aurochses.Testing.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class SignInControllerTests
    {
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

        private readonly SignInController _controller;

        public SignInControllerTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(_mockUserManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<ApplicationUser>>>().Object);

            _controller = new SignInController(_mockSignInManager.Object, _mockUserManager.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<SignInController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Fact]
        public void Index_RedirectToActionResult_WhenUserIsAuthenticated()
        {
            // Arrange
            _controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new GenericIdentity("test"))
            };

            // Act
            var actionResult = _controller.Index();

            // Assert
            MvcAssert.RedirectToActionResult(actionResult, "Index", "User");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public void Index_ReturnViewResultWithViewDataReturnUrl_WhenUserIsNotAuthenticated(string returnUrl)
        {
            // Arrange
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var actionResult = _controller.Index(returnUrl);

            // Assert
            var viewResult = MvcAssert.ViewResult(actionResult);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);
        }

        [Theory]
        [InlineData(typeof(HttpPostAttribute))]
        [InlineData(typeof(ValidateAntiForgeryTokenAttribute))]
        public void IndexPost_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.MethodHasAttribute<SignInController>("Index", new[] { typeof(SignInViewModel), typeof(string) }, attributeType);
        }

        private static SignInViewModel GetSignInViewModel(string methodName)
        {
            return new SignInViewModel
            {
                Email = typeof(SignInControllerTests).GenerateEmail(methodName)
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task IndexPost_ReturnViewResultWithSignInViewModel_WhenModelStateIsNotValid(string returnUrl)
        {
            // Arrange
            const string modelStateKey = "Email";
            const string modelStateErrorMessage = "Required";

            var viewModel = GetSignInViewModel(nameof(IndexPost_ReturnViewResultWithSignInViewModel_WhenModelStateIsNotValid));

            _controller.ModelState.AddModelError(modelStateKey, modelStateErrorMessage);

            // Act
            var actionResult = await _controller.Index(viewModel, returnUrl);

            // Assert
            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);

            MvcAssert.ModelState(viewResult, modelStateKey, modelStateErrorMessage);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task IndexPost_ReturnViewResult_WhenPasswordSignInAsyncSignInResultFailed(string returnUrl)
        {
            // Arrange
            var viewModel = GetSignInViewModel(nameof(IndexPost_ReturnViewResult_WhenPasswordSignInAsyncSignInResultFailed));

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(viewModel, returnUrl);

            // Assert
            _mockSignInManager.Verify();

            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);

            MvcAssert.ModelState(viewResult, "InvalidLoginAttempt", string.Empty);
        }

        [Fact]
        public async Task IndexPost_RedirectToActionResult_WhenPasswordSignInAsyncSignInResultIsLockedOut()
        {
            // Arrange
            var viewModel = GetSignInViewModel(nameof(IndexPost_RedirectToActionResult_WhenPasswordSignInAsyncSignInResultIsLockedOut));

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "LockedOut");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task IndexPost_RedirectToActionResult_WhenPasswordSignInAsyncSignInResultRequiresTwoFactor(string returnUrl)
        {
            // Arrange
            var viewModel = GetSignInViewModel(nameof(IndexPost_RedirectToActionResult_WhenPasswordSignInAsyncSignInResultRequiresTwoFactor));

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(viewModel, returnUrl);

            // Assert
            _mockSignInManager.Verify();

            MvcAssert.RedirectToActionResult(
                actionResult,
                "SendCode",
                "TwoFactorSignIn",
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("ReturnUrl", returnUrl),
                    new KeyValuePair<string, object>("RememberMe", viewModel.RememberMe)
                }
            );
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("/SignIn", true)]
        [InlineData("http://www.example.com", false)]
        public async Task IndexPost_ReturnRedirectResult_WhenPasswordSignInAsyncSignInResultSucceededAndEmailIsConfirmed(string returnUrl, bool isLocalUrl)
        {
            // Arrange
            var user = new ApplicationUser { EmailConfirmed = true };

            var viewModel = GetSignInViewModel(nameof(IndexPost_ReturnRedirectResult_WhenPasswordSignInAsyncSignInResultSucceededAndEmailIsConfirmed));

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.FindByNameAsync(viewModel.Email))
                .ReturnsAsync(user)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.IsEmailConfirmedAsync(user))
                .ReturnsAsync(user.EmailConfirmed)
                .Verifiable();

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(returnUrl))
                .Returns(isLocalUrl)
                .Verifiable();

            _controller.Url = mockUrlHelper.Object;

            // Act
            var actionResult = await _controller.Index(viewModel, returnUrl);

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

        [Fact]
        public async Task IndexPost_RedirectToActionResult_WhenPasswordSignInAsyncSignInResultSucceededAndEmailIsNotConfirmed()
        {
            // Arrange
            var user = new ApplicationUser { EmailConfirmed = false };

            var viewModel = GetSignInViewModel(nameof(IndexPost_RedirectToActionResult_WhenPasswordSignInAsyncSignInResultSucceededAndEmailIsNotConfirmed));

            _mockSignInManager
                .Setup(x => x.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.FindByNameAsync(viewModel.Email))
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
            var actionResult = await _controller.Index(viewModel);

            // Assert
            _mockSignInManager.Verify();
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "EmailConfirmation");
        }
    }
}