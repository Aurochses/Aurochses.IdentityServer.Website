using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;

namespace Aurochses.IdentityServer.WebSite.App.SendGrid
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
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            // Add SendGridClient
            services.AddTransient<ISendGridClient, SendGridClient>(provider => new SendGridClient(configuration.GetValue<string>("SendGrid:ApiKey")));
        }
    }
}