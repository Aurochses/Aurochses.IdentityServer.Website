using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests.Controllers
{
    public class HomeControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public HomeControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Index_RedirectToSignIn()
        {
            // Arrange & Act
            var response = await _fixture.Client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/SignIn", response.Headers.Location.OriginalString);
        }
    }
}