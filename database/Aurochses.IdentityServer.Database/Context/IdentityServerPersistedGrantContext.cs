using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.IdentityServer.Database.Context
{
    /// <summary>
    /// IdentityServerPersistedGrantContext.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class IdentityServerPersistedGrantContext : PersistedGrantDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServerPersistedGrantContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The database context options.</param>
        public IdentityServerPersistedGrantContext(DbContextOptions<PersistedGrantDbContext> dbContextOptions)
            : base(dbContextOptions, new OperationalStoreOptions { DefaultSchema = Configuration.IdentityServerSchemaName })
        {

        }
    }
}