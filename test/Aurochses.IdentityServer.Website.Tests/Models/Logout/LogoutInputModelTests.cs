using Aurochses.IdentityServer.Website.Models.Logout;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.Logout
{
    public class LogoutInputModelTests
    {
        private readonly LogoutInputModel _logoutInputModel;

        public LogoutInputModelTests()
        {
            _logoutInputModel = new LogoutInputModel();
        }

        [Fact]
        public void LogoutId_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _logoutInputModel.LogoutId);
        }

        [Fact]
        public void LogoutId_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _logoutInputModel.LogoutId = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _logoutInputModel.LogoutId);
        }
    }
}