using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Aurochses.IdentityServer.Database.Data.Identity.Models;

namespace Aurochses.IdentityServer.Database.Data.Identity.Data
{
    public static class RoleData
    {
        public static IEnumerable<RoleModel> GetList(string environmentName)
        {
            var configuration = Program.BuildConfiguration(@"Identity\Data", nameof(RoleData), environmentName);

            return configuration.GetSection("Data").Get<IList<RoleModel>>();
        }
    }
}
