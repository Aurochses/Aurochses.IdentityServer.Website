using System;
using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Aurochses.IdentityServer.Website.App.IdentityServer
{
    public class IdentityServerProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        public IdentityServerProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject;
            if (subject == null) throw new ArgumentNullException(nameof(context.Subject));

            var user = await _userManager.FindByIdAsync(subject.GetSubjectId());
            if (user == null) throw new Exception("User not found.");

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            context.AddRequestedClaims(principal.Claims);
        }

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