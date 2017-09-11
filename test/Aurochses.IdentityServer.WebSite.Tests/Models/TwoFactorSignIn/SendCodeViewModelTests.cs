using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Aurochses.IdentityServer.WebSite.Models.TwoFactorSignIn;
using Aurochses.Testing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.TwoFactorSignIn
{
    public class SendCodeViewModelTests
    {
        private readonly SendCodeViewModel _viewModel;

        public SendCodeViewModelTests()
        {
            _viewModel = new SendCodeViewModel();
        }

        [Fact]
        public void Providers_Success()
        {
            // Arrange
            ICollection<SelectListItem> values = new Collection<SelectListItem>
            {
                new SelectListItem {Value = "TestValue1", Text = "TestText1"},
                new SelectListItem {Value = "TestValue2", Text = "TestText2"}
            };

            // Act
            _viewModel.Providers = values;

            // Assert
            Assert.Equal(values, _viewModel.Providers);
        }

        [Theory]
        [InlineData(typeof(RequiredAttribute))]
        public void SelectedProvider_Attribute_Defined(Type attributeType)
        {
            // Arrange & Act & Assert
            TypeAssert.PropertyHasAttribute<SendCodeViewModel>("SelectedProvider", attributeType);
        }

        [Fact]
        public void SelectedProvider_DisplayAttribute_Defined()
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<SendCodeViewModel>("SelectedProvider", typeof(DisplayAttribute));

            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Name), "SelectedProvider");
            AttributeAssert.ValidateProperty(propertyInfo, typeof(DisplayAttribute), nameof(DisplayAttribute.Prompt), "SelectedProvider.Prompt");
        }

        [Fact]
        public void SelectedProvider_Success()
        {
            // Arrange
            const string value = "selectedProvider";

            // Act
            _viewModel.SelectedProvider = value;

            // Assert
            Assert.Equal(value, _viewModel.SelectedProvider);
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