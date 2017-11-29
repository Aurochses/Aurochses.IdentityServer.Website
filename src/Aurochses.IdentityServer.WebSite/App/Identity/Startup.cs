using Aurochses.AspNetCore.Identity;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.AspNetCore.Identity.SendGrid;
using Aurochses.AspNetCore.Identity.Twilio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.WebSite.App.Identity
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
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add IdentityDbContext
            services.AddDbContext<IdentityDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

            // Configure Identity
            services.Configure<IdentityOptions>(
                options =>
                {
                    // Password settings
                    options.Password.RequireDigit = configuration.GetValue<bool>("Identity:Password:RequireDigit");
                    options.Password.RequireLowercase = configuration.GetValue<bool>("Identity:Password:RequireLowercase");
                    options.Password.RequireNonAlphanumeric = configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
                    options.Password.RequireUppercase = configuration.GetValue<bool>("Identity:Password:RequireUppercase");
                    options.Password.RequiredLength = configuration.GetValue<int>("Identity:Password:RequiredLength");
                }
            );

            // Configure Identity cookies
            services.ConfigureApplicationCookie(
                options =>
                {
                    options.LoginPath = configuration.GetValue<string>("Authentication:Cookies:CookieAuthentication:LoginPath");
                    options.LogoutPath = configuration.GetValue<string>("Authentication:Cookies:CookieAuthentication:LogoutPath");
                    options.AccessDeniedPath = configuration.GetValue<string>("Authentication:Cookies:CookieAuthentication:AccessDeniedPath");
                }
            );

            // Add EmailService
            services.Configure<SendGridOptions>(configuration.GetSection("Identity:SendGridOptions"));
            services.AddTransient<IEmailService, EmailService>();

            // Add SmsService.
            services.Configure<TwilioOptions>(configuration.GetSection("Identity:TwilioOptions"));
            services.AddTransient<ISmsService, SmsService>();
        }
    }
}