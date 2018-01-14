using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Aurochses.IdentityServer.Api.WebSite.App.Swagger
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
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        configuration.GetValue<string>("Swagger:Version"),
                        new Info
                        {
                            Version = configuration.GetValue<string>("Swagger:Version"),
                            Title = configuration.GetValue<string>("Swagger:Title"),
                            Description = configuration.GetValue<string>("Swagger:Description"),
                            TermsOfService = configuration.GetValue<string>("Swagger:TermsOfService"),
                            Contact = new Contact
                            {
                                Name = configuration.GetValue<string>("Swagger:Contact:Name"),
                                Url = configuration.GetValue<string>("Swagger:Contact:Url"),
                                Email = configuration.GetValue<string>("Swagger:Contact:Email")
                            },
                            License = new License
                            {
                                Name = configuration.GetValue<string>("Swagger:License:Name"),
                                Url = configuration.GetValue<string>("Swagger:License:Url")
                            }
                        }
                    );
                }
            );
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(IApplicationBuilder app, IConfiguration configuration)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>  options.SwaggerEndpoint($"/swagger/{configuration.GetValue<string>("Swagger:Version")}/swagger.json", configuration.GetValue<string>("Swagger:Description")));
        }
    }
}