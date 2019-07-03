using System;
using Aurochses.IdentityServer.Website.Controllers;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.IdentityServer.Website.Options;
using Aurochses.Xunit;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Controllers
{
    public class SignInControllerTests
    {
        private readonly Mock<IOptions<AccountOptions>> _mockAccountOptions;
        private readonly Mock<IIdentityServerInteractionService> _mockIdentityServerInteractionService;
        private readonly Mock<IAuthenticationSchemeProvider> _mockAuthenticationSchemeProvider;
        private readonly Mock<IClientStore> _mockClientStore;

        private readonly SignInController _controller;

        public SignInControllerTests()
        {
            _mockAccountOptions = new Mock<IOptions<AccountOptions>>(MockBehavior.Strict);
            _mockAccountOptions
                .SetupGet(x => x.Value)
                .Returns(new AccountOptions());

            _mockIdentityServerInteractionService = new Mock<IIdentityServerInteractionService>(MockBehavior.Strict);
            _mockAuthenticationSchemeProvider = new Mock<IAuthenticationSchemeProvider>(MockBehavior.Strict);
            _mockClientStore = new Mock<IClientStore>(MockBehavior.Strict);

            _controller = new SignInController(
                new NullLogger<SignInController>(),
                _mockAccountOptions.Object,
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
    }
}