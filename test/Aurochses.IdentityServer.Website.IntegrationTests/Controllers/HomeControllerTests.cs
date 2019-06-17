using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace Aurochses.IdentityServer.Website.IntegrationTests.Controllers
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Website.Startup>>
    {
        private readonly WebApplicationFactory<Website.Startup> _factory;

        public HomeControllerTests(WebApplicationFactory<Website.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Index()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
