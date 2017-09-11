using System;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.WebSite.Models.EmailConfirmation;
using Aurochses.Testing;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.EmailConfirmation
{
    public class EmailConfirmationViewModelTests
    {
        private readonly EmailConfirmationViewModel _viewModel;

        public EmailConfirmationViewModelTests()
        {
            _viewModel = new EmailConfirmationViewModel();
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        [InlineData(typeof(EmailAddressAttribute))]
        public void Email_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<EmailConfirmationViewModel>("Email", attributeType);
        }

        [Fact]
        public void Email_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<EmailConfirmationViewModel>("Email", typeof(DisplayAttribute));

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