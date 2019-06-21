using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.Database.Data.IdentityServer.Data
{
    public static class ClientData
    {
        public static IEnumerable<Client> GetList(string environmentName)
        {
            var configuration = Program.BuildConfiguration(@"IdentityServer\Data", nameof(ClientData), environmentName);

            return configuration.GetSection("Data").Get<IList<Client>>();
        }
    }
}
