using System.Threading.Tasks;
using Aurochses.IdentityServer.WebSite.Filters;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.WebSite.Controllers
{
    /// <summary>
    /// Class ErrorController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [SecurityHeaders]
    public class ErrorController : Controller
    {
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="identityServerInteractionService">The interaction.</param>
        public ErrorController(IIdentityServerInteractionService identityServerInteractionService)
        {
            _identityServerInteractionService = identityServerInteractionService;
        }

        /// <summary>
        /// Indexes the specified error identifier.
        /// </summary>
        /// <param name="errorId">The error identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Index(string errorId)
        {
            // retrieve error details from identityserver
            var errorMessage = await _identityServerInteractionService.GetErrorContextAsync(errorId);
            if (errorMessage != null)
            {
                return View("Error", errorMessage);
            }

            return View("Error");
        }
    }
}
