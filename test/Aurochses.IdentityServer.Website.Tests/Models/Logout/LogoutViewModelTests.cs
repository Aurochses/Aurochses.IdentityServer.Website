using Aurochses.IdentityServer.Website.Models.Logout;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.Logout
{
    public class LogoutViewModelTests
    {
        private readonly LogoutViewModel _logoutViewModel;

        public LogoutViewModelTests()
        {
            _logoutViewModel = new LogoutViewModel();
        }

        [Fact]
        public void Inherit_LogoutInputModel()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<LogoutInputModel>(_logoutViewModel);
        }

        [Fact]
        public void ShowLogoutPrompt_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _logoutViewModel.ShowLogoutPrompt);
        }

        [Fact]
        public void ShowLogoutPrompt_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _logoutViewModel.ShowLogoutPrompt = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _logoutViewModel.ShowLogoutPrompt);
        }
    }
}