using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.Database.Context
{
    public class IdentityServerPersistedGrantContext : PersistedGrantDbContext
    {
        public IdentityServerPersistedGrantContext(DbContextOptions<PersistedGrantDbContext> dbContextOptions, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(dbContextOptions, operationalStoreOptions.Value)
        {

        }
    }
}