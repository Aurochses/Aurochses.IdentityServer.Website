using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Aurochses.IdentityServer.Database.Data.IdentityServer.Data
{
    public static class ClientData
    {
        public static IEnumerable<Client> GetList()
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
