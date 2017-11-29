using System;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.WebSite.Models.Registration;
using Aurochses.Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.Registration
{
    public class RegistrationViewModelTests
    {
        private readonly RegistrationViewModel _viewModel;

        public RegistrationViewModelTests()
        {
            _viewModel = new RegistrationViewModel();
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        [InlineData(typeof(EmailAddressAttribute))]
        public void Email_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<RegistrationViewModel>("Email", attributeType);
        }

        [Fact]
        public void Email_RemoteAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("Email", typeof(RemoteAttribute));

            var routeData = new RouteValueDictionary
            {
                {"action", "ValidateEmail"},
                {"controller", "Registration"}
            };

            AttributeAssert.ValidateProperty(propertyInfo, typeof(RemoteAttribute), "RouteData", routeData);
            AttributeAssert.ValidateProperty(propertyInfo, typeof(RemoteAttribute), nameof(RemoteAttribute.AdditionalFields), "__RequestVerificationToken");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(RemoteAttribute), nameof(RemoteAttribute.HttpMethod), "Post");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(RemoteAttribute), nameof(RemoteAttribute.ErrorMessage), "UserAlreadyExists");
        }

        [Fact]
        public void Email_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("Email", typeof(DisplayAttribute));

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
            TypeAssert.PropertyHasAttribute<RegistrationViewModel>("Password", attributeType);
        }

        [Fact]
        public void Password_DataTypeAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("Password", typeof(DataTypeAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DataTypeAttribute), nameof(DataTypeAttribute.DataType), DataType.Password);
        }

        [Fact]
        public void Password_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("Password", typeof(DisplayAttribute));

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
            TypeAssert.PropertyHasAttribute<RegistrationViewModel>("ConfirmPassword", attributeType);
        }

        [Fact]
        public void ConfirmPassword_CompareAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("ConfirmPassword", typeof(CompareAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(CompareAttribute), nameof(CompareAttribute.OtherProperty), "Password");
        }

        [Fact]
        public void ConfirmPassword_DataTypeAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("ConfirmPassword", typeof(DataTypeAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DataTypeAttribute), nameof(DataTypeAttribute.DataType), DataType.Password);
        }

        [Fact]
        public void ConfirmPassword_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("ConfirmPassword", typeof(DisplayAttribute));

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

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void FirstName_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<RegistrationViewModel>("FirstName", attributeType);
        }

        [Fact]
        public void FirstName_CompareAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("FirstName", typeof(StringLengthAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(StringLengthAttribute), nameof(StringLengthAttribute.MaximumLength), 50);
            AttributeAssert.ValidateProperty(propertyInfo, typeof(StringLengthAttribute), nameof(StringLengthAttribute.MinimumLength), 2);
        }

        [Fact]
        public void FirstName_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("FirstName", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "FirstName");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "FirstName.Prompt");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Description), "FirstName.Description");
        }

        [Fact]
        public void FirstName_Success()
        {
            // Arrange
            const string value = "FirstName";

            // Act
            _viewModel.FirstName = value;

            // Assert
            Assert.Equal(value, _viewModel.FirstName);
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void LastName_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<RegistrationViewModel>("LastName", attributeType);
        }

        [Fact]
        public void LastName_CompareAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("LastName", typeof(StringLengthAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(StringLengthAttribute), nameof(StringLengthAttribute.MaximumLength), 50);
            AttributeAssert.ValidateProperty(propertyInfo, typeof(StringLengthAttribute), nameof(StringLengthAttribute.MinimumLength), 2);
        }

        [Fact]
        public void LastName_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RegistrationViewModel>("LastName", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "LastName");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "LastName.Prompt");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Description), "LastName.Description");
        }

        [Fact]
        public void LastName_Success()
        {
            // Arrange
            const string value = "LastName";

            // Act
            _viewModel.LastName = value;

            // Assert
            Assert.Equal(value, _viewModel.LastName);
        }
    }
}