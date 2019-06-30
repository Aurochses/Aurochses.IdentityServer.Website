using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Aurochses.IdentityServer.Website.IntegrationTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private string _environmentName;

        public TestWebApplicationFactory()
        {
            _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            ClientOptions.AllowAutoRedirect = false;
        }

        public string EnvironmentName
        {
            get => _environmentName;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                _environmentName = value;
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment(_environmentName);
        }
    }
}