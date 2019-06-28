using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aurochses.IdentityServer.Website.Filters
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
        private const string ContentSecurityPolicy = "default-src 'self';" +
                                                     "object-src 'none';" +
                                                     "frame-ancestors 'none';" +
                                                     "sandbox allow-forms allow-same-origin allow-scripts;" +
                                                     "base-uri 'self';" +
                                                     "upgrade-insecure-requests;";

        private const string ReferrerPolicy = "no-referrer";

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;

            if (result is ViewResult)
            {
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                }

                // once for standards compliant browsers
                if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Content-Security-Policy", ContentSecurityPolicy);
                }
                // and once again for IE
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", ContentSecurityPolicy);
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
                if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Referrer-Policy", ReferrerPolicy);
                }
            }
        }
    }
}