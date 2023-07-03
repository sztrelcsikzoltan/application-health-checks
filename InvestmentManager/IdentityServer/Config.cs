using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                // further resources (not used)
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("InvestmentManagerAPI","Investment Manager API")
            };
        }

        /*
        // with specifying JwtClaimTypes
        public static IEnumerable<ApiResource> GetApis()
        {
            var resources = new List<ApiResource>();

            resources.Add(new ApiResource("InvestmentManagerAPI", "Investment Manager API", new[] { JwtClaimTypes.Subject, JwtClaimTypes.Email, JwtClaimTypes.Role, JwtClaimTypes.Profile }));

            return resources;
        }
        */

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client1",
                    AllowedGrantTypes  = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret1".Sha256())
                    },
                    AllowedScopes = {"InvestmentManagerAPI"},
                    Claims = { new System.Security.Claims.Claim("policy","healthChecks")} //client_policy at client end!

                },
                new Client
                {
                    ClientId = "client2",
                    AllowedGrantTypes  = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret2".Sha256())
                    },
                    AllowedScopes = {"InvestmentManagerAPI"},
                    // Claims = { new System.Security.Claims.Claim("policy","healthChecks")} //client_policy at client end!

                }
            };
        }

        // configuring Clients in a more detailed way (not used)
        // private static object _securityConfig;
        public static IEnumerable<Client> Clients()
        {

            var Clients = new List<Client>();

            Clients.Add(new Client
            {
                ClientId = "client",
                // ClientSecrets = { new Secret(_securityConfig.Secret.Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "identity" }
            });

            Clients.Add(new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",

                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                //RequirePkce = true,
                ClientSecrets = { new Secret("_securityConfig.Secret".Sha256()) },
                RequireConsent = false,
                //RedirectUris = _securityConfig.RedirectURIs,
                //FrontChannelLogoutUri = _securityConfig.SignoutUris,
                //PostLogoutRedirectUris = _securityConfig.PostLogoutUris,
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "identity"
                }

            });

            return Clients;
        }

        public static IEnumerable<Client> GetClients2()
        {
            // client credentials client
            return new List<Client>
            {
                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.angular",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        "api1"
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                }
            };
        }

    }
}