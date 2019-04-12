using Aurochses.IdentityServer.Database.Data.Identity;
using Aurochses.IdentityServer.Database.Data.IdentityServer;

namespace Aurochses.IdentityServer.Database.Data
{
    public class Service
    {
        private readonly IdentityService _identityService;
        private readonly IdentityServerService _identityServerService;

        public Service(IdentityService identityService, IdentityServerService identityServerService)
        {
            _identityService = identityService;
            _identityServerService = identityServerService;
        }

        public void Run(string environmentName)
        {
            // Identity
            _identityService.Run(environmentName);

            // IdentityServer
            _identityServerService.Run(environmentName);
        }
    }
}