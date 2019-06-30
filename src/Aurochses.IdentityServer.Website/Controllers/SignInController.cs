using System;
using System.Threading.Tasks;
using Aurochses.IdentityServer.Website.Models.SignIn;
using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.Website.Controllers
{
    public class SignInController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var viewModel = await BuildSignInViewModel(returnUrl);

            if (viewModel.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { provider = viewModel.ExternalLoginScheme, returnUrl });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SignInInputModel model, string button)
        {
            throw new NotImplementedException();
        }

        private async Task<SignInViewModel> BuildSignInViewModel(string returnUrl)
        {
            throw new NotImplementedException();
        }
    }
}