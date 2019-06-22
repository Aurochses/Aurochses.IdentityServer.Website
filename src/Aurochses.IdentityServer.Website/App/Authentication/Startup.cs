using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Website.App.Authentication
{
    public static class Startup
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            // Enable Facebook Authentication
            if (configuration.GetValue<bool>("Authentication:HasFacebook"))
            {
                services.AddAuthentication()
                    .AddFacebook(
                        options =>
                        {
                            options.AppId = configuration["Facebook:AppId"];
                            options.AppSecret = configuration["Facebook:AppSecret"];
                        }
                    );
            }
        }
    }
}