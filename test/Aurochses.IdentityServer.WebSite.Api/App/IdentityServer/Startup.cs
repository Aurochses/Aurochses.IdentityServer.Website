using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.WebSite.Api.App.IdentityServer
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(
                    options =>
                    {
                        options.Authority = configuration.GetValue<string>("IdentityServer:Authority");
                        options.RequireHttpsMetadata = configuration.GetValue<bool>("IdentityServer:RequireHttpsMetadata");
                    }
                );
        }
    }
}