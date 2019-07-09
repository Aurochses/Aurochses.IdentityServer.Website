using Aurochses.IdentityServer.Website.Models.Shared;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.Shared
{
    public class RedirectViewModelTests
    {
        private readonly RedirectViewModel _redirectViewModel;

        public RedirectViewModelTests()
        {
            _redirectViewModel = new RedirectViewModel();
        }

        [Fact]
        public void RedirectUrl_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _redirectViewModel.RedirectUrl);
        }

        [Fact]
        public void RedirectUrl_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _redirectViewModel.RedirectUrl = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _redirectViewModel.RedirectUrl);
        }
    }
}