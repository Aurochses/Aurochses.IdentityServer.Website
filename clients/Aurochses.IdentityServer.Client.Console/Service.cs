﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
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
        public Task Run()
        {
            throw new NotImplementedException();
        }
    }
}
