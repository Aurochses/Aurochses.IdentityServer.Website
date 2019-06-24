using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Website.App.IdentityServer
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = services
                .AddIdentityServer(
                    options =>
                    {
                        options.UserInteraction.LoginUrl = configuration["IdentityServer:UserInteraction:LoginUrl"];
                        options.UserInteraction.LogoutUrl = configuration["IdentityServer:UserInteraction:LogoutUrl"];
                        options.UserInteraction.ConsentUrl = configuration["IdentityServer:UserInteraction:ConsentUrl"];
                        options.UserInteraction.ErrorUrl = configuration["IdentityServer:UserInteraction:ErrorUrl"];
                    }
                )
                .AddConfigurationStore(
                    options =>
                    {
                        options.ConfigureDbContext = builder1 =>
                        {
                            builder1.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                        };
                        options.DefaultSchema = configuration["IdentityServer:ConfigurationStore:DefaultSchema"];
                    }
                )
                .AddOperationalStore(
                    options =>
                    {
                        options.ConfigureDbContext = builder1 =>
                        {
                            builder1.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                        };
                        options.DefaultSchema = configuration["IdentityServer:OperationalStore:DefaultSchema"];
                        options.EnableTokenCleanup = configuration.GetValue<bool>("IdentityServer:OperationalStore:EnableTokenCleanup");
                        options.TokenCleanupInterval = configuration.GetValue<int>("IdentityServer:OperationalStore:TokenCleanupInterval");
                    }
                )
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityServerProfileService>();

            if (env.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                // todo: solve this
                // https://tatvog.wordpress.com/2018/06/05/identityserver4-addsigningcredential-using-certificate-stored-in-azure-key-vault/
                //                builder.AddSigningCredential(
                //                    new X509Certificate2(
                //                        Path.Combine(
                //                            Directory.GetCurrentDirectory(),
                //                            configuration["IdentityServer:SigningCredential:Certificate:Path"]
                //                        ),
                //                        configuration["IdentityServer:SigningCredential:Certificate:Password"]
                //                    )
                //                );
            }
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}