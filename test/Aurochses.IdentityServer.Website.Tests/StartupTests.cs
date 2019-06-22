using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests
{
    public class StartupTests
    {
        [Fact]
        public void HostingEnvironment_Get_Success()
        {
            // Arrange
            var hostingEnvironment = new HostingEnvironment();
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());

            var startup = new Startup(hostingEnvironment, configuration);

            // Act & Assert
            Assert.Equal(hostingEnvironment, startup.HostingEnvironment);
        }

        [Fact]
        public void Configuration_Get_Success()
        {
            // Arrange
            var hostingEnvironment = new HostingEnvironment();
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());

            var startup = new Startup(hostingEnvironment, configuration);

            // Act & Assert
            Assert.Equal(configuration, startup.Configuration);
        }
    }
}