using System.Collections.Generic;
using System.Linq;
using Aurochses.IdentityServer.Website.Models.Login;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.Login
{
    public class LoginViewModelTests
    {
        private readonly LoginViewModel _loginViewModel;

        public LoginViewModelTests()
        {
            _loginViewModel = new LoginViewModel();
        }

        [Fact]
        public void Inherit_LoginInputModel()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<LoginInputModel>(_loginViewModel);
        }

        [Fact]
        public void AllowRememberLogin_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _loginViewModel.AllowRememberLogin);
        }

        [Fact]
        public void AllowRememberLogin_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _loginViewModel.AllowRememberLogin = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loginViewModel.AllowRememberLogin);
        }

        [Fact]
        public void EnableLocalLogin_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default, _loginViewModel.EnableLocalLogin);
        }

        [Fact]
        public void EnableLocalLogin_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _loginViewModel.EnableLocalLogin = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loginViewModel.EnableLocalLogin);
        }

        [Fact]
        public void ExternalProviders_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(Enumerable.Empty<ExternalProvider>(), _loginViewModel.ExternalProviders);
        }

        [Fact]
        public void ExternalProviders_Set_Success()
        {
            // Arrange
            var expectedValue = new List<ExternalProvider>
            {
                new ExternalProvider
                {
                    DisplayName = "Test DisplayName",
                    AuthenticationScheme = "Test AuthenticationScheme"
                }
            };

            // Act
            _loginViewModel.ExternalProviders = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loginViewModel.ExternalProviders);
        }

        [Fact]
        public void VisibleExternalProviders_Get_Success()
        {
            // Arrange
            var externalProviders = new List<ExternalProvider>
            {
                new ExternalProvider
                {
                    DisplayName = null,
                    AuthenticationScheme = "Test AuthenticationScheme 1"
                },
                new ExternalProvider
                {
                    DisplayName = "",
                    AuthenticationScheme = "Test AuthenticationScheme 2"
                },
                new ExternalProvider
                {
                    DisplayName = " ",
                    AuthenticationScheme = "Test AuthenticationScheme 3"
                },
                new ExternalProvider
                {
                    DisplayName = "Test DisplayName 4",
                    AuthenticationScheme = "Test AuthenticationScheme 4"
                },
                new ExternalProvider
                {
                    DisplayName = "Test DisplayName 5",
                    AuthenticationScheme = "Test AuthenticationScheme 5"
                }
            };

            var expectedValue = new List<ExternalProvider>
            {
                new ExternalProvider
                {
                    DisplayName = "Test DisplayName 4",
                    AuthenticationScheme = "Test AuthenticationScheme 4"
                },
                new ExternalProvider
                {
                    DisplayName = "Test DisplayName 5",
                    AuthenticationScheme = "Test AuthenticationScheme 5"
                }
            };

            // Act
            _loginViewModel.ExternalProviders = externalProviders;

            // Assert
            ObjectAssert.DeepEquals(expectedValue, _loginViewModel.VisibleExternalProviders);
        }

        public static IEnumerable<object[]> IsExternalLoginOnlyMemberData => new[]
        {
            new object[]
            {
                new LoginViewModel(),
                false
            },
            new object[]
            {
                new LoginViewModel
                {
                    EnableLocalLogin = false
                },
                false
            },
            new object[]
            {
                new LoginViewModel
                {
                    EnableLocalLogin = true
                },
                false
            },
            new object[]
            {
                new LoginViewModel
                {
                    ExternalProviders = null
                },
                false
            },
            new object[]
            {
                new LoginViewModel
                {
                    EnableLocalLogin = true,
                    ExternalProviders = new List<ExternalProvider>
                    {
                        new ExternalProvider()
                    }
                },
                false
            },
            new object[]
            {
                new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ExternalProviders = new List<ExternalProvider>
                    {
                        new ExternalProvider(),
                        new ExternalProvider()
                    }
                },
                false
            },
            new object[]
            {
                new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ExternalProviders = new List<ExternalProvider>
                    {
                        new ExternalProvider()
                    }
                },
                true
            }
        };

        [Theory]
        [MemberData(nameof(IsExternalLoginOnlyMemberData))]
        public void IsExternalLoginOnly_Get_Success(LoginViewModel loginViewModel, bool expectedValue)
        {
            // Arrange & Act & Assert
            ObjectAssert.DeepEquals(expectedValue, loginViewModel.IsExternalLoginOnly);
        }

        public static IEnumerable<object[]> ExternalLoginSchemeMemberData => new[]
        {
            new object[]
            {
                new LoginViewModel(),
                null
            },
            new object[]
            {
                new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ExternalProviders = new List<ExternalProvider>
                    {
                        new ExternalProvider(),
                        new ExternalProvider()
                    }
                },
                null
            },
            new object[]
            {
                new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ExternalProviders = new List<ExternalProvider>
                    {
                        null
                    }
                },
                null
            },
            new object[]
            {
                new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ExternalProviders = new List<ExternalProvider>
                    {
                        new ExternalProvider
                        {
                            AuthenticationScheme = "Test AuthenticationScheme"
                        }
                    }
                },
                "Test AuthenticationScheme"
            }
        };

        [Theory]
        [MemberData(nameof(ExternalLoginSchemeMemberData))]
        public void ExternalLoginScheme_Get_Success(LoginViewModel loginViewModel, string expectedValue)
        {
            // Arrange & Act & Assert
            ObjectAssert.DeepEquals(expectedValue, loginViewModel.ExternalLoginScheme);
        }
    }
}