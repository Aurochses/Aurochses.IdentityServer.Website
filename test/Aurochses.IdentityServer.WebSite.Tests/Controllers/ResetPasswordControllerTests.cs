using System;
using System.Threading.Tasks;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.ResetPassword;
using Aurochses.Testing;
using Aurochses.Testing.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class ResetPasswordControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

        private readonly ResetPasswordController _controller;

        public ResetPasswordControllerTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);

            _controller = new ResetPasswordController(_mockUserManager.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<ResetPasswordController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Fact]
        public void Index_ReturnRedirectToActionResult_WhenTokenIsNull()
        {
            // Arrange & Act
            var actionResult = _controller.Index(token: null);

            // Assert
            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Fact]
        public void Index_ReturnViewResult()
        {
            // Arrange & Act
            var actionResult = _controller.Index("token");

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
            TypeAssert.MethodHasAttribute<ResetPasswordController>("Index", new[] { typeof(ResetPasswordViewModel) }, attributeType);
        }

        private static ResetPasswordViewModel GetResetPasswordViewModel(string methodName)
        {
            return new ResetPasswordViewModel
            {
                Email = typeof(ResetPasswordController).GenerateEmail(methodName),
                Password = "TestPassword",
                ConfirmPassword = "TestPassword",
                Token = "Token"
            };
        }

        [Fact]
        public async Task IndexPost_ReturnViewResultWithResetPasswordViewModel_WhenModelStateIsNotValid()
        {
            // Arrange
            const string modelStateKey = "Email";
            const string modelStateErrorMessage = "Required";

            var viewModel = GetResetPasswordViewModel(nameof(IndexPost_ReturnViewResultWithResetPasswordViewModel_WhenModelStateIsNotValid));

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
            var viewModel = GetResetPasswordViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenUserNotFound));

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            MvcAssert.RedirectToActionResult(actionResult, "Error");
        }

        [Fact]
        public async Task IndexPost_ReturnRedirectToActionResult_WhenResetPasswordSuccess()
        {
            // Arrange
            var viewModel = GetResetPasswordViewModel(nameof(IndexPost_ReturnRedirectToActionResult_WhenResetPasswordSuccess));

            var applicationUser = new ApplicationUser();

            _mockUserManager
                .Setup(x => x.FindByNameAsync(viewModel.Email))
                .ReturnsAsync(applicationUser)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.ResetPasswordAsync(applicationUser, viewModel.Token, viewModel.Password))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Success");
        }

        [Fact]
        public async Task IndexPost_ReturnViewResult_WhenResetPasswordFailed()
        {
            // Arrange
            var viewModel = GetResetPasswordViewModel(nameof(IndexPost_ReturnViewResult_WhenResetPasswordFailed));

            var identityError = new IdentityError
            {
                Code = "IdentityErrorCode",
                Description = "IdentityErrorDescription"
            };

            var applicationUser = new ApplicationUser();

            _mockUserManager
                .Setup(x => x.FindByNameAsync(viewModel.Email))
                .ReturnsAsync(applicationUser)
                .Verifiable();

            _mockUserManager
                .Setup(x => x.ResetPasswordAsync(applicationUser, viewModel.Token, viewModel.Password))
                .ReturnsAsync(IdentityResult.Failed(identityError))
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(viewModel);

            // Assert
            _mockUserManager.Verify();

            var viewResult = MvcAssert.ViewResult(actionResult);

            MvcAssert.ModelState(viewResult, string.Empty, identityError.Description);
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
            var actionResult = _controller.Success();

            // Assert
            MvcAssert.ViewResult(actionResult);
        }
    }
}