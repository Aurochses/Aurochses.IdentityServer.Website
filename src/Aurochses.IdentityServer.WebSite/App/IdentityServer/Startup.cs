using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aurochses.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Aurochses.IdentityServer.WebSite.App.IdentityServer
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
            // Configure IdentityServer
            services
                .AddIdentityServer(
                    options =>
                    {
                        options.UserInteraction.LoginUrl = configuration["IdentityServer:UserInteraction:LoginUrl"];
                        options.UserInteraction.LogoutUrl = configuration["IdentityServer:UserInteraction:LogoutUrl"];
                        options.UserInteraction.ConsentUrl = configuration["IdentityServer:UserInteraction:ConsentUrl"];
                        options.UserInteraction.ErrorUrl = configuration["IdentityServer:UserInteraction:ErrorUrl"];
                    }
                )
                .AddSigningCredential(
                    new X509Certificate2(
                        Path.Combine(
                            PlatformServices.Default.Application.ApplicationBasePath,
                            configuration["SigningCertificate:Path"]
                        ),
                        configuration["SigningCertificate:Password"]
                    )
                )
                .AddConfigurationStore(
                    builder => builder.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"]),
                    options =>
                    {
                        options.DefaultSchema = configuration["IdentityServer:ConfigurationStore:DefaultSchema"];
                    }
                )
                .AddOperationalStore(
                    builder => builder.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"]),
                    options =>
                    {
                        options.DefaultSchema = configuration["IdentityServer:OperationalStore:DefaultSchema"];
                    }
                )
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityProfileService>();
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void Configure(IApplicationBuilder app)
        {
            // Enable IdentityServer
            app.UseIdentityServer();
        }
    }
}