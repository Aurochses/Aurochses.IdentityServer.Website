using System.Threading.Tasks;
using IdentityServer4.Stores;

namespace Aurochses.IdentityServer.Website.Extensions
{
    public static class ClientStoreExtensions
    {
        public static async Task<bool> IsPkceClient(this IClientStore clientStore, string clientId)
        {
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                var client = await clientStore.FindEnabledClientByIdAsync(clientId);

                return client?.RequirePkce == true;
            }

            return false;
        }
    }
}