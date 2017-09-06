using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.Database.Context;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Database
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
            // Set up configuration providers.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BaseContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddDbContext<IdentityContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            // have to add this context because IdentityContext use it
            services.AddDbContext<IdentityDbContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddDbContext<IdentityServerConfigurationContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            // have to add this context because IdentityServerConfigurationContext use it
            services.AddDbContext<ConfigurationDbContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddDbContext<IdentityServerPersistedGrantContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            // have to add this context because IdentityServerPersistedGrantContext use it
            services.AddDbContext<PersistedGrantDbContext>(
                options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public void Configure()
        {

        }
    }
}