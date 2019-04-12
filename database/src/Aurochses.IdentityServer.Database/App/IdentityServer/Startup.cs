using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Database.App.IdentityServer
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfigurationStoreOptions>(configuration.GetSection("IdentityServer:ConfigurationStore"));

            services.Configure<OperationalStoreOptions>(configuration.GetSection("IdentityServer:OperationalStore"));
        }
    }
}