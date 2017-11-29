using System;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.WebSite.Models.ResetPassword;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.ResetPassword
{
    public class ResetPasswordViewModelTests
    {
        private readonly ResetPasswordViewModel _viewModel;

        public ResetPasswordViewModelTests()
        {
            _viewModel = new ResetPasswordViewModel();
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        [InlineData(typeof(EmailAddressAttribute))]
        public void Email_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("Email", attributeType);
        }

        [Fact]
        public void Email_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("Email", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "Email");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "Email.Prompt");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Description), "Email.Description");
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
            TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("Password", attributeType);
        }

        [Fact]
        public void Password_DataTypeAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("Password", typeof(DataTypeAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DataTypeAttribute), nameof(DataTypeAttribute.DataType), DataType.Password);
        }

        [Fact]
        public void Password_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("Password", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "Password");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "Password.Prompt");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Description), "Password.Description");
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

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void ConfirmPassword_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("ConfirmPassword", attributeType);
        }

        [Fact]
        public void ConfirmPassword_CompareAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("ConfirmPassword", typeof(CompareAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(CompareAttribute), nameof(CompareAttribute.OtherProperty), "Password");
        }

        [Fact]
        public void ConfirmPassword_DataTypeAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("ConfirmPassword", typeof(DataTypeAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DataTypeAttribute), nameof(DataTypeAttribute.DataType), DataType.Password);
        }

        [Fact]
        public void ConfirmPassword_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<ResetPasswordViewModel>("ConfirmPassword", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "ConfirmPassword");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "ConfirmPassword.Prompt");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Description), "ConfirmPassword.Description");
        }

        [Fact]
        public void ConfirmPassword_Success()
        {
            // Arrange
            const string value = "confirmPassword";

            // Act
            _viewModel.ConfirmPassword = value;

            // Assert
            Assert.Equal(value, _viewModel.ConfirmPassword);
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
    }
}