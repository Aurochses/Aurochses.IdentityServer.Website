using Aurochses.IdentityServer.WebSite.App.Project;
using Aurochses.IdentityServer.WebSite.Models.Shared.Components.ProjectName;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aurochses.IdentityServer.WebSite.Components
{
    /// <summary>
    /// Class ProjectNameViewComponent.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ViewComponent" />
    public class ProjectNameViewComponent : ViewComponent
    {
        private readonly ProjectOptions _projectOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectNameViewComponent"/> class.
        /// </summary>
        /// <param name="projectOptions"></param>
        public ProjectNameViewComponent(IOptions<ProjectOptions> projectOptions)
        {
            _projectOptions = projectOptions.Value;
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <returns>IViewComponentResult.</returns>
        public IViewComponentResult Invoke()
        {
            var model = new ProjectNameViewModel
            {
                Value = _projectOptions.Name
            };

            return View(model);
        }
    }
}