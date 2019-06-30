using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Aurochses.IdentityServer.Website.IntegrationTests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Index_WhenHostingEnvironmentIsDevelopment_ReturnView()
        {
            // Arrange
            var client = new TestWebApplicationFactory {EnvironmentName = "Development"}.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Index_WhenHostingEnvironmentIsNotDevelopment_RedirectToSignIn()
        {
            // Arrange
            var client = new TestWebApplicationFactory {EnvironmentName = "IsNotDevelopment"}.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/SignIn", response.Headers.Location.OriginalString);
        }
    }
}