using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.WebSite.Api.App.Authorization
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
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy("ValueRead", policy => policy.RequireClaim("permission", "value.read"));
                }
            );
        }
    }
}