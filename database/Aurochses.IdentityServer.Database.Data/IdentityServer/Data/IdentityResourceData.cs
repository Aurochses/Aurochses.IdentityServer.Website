using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.Database.Data.IdentityServer.Data
{
    public static class IdentityResourceData
    {
        public static IList<IdentityResource> GetList(string environmentName)
        {
            var list = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResources.Phone()
            };

            var configuration = Program.BuildConfiguration(@"IdentityServer\Data", nameof(IdentityResource), environmentName);

            list.AddRange(configuration.GetSection("Data").Get<IList<IdentityResource>>());

            return list;
        }
    }
}
