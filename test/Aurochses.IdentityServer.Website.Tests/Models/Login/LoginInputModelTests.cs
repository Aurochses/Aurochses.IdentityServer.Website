using System;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.Website.Models.Login;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.Login
{
    public class LoginInputModelTests
    {
        private readonly LoginInputModel _loginInputModel;

        public LoginInputModelTests()
        {
            _loginInputModel = new LoginInputModel();
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void UserName_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<LoginInputModel>("UserName", attributeType);
        }

        [Fact]
        public void UserName_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(string), _loginInputModel.UserName);
        }

        [Fact]
        public void UserName_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _loginInputModel.UserName = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loginInputModel.UserName);
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute), null, null)]
        [InlineData(typeof(DataTypeAttribute), nameof(DataTypeAttribute.DataType), DataType.Password)]
        public void Password_Attribute_Defined(Type attributeType, string attributePropertyName, object attributePropertyValue)
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<LoginInputModel>("Password", attributeType);

            if (attributePropertyName != null)
            {
                AttributeAssert.ValidateProperty(propertyInfo, attributeType, attributePropertyName, attributePropertyValue);
            }
        }

        [Fact]
        public void Password_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(string), _loginInputModel.Password);
        }

        [Fact]
        public void Password_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _loginInputModel.Password = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loginInputModel.Password);
        }

        [Fact]
        public void RememberLogin_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(bool), _loginInputModel.RememberLogin);
        }

        [Fact]
        public void RememberLogin_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _loginInputModel.RememberLogin = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loginInputModel.RememberLogin);
        }

        [Fact]
        public void ReturnUrl_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(string), _loginInputModel.ReturnUrl);
        }

        [Fact]
        public void ReturnUrl_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _loginInputModel.ReturnUrl = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _loginInputModel.ReturnUrl);
        }
    }
}