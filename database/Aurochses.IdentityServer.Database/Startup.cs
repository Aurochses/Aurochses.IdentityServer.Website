using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
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