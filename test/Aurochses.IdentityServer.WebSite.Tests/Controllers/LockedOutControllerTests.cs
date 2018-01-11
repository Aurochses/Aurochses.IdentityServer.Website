using System;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class LockedOutControllerTests
    {
        private readonly LockedOutController _controller;

        public LockedOutControllerTests()
        {
            _controller = new LockedOutController();
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<LockedOutController>(attributeType);
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
    }
}