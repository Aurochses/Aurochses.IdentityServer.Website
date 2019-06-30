using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Aurochses.IdentityServer.Website.IntegrationTests.Controllers
{
    public class ErrorControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ErrorControllerTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_ReturnView()
        {
            // Arrange & Act
            var response = await _client.GetAsync("/Error");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}