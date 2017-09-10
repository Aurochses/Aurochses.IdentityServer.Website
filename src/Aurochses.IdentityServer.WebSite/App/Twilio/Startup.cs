using Microsoft.Extensions.Configuration;
using Twilio;

namespace Aurochses.IdentityServer.WebSite.App.Twilio
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureServices(IConfigurationRoot configuration)
        {
            // Add TwilioClient
            TwilioClient.Init(configuration["Twilio:AccountSid"], configuration["Twilio:AuthToken"]);
        }
    }
}