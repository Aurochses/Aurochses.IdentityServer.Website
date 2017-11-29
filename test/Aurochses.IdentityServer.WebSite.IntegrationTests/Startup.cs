using System;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests
{
    public class Startup : WebSite.Startup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
            Configuration["ConnectionStrings:DefaultConnection"] =
                $"Server=(localdb)\\mssqllocaldb;Database=Aurochses.IdentityServer.WebSite.IntegrationTests_{Guid.NewGuid()};Trusted_Connection=True;";

            // Disable ReCaptcha
            Configuration["Recaptcha:Enabled"] = false.ToString();
        }
    }
}