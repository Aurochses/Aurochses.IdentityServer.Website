using System.Threading.Tasks;
using Aurochses.IdentityServer.Website.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Extensions
{
    public class ClientStoreExtensionsTests
    {
        private readonly Mock<IClientStore> _mockClientStore;

        public ClientStoreExtensionsTests()
        {
            _mockClientStore = new Mock<IClientStore>(MockBehavior.Strict);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task IsPkceClient_WhenClientIdIsNullOrWhiteSpace_ReturnFalse(string clientId)
        {
            // Arrange & Act & Assert
            Assert.False(await _mockClientStore.Object.IsPkceClient(clientId));
        }

        [Fact]
        public async Task IsPkceClient_FindEnabledClientByIdAsyncIsNull_ReturnFalse()
        {
            // Arrange
            const string clientId = "Test ClientId";

            _mockClientStore
                .Setup(x => x.FindClientByIdAsync(clientId))
                .ReturnsAsync(() => null);

            // Act & Assert
            Assert.False(await _mockClientStore.Object.IsPkceClient(clientId));
        }

        [Fact]
        public async Task IsPkceClient_ClientRequirePkceIsFalse_ReturnFalse()
        {
            // Arrange
            const string clientId = "Test ClientId";

            _mockClientStore
                .Setup(x => x.FindClientByIdAsync(clientId))
                .ReturnsAsync(
                    new Client
                    {
                        RequirePkce = false
                    }
                );

            // Act & Assert
            Assert.False(await _mockClientStore.Object.IsPkceClient(clientId));
        }

        [Fact]
        public async Task IsPkceClient_ClientRequirePkceIsTrue_ReturnTrue()
        {
            // Arrange
            const string clientId = "Test ClientId";

            _mockClientStore
                .Setup(x => x.FindClientByIdAsync(clientId))
                .ReturnsAsync(
                    new Client
                    {
                        RequirePkce = true
                    }
                );

            // Act & Assert
            Assert.True(await _mockClientStore.Object.IsPkceClient(clientId));
        }
    }
}