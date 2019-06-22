using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.Website.App.ExceptionHandler
{
    public static class Startup
    {
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();

                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler(configuration["ExceptionHandler:Path"]);

                app.UseStatusCodePagesWithRedirects(configuration["StatusCodePages:Path"]);
            }
        }
    }
}