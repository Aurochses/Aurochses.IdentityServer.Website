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

            services.AddTransient<IdentityServer.IdentityServerService>();

            services.AddTransient<Service>();
        }
    }
}