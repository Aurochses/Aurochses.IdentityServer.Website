using System.Collections.Generic;
using System.Linq;
using Aurochses.IdentityServer.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Aurochses.IdentityServer.Database.IntegrationTests.Migrations
{
    public class MigrationsTests : IClassFixture<TestServerFixture>
    {
        private readonly IList<DbContext> _dbContexts;

        public MigrationsTests(TestServerFixture fixture)
        {
            _dbContexts = new List<DbContext>
            {
                fixture.Server.Host.Services.GetService<BaseContext>(),

                fixture.Server.Host.Services.GetService<IdentityContext>(),
                fixture.Server.Host.Services.GetService<IdentityServerConfigurationContext>(),
                fixture.Server.Host.Services.GetService<IdentityServerPersistedGrantContext>(),
            };
        }

        [Fact]
        public void Migrate()
        {
            // Arrange & Act & Assert
            try
            {
                Up();

                Down();

                Up();
            }
            finally
            {
                _dbContexts.First().Database.EnsureDeleted();
            }
        }

        private void Up()
        {
            foreach (var dbContext in _dbContexts)
            {
                dbContext.Database.Migrate();
            }
        }

        private void Down()
        {
            foreach (var dbContext in _dbContexts.Reverse())
            {
                dbContext.GetService<IMigrator>().Migrate("0");
            }
        }
    }
}
