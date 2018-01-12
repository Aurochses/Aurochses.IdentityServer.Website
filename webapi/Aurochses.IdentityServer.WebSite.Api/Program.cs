using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Aurochses.IdentityServer.WebSite.Api
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
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Build web host
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>IWebHost</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}