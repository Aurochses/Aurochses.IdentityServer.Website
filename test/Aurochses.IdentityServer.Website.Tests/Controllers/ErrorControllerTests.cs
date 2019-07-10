using System;
using System.Threading.Tasks;
using Aurochses.IdentityServer.Website.Controllers;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.IdentityServer.Website.Tests.Fakes;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Controllers
{
    public class ErrorControllerTests : ControllerTestsBase<ErrorController>
    {
        private readonly Mock<IIdentityServerInteractionService> _mockIdentityServerInteractionService;

        private readonly ErrorController _controller;

        public ErrorControllerTests()
        {
            _mockIdentityServerInteractionService = new Mock<IIdentityServerInteractionService>(MockBehavior.Strict);

            _controller = new ErrorController(
                MockLogger.Object,
                MockHostingEnvironment.Object,
                _mockIdentityServerInteractionService.Object
            );
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        [InlineData(typeof(AllowAnonymousAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<ErrorController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Fact]
        public async Task Index_WhenErrorMessageIsNull_ReturnViewResult()
        {
            // Arrange
            const string errorId = "Test ErrorId";

            _mockIdentityServerInteractionService
                .Setup(x => x.GetErrorContextAsync(errorId))
                .ReturnsAsync(() => null);

            // Act
            var actionResult = await _controller.Index(errorId);

            // Assert
            MvcAssert.ViewResult(actionResult, "Error");
        }

        [Fact]
        public async Task Index_WhenErrorMessageIsNotNull_ReturnViewResult()
        {
            // Arrange
            const string errorId = "Test ErrorId";

            _mockIdentityServerInteractionService
                .Setup(x => x.GetErrorContextAsync(errorId))
                .ReturnsAsync(
                    () => new ErrorMessage
                    {
                        DisplayMode = "Test DisplayMode",
                        UiLocales = "Test UiLocales",
                        Error = "Test Error",
                        ErrorDescription = "Test ErrorDescription",
                        RequestId = "Test RequestId",
                        RedirectUri = "Test RedirectUri",
                        ResponseMode = "Test ResponseMode"
                    }
                );

            // Act
            var actionResult = await _controller.Index(errorId);

            // Assert
            VerifyLogger(LogLevel.Error, Times.Once);

            MvcAssert.ViewResult(
                actionResult,
                "Error",
                new ErrorMessage
                {
                    DisplayMode = "Test DisplayMode",
                    UiLocales = "Test UiLocales",
                    Error = "Test Error",
                    ErrorDescription = null,
                    RequestId = "Test RequestId",
                    RedirectUri = "Test RedirectUri",
                    ResponseMode = "Test ResponseMode"
                }
            );
        }

        [Fact]
        public async Task Index_WhenErrorMessageIsNotNull_And_IsDevelopment_ReturnViewResult()
        {
            // Arrange
            SetupHostingEnvironmentName("Development");

            const string errorId = "Test ErrorId";

            _mockIdentityServerInteractionService
                .Setup(x => x.GetErrorContextAsync(errorId))
                .ReturnsAsync(
                    () => new ErrorMessage
                    {
                        DisplayMode = "Test DisplayMode",
                        UiLocales = "Test UiLocales",
                        Error = "Test Error",
                        ErrorDescription = "Test ErrorDescription",
                        RequestId = "Test RequestId",
                        RedirectUri = "Test RedirectUri",
                        ResponseMode = "Test ResponseMode"
                    }
                );

            // Act
            var actionResult = await _controller.Index(errorId);

            // Assert
            MvcAssert.ViewResult(
                actionResult,
                "Error",
                new ErrorMessage
                {
                    DisplayMode = "Test DisplayMode",
                    UiLocales = "Test UiLocales",
                    Error = "Test Error",
                    ErrorDescription = "Test ErrorDescription",
                    RequestId = "Test RequestId",
                    RedirectUri = "Test RedirectUri",
                    ResponseMode = "Test ResponseMode"
                }
            );
        }
    }
}