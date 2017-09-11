using System.Collections.Generic;
using Aurochses.IdentityServer.WebSite.Filters;
using Aurochses.IdentityServer.WebSite.Tests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Filters
{
    public class SecurityHeadersAttributeTests
    {
        private const string Csp = "default-src 'self';" +
                                   "script-src 'self' 'unsafe-inline' www.google.com www.gstatic.com localhost:*;" +
                                   "style-src 'self' 'unsafe-inline';" +
                                   "frame-src 'self' www.google.com;" +
                                   "connect-src 'self' localhost:* ws://localhost:*;";

        private static readonly List<KeyValuePair<string, string>> ResponseHeaders = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("X-Content-Type-Options", "nosniff"),
            new KeyValuePair<string, string>("X-Frame-Options", "SAMEORIGIN"),
            new KeyValuePair<string, string>("Content-Security-Policy", Csp),
            new KeyValuePair<string, string>("X-Content-Security-Policy", Csp)
        };

        private readonly SecurityHeadersAttribute _attribute;

        public SecurityHeadersAttributeTests()
        {
            _attribute = new SecurityHeadersAttribute();
        }

        [Fact]
        public void Inherit_Controller()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<ActionFilterAttribute>(_attribute);
        }

        [Fact]
        public void OnResultExecuting_WhenContextResultIsNotViewResult()
        {
            // Arrange
            var context = new ResultExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new RedirectResult("http://www.example.com"),
                new FakeController()
            );

            // Act
            _attribute.OnResultExecuting(context);

            // Assert
            Assert.Empty(context.HttpContext.Response.Headers);
        }

        [Theory]
        [InlineData("X-Content-Type-Options", "TestValue")]
        [InlineData("X-Frame-Options", "TestValue")]
        [InlineData("Content-Security-Policy", "TestValue")]
        [InlineData("X-Content-Security-Policy", "TestValue")]
        public void OnResultExecuting_WhenContextResultIsViewResult(string key, string value)
        {
            // Arrange
            var context = new ResultExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new ViewResult(),
                new FakeController()
            );

            context.HttpContext.Response.Headers.Add(key, value);

            // Act
            _attribute.OnResultExecuting(context);

            // Assert
            Assert.Equal(ResponseHeaders.Count, context.HttpContext.Response.Headers.Count);
            Assert.Equal(value, context.HttpContext.Response.Headers[key]);

            foreach (var responseHeader in ResponseHeaders)
            {
                if (responseHeader.Key == key) continue;

                Assert.Equal(responseHeader.Value, context.HttpContext.Response.Headers[responseHeader.Key]);
            }
        }
    }
}