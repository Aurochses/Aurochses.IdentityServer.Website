using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aurochses.IdentityServer.WebSite.Filters
{
    /// <inheritdoc />
    /// <summary>
    /// Class SecurityHeadersAttribute.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        private const string Csp = "default-src 'self';" +
                                   "script-src 'self' 'unsafe-inline' www.google.com www.gstatic.com localhost:*;" +
                                   "style-src 'self' 'unsafe-inline';" +
                                   "frame-src 'self' www.google.com;" +
                                   "connect-src 'self' localhost:* ws://localhost:*;";

        /// <summary>
        /// Called when [result executing].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <inheritdoc />
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;

            if (result is ViewResult)
            {
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                }

                // once for standards compliant browsers
                if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Content-Security-Policy", Csp);
                }

                // and once again for IE
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", Csp);
                }
            }
        }
    }
}