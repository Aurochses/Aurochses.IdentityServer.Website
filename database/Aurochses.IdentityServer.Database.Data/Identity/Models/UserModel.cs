using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace Aurochses.IdentityServer.Database.Data.Identity.Models
{
    public class UserModel
    {
        public ApplicationUser User { get; set; }

        public string Password { get; set; }

        public IList<string> Roles { get; set; }
    }
}