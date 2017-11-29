using System.Net;
using System.Threading.Tasks;
using Aurochses.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
            // Arrange
            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(
                    ProjectHelpers.GetFolderPath("Aurochses.IdentityServer.WebSite", "src", "Aurochses.IdentityServer.WebSite")
                )
                .UseEnvironment("Production")
                .UseStartup<Startup>();

            var server = new TestServer(webHostBuilder);

            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/SignIn", response.Headers.Location.OriginalString);
        }
    }
}