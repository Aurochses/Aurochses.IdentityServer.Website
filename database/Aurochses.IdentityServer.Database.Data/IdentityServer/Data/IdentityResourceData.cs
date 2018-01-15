using System.Collections.Generic;
using IdentityServer4.Models;

namespace Aurochses.IdentityServer.Database.Data.IdentityServer.Data
{
    public static class IdentityResourceData
    {
        public static IList<IdentityResource> GetList()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResource
                {
                    Name = "permissions",
                    UserClaims = { "permission" }
                }
            };
        }
    }
}
