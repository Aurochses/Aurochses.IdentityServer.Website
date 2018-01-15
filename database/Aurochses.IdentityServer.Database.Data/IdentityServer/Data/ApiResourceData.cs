using System.Collections.Generic;
using IdentityServer4.Models;

namespace Aurochses.IdentityServer.Database.Data.IdentityServer.Data
{
    public static class ApiResourceData
    {
        public static IList<ApiResource> GetList()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "API")
            };
        }
    }
}
