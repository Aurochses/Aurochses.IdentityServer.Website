using System;
using System.Linq;
using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests
{
    public class TestServerWithDatabaseFixture : TestServerFixture
    {
        private IdentityDbContext _identityDbContext;

        public TestServerWithDatabaseFixture()
        {
            Migrate();

            UserManager = Server.Host.Services.GetService<UserManager<ApplicationUser>>();
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public override void Dispose()
        {
            _identityDbContext.Database.EnsureDeleted();

            base.Dispose();
        }

        private void Migrate()
        {
            _identityDbContext = Server.Host.Services.GetService<IdentityDbContext>();
            var configurationDbContext = Server.Host.Services.GetService<ConfigurationDbContext>();
            var persistedGrantDbContext = Server.Host.Services.GetService<PersistedGrantDbContext>();

            _identityDbContext.Database.EnsureCreated();
            configurationDbContext.GetService<IRelationalDatabaseCreator>().CreateTables();
            persistedGrantDbContext.GetService<IRelationalDatabaseCreator>().CreateTables();
        }

        public async Task<ApplicationUser> AddUser(string email, string password = "testpassword", bool emailConfirmed = true)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                EmailConfirmed = emailConfirmed
            };

            var result = await UserManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"User cannot be created! {result.Errors.First().Description}");
            }

            return user;
        }
    }
}