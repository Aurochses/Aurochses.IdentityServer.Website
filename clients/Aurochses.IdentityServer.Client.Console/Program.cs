using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Client.Console
{
    /// <summary>
    /// Class Program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // get environment name
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // startup
            var startup = new Startup(BuildConfiguration(environmentName));

            // dependency injection
            var serviceCollection = new ServiceCollection();
            startup.ConfigureServices(serviceCollection);
            var services = serviceCollection.BuildServiceProvider();

            // Service
            var service = services.GetService<Service>();

            await service.Run();
        }

        /// <summary>
        /// Builds the configuration.
        /// </summary>
        /// <param name="environmentName">Name of the environment.</param>
        /// <returns>IConfigurationRoot.</returns>
        public static IConfigurationRoot BuildConfiguration(string environmentName) =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
    }
}
