using System;
using System.Collections.Generic;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.Testing;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Aurochses.Testing.Mvc;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<HomeController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            var controller = new HomeController(new HostingEnvironment());

            Assert.IsAssignableFrom<Controller>(controller);
        }

        [Fact]
        public void Index_ReturnViewResult_WhenHostingEnvironmentIsDevelopment()
        {
            // Arrange
            var controller = new HomeController(new HostingEnvironment { EnvironmentName = "Development" });

            // Act
            var actionResult = controller.Index();

            // Assert
            MvcAssert.ViewResult(actionResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("http://www.example.com")]
        public void Index_RedirectToActionResult_WhenHostingEnvironmentIsNotDevelopment(string returnUrl)
        {
            // Arrange
            var controller = new HomeController(new HostingEnvironment());

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