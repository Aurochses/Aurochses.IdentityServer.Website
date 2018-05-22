using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aurochses.IdentityServer.WebSite.App.Logging
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            // Logging
            services.AddLogging(
                builder =>
                {
                    builder.AddConfiguration(configuration.GetSection("Logging"))
                        .AddConsole()
                        .AddDebug();
                }
            );
        }
    }
}
