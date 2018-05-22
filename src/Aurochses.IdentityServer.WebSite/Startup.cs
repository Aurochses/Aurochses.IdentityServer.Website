using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.WebSite
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Logging
            App.Logging.Startup.Configure(services, Configuration);

            // Localization
            App.Localization.Startup.ConfigureServices(services, Configuration);

            // SendGrid
            App.SendGrid.Startup.ConfigureServices(services, Configuration);

            // Twilio
            App.Twilio.Startup.ConfigureServices(Configuration);

            // Identity
            App.Identity.Startup.ConfigureServices(services, Configuration);

            // IdentityServer
            App.IdentityServer.Startup.ConfigureServices(services, Configuration);

            // Authentication
            App.Authentication.Startup.Configure(services, Configuration);

            // Project
            App.Project.Startup.ConfigureServices(services, Configuration);

            // Recaptcha
            App.Recaptcha.Startup.ConfigureServices(services, Configuration);

            // MVC
            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="requestLocalizationOptions">The request localization options.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
        {
            // Exception Handler
            App.ExceptionHandler.Startup.Configure(app, env, Configuration);

            // Localization
            App.Localization.Startup.Configure(app, requestLocalizationOptions);

            // IdentityServer
            App.IdentityServer.Startup.Configure(app);

            // StaticFiles
            app.UseStaticFiles();

            // MVC
            app.UseMvcWithDefaultRoute();
        }
    }
}