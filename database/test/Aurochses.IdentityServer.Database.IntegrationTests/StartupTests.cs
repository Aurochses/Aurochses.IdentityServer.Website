using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Aurochses.IdentityServer.Database.IntegrationTests
{
    public class StartupTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public StartupTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ConfigureServices_Success()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_fixture.Server.Host.Services.GetService<BaseContext>());

            Assert.NotNull(_fixture.Server.Host.Services.GetService<IdentityContext>());
            Assert.NotNull(_fixture.Server.Host.Services.GetService<IdentityDbContext>());
            Assert.NotNull(_fixture.Server.Host.Services.GetService<IdentityServerConfigurationContext>());
            //Assert.NotNull(_fixture.Server.Host.Services.GetService<ConfigurationDbContext>());
            Assert.NotNull(_fixture.Server.Host.Services.GetService<IdentityServerPersistedGrantContext>());
            //Assert.NotNull(_fixture.Server.Host.Services.GetService<PersistedGrantDbContext>());
        }
    }
}
