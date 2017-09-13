using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurochses.Identity;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.Registration;
using Aurochses.Runtime;
using Aurochses.Testing;
using Aurochses.Testing.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class RegistrationControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IEmailService> _mockEmailService;

        private readonly RegistrationController _controller;

        public RegistrationControllerTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _mockEmailService = new Mock<IEmailService>(MockBehavior.Strict);

            _controller = new RegistrationController(_mockUserManager.Object, _mockEmailService.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<RegistrationController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public void Index_ReturnViewResultWithViewDataReturnUrl(string returnUrl)
        {
            // Arrange & Act
            var actionResult = _controller.Index(returnUrl);

            // Assert
            var viewResult = MvcAssert.ViewResult(actionResult);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);
        }

        [Theory]
        [InlineData(typeof(HttpPostAttribute))]
        [InlineData(typeof(ValidateAntiForgeryTokenAttribute))]
        public void ValidateEmailPost_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.MethodHasAttribute<RegistrationController>("ValidateEmail", new[] { typeof(string) }, attributeType);
        }

        [Fact]
        public async Task ValidateEmailPost_ReturnJsonResultFalse_WhenUserFoundByEmail()
        {
            // Arrange
            var email = GetType().GenerateEmail(nameof(ValidateEmailPost_ReturnJsonResultFalse_WhenUserFoundByEmail));

            _mockUserManager
                .Setup(x => x.FindByNameAsync(email))
                .ReturnsAsync(new ApplicationUser())
                .Verifiable();

            // Act
            var result = await _controller.ValidateEmail(email);

            // Assert
            MvcAssert.JsonResult(result, false);
        }

        [Fact]
        public async Task ValidateEmailPost_ReturnJsonResultTrue_WhenUserNotFoundByEmail()
        {
            // Arrange
            var email = GetType().GenerateEmail(nameof(ValidateEmailPost_ReturnJsonResultTrue_WhenUserNotFoundByEmail));

            _mockUserManager
                .Setup(x => x.FindByNameAsync(email))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var result = await _controller.ValidateEmail(email);

            // Assert
            MvcAssert.JsonResult(result, true);
        }

        [Theory]
        [InlineData(typeof(HttpPostAttribute))]
        [InlineData(typeof(ValidateRecaptchaAttribute))]
        [InlineData(typeof(ValidateAntiForgeryTokenAttribute))]
        public void IndexPost_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.MethodHasAttribute<RegistrationController>("Index", new[] { typeof(RegistrationViewModel), typeof(string) }, attributeType);
        }

        private static RegistrationViewModel GetRegistrationViewModel(string methodName)
        {
            return new RegistrationViewModel
            {
                Email = typeof(RegistrationControllerTests).GenerateEmail(methodName),
                Password = "TestPassword",
                ConfirmPassword = "TestPassword",
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task IndexPost_ReturnViewResultWithRegistrationViewModel_WhenModelStateIsNotValid(string returnUrl)
        {
            // Arrange
            const string modelStateKey = "Email";
            const string modelStateErrorMessage = "Required";

            var viewModel = GetRegistrationViewModel(nameof(IndexPost_ReturnViewResultWithRegistrationViewModel_WhenModelStateIsNotValid));

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
        public async Task IndexPost_ReturnViewResultWithRegistrationViewModel_WhenCreateAsyncIdentityResultFailed(string returnUrl)
        {
            // Arrange
            var viewModel = GetRegistrationViewModel(nameof(IndexPost_ReturnViewResultWithRegistrationViewModel_WhenCreateAsyncIdentityResultFailed));

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
            var actionResult = await _controller.Index(viewModel, returnUrl);

            // Assert
            _mockUserManager.Verify();

            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);

            MvcAssert.ModelState(viewResult, string.Empty, identityError.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public async Task IndexPost_ReturnRedirectToActionResult_WhenIdentityResultSucceeded(string returnUrl)
        {
            // Arrange
            var userId = Guid.Empty;
            const string token = "token";
            const string scheme = "http";
            const string callbackUrl = "callbackUrl";

            var viewModel = GetRegistrationViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenIdentityResultSucceeded));

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
                                       && context.Values.ValueEquals(new { UserId = userId, Token = token })
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

            // Act
            var actionResult = await _controller.Index(viewModel, returnUrl);

            // Assert
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

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public void EmailSent_ReturnViewResultWithViewDataReturnUrl(string returnUrl)
        {
            // Arrange & Act
            var actionResult = _controller.EmailSent(returnUrl);

            // Assert
            var viewResult = MvcAssert.ViewResult(actionResult);

            MvcAssert.ViewData(viewResult, "ReturnUrl", returnUrl);
        }
    }
}