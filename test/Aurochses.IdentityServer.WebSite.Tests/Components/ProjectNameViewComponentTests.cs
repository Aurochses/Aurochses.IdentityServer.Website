using Aurochses.IdentityServer.WebSite.App.Project;
using Aurochses.IdentityServer.WebSite.Components;
using Aurochses.IdentityServer.WebSite.Models.Shared.Components.ProjectName;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Components
{
    public class ProjectNameViewComponentTests
    {
        private readonly Mock<IOptions<ProjectOptions>> _mockProjectOptions;

        private readonly ProjectNameViewComponent _viewComponent;

        public ProjectNameViewComponentTests()
        {
            _mockProjectOptions = new Mock<IOptions<ProjectOptions>>(MockBehavior.Strict);

            _mockProjectOptions
                .SetupGet(x => x.Value)
                .Returns(
                    new ProjectOptions
                    {
                        Name = "Aurochses"
                    }
                );

            _viewComponent = new ProjectNameViewComponent(_mockProjectOptions.Object);
        }

        [Fact]
        public void Inherit_ViewComponent()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<ViewComponent>(_viewComponent);
        }

        [Fact]
        public void Invoke_ReturnViewViewComponentResultWithProjectNameViewModel()
        {
            // Arrange & Act
            var viewComponentResult = _viewComponent.Invoke();

            // Assert
            var viewViewComponentResult = Assert.IsType<ViewViewComponentResult>(viewComponentResult);

            var model = Assert.IsAssignableFrom<ProjectNameViewModel>(viewViewComponentResult.ViewData.Model);

            Assert.Equal(_mockProjectOptions.Object.Value.Name, model.Value);
        }
    }
}