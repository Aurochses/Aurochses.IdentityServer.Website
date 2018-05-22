using Aurochses.AspNetCore.Identity.EntityFrameworkCore;

namespace Aurochses.IdentityServer.Database.Data.Identity.Models
{
    public class UserModel
    {
        public ApplicationUser User { get; set; }
        public string Password { get; set; }
    }
}