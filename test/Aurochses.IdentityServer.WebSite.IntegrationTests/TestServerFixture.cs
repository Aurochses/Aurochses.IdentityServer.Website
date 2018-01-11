using System;
using System.Net.Http;
using Aurochses.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        public TestServerFixture()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(
                    ProjectHelpers.GetFolderPath("Aurochses.IdentityServer.WebSite", "src", "Aurochses.IdentityServer.WebSite")
                )
                .UseEnvironment(environmentName)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(
                    (context, builder) =>
                    {
                        var env = context.HostingEnvironment;

                        builder.AddJsonFile("appsettings.json")
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
                    }
                );

            Server = new TestServer(webHostBuilder);

            Client = Server.CreateClient();
        }

        protected TestServer Server { get; }

        public HttpClient Client { get; }

        public virtual void Dispose()
        {
            Client.Dispose();

            Server.Dispose();
        }
    }
}