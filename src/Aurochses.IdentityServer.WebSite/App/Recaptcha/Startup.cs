using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace Aurochses.IdentityServer.WebSite.App.Recaptcha
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
            services.AddRecaptcha(
                new RecaptchaOptions
                {
                    SiteKey = configuration["Recaptcha:SiteKey"],
                    SecretKey = configuration["Recaptcha:SecretKey"],
                    ValidationMessage = configuration["Recaptcha:ValidationMessage"],
                    Enabled = configuration.GetValue("Recaptcha:Enabled", true)
                }
            );
        }
    }
}
