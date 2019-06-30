using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Aurochses.IdentityServer.Website.IntegrationTests
{
    // todo: add tests for other endpoints: http://docs.identityserver.io/en/latest/endpoints/discovery.html
    public class IdentityServerEndpointTests : IClassFixture<TestWebApplicationFactoryWithDatabase>
    {
        private readonly HttpClient _client;

        public IdentityServerEndpointTests(TestWebApplicationFactoryWithDatabase factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task DiscoveryEndpoint_ReturnJson()
        {
            // Arrange & Act
            var response = await _client.GetAsync("/.well-known/openid-configuration");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=UTF-8", response.Content.Headers.ContentType.ToString());
        }
    }
}