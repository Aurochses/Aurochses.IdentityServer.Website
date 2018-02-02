using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.Database.Data.IdentityServer.Data
{
    public static class ApiResourceData
    {
        public static IList<ApiResource> GetList(string environmentName)
        {
            var configuration = Program.BuildConfiguration(@"IdentityServer\Data", nameof(ApiResourceData), environmentName);

            return configuration.GetSection("Data").Get<IList<ApiResource>>();
        }
    }
}
