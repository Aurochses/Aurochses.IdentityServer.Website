using System;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.WebSite.Models.SignIn;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.SignIn
{
    public class SignInViewModelTests
    {
        private readonly SignInViewModel _viewModel;

        public SignInViewModelTests()
        {
            _viewModel = new SignInViewModel();
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        [InlineData(typeof(EmailAddressAttribute))]
        public void Email_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<SignInViewModel>("Email", attributeType);
        }

        [Fact]
        public void Email_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<SignInViewModel>("Email", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "Email");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "Email.Prompt");
        }

        [Fact]
        public void Email_Success()
        {
            // Arrange
            const string value = "email";

            // Act
            _viewModel.Email = value;

            // Assert
            Assert.Equal(value, _viewModel.Email);
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void Password_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<SignInViewModel>("Password", attributeType);
        }

        [Fact]
        public void Password_DataTypeAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<SignInViewModel>("Password", typeof(DataTypeAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DataTypeAttribute), nameof(DataTypeAttribute.DataType), DataType.Password);
        }

        [Fact]
        public void Password_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<SignInViewModel>("Password", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "Password");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "Password.Prompt");
        }

        [Fact]
        public void Password_Success()
        {
            // Arrange
            const string value = "password";

            // Act
            _viewModel.Password = value;

            // Assert
            Assert.Equal(value, _viewModel.Password);
        }

        [Fact]
        public void RememberMe_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<SignInViewModel>("RememberMe", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "RememberMe");
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