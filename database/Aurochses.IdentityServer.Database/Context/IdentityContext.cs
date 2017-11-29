using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.IdentityServer.Database.Context
{
    /// <summary>
    /// IdentityContext.
    /// </summary>
    /// <seealso cref="Aurochses.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext" />
    public class IdentityContext : IdentityDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The database context options.</param>
        public IdentityContext(DbContextOptions<IdentityDbContext> dbContextOptions)
            // ReSharper disable once RedundantArgumentDefaultValue
            : base(dbContextOptions, Configuration.IdentitySchemaName)
        {

        }
    }
}