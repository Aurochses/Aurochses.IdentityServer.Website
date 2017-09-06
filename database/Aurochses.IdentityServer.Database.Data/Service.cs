using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;

namespace Aurochses.IdentityServer.Database.Data
{
    public class Service
    {
        private readonly ConfigurationDbContext _context;

        public Service(ConfigurationDbContext context)
        {
            _context = context;
        }

        public void Run()
        {
            // IdentityResources
            _context.RemoveRange(_context.IdentityResources);
            _context.SaveChanges();

            foreach (var resource in GetIdentityResources())
            {
                _context.IdentityResources.Add(resource.ToEntity());
            }
            _context.SaveChanges();

            // ApiResources
            _context.RemoveRange(_context.ApiResources);
            _context.SaveChanges();

            foreach (var resource in GetApiResources())
            {
                _context.ApiResources.Add(resource.ToEntity());
            }
            _context.SaveChanges();

            // Clients
            _context.RemoveRange(_context.Clients);
            _context.SaveChanges();

            foreach (var client in GetClients())
            {
                _context.Clients.Add(client.ToEntity());
            }
            _context.SaveChanges();
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResource
                {
                    Name = "permissions",
                    UserClaims = { "permission" }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> { new ApiResource("api", "API") };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "PostmanUser",
                    ClientName = "PostmanUser",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        "api"
                    }
                },
                new Client
                {
                    ClientId = "angular2client",
                    ClientName = "angular2client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false, // to not show consent page
                    AccessTokenLifetime = 300, // 5 minute, default 60 minutes
                    RedirectUris = new List<string>
                    {
                        "http://localhost:4100/auth",
                        "http://localhost:4100/renew"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:4100/unauthorized"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4100"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        "permissions",
                        "api"
                    }
                }
            };
        }
    }
}