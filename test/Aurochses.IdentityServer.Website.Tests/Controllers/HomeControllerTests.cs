using System;
using System.Collections.Generic;
using Aurochses.IdentityServer.Website.Controllers;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.IdentityServer.Website.Tests.Fakes;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Controllers
{
    public class HomeControllerTests : ControllerTestsBase<HomeController>
    {
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _controller = new HomeController(
                MockLogger.Object,
                MockHostingEnvironment.Object
            );
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        [InlineData(typeof(AllowAnonymousAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<HomeController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Fact]
        public void Index_WhenIsDevelopment_ReturnViewResult()
        {
            // Arrange
            SetupHostingEnvironmentName("Development");

            // Act
            var actionResult = _controller.Index(string.Empty);

            // Assert
            MvcAssert.ViewResult(actionResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public void Index_WhenIsNotDevelopment_RedirectToActionResult(string returnUrl)
        {
            // Arrange & Act
            var actionResult = _controller.Index(returnUrl);

            // Assert
            VerifyLogger(LogLevel.Information, Times.Once);

            MvcAssert.RedirectToActionResult(
                actionResult,
                "Index",
                "Login",
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("ReturnUrl", returnUrl)
                }
            );
        }
    }
}