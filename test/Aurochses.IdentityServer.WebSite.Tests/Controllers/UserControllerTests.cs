using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.App.Project;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

        public UserControllerTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
        }

        [Theory]
        [InlineData(typeof(AuthorizeAttribute))]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<UserController>(attributeType);
        }

        private UserController GetUserController(ProjectOptions projectOptions)
        {
            var mockProjectOptions = new Mock<IOptions<ProjectOptions>>(MockBehavior.Strict);

            mockProjectOptions
                .SetupGet(x => x.Value)
                .Returns(projectOptions);

            return new UserController(_mockUserManager.Object, mockProjectOptions.Object);
        }


        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(GetUserController(new ProjectOptions()));
        }

        [Fact]
        public async Task Index_RedirectToActionResult_WhenUserIsNull()
        {
            // Arrange
            _mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            // Act
            var actionResult = await GetUserController(new ProjectOptions()).Index();

            // Assert
            _mockUserManager.Verify();

            MvcAssert.RedirectToActionResult(actionResult, "Index", "Error");
        }

        [Fact]
        public async Task Index_RedirectToAccountUrl_WhenAccountUrlIsNotNull()
        {
            // Arrange
            _mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => new ApplicationUser())
                .Verifiable();

            var projectOptions = new ProjectOptions
            {
                AccountUrl = "https://myaccount.example.com"
            };

            // Act
            var actionResult = await GetUserController(projectOptions).Index();

            // Assert
            _mockUserManager.Verify();

            MvcAssert.RedirectResult(actionResult, projectOptions.AccountUrl);
        }

        [Fact]
        public async Task Index_ReturnViewResultWithApplicationUser_WhenUserIsNotNull()
        {
            // Arrange
            var user = new ApplicationUser();

            _mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(() => user)
                .Verifiable();

            // Act
            var actionResult = await GetUserController(new ProjectOptions()).Index();

            // Assert
            _mockUserManager.Verify();

            MvcAssert.ViewResult(actionResult, null, user);
        }
    }
}