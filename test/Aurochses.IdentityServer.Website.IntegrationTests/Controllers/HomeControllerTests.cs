using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Aurochses.IdentityServer.Website.IntegrationTests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Index_WhenHostingEnvironmentIsDevelopment_ReturnView()
        {
            // Arrange
            var client = new WebApplicationFactory<Startup>().CreateClient();

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
            var client = new TestWebApplicationFactory("IsNotDevelopment").CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                }
            );

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/SignIn", response.Headers.Location.OriginalString);
        }
    }
}