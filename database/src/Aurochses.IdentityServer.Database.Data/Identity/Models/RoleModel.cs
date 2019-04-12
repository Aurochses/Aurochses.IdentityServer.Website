using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace Aurochses.IdentityServer.Database.Data.Identity.Models
{
    public class RoleModel
    {
        public ApplicationRole Role { get; set; }

        public IList<RoleClaimModel> RoleClaims { get; set; }

        public class RoleClaimModel
        {
            public string ClaimType { get; set; }

            public string ClaimValue { get; set; }
        }
    }
}
