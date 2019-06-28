using System.Collections.Generic;
using Aurochses.IdentityServer.Website.Filters;
using Aurochses.IdentityServer.Website.Tests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Aurochses.IdentityServer.Website.Tests.Filters
{
    public class SecurityHeadersAttributeTests
    {
        private const string ContentSecurityPolicy = "default-src 'self';" +
                                                     "object-src 'none';" +
                                                     "frame-ancestors 'none';" +
                                                     "sandbox allow-forms allow-same-origin allow-scripts;" +
                                                     "base-uri 'self';" +
                                                     "upgrade-insecure-requests;";

        private const string ReferrerPolicy = "no-referrer";

        private static readonly List<KeyValuePair<string, string>> ResponseHeaders = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("X-Content-Type-Options", "nosniff"),
            new KeyValuePair<string, string>("X-Frame-Options", "SAMEORIGIN"),
            new KeyValuePair<string, string>("Content-Security-Policy", ContentSecurityPolicy),
            new KeyValuePair<string, string>("X-Content-Security-Policy", ContentSecurityPolicy),
            new KeyValuePair<string, string>("Referrer-Policy", ReferrerPolicy)
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
        [InlineData("Referrer-Policy", "TestValue")]
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