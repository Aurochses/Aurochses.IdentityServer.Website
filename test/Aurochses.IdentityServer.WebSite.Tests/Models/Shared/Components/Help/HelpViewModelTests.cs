using Aurochses.IdentityServer.WebSite.Models.Shared.Components.Help;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.Shared.Components.Help
{
    public class HelpViewModelTests
    {
        private readonly HelpViewModel _viewModel;

        public HelpViewModelTests()
        {
            _viewModel = new HelpViewModel();
        }

        [Fact]
        public void SupportEmail_Success()
        {
            // Arrange
            const string value = "supportEmail";

            // Act
            _viewModel.SupportEmail = value;

            // Assert
            Assert.Equal(value, _viewModel.SupportEmail);
        }

        [Fact]
        public void SupportUrl_Success()
        {
            // Arrange
            const string value = "supportUrl";

            // Act
            _viewModel.SupportUrl = value;

            // Assert
            Assert.Equal(value, _viewModel.SupportUrl);
        }
    }
}