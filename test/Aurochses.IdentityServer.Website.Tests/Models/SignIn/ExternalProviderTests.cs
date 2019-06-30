using Aurochses.IdentityServer.Website.Models.SignIn;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.SignIn
{
    public class ExternalProviderTests
    {
        private readonly ExternalProvider _externalProvider;

        public ExternalProviderTests()
        {
            _externalProvider = new ExternalProvider();
        }

        [Fact]
        public void DisplayName_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(string), _externalProvider.DisplayName);
        }

        [Fact]
        public void DisplayName_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _externalProvider.DisplayName = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _externalProvider.DisplayName);
        }

        [Fact]
        public void AuthenticationScheme_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(string), _externalProvider.AuthenticationScheme);
        }

        [Fact]
        public void AuthenticationScheme_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _externalProvider.AuthenticationScheme = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _externalProvider.AuthenticationScheme);
        }
    }
}