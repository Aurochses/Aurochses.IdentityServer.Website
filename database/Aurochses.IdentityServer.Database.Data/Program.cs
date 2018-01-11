using System;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Database.Data
{
    public class Program
    {
        public static Startup Startup { get; set; }
        public static IServiceProvider Services { get; set; }

        public static void Main(string[] args)
        {
            // get environment name
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // startup
            Startup = new Startup(environmentName);

            // dependency injection
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();

            // Service
            var service = Services.GetService<Service>();

            service.Run();
        }
    }
}