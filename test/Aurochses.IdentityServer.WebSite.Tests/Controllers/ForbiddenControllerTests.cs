using System;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.Testing;
using Aurochses.Testing.Mvc;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class ForbiddenControllerTests
    {
        private readonly ForbiddenController _controller;

        public ForbiddenControllerTests()
        {
            _controller = new ForbiddenController();
        }
        
        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<ForbiddenController>(attributeType);
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
    }
}
