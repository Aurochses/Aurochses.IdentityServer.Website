using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Client.Console.App.IdentityServer
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static async Task ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // discover endpoints from metadata
            var discoveryClient = new DiscoveryClient(configuration.GetValue<string>("IdentityServer:Authority"))
            {
                Policy =
                {
                    RequireHttps = configuration.GetValue<bool>("IdentityServer:RequireHttps")
                }
            };

            var discoveryResponse = await discoveryClient.GetAsync();
            if (discoveryResponse.IsError) throw new Exception(discoveryResponse.Error);

            // request token
            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, configuration.GetValue<string>("IdentityServer:ClientId"), configuration.GetValue<string>("IdentityServer:ClientSecret"));
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(configuration.GetValue<string>("IdentityServer:Scope"));
            if (tokenResponse.IsError) throw new Exception(tokenResponse.Error);

            // configure api HTTP client
            var apiHttpClient = new ApiHttpClient();
            apiHttpClient.SetBearerToken(tokenResponse.AccessToken);

            services.AddTransient(provider => apiHttpClient);
        }
    }
}