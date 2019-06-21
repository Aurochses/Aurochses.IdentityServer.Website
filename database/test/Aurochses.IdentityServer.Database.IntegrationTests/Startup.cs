using System;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.Database.IntegrationTests
{
    public class Startup : Database.Startup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
            Configuration["ConnectionStrings:DefaultConnection"] = $"Server=(localdb)\\mssqllocaldb;Database=Aurochses.IdentityServer.Database_{Guid.NewGuid()};Trusted_Connection=True;";
        }
    }
}
