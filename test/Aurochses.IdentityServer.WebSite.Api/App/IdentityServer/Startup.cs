using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.WebSite.Api.App.IdentityServer
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
            // IdentityServer Authentication
            app.UseIdentityServerAuthentication(
                new IdentityServerAuthenticationOptions
                {
                    Authority = configuration.GetValue<string>("IdentityServer:Authority"),
                    AllowedScopes = configuration.GetSection("IdentityServer:AllowedScopes").GetChildren().Select(x => x.Value).ToList(),
                    RequireHttpsMetadata = configuration.GetValue<bool>("IdentityServer:RequireHttpsMetadata")
                }
            );
        }
    }
}