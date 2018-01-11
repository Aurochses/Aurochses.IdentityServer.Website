using System.Threading.Tasks;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests
{
    public class IdentityServerTests : IClassFixture<TestServerWithDatabaseFixture>
    {
        private readonly TestServerWithDatabaseFixture _fixture;

        public IdentityServerTests(TestServerWithDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task WellKnown_OpenIdConfiguration_Success()
        {
            // Arrange & Act
            var response = await _fixture.Client.GetAsync("/.well-known/openid-configuration");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}