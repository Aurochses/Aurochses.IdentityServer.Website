using System;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.Website.Models.SignIn;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Models.SignIn
{
    public class SignInInputModelTests
    {
        private readonly SignInInputModel _signInInputModel;

        public SignInInputModelTests()
        {
            _signInInputModel = new SignInInputModel();
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void UserName_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<SignInInputModel>("UserName", attributeType);
        }

        [Fact]
        public void UserName_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(string), _signInInputModel.UserName);
        }

        [Fact]
        public void UserName_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _signInInputModel.UserName = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _signInInputModel.UserName);
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute), null, null)]
        [InlineData(typeof(DataTypeAttribute), nameof(DataTypeAttribute.DataType), DataType.Password)]
        public void Password_Attribute_Defined(Type attributeType, string attributePropertyName, object attributePropertyValue)
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<SignInInputModel>("Password", attributeType);

            if (attributePropertyName != null)
            {
                AttributeAssert.ValidateProperty(propertyInfo, attributeType, attributePropertyName, attributePropertyValue);
            }
        }

        [Fact]
        public void Password_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(string), _signInInputModel.Password);
        }

        [Fact]
        public void Password_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _signInInputModel.Password = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _signInInputModel.Password);
        }

        [Fact]
        public void RememberLogin_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(bool), _signInInputModel.RememberLogin);
        }

        [Fact]
        public void RememberLogin_Set_Success()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            _signInInputModel.RememberLogin = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _signInInputModel.RememberLogin);
        }

        [Fact]
        public void ReturnUrl_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(string), _signInInputModel.ReturnUrl);
        }

        [Fact]
        public void ReturnUrl_Set_Success()
        {
            // Arrange
            const string expectedValue = "ExpectedValue";

            // Act
            _signInInputModel.ReturnUrl = expectedValue;

            // Assert
            Assert.Equal(expectedValue, _signInInputModel.ReturnUrl);
        }
    }
}