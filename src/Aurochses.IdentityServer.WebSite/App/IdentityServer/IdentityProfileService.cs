using System;
using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Aurochses.IdentityServer.WebSite.App.IdentityServer
{
    /// <summary>
    /// Class IdentityProfileService.
    /// </summary>
    /// <seealso cref="T:IdentityServer4.Services.IProfileService" />
    public class IdentityProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityProfileService"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="userClaimsPrincipalFactory">The user claims principal factory.</param>
        public IdentityProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException">Subject</exception>
        /// <exception cref="T:System.Exception">User not found.</exception>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject;
            if (subject == null) throw new ArgumentNullException(nameof(context.Subject));

            var user = await _userManager.FindByIdAsync(subject.GetSubjectId());
            if (user == null) throw new Exception("User not found.");

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            context.AddFilteredClaims(principal.Claims);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException">Subject</exception>
        /// <exception cref="T:System.Exception">User not found.</exception>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject;
            if (subject == null) throw new ArgumentNullException(nameof(context.Subject));

            var user = await _userManager.FindByIdAsync(subject.GetSubjectId());
            if (user == null) throw new Exception("User not found.");

            context.IsActive = true;
        }
    }
}