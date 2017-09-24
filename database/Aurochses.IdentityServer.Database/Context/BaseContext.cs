using Microsoft.EntityFrameworkCore;

namespace Aurochses.IdentityServer.Database.Context
{
    /// <summary>
    /// BaseContext.
    /// </summary>
    /// <seealso cref="T:Microsoft.EntityFrameworkCore.DbContext" />
    public class BaseContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aurochses.IdentityServer.Database.Context.BaseContext" /> class.
        /// </summary>
        /// <param name="dbContextOptions">The database context options.</param>
        public BaseContext(DbContextOptions<BaseContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }
    }
}