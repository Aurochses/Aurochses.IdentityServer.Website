using System;
using Aurochses.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Aurochses.IdentityServer.Database.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        public TestServerFixture()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(ProjectHelpers.GetProjectPath(@"src", typeof(Database.Startup).Assembly))
                .UseStartup<Startup>();

            Server = new TestServer(webHostBuilder);
        }

        public TestServer Server { get; }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
