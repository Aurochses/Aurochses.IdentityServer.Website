using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Aurochses.IdentityServer.Website.IntegrationTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly string _environmentName;

        public TestWebApplicationFactory(string environmentName = "")
        {
            _environmentName = string.IsNullOrWhiteSpace(environmentName)
                ? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
                : environmentName;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment(_environmentName);
        }
    }
}