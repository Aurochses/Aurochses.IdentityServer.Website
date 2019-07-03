using System.Threading.Tasks;
using Aurochses.IdentityServer.Website.Filters;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aurochses.IdentityServer.Website.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ErrorController(
            ILogger<ErrorController> logger,
            IHostingEnvironment hostingEnvironment,
            IIdentityServerInteractionService identityServerInteractionService
        )
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _identityServerInteractionService = identityServerInteractionService;
        }

        public async Task<IActionResult> Index(string errorId)
        {
            var errorMessage = await _identityServerInteractionService.GetErrorContextAsync(errorId);
            if (errorMessage != null)
            {
                _logger.LogError("IdentityServer interaction error. Id: {@errorId}, Message: {@errorMessage}", errorId, errorMessage);

                if (!_hostingEnvironment.IsDevelopment())
                {
                    errorMessage.ErrorDescription = null;
                }
            }

            return View("Error", errorMessage);
        }
    }
}
