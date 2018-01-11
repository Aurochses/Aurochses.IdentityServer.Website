using System.Threading.Tasks;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Models.EmailConfirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Xunit;
using System;
using Aurochses.AspNetCore.Identity;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.Runtime;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class EmailConfirmationControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IEmailService> _mockEmailService;

        private readonly EmailConfirmationController _controller;

        public EmailConfirmationControllerTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _mockEmailService = new Mock<IEmailService>(MockBehavior.Strict);

            _controller = new EmailConfirmationController(_mockUserManager.Object, _mockEmailService.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<EmailConfirmationController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Fact]
        public void Index_ReturnViewResult()
        {
            // Arrange & Act
            var actionResult = _controller.Index();

            // Assert
            MvcAssert.ViewResult(actionResult);
        }

        [Theory]
        [InlineData(typeof(HttpPostAttribute))]
        [InlineData(typeof(ValidateRecaptchaAttribute))]
        [InlineData(typeof(ValidateAntiForgeryTokenAttribute))]
        public void IndexPost_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.MethodHasAttribute<EmailConfirmationController>("Index", new[] { typeof(EmailConfirmationViewModel) }, attributeType);
        }

        private static EmailConfirmationViewModel GetEmailConfirmationViewModel(string methodName)
        {
            return new EmailConfirmationViewModel
            {
                Email = typeof(EmailConfirmationControllerTests).GenerateEmail(methodName)
            };
        }

        [Fact]
        public async Task IndexPost_ReturnViewResultWithEmailConfirmationViewModel_WhenModelStateIsNotValid()
        {
            // Arrange
            const string modelStateKey = "Email";
            const string modelStateErrorMessage = "Required";

            var viewModel = GetEmailConfirmationViewModel(nameof(IndexPost_ReturnViewResultWithEmailConfirmationViewModel_WhenModelStateIsNotValid));

            _controller.ModelState.AddModelError(modelStateKey, modelStateErrorMessage);

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            var viewResult = MvcAssert.ViewResult(actionResult, null, viewModel);

            MvcAssert.ModelState(viewResult, modelStateKey, modelStateErrorMessage);
        }

        [Fact]
        public async Task IndexPost_ReturnRedirectToActionResult_WhenUserNotFound()
        {
            // Arrange
            var viewModel = GetEmailConfirmationViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenUserNotFound));

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            MvcAssert.RedirectToActionResult(actionResult, "UserNotFound");
        }

        [Fact]
        public async Task IndexPost_ReturnRedirectToActionResult_WhenEmailIsAlreadyConfirmed()
        {
            // Arrange
            var viewModel = GetEmailConfirmationViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenEmailIsAlreadyConfirmed));

            var applicationUser = new ApplicationUser();

            _mockUserManager
                .Setup(x => x.FindByNameAsync(viewModel.Email))
                .ReturnsAsync(applicationUser)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.IsEmailConfirmedAsync(applicationUser))
                .ReturnsAsync(true)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "IsAlreadyConfirmed");
        }

        [Fact]
        public async Task IndexPost_ReturnRedirectToActionResult_WhenEmailSent()
        {
            // Arrange
            const string token = "token";
            const string scheme = "http";
            const string callbackUrl = "callbackUrl";

            var viewModel = GetEmailConfirmationViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenEmailSent));

            var applicationUser = new ApplicationUser { Id = Guid.Empty };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(viewModel.Email))
                .ReturnsAsync(applicationUser)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.IsEmailConfirmedAsync(applicationUser))
                .ReturnsAsync(false)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.GenerateEmailConfirmationTokenAsync(applicationUser))
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
                                       && context.Values.ValueEquals(new {UserId = applicationUser.Id, Token = token})
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
                .Setup(x => x.SendEmailConfirmationTokenAsync(applicationUser, callbackUrl))
                .ReturnsAsync(() => SendResult.Success)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            _mockUserManager.Verify();
            mockUrlHelper.Verify();
            _mockEmailService.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "EmailSent");
        }

        [Fact]
        public void UserNotFound_ReturnViewResult()
        {
            // Arrange & Act
            var actionResult = _controller.UserNotFound();

            // Assert
            MvcAssert.ViewResult(actionResult);
        }

        [Fact]
        public void IsAlreadyConfirmed_ReturnViewResult()
        {
            // Arrange & Act
            var actionResult = _controller.IsAlreadyConfirmed();

            // Assert
            MvcAssert.ViewResult(actionResult);
        }

        [Fact]
        public void EmailSent_ReturnViewResult()
        {
            // Arrange & Act
            var actionResult = _controller.EmailSent();

            // Assert
            MvcAssert.ViewResult(actionResult);
        }

        [Fact]
        public async Task Confirm_ReturnRedirectToActionResult_WhenUserIdIsNull()
        {
            // Arrange & Act
            var actionResult = await _controller.Confirm(null, "token");

            // Assert
            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Fact]
        public async Task Confirm_ReturnRedirectToActionResult_WhenTokenIsNull()
        {
            // Arrange & Act
            var actionResult = await _controller.Confirm("userId", null);

            // Assert
            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Fact]
        public async Task Confirm_ReturnRedirectToActionResult_WhenUserNotFound()
        {
            // Arrange
            const string userId = "userId";

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await _controller.Confirm(userId, "token");

            // Assert
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Fact]
        public async Task Confirm_ReturnRedirectToActionResult_WhenConfirmEmailFailed()
        {
            // Arrange
            const string userId = "userId";
            const string token = "token";
            var applicationUser = new ApplicationUser();

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(applicationUser)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.ConfirmEmailAsync(applicationUser, token))
                .ReturnsAsync(IdentityResult.Failed())
                .Verifiable();

            // Act
            var actionResult = await _controller.Confirm(userId, token);

            // Assert
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Fact]
        public async Task Confirm_ReturnRedirectToActionResult_Success()
        {
            // Arrange
            const string userId = "userId";
            const string token = "token";
            var applicationUser = new ApplicationUser();

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(applicationUser)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.ConfirmEmailAsync(applicationUser, token))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            // Act
            var actionResult = await _controller.Confirm(userId, token);

            // Assert
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Success");
        }

        [Fact]
        public void Success_ReturnViewResult()
        {
            // Arrange & Act
            var actionResult = _controller.Success();

            // Assert
            MvcAssert.ViewResult(actionResult);
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
