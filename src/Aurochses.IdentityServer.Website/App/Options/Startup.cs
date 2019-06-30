using Aurochses.IdentityServer.Website.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Website.App.Options
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<AccountOptions>(configuration.GetSection("Account"));
        }
    }
}