using System;
using Aurochses.Identity.EntityFramework;
using Aurochses.Identity.EntityFramework.SendGrid;
using Aurochses.Identity.EntityFramework.Twilio;
using Microsoft.AspNetCore.Builder;
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
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            // Add IdentityDbContext
            services.AddDbContext<IdentityDbContext>(
                options => options.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"]));

            // Add Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityDbContext, Guid>()
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
                    // Cookies
                    options.Cookies.ApplicationCookie.LoginPath = configuration.GetValue<string>("Identity:Cookies:ApplicationCookie:LoginPath");
                    options.Cookies.ApplicationCookie.LogoutPath = configuration.GetValue<string>("Identity:Cookies:ApplicationCookie:LogoutPath");
                    options.Cookies.ApplicationCookie.AccessDeniedPath = configuration.GetValue<string>("Identity:Cookies:ApplicationCookie:AccessDeniedPath");
                }
            );

            // Add EmailService
            services.Configure<SendGridOptions>(configuration.GetSection("Identity:SendGridOptions"));
            services.AddTransient<IEmailService, EmailService>();

            // Add SmsService.
            services.Configure<TwilioOptions>(configuration.GetSection("Identity:TwilioOptions"));
            services.AddTransient<ISmsService, SmsService>();
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void Configure(IApplicationBuilder app)
        {
            // Enable Identity
            app.UseIdentity();
        }
    }
}
