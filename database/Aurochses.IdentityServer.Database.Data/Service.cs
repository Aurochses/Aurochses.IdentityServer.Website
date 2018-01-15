using Aurochses.IdentityServer.Database.Data.IdentityServer;

namespace Aurochses.IdentityServer.Database.Data
{
    public class Service
    {
        private readonly IdentityServerService _identityServerService;

        public Service(IdentityServerService identityServerService)
        {
            _identityServerService = identityServerService;
        }

        public void Run()
        {
            // IdentityServer
            _identityServerService.Run();
        }
    }
}