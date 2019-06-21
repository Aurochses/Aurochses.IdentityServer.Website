using Microsoft.EntityFrameworkCore;

namespace Aurochses.IdentityServer.Database.Context
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }
    }
}