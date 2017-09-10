using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.WebSite.App.Localization
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
            // Add Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Configure the localization options
            var culture = configuration.GetSection("Localization:Culture").Value;
            var uiCulture = configuration.GetSection("Localization:UiCulture").Value;

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    options.SupportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo(culture)
                    };
                    options.SupportedUICultures = new List<CultureInfo>
                    {
                        new CultureInfo(uiCulture)
                    };
                    options.DefaultRequestCulture = new RequestCulture(culture, uiCulture);
                }
            );
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="requestLocalizationOptions">The request localization options.</param>
        public static void Configure(IApplicationBuilder app, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
        {
            // Enable Localization
            app.UseRequestLocalization(requestLocalizationOptions.Value);
        }
    }
}
