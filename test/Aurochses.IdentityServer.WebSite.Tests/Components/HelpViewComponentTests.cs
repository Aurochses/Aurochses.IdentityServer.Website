using Aurochses.IdentityServer.WebSite.App.Project;
using Aurochses.IdentityServer.WebSite.Components;
using Aurochses.IdentityServer.WebSite.Models.Shared.Components.Help;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Components
{
    public class HelpViewComponentTests
    {
        private readonly Mock<IOptions<ProjectOptions>> _mockProjectOptions;

        private readonly HelpViewComponent _viewComponent;

        public HelpViewComponentTests()
        {
            _mockProjectOptions = new Mock<IOptions<ProjectOptions>>(MockBehavior.Strict);

            _mockProjectOptions
                .SetupGet(x => x.Value)
                .Returns(
                    new ProjectOptions
                    {
                        SupportEmail = "support@aurochses.com",
                        SupportUrl = "https://support.aurochses.com"
                    }
                );

            _viewComponent = new HelpViewComponent(_mockProjectOptions.Object);
        }

        [Fact]
        public void Inherit_ViewComponent()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<ViewComponent>(_viewComponent);
        }

        [Fact]
        public void Invoke_ReturnViewViewComponentResultWithHelpViewModel()
        {
            // Arrange & Act
            var viewComponentResult = _viewComponent.Invoke();

            // Assert
            var viewViewComponentResult = Assert.IsType<ViewViewComponentResult>(viewComponentResult);

            var model = Assert.IsAssignableFrom<HelpViewModel>(viewViewComponentResult.ViewData.Model);

            Assert.Equal(_mockProjectOptions.Object.Value.SupportEmail, model.SupportEmail);
            Assert.Equal(_mockProjectOptions.Object.Value.SupportUrl, model.SupportUrl);
        }
    }
}