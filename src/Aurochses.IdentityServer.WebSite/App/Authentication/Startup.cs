using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.WebSite.App.Authentication
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
            // Enable Facebook Authentication
            if (!string.IsNullOrWhiteSpace(configuration["Authentication:Facebook:AppId"]))
            {
                services.AddAuthentication()
                    .AddFacebook(
                        options =>
                        {
                            options.AppId = configuration["Authentication:Facebook:AppId"];
                            options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                        }
                    );
            }
        }
    }
}