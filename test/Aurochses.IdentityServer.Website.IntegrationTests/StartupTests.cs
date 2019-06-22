using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Aurochses.IdentityServer.Website.IntegrationTests
{
    public class StartupTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;

        public StartupTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public void ConfigureServices_Success()
        {
            // Arrange & Act
            _factory.CreateClient();

            // Assert
            // todo: check other services
            Assert.NotNull(_factory.Server.Host.Services.GetService<IdentityDbContext>());
        }
    }
}