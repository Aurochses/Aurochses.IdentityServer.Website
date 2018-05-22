using System.Collections.Generic;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.Database.Data.IdentityServer.Data
{
    public static class ApiResourceData
    {
        public static IList<ApiResource> GetList(string environmentName)
        {
            var configuration = Program.BuildConfiguration(@"IdentityServer\Data", nameof(ApiResourceData), environmentName);

            var apiResources = configuration.GetSection("Data").Get<IList<ApiResource>>();

            foreach (var apiResource in apiResources)
            {
                if (apiResource.Scopes.IsNullOrEmpty())
                {
                    apiResource.Scopes.Add(new Scope(apiResource.Name, apiResource.DisplayName));
                }
            }

            return apiResources;
        }
    }
}
