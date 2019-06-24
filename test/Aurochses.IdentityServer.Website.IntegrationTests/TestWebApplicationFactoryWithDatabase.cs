using System;
using System.Collections.Generic;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Website.IntegrationTests
{
    public class TestWebApplicationFactoryWithDatabase : TestWebApplicationFactory
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureAppConfiguration(
                (context, configuration) =>
                {
                    configuration.AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            ["ConnectionStrings:DefaultConnection"] = $"Server=(localdb)\\mssqllocaldb;Database=Aurochses.IdentityServer.WebSite.IntegrationTests_{Guid.NewGuid()};Trusted_Connection=True;",

                            // Disable ReCaptcha
                            ["Recaptcha:Enabled"] = false.ToString()
                        }
                    );
                }
            );

            builder.ConfigureTestServices(
                services =>
                {
                    // Build the service provider.
                    var serviceProvider = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database contexts
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                        var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                        var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

                        identityDbContext.Database.EnsureCreated();
                        configurationDbContext.GetService<IRelationalDatabaseCreator>().CreateTables();
                        persistedGrantDbContext.GetService<IRelationalDatabaseCreator>().CreateTables();
                    }
                }
            );
        }


    }
}