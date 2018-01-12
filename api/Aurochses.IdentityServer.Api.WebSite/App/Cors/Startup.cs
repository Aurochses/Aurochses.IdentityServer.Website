using Microsoft.AspNetCore.Builder;

namespace Aurochses.IdentityServer.Api.WebSite.App.Cors
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
        public static void Configure(IApplicationBuilder app)
        {
            // Enable Cors
            app.UseCors(
                builder => builder
                    .AllowAnyOrigin() // todo: must be allowed only for correct domains!
                    .AllowAnyHeader() // todo: this is for Options method (must be solved correct)
                    .AllowAnyMethod() // todo: this is for Options method (must be solved correct)
            );
        }
    }
}