using System;
using System.Threading.Tasks;
using Aurochses.IdentityServer.Website.Controllers;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Controllers
{
    public class ErrorControllerTests
    {
        private readonly Mock<IIdentityServerInteractionService> _mockIdentityServerInteractionService;

        public ErrorControllerTests()
        {
            _mockIdentityServerInteractionService = new Mock<IIdentityServerInteractionService>(MockBehavior.Strict);
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
        public async Task Index_WhenIdentityServerInteractionServiceErrorMessageIsNull_ReturnViewResult()
        {
            // Arrange
            const string errorId = "Test ErrorId";

            var controller = new ErrorController(new NullLogger<ErrorController>(), new HostingEnvironment(), _mockIdentityServerInteractionService.Object);

            _mockIdentityServerInteractionService
                .Setup(x => x.GetErrorContextAsync(errorId))
                .ReturnsAsync(() => null);

            // Act
            var actionResult = await controller.Index(errorId);

            // Assert
            MvcAssert.ViewResult(actionResult, "Error");
        }

        [Fact]
        public async Task Index_WhenIdentityServerInteractionServiceErrorMessageIsNotNull_ReturnViewResult()
        {
            // Arrange
            const string errorId = "Test ErrorId";

            var controller = new ErrorController(new NullLogger<ErrorController>(), new HostingEnvironment(), _mockIdentityServerInteractionService.Object);

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
            var actionResult = await controller.Index(errorId);

            // Assert
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
        public async Task Index_WhenIdentityServerInteractionServiceErrorMessageIsNotNullAndHostingEnvironmentIsDevelopment_ReturnViewResult()
        {
            // Arrange
            const string errorId = "Test ErrorId";

            var controller = new ErrorController(new NullLogger<ErrorController>(), new HostingEnvironment { EnvironmentName = "Development" }, _mockIdentityServerInteractionService.Object);

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
            var actionResult = await controller.Index(errorId);

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