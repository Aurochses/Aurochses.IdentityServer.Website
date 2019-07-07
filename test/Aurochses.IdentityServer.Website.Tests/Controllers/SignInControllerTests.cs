﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurochses.IdentityServer.Website.Controllers;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.IdentityServer.Website.Models.SignIn;
using Aurochses.IdentityServer.Website.Options;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Controllers
{
    public class SignInControllerTests : ControllerTestsBase<SignInController>
    {
        private const string WindowsAuthenticationSchemeName = "Test WindowsAuthenticationSchemeName";

        private readonly Mock<IIdentityServerInteractionService> _mockIdentityServerInteractionService;
        private readonly Mock<IAuthenticationSchemeProvider> _mockAuthenticationSchemeProvider;
        private readonly Mock<IClientStore> _mockClientStore;

        private readonly SignInController _controller;

        public SignInControllerTests()
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
            _mockAuthenticationSchemeProvider = new Mock<IAuthenticationSchemeProvider>(MockBehavior.Strict);
            _mockClientStore = new Mock<IClientStore>(MockBehavior.Strict);

            _controller = new SignInController(
                MockLogger.Object,
                mockAccountOptions.Object,
                _mockIdentityServerInteractionService.Object,
                _mockAuthenticationSchemeProvider.Object,
                _mockClientStore.Object
            );
        }

        [Theory]
        [InlineData(typeof(SecurityHeadersAttribute))]
        [InlineData(typeof(AllowAnonymousAttribute))]
        public void Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.HasAttribute<SignInController>(attributeType);
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Controller>(_controller);
        }

        [Fact]
        public async Task Index_WhenIdentityServerInteractionServiceAuthorizationContextIdPIsNotNull_ReturnViewResult()
        {
            // Arrange
            const string returnUrl = "Test ReturnUrl";
            const string idP = "Test IdP";

            _mockIdentityServerInteractionService
                .Setup(x => x.GetAuthorizationContextAsync(returnUrl))
                .ReturnsAsync(new AuthorizationRequest {IdP = idP});

            // Act
            var actionResult = await _controller.Index(returnUrl);

            // Assert
            VerifyLogger(LogLevel.Information, Times.Once);

            MvcAssert.RedirectToActionResult(
                actionResult,
                "Challenge",
                "External",
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("Provider", idP),
                    new KeyValuePair<string, object>("ReturnUrl", returnUrl)
                }
            );
        }

        [Fact]
        public async Task Index_WhenIdentityServerInteractionServiceAuthorizationContextClientIdIsNotNull_ReturnViewResult()
        {
            // Arrange
            const string returnUrl = "Test ReturnUrl";
            const string clientId = "Test ClientId";
            const string loginHint = "Test LoginHint";

            _mockIdentityServerInteractionService
                .Setup(x => x.GetAuthorizationContextAsync(returnUrl))
                .ReturnsAsync(new AuthorizationRequest {ClientId = clientId, LoginHint = loginHint});

            _mockAuthenticationSchemeProvider
                .Setup(x => x.GetAllSchemesAsync())
                .ReturnsAsync(
                    new List<AuthenticationScheme>
                    {
                        new AuthenticationScheme("Test Name", "Test DisplayName", typeof(IAuthenticationHandler)),
                        new AuthenticationScheme(WindowsAuthenticationSchemeName, null, typeof(IAuthenticationHandler))
                    }
                );

            _mockClientStore
                .Setup(x => x.FindClientByIdAsync(clientId))
                .ReturnsAsync(
                    new Client
                    {
                        Enabled = true,
                        EnableLocalLogin = true,
                        IdentityProviderRestrictions = new List<string>
                        {
                            WindowsAuthenticationSchemeName
                        }
                    }
                );

            var expectedModel = new SignInViewModel
            {
                UserName = loginHint,
                ReturnUrl = returnUrl,

                AllowRememberLogin = true,
                EnableLocalLogin = true,

                ExternalProviders = new[]
                {
                    new ExternalProvider
                    {
                        DisplayName = null,
                        AuthenticationScheme = WindowsAuthenticationSchemeName
                    }
                }
            };

            // Act
            var actionResult = await _controller.Index(returnUrl);

            // Assert
            MvcAssert.ViewResult(actionResult, model: expectedModel);
        }

        [Fact]
        public async Task Index_WhenIdentityServerInteractionServiceAuthorizationContextClientIdIsNotNull_And_ClientEnableLocalLoginIsFalse_ReturnViewResult()
        {
            // Arrange
            const string returnUrl = "Test ReturnUrl";
            const string clientId = "Test ClientId";
            const string loginHint = "Test LoginHint";

            _mockIdentityServerInteractionService
                .Setup(x => x.GetAuthorizationContextAsync(returnUrl))
                .ReturnsAsync(new AuthorizationRequest { ClientId = clientId, LoginHint = loginHint });

            _mockAuthenticationSchemeProvider
                .Setup(x => x.GetAllSchemesAsync())
                .ReturnsAsync(
                    new List<AuthenticationScheme>
                    {
                        new AuthenticationScheme("Test Name", "Test DisplayName", typeof(IAuthenticationHandler)),
                        new AuthenticationScheme(WindowsAuthenticationSchemeName, null, typeof(IAuthenticationHandler))
                    }
                );

            _mockClientStore
                .Setup(x => x.FindClientByIdAsync(clientId))
                .ReturnsAsync(
                    new Client
                    {
                        Enabled = true,
                        EnableLocalLogin = false,
                        IdentityProviderRestrictions = new List<string>
                        {
                            WindowsAuthenticationSchemeName
                        }
                    }
                );

            // Act
            var actionResult = await _controller.Index(returnUrl);

            // Assert
            VerifyLogger(LogLevel.Information, Times.Once);

            MvcAssert.RedirectToActionResult(
                actionResult,
                "Challenge",
                "External",
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("Provider", WindowsAuthenticationSchemeName),
                    new KeyValuePair<string, object>("ReturnUrl", returnUrl)
                }
            );
        }

        [Fact]
        public async Task Index_WhenIdentityServerInteractionServiceAuthorizationContextClientIdIsNotNull_And_IdentityProviderRestrictionsIsNull_ReturnViewResult()
        {
            // Arrange
            const string returnUrl = "Test ReturnUrl";
            const string clientId = "Test ClientId";
            const string loginHint = "Test LoginHint";

            _mockIdentityServerInteractionService
                .Setup(x => x.GetAuthorizationContextAsync(returnUrl))
                .ReturnsAsync(new AuthorizationRequest { ClientId = clientId, LoginHint = loginHint });

            _mockAuthenticationSchemeProvider
                .Setup(x => x.GetAllSchemesAsync())
                .ReturnsAsync(
                    new List<AuthenticationScheme>
                    {
                        new AuthenticationScheme("Test Name", "Test DisplayName", typeof(IAuthenticationHandler)),
                        new AuthenticationScheme(WindowsAuthenticationSchemeName, null, typeof(IAuthenticationHandler))
                    }
                );

            _mockClientStore
                .Setup(x => x.FindClientByIdAsync(clientId))
                .ReturnsAsync(
                    new Client
                    {
                        Enabled = true,
                        EnableLocalLogin = true,
                        IdentityProviderRestrictions = null
                    }
                );

            var expectedModel = new SignInViewModel
            {
                UserName = loginHint,
                ReturnUrl = returnUrl,

                AllowRememberLogin = true,
                EnableLocalLogin = true,

                ExternalProviders = new[]
                {
                    new ExternalProvider
                    {
                        DisplayName = "Test DisplayName",
                        AuthenticationScheme = "Test Name"
                    },
                    new ExternalProvider
                    {
                        DisplayName = null,
                        AuthenticationScheme = WindowsAuthenticationSchemeName
                    }
                }
            };

            // Act
            var actionResult = await _controller.Index(returnUrl);

            // Assert
            MvcAssert.ViewResult(actionResult, model: expectedModel);
        }

        [Fact]
        public async Task Index_WhenIdentityServerInteractionServiceAuthorizationContextIsNull_ReturnViewResult()
        {
            // Arrange
            const string returnUrl = "Test ReturnUrl";

            _mockIdentityServerInteractionService
                .Setup(x => x.GetAuthorizationContextAsync(returnUrl))
                .ReturnsAsync(() => null);

            _mockAuthenticationSchemeProvider
                .Setup(x => x.GetAllSchemesAsync())
                .ReturnsAsync(
                    new List<AuthenticationScheme>
                    {
                        new AuthenticationScheme("Test Name", "Test DisplayName", typeof(IAuthenticationHandler)),
                        new AuthenticationScheme(WindowsAuthenticationSchemeName, null, typeof(IAuthenticationHandler))
                    }
                );

            var expectedModel = new SignInViewModel
            {
                UserName = null,
                ReturnUrl = returnUrl,

                AllowRememberLogin = true,
                EnableLocalLogin = true,

                ExternalProviders = new[]
                {
                    new ExternalProvider
                    {
                        DisplayName = "Test DisplayName",
                        AuthenticationScheme = "Test Name"
                    },
                    new ExternalProvider
                    {
                        DisplayName = null,
                        AuthenticationScheme = WindowsAuthenticationSchemeName
                    }
                }
            };

            // Act
            var actionResult = await _controller.Index(returnUrl);

            // Assert
            MvcAssert.ViewResult(actionResult, model: expectedModel);
        }
    }
}