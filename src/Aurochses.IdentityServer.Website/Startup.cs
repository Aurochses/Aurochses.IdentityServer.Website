using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Website
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IHostingEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            App.Options.Startup.ConfigureServices(services, Configuration);

            App.Logging.Startup.ConfigureServices(services, Configuration);

            App.CookiePolicy.Startup.ConfigureServices(services, Configuration);

            App.Identity.Startup.ConfigureServices(services, Configuration);

            App.IdentityServer.Startup.ConfigureServices(services, HostingEnvironment, Configuration);

            App.Authentication.Startup.ConfigureServices(services, Configuration);

            services.AddMvc(
                    options =>
                    {
                        // Require authenticated users
                        var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();

                        options.Filters.Add(new AuthorizeFilter(policy));
                    }
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app)
        {
            App.ExceptionHandler.Startup.Configure(app, HostingEnvironment, Configuration);

            App.CookiePolicy.Startup.Configure(app);

            App.IdentityServer.Startup.Configure(app);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}