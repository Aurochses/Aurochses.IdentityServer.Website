using System;
using System.Collections.Generic;
using Aurochses.IdentityServer.Website.Controllers;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Controllers
{
    public class HomeControllerTests
    {
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
            // Arrange & Act
            var controller = new HomeController(new NullLogger<HomeController>(), new HostingEnvironment());

            // Assert
            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void Index_WhenHostingEnvironmentIsDevelopment_ReturnViewResult()
        {
            // Arrange
            var controller = new HomeController(new NullLogger<HomeController>(), new HostingEnvironment {EnvironmentName = "Development"});

            // Act
            var actionResult = controller.Index(string.Empty);

            // Assert
            MvcAssert.ViewResult(actionResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public void Index_WhenHostingEnvironmentIsNotDevelopment_RedirectToActionResult(string returnUrl)
        {
            // Arrange
            var controller = new HomeController(new NullLogger<HomeController>(), new HostingEnvironment());

            // Act
            var actionResult = controller.Index(returnUrl);

            // Assert
            MvcAssert.RedirectToActionResult(
                actionResult,
                "Index",
                "SignIn",
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("ReturnUrl", returnUrl)
                }
            );
        }
    }
}