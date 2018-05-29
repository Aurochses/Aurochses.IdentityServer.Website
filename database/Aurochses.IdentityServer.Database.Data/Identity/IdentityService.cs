using System;
using System.Linq;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.Database.Data.Identity.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Aurochses.IdentityServer.Database.Data.Identity
{
    public class IdentityService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public void Run(string environmentName)
        {
            // User
            foreach (var item in UserData.GetList(environmentName))
            {
                var existingItem = _userManager.FindByNameAsync(item.User.Email).Result;

                if (existingItem != null)
                {
                    existingItem = _mapper.Map(item.User, existingItem);

                    var updateIdentityResult = _userManager.UpdateAsync(existingItem).Result;
                    if (!updateIdentityResult.Succeeded) throw new Exception($"Can't update user: {item.User.Email}. Description '{updateIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");

                    existingItem = _userManager.FindByNameAsync(item.User.Email).Result;
                    if (existingItem == null) continue;

                    if (!_userManager.CheckPasswordAsync(existingItem, item.Password).Result)
                    {
                        var token = _userManager.GeneratePasswordResetTokenAsync(existingItem).Result;
                        var resetPasswordIdentityResult = _userManager.ResetPasswordAsync(existingItem, token, item.Password).Result;

                        if (!resetPasswordIdentityResult.Succeeded) throw new Exception($"Can't reset password for user: {existingItem.Email}. Description '{resetPasswordIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");
                    }
                }
                else
                {
                    var createIdentityResult = _userManager.CreateAsync(item.User, item.Password).Result;

                    if (!createIdentityResult.Succeeded) throw new Exception($"Can't create user: {item.User.Email}. Description '{createIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");
                }
            }
        }
    }
}