using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.WebSite.Helpers
{
    /// <summary>
    /// Class ControllerHelpers.
    /// </summary>
    public static class ControllerHelpers
    {
        /// <summary>
        /// Adds the Identity errors to model state.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="result">The result.</param>
        public static void AddErrors(this Controller controller, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                controller.ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}