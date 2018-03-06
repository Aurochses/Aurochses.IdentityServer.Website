using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Api.WebSite
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

            // Authorization
            App.Authorization.Startup.ConfigureServices(services);

            // IdentityServer
            App.IdentityServer.Startup.ConfigureServices(services, Configuration);

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Cors
            App.Cors.Startup.Configure(app);

            // Swagger
            App.Swagger.Startup.Configure(app, Configuration);

            // StaticFiles
            app.UseStaticFiles();

            // Authentication
            app.UseAuthentication();

            // MVC
            app.UseMvc();
        }
    }
}