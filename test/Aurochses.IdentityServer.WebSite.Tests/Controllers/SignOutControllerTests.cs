using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.Controllers;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Models.SignOut;
using Aurochses.IdentityServer.WebSite.Tests.Fakes;
using Aurochses.Runtime;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using AuthenticationProperties = Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties;

namespace Aurochses.IdentityServer.WebSite.Tests.Controllers
{
    public class SignOutControllerTests
    {
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<IIdentityServerInteractionService> _mockIdentityServerInteractionService;

        private readonly Mock<AuthenticationManager> _mockAuthenticationManager;
        private readonly Mock<IUrlHelper> _mockUrlHelper;

        private readonly SignOutController _controller;

        public SignOutControllerTests()
        {
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<ApplicationUser>>>().Object, new Mock<IAuthenticationSchemeProvider>().Object);
            _mockIdentityServerInteractionService = new Mock<IIdentityServerInteractionService>(MockBehavior.Strict);

            _mockAuthenticationManager = new Mock<AuthenticationManager>(MockBehavior.Strict);
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            _controller = new SignOutController(_mockSignInManager.Object, _mockIdentityServerInteractionService.Object);
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<SignOutController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        private static SignedOutViewModel GetSignedOutViewModel(LogoutRequest logoutRequest, string logoutId, string identityProvider)
        {
            return new SignedOutViewModel
            {
                PostLogoutRedirectUri = logoutRequest?.PostLogoutRedirectUri,
                ClientName = logoutRequest?.ClientId,
                SignOutIframeUrl = logoutRequest?.SignOutIFrameUrl,
                AutomaticRedirectAfterSignOut = true,
                LogoutId = logoutId,
                ExternalAuthenticationScheme = identityProvider
            };
        }

        private void Setup(string logoutId, LogoutRequest logoutRequest, ClaimsPrincipal user)
        {
            _mockIdentityServerInteractionService
                .Setup(x => x.GetLogoutContextAsync(logoutId))
                .ReturnsAsync(logoutRequest)
                .Verifiable();

            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider
                .Setup(provider => provider.GetService(typeof(IdentityServerOptions)))
                .Returns(() => new IdentityServerOptions());

            _mockAuthenticationManager
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>()))
                .ReturnsAsync(() => user);

            var httpContext = new FakeHttpContext(_mockAuthenticationManager.Object)
            {
                RequestServices = mockServiceProvider.Object
            };

            _controller.ControllerContext.HttpContext = httpContext;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("logoutId")]
        public async Task Index_ReturnSignedOutViewResultWithSignedOutViewModel_WhenUserIsNull(string logoutId)
        {
            // Arrange
            var logoutRequest = new LogoutRequest(null, new LogoutMessage());

            Setup(logoutId, logoutRequest, null);

            _mockSignInManager
                .Setup(x => x.SignOutAsync())
                .Returns(() => Task.FromResult(0))
                .Verifiable();

            _controller.TempData =
                new TempDataDictionaryFactory(new SessionStateTempDataProvider())
                    .GetTempData(
                        _controller.ControllerContext.HttpContext
                    );

            // Act
            var actionResult = await _controller.Index(logoutId);

            // Assert
            _mockIdentityServerInteractionService.Verify();
            _mockSignInManager.Verify();

            MvcAssert.ViewResult(actionResult, "SignedOut", GetSignedOutViewModel(logoutRequest, logoutId, null));
        }

        private void SetupAuthenticationSignOutAsync(string logoutId, string identityProvider)
        {
            const string redirectUri = "redirectUri";

            _mockUrlHelper
                .Setup
                (
                    x => x.Action
                    (
                        It.Is<UrlActionContext>
                        (
                            context => context.Action == "Index"
                                       && context.Controller == "SignOut"
                                       && context.Values.ValueEquals(new { LogoutId = logoutId })
                        )
                    )
                )
                .Returns(redirectUri)
                .Verifiable();

            _controller.Url = _mockUrlHelper.Object;

            _mockAuthenticationManager
                .Setup(
                    x =>
                        x.SignOutAsync(
                            identityProvider,
                            It.Is<AuthenticationProperties>
                            (
                                properties => properties.RedirectUri == redirectUri
                            )
                        )
                )
                .Returns(() => Task.FromResult(0))
                .Verifiable();

        }

        //[Fact]
        //public async Task Index_ReturnSignedOutViewResultWithSignedOutViewModel_WhenUserIsNotNull()
        //{
        //    // Arrange
        //    const string logoutId = "logoutId";
        //    var logoutRequest = new LogoutRequest(null, new LogoutMessage());
        //    const string identityProvider = "identityProvider";

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtClaimTypes.IdentityProvider, identityProvider)
        //    };

        //    Setup(logoutId, logoutRequest, new ClaimsPrincipal(new ClaimsIdentity(claims)));

        //    SetupAuthenticationSignOutAsync(logoutId, identityProvider);

        //    _mockSignInManager
        //        .Setup(x => x.SignOutAsync())
        //        .Returns(() => Task.FromResult(0))
        //        .Verifiable();

        //    _controller.TempData =
        //        new TempDataDictionaryFactory(new SessionStateTempDataProvider())
        //            .GetTempData(
        //                _controller.ControllerContext.HttpContext
        //            );

        //    // Act
        //    var actionResult = await _controller.Index(logoutId);

        //    // Assert
        //    _mockIdentityServerInteractionService.Verify();
        //    _mockUrlHelper.Verify();
        //    _mockAuthenticationManager.Verify();
        //    _mockSignInManager.Verify();

        //    MvcAssert.ViewResult(actionResult, "SignedOut", GetSignedOutViewModel(logoutRequest, logoutId, identityProvider));
        //}

        //[Fact]
        //public async Task Index_ReturnSignedOutViewResultWithSignedOutViewModel_WhenUserIsNotNullAndLogoutIdIsNull()
        //{
        //    // Arrange
        //    const string logoutId = null;
        //    const string newLogoutId = "logoutId";
        //    var logoutRequest = new LogoutRequest(null, new LogoutMessage());
        //    const string identityProvider = "identityProvider";

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtClaimTypes.IdentityProvider, identityProvider)
        //    };

        //    Setup(logoutId, logoutRequest, new ClaimsPrincipal(new ClaimsIdentity(claims)));

        //    _mockIdentityServerInteractionService
        //        .Setup(x => x.CreateLogoutContextAsync())
        //        .ReturnsAsync(() => newLogoutId)
        //        .Verifiable();

        //    SetupAuthenticationSignOutAsync(newLogoutId, identityProvider);

        //    _mockSignInManager
        //        .Setup(x => x.SignOutAsync())
        //        .Returns(() => Task.FromResult(0))
        //        .Verifiable();

        //    _controller.TempData =
        //        new TempDataDictionaryFactory(new SessionStateTempDataProvider())
        //            .GetTempData(
        //                _controller.ControllerContext.HttpContext
        //            );

        //    // Act
        //    var actionResult = await _controller.Index(logoutId);

        //    // Assert
        //    _mockIdentityServerInteractionService.Verify();
        //    _mockSignInManager.Verify();

        //    MvcAssert.ViewResult(actionResult, "SignedOut", GetSignedOutViewModel(logoutRequest, newLogoutId, identityProvider));
        //}
    }
}