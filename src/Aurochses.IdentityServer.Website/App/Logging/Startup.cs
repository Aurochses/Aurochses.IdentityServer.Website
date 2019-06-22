using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aurochses.IdentityServer.Website.App.Logging
{
    public static class Startup
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
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