using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Aurochses.IdentityServer.Database.Data.Identity.Models;

namespace Aurochses.IdentityServer.Database.Data.Identity.Data
{
    public static class UserData
    {
        public static IEnumerable<UserModel> GetList(string environmentName)
        {
            var configuration = Program.BuildConfiguration(@"Identity\Data", nameof(UserData), environmentName);

            return configuration.GetSection("Data").Get<IList<UserModel>>();
        }
    }
}
