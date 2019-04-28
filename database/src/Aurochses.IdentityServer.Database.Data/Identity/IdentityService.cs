using System;
using System.Linq;
using System.Security.Claims;
using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.Database.Data.Identity.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Aurochses.IdentityServer.Database.Data.Identity
{
    public class IdentityService
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(IMapper mapper, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Run(string environmentName)
        {
            // Role
            foreach (var item in _roleManager.Roles.ToList())
            {
                var deleteIdentityResult = _roleManager.DeleteAsync(item).Result;

                if (!deleteIdentityResult.Succeeded) throw new Exception($"Can't delete role: {item.Name}. Description '{deleteIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");
            }
            
            foreach (var item in RoleData.GetList(environmentName))
            {
                var createIdentityResult = _roleManager.CreateAsync(item.Role).Result;

                if (!createIdentityResult.Succeeded) throw new Exception($"Can't create user: {item.Role.Name}. Description '{createIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");

                foreach (var roleClaim in item.RoleClaims)
                {
                    var addClaimIdentityResult = _roleManager.AddClaimAsync(item.Role, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue)).Result;

                    if (!addClaimIdentityResult.Succeeded) throw new Exception($"Can't add claim to role: {item.Role.Name}. Description '{addClaimIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");
                }
            }

            // User
            foreach (var item in _userManager.Users.ToList())
            {
                var deleteIdentityResult = _userManager.DeleteAsync(item).Result;

                if (!deleteIdentityResult.Succeeded) throw new Exception($"Can't delete user: {item.Email}. Description '{deleteIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");
            }

            foreach (var item in UserData.GetList(environmentName))
            {
                var createIdentityResult = _userManager.CreateAsync(item.User, item.Password).Result;

                if (!createIdentityResult.Succeeded) throw new Exception($"Can't create user: {item.User.Email}. Description '{createIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");

                if (item.Roles != null)
                {
                    var addToRolesIdentityResult = _userManager.AddToRolesAsync(item.User, item.Roles).Result;

                    if (!addToRolesIdentityResult.Succeeded) throw new Exception($"Can't add user: {item.User.Email} to roles. Description '{addToRolesIdentityResult.Errors.Select(x => x.Description).FirstOrDefault()}'");
                }
            }
        }
    }
}