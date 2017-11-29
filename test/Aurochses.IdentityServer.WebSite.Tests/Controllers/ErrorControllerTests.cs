using System;
using System.Threading.Tasks;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class ErrorControllerTests
    {
        private readonly Mock<IIdentityServerInteractionService> _mockIdentityServerInteractionService;

        private readonly ErrorController _controller;

        public ErrorControllerTests()
        {
            _mockIdentityServerInteractionService = new Mock<IIdentityServerInteractionService>(MockBehavior.Strict);

            _controller = new ErrorController(_mockIdentityServerInteractionService.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
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
        public async Task Index_ReturnViewResultWithErrorMessage()
        {
            // Arrange
            const string errorId = "errorId";
            var errorMessage = new IdentityServer4.Models.ErrorMessage();

            _mockIdentityServerInteractionService
                .Setup(x => x.GetErrorContextAsync(errorId))
                .ReturnsAsync(errorMessage)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(errorId);

            // Assert
            _mockIdentityServerInteractionService.Verify();

            MvcAssert.ViewResult(actionResult, "Error", errorMessage);
        }

        [Fact]
        public async Task Index_ReturnViewResult_WhenErrorMessageIsNull()
        {
            // Arrange
            _mockIdentityServerInteractionService
                .Setup(x => x.GetErrorContextAsync(null))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await _controller.Index(null);

            // Assert
            _mockIdentityServerInteractionService.Verify();

            MvcAssert.ViewResult(actionResult, "Error");
        }
    }
}