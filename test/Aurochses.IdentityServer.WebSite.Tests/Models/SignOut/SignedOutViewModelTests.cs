using Aurochses.IdentityServer.WebSite.Models.SignOut;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.SignOut
{
    public class SignedOutViewModelTests
    {
        private readonly SignedOutViewModel _viewModel;

        public SignedOutViewModelTests()
        {
            _viewModel = new SignedOutViewModel();
        }

        [Fact]
        public void PostLogoutRedirectUri_Success()
        {
            // Arrange
            const string value = "postLogoutRedirectUri";

            // Act
            _viewModel.PostLogoutRedirectUri = value;

            // Assert
            Assert.Equal(value, _viewModel.PostLogoutRedirectUri);
        }

        [Fact]
        public void ClientName_Success()
        {
            // Arrange
            const string value = "clientName";

            // Act
            _viewModel.ClientName = value;

            // Assert
            Assert.Equal(value, _viewModel.ClientName);
        }

        [Fact]
        public void SignOutIframeUrl_Success()
        {
            // Arrange
            const string value = "signOutIframeUrl";

            // Act
            _viewModel.SignOutIframeUrl = value;

            // Assert
            Assert.Equal(value, _viewModel.SignOutIframeUrl);
        }

        [Fact]
        public void AutomaticRedirectAfterSignOut_Success()
        {
            // Arrange
            const bool value = true;

            // Act
            _viewModel.AutomaticRedirectAfterSignOut = value;

            // Assert
            Assert.Equal(value, _viewModel.AutomaticRedirectAfterSignOut);
        }

        [Fact]
        public void LogoutId_Success()
        {
            // Arrange
            const string value = "logoutId";

            // Act
            _viewModel.LogoutId = value;

            // Assert
            Assert.Equal(value, _viewModel.LogoutId);
        }

        [Fact]
        public void ExternalAuthenticationScheme_Success()
        {
            // Arrange
            const string value = "externalAuthenticationScheme";

            // Act
            _viewModel.ExternalAuthenticationScheme = value;

            // Assert
            Assert.Equal(value, _viewModel.ExternalAuthenticationScheme);
        }

        [Fact]
        public void TriggerExternalSignout_RetrunTrue_WhenExternalAuthenticationSchemeIsNotNull()
        {
            // Arrange
            var model = new SignedOutViewModel
            {
                ExternalAuthenticationScheme = "externalAuthenticationScheme"
            };

            // Act & Assert
            Assert.True(model.TriggerExternalSignout);
        }

        [Fact]
        public void TriggerExternalSignout_RetrunFalse_WhenExternalAuthenticationSchemeIsNull()
        {
            // Arrange & Act & Assert
            Assert.False(_viewModel.TriggerExternalSignout);
        }
    }
}