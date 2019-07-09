using Aurochses.IdentityServer.Website.Options;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Options
{
    public class AccountOptionsTests
    {
        private readonly AccountOptions _accountOptions;

        public AccountOptionsTests()
        {
            _accountOptions = new AccountOptions();
        }

        [Fact]
        public void AllowLocalLogin_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _accountOptions.AllowLocalLogin);
        }

        [Fact]
        public void AllowLocalLogin_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _accountOptions.AllowLocalLogin = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _accountOptions.AllowLocalLogin);
        }

        [Fact]
        public void AllowRememberLogin_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _accountOptions.AllowRememberLogin);
        }

        [Fact]
        public void AllowRememberLogin_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _accountOptions.AllowRememberLogin = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _accountOptions.AllowRememberLogin);
        }

        [Fact]
        public void WindowsAuthenticationSchemeName_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _accountOptions.WindowsAuthenticationSchemeName);
        }

        [Fact]
        public void DisplayName_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _accountOptions.WindowsAuthenticationSchemeName = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _accountOptions.WindowsAuthenticationSchemeName);
        }

        [Fact]
        public void LockoutOnFailure_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _accountOptions.LockoutOnFailure);
        }

        [Fact]
        public void LockoutOnFailure_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _accountOptions.LockoutOnFailure = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _accountOptions.LockoutOnFailure);
        }

        [Fact]
        public void RequireEmailConfirmation_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _accountOptions.RequireEmailConfirmation);
        }

        [Fact]
        public void RequireEmailConfirmation_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _accountOptions.RequireEmailConfirmation = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _accountOptions.RequireEmailConfirmation);
        }

        [Fact]
        public void AllowTwoFactorAuthentication_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _accountOptions.AllowTwoFactorAuthentication);
        }

        [Fact]
        public void AllowTwoFactorAuthentication_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _accountOptions.AllowTwoFactorAuthentication = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _accountOptions.AllowTwoFactorAuthentication);
        }
    }
}