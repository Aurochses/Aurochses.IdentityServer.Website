using System;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.WebSite.Models.RemindPassword;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.RemindPassword
{
    public class RemindPasswordViewModelTests
    {
        private readonly RemindPasswordViewModel _viewModel;

        public RemindPasswordViewModelTests()
        {
            _viewModel = new RemindPasswordViewModel();
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        [InlineData(typeof(EmailAddressAttribute))]
        public void Email_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<RemindPasswordViewModel>("Email", attributeType);
        }

        [Fact]
        public void Email_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<RemindPasswordViewModel>("Email", typeof(DisplayAttribute));

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
    }
}