using System.Threading.Tasks;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests.Controllers
{
    public class ForbiddenControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public ForbiddenControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Index_Return_View()
        {
            // Arrange
            var response = await _fixture.Client.GetAsync("/Forbidden");

            // Act & Assert
            response.EnsureSuccessStatusCode();
        }

        // todo: add test for returnUrl parameter
    }
}
