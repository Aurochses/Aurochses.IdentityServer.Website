using Aurochses.IdentityServer.WebSite.Models.Shared.Components.ProjectName;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Models.Shared.Components.ProjectName
{
    public class ProjectNameViewModelTests
    {
        private readonly ProjectNameViewModel _viewModel;

        public ProjectNameViewModelTests()
        {
            _viewModel = new ProjectNameViewModel();
        }

        [Fact]
        public void SupportEmail_Success()
        {
            // Arrange
            const string value = "value";

            // Act
            _viewModel.Value = value;

            // Assert
            Assert.Equal(value, _viewModel.Value);
        }
    }
}