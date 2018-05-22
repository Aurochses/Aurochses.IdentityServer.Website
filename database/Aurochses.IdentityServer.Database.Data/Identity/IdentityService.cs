using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.Database.Data.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Aurochses.IdentityServer.Database.Data.Identity
{
    public class IdentityService
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(IdentityDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Run(string environmentName)
        {
            // User
            _context.RemoveRange(_context.Users);
            _context.SaveChanges();

            foreach (var item in UserData.GetList(environmentName))
            {
                _userManager.CreateAsync(item.User, item.Password).Wait();
            }
        }
    }
}