using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Aurochses.IdentityServer.Website.IntegrationTests
{
    // todo: http://docs.identityserver.io/en/latest/endpoints/discovery.html
    public class IdentityServerEndpointTests : IClassFixture<TestWebApplicationFactoryWithDatabase>
    {
        private readonly HttpClient _client;

        public IdentityServerEndpointTests(TestWebApplicationFactoryWithDatabase factory)
        {
            _client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                }
            );
        }

        [Fact]
        public async Task DiscoveryEndpoint_Success()
        {
            // Arrange & Act
            var response = await _client.GetAsync("/.well-known/openid-configuration");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=UTF-8", response.Content.Headers.ContentType.ToString());
        }
    }
}