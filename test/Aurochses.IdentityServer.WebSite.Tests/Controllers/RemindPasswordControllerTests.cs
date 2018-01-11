using System;
using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.RemindPassword;
using Aurochses.Runtime;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class RemindPasswordControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IEmailService> _mockEmailService;

        private readonly RemindPasswordController _controller;

        public RemindPasswordControllerTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _mockEmailService = new Mock<IEmailService>(MockBehavior.Strict);

            _controller = new RemindPasswordController(_mockUserManager.Object, _mockEmailService.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<RemindPasswordController>(attributeType);
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
            TypeAssert.MethodHasAttribute<RemindPasswordController>("Index", new[] { typeof(RemindPasswordViewModel) }, attributeType);
        }

        private static RemindPasswordViewModel GetRemindPasswordViewModel(string methodName)
        {
            return new RemindPasswordViewModel
            {
                Email = typeof(RemindPasswordControllerTests).GenerateEmail(methodName)
            };
        }

        [Fact]
        public async Task IndexPost_ReturnViewResultWithRemindPasswordViewModel_WhenModelStateIsNotValid()
        {
            // Arrange
            const string modelStateKey = "Email";
            const string modelStateErrorMessage = "Required";

            var viewModel = GetRemindPasswordViewModel(nameof(IndexPost_ReturnViewResultWithRemindPasswordViewModel_WhenModelStateIsNotValid));

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
            var viewModel = GetRemindPasswordViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenUserNotFound));

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            MvcAssert.RedirectToActionResult(actionResult, "UserNotFound");
        }

        [Fact]
        public async Task IndexPost_ReturnRedirectToActionResult_WhenEmailIsNotConfirmed()
        {
            // Arrange
            var viewModel = GetRemindPasswordViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenEmailIsNotConfirmed));

            var applicationUser = new ApplicationUser();

            _mockUserManager
                .Setup(x => x.FindByNameAsync(viewModel.Email))
                .ReturnsAsync(applicationUser)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.IsEmailConfirmedAsync(applicationUser))
                .ReturnsAsync(false)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "EmailConfirmation");
        }

        [Fact]
        public async Task IndexPost_ReturnRedirectToActionResult_WhenEmailSent()
        {
            // Arrange
            const string token = "token";
            const string scheme = "http";
            const string callbackUrl = "callbackUrl";

            var viewModel = GetRemindPasswordViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenEmailSent));

            var applicationUser = new ApplicationUser { Id = Guid.Empty };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(viewModel.Email))
                .ReturnsAsync(applicationUser)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.IsEmailConfirmedAsync(applicationUser))
                .ReturnsAsync(true)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.GeneratePasswordResetTokenAsync(applicationUser))
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
                            context => context.Action == "Index"
                                       && context.Controller == "ResetPassword"
                                       && context.Values.ValueEquals(new { UserId = applicationUser.Id, Token = token })
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
                .Setup(x => x.SendPasswordResetTokenAsync(applicationUser, callbackUrl))
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
        public void EmailSent_ReturnViewResult()
        {
            // Arrange & Act
            var actionResult = _controller.EmailSent();

            // Assert
            MvcAssert.ViewResult(actionResult);
        }
    }
}