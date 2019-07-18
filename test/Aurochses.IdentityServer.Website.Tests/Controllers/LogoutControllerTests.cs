using System;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.Website.Controllers;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.IdentityServer.Website.Options;
using Aurochses.IdentityServer.Website.Tests.Fakes;
using Aurochses.Xunit;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Controllers
{
    public class LogoutControllerTests : ControllerTestsBase<LogoutController>
    {
        private const string WindowsAuthenticationSchemeName = "Test WindowsAuthenticationSchemeName";

        private readonly Mock<IIdentityServerInteractionService> _mockIdentityServerInteractionService;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IEventService> _mockEventService;

        private readonly LogoutController _controller;

        public LogoutControllerTests()
        {
            var mockAccountOptions = new Mock<IOptions<AccountOptions>>(MockBehavior.Strict);
            mockAccountOptions
                .SetupGet(x => x.Value)
                .Returns(
                    new AccountOptions
                    {
                        AllowLocalLogin = true,
                        AllowRememberLogin = true,
                        WindowsAuthenticationSchemeName = WindowsAuthenticationSchemeName
                    }
                );

            _mockIdentityServerInteractionService = new Mock<IIdentityServerInteractionService>(MockBehavior.Strict);
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(_mockUserManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<ApplicationUser>>>().Object, new Mock<IAuthenticationSchemeProvider>().Object);
            _mockEventService = new Mock<IEventService>(MockBehavior.Strict);

            _controller = new LogoutController(
                MockLogger.Object,
                mockAccountOptions.Object,
                _mockIdentityServerInteractionService.Object,
                _mockSignInManager.Object,
                _mockEventService.Object
            );
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        [InlineData(typeof(AllowAnonymousAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<LogoutController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }
    }
}