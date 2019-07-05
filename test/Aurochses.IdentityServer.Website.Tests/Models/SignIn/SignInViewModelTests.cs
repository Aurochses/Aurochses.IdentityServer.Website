using System.Collections.Generic;
using System.Linq;
using Aurochses.IdentityServer.Website.Models.SignIn;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.SignIn
{
    public class SignInViewModelTests
    {
        private readonly SignInViewModel _signInViewModel;

        public SignInViewModelTests()
        {
            _signInViewModel = new SignInViewModel();
        }

        [Fact]
        public void Inherit_SignInInputModel()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<SignInInputModel>(_signInViewModel);
        }

        [Fact]
        public void AllowRememberLogin_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(bool), _signInViewModel.AllowRememberLogin);
        }

        [Fact]
        public void AllowRememberLogin_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _signInViewModel.AllowRememberLogin = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _signInViewModel.AllowRememberLogin);
        }

        [Fact]
        public void EnableLocalLogin_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(bool), _signInViewModel.EnableLocalLogin);
        }

        [Fact]
        public void EnableLocalLogin_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _signInViewModel.EnableLocalLogin = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _signInViewModel.EnableLocalLogin);
        }

        [Fact]
        public void ExternalProviders_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(Enumerable.Empty<ExternalProvider>(), _signInViewModel.ExternalProviders);
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
            _signInViewModel.ExternalProviders = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _signInViewModel.ExternalProviders);
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
            _signInViewModel.ExternalProviders = externalProviders;

            // Assert
            ObjectAssert.DeepEquals(expectedValue, _signInViewModel.VisibleExternalProviders);
        }

        public static IEnumerable<object[]> IsExternalLoginOnlyMemberData => new[]
        {
            new object[]
            {
                new SignInViewModel(),
                false
            },
            new object[]
            {
                new SignInViewModel
                {
                    EnableLocalLogin = false
                },
                false
            },
            new object[]
            {
                new SignInViewModel
                {
                    EnableLocalLogin = true
                },
                false
            },
            new object[]
            {
                new SignInViewModel
                {
                    ExternalProviders = null
                },
                false
            },
            new object[]
            {
                new SignInViewModel
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
                new SignInViewModel
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
                new SignInViewModel
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
        public void IsExternalLoginOnly_Get_Success(SignInViewModel signInViewModel, bool expectedValue)
        {
            // Arrange & Act & Assert
            ObjectAssert.DeepEquals(expectedValue, signInViewModel.IsExternalLoginOnly);
        }

        public static IEnumerable<object[]> ExternalLoginSchemeMemberData => new[]
        {
            new object[]
            {
                new SignInViewModel(),
                null
            },
            new object[]
            {
                new SignInViewModel
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
                new SignInViewModel
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
                new SignInViewModel
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
        public void ExternalLoginScheme_Get_Success(SignInViewModel signInViewModel, string expectedValue)
        {
            // Arrange & Act & Assert
            ObjectAssert.DeepEquals(expectedValue, signInViewModel.ExternalLoginScheme);
        }
    }
}