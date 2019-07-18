using System.Collections.Generic;
using Aurochses.IdentityServer.Website.Models.Login;
using Aurochses.IdentityServer.Website.Models.Logout;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.Logout
{
    public class LoggedOutViewModelTests
    {
        private readonly LoggedOutViewModel _loggedOutViewModel;

        public LoggedOutViewModelTests()
        {
            _loggedOutViewModel = new LoggedOutViewModel();
        }

        [Fact]
        public void PostLogoutRedirectUri_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _loggedOutViewModel.PostLogoutRedirectUri);
        }

        [Fact]
        public void PostLogoutRedirectUri_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _loggedOutViewModel.PostLogoutRedirectUri = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loggedOutViewModel.PostLogoutRedirectUri);
        }

        [Fact]
        public void ClientName_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _loggedOutViewModel.ClientName);
        }

        [Fact]
        public void ClientName_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _loggedOutViewModel.ClientName = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loggedOutViewModel.ClientName);
        }

        [Fact]
        public void SignOutIframeUrl_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _loggedOutViewModel.SignOutIframeUrl);
        }

        [Fact]
        public void SignOutIframeUrl_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _loggedOutViewModel.SignOutIframeUrl = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loggedOutViewModel.SignOutIframeUrl);
        }

        [Fact]
        public void AutomaticRedirectAfterSignOut_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _loggedOutViewModel.AutomaticRedirectAfterSignOut);
        }

        [Fact]
        public void AutomaticRedirectAfterSignOut_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _loggedOutViewModel.AutomaticRedirectAfterSignOut = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loggedOutViewModel.AutomaticRedirectAfterSignOut);
        }

        [Fact]
        public void LogoutId_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _loggedOutViewModel.LogoutId);
        }

        [Fact]
        public void LogoutId_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _loggedOutViewModel.LogoutId = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loggedOutViewModel.LogoutId);
        }

        public static IEnumerable<object[]> TriggerExternalSignOutMemberData => new[]
        {
            new object[]
            {
                new LoggedOutViewModel(),
                false
            },
            new object[]
            {
                new LoggedOutViewModel
                {
                    ExternalAuthenticationScheme = null
                },
                false
            },
            new object[]
            {
                new LoggedOutViewModel
                {
                    ExternalAuthenticationScheme = ""
                },
                false
            },
            new object[]
            {
                new LoggedOutViewModel
                {
                    ExternalAuthenticationScheme = " "
                },
                false
            },
            new object[]
            {
                new LoggedOutViewModel
                {
                    ExternalAuthenticationScheme = "Test ExternalAuthenticationScheme"
                },
                true
            },
        };

        [Theory]
        [MemberData(nameof(TriggerExternalSignOutMemberData))]
        public void TriggerExternalSignOut_Get_Success(LoggedOutViewModel loggedOutViewModel, bool expectedValue)
        {
            // Arrange & Act & Assert
            ObjectAssert.DeepEquals(expectedValue, loggedOutViewModel.TriggerExternalSignOut);
        }

        [Fact]
        public void ExternalAuthenticationScheme_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _loggedOutViewModel.ExternalAuthenticationScheme);
        }

        [Fact]
        public void ExternalAuthenticationScheme_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _loggedOutViewModel.ExternalAuthenticationScheme = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loggedOutViewModel.ExternalAuthenticationScheme);
        }
    }
}