using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Website.App.Authentication
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var builder = services.AddAuthentication();

            // Enable Facebook Authentication
            if (configuration.GetValue<bool>("Authentication:HasFacebook"))
            {
                builder.AddFacebook(
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