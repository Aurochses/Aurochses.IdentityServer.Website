using System;
using Microsoft.AspNetCore.Hosting;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests
{
    public class Startup : WebSite.Startup
    {
        public Startup(IHostingEnvironment env)
            : base(env)
        {
            Configuration["Data:DefaultConnection:ConnectionString"] =
                $"Server=(localdb)\\mssqllocaldb;Database=Aurochses.IdentityServer.WebSite.IntegrationTests_{Guid.NewGuid()};Trusted_Connection=True;";

            // Disable ReCaptcha
            Configuration["Recaptcha:Enabled"] = false.ToString();
        }
    }
}