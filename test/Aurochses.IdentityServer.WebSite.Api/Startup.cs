using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aurochses.IdentityServer.WebSite.Api
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
                .AddJsonFile("appsettings.json", false, true)
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
            // Authorization
            App.Authorization.Startup.ConfigureServices(services);

            // Swagger
            App.Swagger.Startup.ConfigureServices(services, Configuration);

            // MVC
            services.AddMvc(
                config =>
                {
                    config.Filters.Add(
                        // Authorize All
                        new AuthorizeFilter(
                            new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build()
                        )
                    );
                }
            );
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Logging
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            // Cors
            App.Cors.Startup.Configure(app);

            // IdentityServer
            App.IdentityServer.Startup.Configure(app, Configuration);

            // Swagger
            App.Swagger.Startup.Configure(app, Configuration);

            // MVC
            app.UseMvc();
        }
    }
}