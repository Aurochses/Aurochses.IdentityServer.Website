using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Database.Data.App.AutoMapper
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
            services.AddAutoMapper(
                typeof(Startup).Assembly
            );
        }
    }
}
