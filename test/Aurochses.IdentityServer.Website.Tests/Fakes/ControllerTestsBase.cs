using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;

namespace Aurochses.IdentityServer.Website.Tests.Fakes
{
    public class ControllerTestsBase<TController>
        where TController : Controller
    {
        public ControllerTestsBase()
        {
            MockLogger = new Mock<ILogger<TController>>(MockBehavior.Strict);
            MockLogger
                .Setup(
                    x => x.Log(
                        It.IsAny<LogLevel>(),
                        It.IsAny<EventId>(),
                        It.IsAny<FormattedLogValues>(),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<object, Exception, string>>()
                    )
                )
                .Verifiable();

            MockHostingEnvironment = new Mock<IHostingEnvironment>(MockBehavior.Strict);
            SetupHostingEnvironmentName(string.Empty);
        }

        public Mock<ILogger<TController>> MockLogger { get; }
        public Mock<IHostingEnvironment> MockHostingEnvironment { get; }

        public void VerifyLogger(LogLevel logLevel, Func<Times> times)
        {
            MockLogger
                .Verify(
                    x => x.Log(
                        logLevel,
                        It.IsAny<EventId>(),
                        It.IsAny<FormattedLogValues>(),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<object, Exception, string>>()
                    ),
                    times
                );
        }

        public void SetupHostingEnvironmentName(string environmentName)
        {
            MockHostingEnvironment
                .SetupGet(x => x.EnvironmentName)
                .Returns(environmentName);
        }
    }
}