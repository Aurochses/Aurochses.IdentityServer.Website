using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Aurochses.IdentityServer.WebSite.App.Logging
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            // Logging
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
        }
    }
}
