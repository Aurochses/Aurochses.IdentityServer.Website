using System.Net.Http;
using System.Threading.Tasks;
using Aurochses.IdentityServer.Client.Console.Settings;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.Client.Console
{
    /// <summary>
    /// Class Service.
    /// </summary>
    public class Service
    {
        private readonly ServiceSettings _serviceSettings;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class.
        /// </summary>
        /// <param name="serviceSettings">The service settings.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public Service(IOptions<ServiceSettings> serviceSettings, HttpClient httpClient)
        {
            _serviceSettings = serviceSettings.Value;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Run.
        /// </summary>
        public async Task Run()
        {
            var response = await _httpClient.GetAsync($"{_serviceSettings.ApiUrl}/api/values/1");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                System.Console.WriteLine(content);
            }
            else
            {
                System.Console.WriteLine(response.StatusCode);
            }
        }
    }
}
