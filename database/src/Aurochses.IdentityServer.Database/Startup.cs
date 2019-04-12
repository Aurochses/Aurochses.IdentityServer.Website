using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.Database.EntityFrameworkCore;
using Aurochses.IdentityServer.Database.Context;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Database
{
    public class Startup : StartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // IdentityServer
            App.IdentityServer.Startup.ConfigureServices(services, Configuration);

            // Contexts
            services.AddDbContext<BaseContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IdentityContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // have to add this context because IdentityContext use it
            services.AddDbContext<IdentityDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IdentityServerConfigurationContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // have to add this context because IdentityServerConfigurationContext use it
            services.AddDbContext<ConfigurationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IdentityServerPersistedGrantContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // have to add this context because IdentityServerPersistedGrantContext use it
            services.AddDbContext<PersistedGrantDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddOptions();
        }
    }
}