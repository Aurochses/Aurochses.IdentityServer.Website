using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Database.Data
{
    public class Startup : Database.Startup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            // AutoMapper
            App.AutoMapper.Startup.ConfigureServices(services);

            // Add Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();

            services.AddTransient<Identity.IdentityService>();

            services.AddTransient<IdentityServer.IdentityServerService>();

            services.AddTransient<Service>();
        }
    }
}