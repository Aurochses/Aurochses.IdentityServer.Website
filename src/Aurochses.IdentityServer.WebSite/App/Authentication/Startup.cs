using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

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
        /// <param name="app">The application.</param>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(IApplicationBuilder app, IConfigurationRoot configuration)
        {
            // Enable Facebook Authentication
            if (!string.IsNullOrWhiteSpace(configuration["Authentication:Facebook:AppId"]))
            {
                app.UseFacebookAuthentication(
                    new FacebookOptions
                    {
                        AppId = configuration["Authentication:Facebook:AppId"],
                        AppSecret = configuration["Authentication:Facebook:AppSecret"]
                    }
                );
            }
        }
    }
}