using System.IO;
using System.Security.Cryptography.X509Certificates;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Website.App.IdentityServer
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
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
                            Directory.GetCurrentDirectory(),
                            configuration["IdentityServer:SigningCredential:Certificate:Path"]
                        ),
                        configuration["IdentityServer:SigningCredential:Certificate:Password"]
                    )
                )
                .AddConfigurationStore(
                    options =>
                    {
                        options.ConfigureDbContext = builder =>
                        {
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                        };
                        options.DefaultSchema = configuration["IdentityServer:ConfigurationStore:DefaultSchema"];
                    }
                )
                .AddOperationalStore(
                    options =>
                    {
                        options.ConfigureDbContext = builder =>
                        {
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                        };
                        options.DefaultSchema = configuration["IdentityServer:OperationalStore:DefaultSchema"];
                        options.EnableTokenCleanup = configuration.GetValue<bool>("IdentityServer:OperationalStore:EnableTokenCleanup");
                        options.TokenCleanupInterval = configuration.GetValue<int>("IdentityServer:OperationalStore:TokenCleanupInterval");
                    }
                )
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityProfileService>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}