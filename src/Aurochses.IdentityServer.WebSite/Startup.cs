using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
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
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="requestLocalizationOptions">The request localization options.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
        {
            // Logging
            App.Logging.Startup.Configure(loggerFactory, Configuration);

            // Exception Handler
            App.ExceptionHandler.Startup.Configure(app, env, Configuration);

            // Localization
            App.Localization.Startup.Configure(app, requestLocalizationOptions);

            // Identity
            App.Identity.Startup.Configure(app);

            // Authentication
            App.Authentication.Startup.Configure(app, Configuration);

            // IdentityServer
            App.IdentityServer.Startup.Configure(app);

            // StaticFiles
            app.UseStaticFiles();

            // MVC
            app.UseMvcWithDefaultRoute();
        }
    }
}