using System;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.WebSite.Models.TwoFactorSignIn;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.TwoFactorSignIn
{
    public class VerifyCodeViewModelTests
    {
        private readonly VerifyCodeViewModel _viewModel;

        public VerifyCodeViewModelTests()
        {
            _viewModel = new VerifyCodeViewModel();
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void Provider_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<VerifyCodeViewModel>("Provider", attributeType);
        }

        [Fact]
        public void Provider_Success()
        {
            // Arrange
            const string value = "provider";

            // Act
            _viewModel.Provider = value;

            // Assert
            Assert.Equal(value, _viewModel.Provider);
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void Token_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<VerifyCodeViewModel>("Token", attributeType);
        }

        [Fact]
        public void Token_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<VerifyCodeViewModel>("Token", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "Token");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "Token.Prompt");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Description), "Token.Description");
        }

        [Fact]
        public void Token_Success()
        {
            // Arrange
            const string value = "token";

            // Act
            _viewModel.Token = value;

            // Assert
            Assert.Equal(value, _viewModel.Token);
        }

        [Fact]
        public void ReturnUrl_Success()
        {
            // Arrange
            const string value = "returnUrl";

            // Act
            _viewModel.ReturnUrl = value;

            // Assert
            Assert.Equal(value, _viewModel.ReturnUrl);
        }

        [Fact]
        public void RememberBrowser_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<VerifyCodeViewModel>("RememberBrowser", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "RememberBrowser");
        }

        [Fact]
        public void RememberBrowser_Success()
        {
            // Arrange
            const bool value = true;

            // Act
            _viewModel.RememberBrowser = value;

            // Assert
            Assert.Equal(value, _viewModel.RememberBrowser);
        }

        [Fact]
        public void RememberMe_Success()
        {
            // Arrange
            const bool value = true;

            // Act
            _viewModel.RememberMe = value;

            // Assert
            Assert.Equal(value, _viewModel.RememberMe);
        }
    }
}