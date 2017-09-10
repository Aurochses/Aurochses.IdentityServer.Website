using Aurochses.IdentityServer.WebSite.App.Project;
using Aurochses.IdentityServer.WebSite.Models.Shared.Components.Help;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.WebSite.Components
{
    /// <summary>
    /// Class HelpViewComponent.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ViewComponent" />
    public class HelpViewComponent : ViewComponent
    {
        private readonly ProjectOptions _projectOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpViewComponent"/> class.
        /// </summary>
        /// <param name="projectOptions"></param>
        public HelpViewComponent(IOptions<ProjectOptions> projectOptions)
        {
            _projectOptions = projectOptions.Value;
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <returns>IViewComponentResult.</returns>
        public IViewComponentResult Invoke()
        {
            var model = new HelpViewModel
            {
                SupportEmail = _projectOptions.SupportEmail,
                SupportUrl = _projectOptions.SupportUrl
            };

            return View(model);
        }
    }
}