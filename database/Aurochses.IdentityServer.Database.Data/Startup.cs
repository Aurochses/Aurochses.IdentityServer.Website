using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Aurochses.IdentityServer.Database.Data
{
    public class Startup
    {
        public Startup(string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(
                provider => new ConfigurationStoreOptions
                {
                    DefaultSchema = Configuration["IdentityServer:ConfigurationStore:DefaultSchema"]
                }
            );

            services.AddDbContext<ConfigurationDbContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddTransient<Service>();
        }
    }
}