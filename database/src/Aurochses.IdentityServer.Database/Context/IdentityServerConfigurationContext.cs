using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.Database.Context
{
    public class IdentityServerConfigurationContext : ConfigurationDbContext
    {
        public IdentityServerConfigurationContext(DbContextOptions<ConfigurationDbContext> dbContextOptions, IOptions<ConfigurationStoreOptions> configurationStoreOptions)
            : base(dbContextOptions, configurationStoreOptions.Value)
        {

        }
    }
}