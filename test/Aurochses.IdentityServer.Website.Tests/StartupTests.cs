using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests
{
    public class StartupTests
    {
        [Fact]
        public void Configuration_Get_Success()
        {
            // Arrange
            var configuration = new ConfigurationRoot(new List<IConfigurationProvider>());

            var startup = new Startup(configuration);

            // Act & Assert
            Assert.Equal(configuration, startup.Configuration);
        }
    }
}