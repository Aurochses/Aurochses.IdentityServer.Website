using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.IdentityServer.Database.Context
{
    /// <summary>
    /// IdentityServerConfigurationContext.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class IdentityServerConfigurationContext : ConfigurationDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServerConfigurationContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The database context options.</param>
        public IdentityServerConfigurationContext(DbContextOptions<ConfigurationDbContext> dbContextOptions)
            : base(dbContextOptions, new ConfigurationStoreOptions { DefaultSchema = Configuration.IdentityServerSchemaName })
        {

        }
    }
}