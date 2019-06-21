using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.IdentityServer.Database.Context
{
    public class IdentityContext : IdentityDbContext
    {
        public IdentityContext(DbContextOptions<IdentityDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }
    }
}