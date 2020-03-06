// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = IdentityServerConstants.StandardScopes.Address,
                    DisplayName = "Address",
                    UserClaims = new [] { "address", "phone_number" }
                },
                new IdentityResource
                {
                    Name = "api_role",
                    DisplayName = "Api Role",
                    UserClaims = new [] { "api_role" }
                }
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api", "Weather Forecast API", new[] { "api_role" })
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "Client.Code",
                    ClientName = "Client With Grant Type Code",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris = {
                        "http://localhost:5005/signin-callback-oidc",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5005/" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api"
                    }
                },

                new Client
                {
                    ClientId = "Client.Implicit.UsePopup",
                    ClientName = "Client.Implicit.UsePopup",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireClientSecret = false,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = {
                        "http://localhost:5003/signin-popup-oidc",
                        "http://localhost:5003/signout-popup-oidc",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5003/", "http://localhost:5003/signout-popup-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api"
                    }
                },

                new Client
                {
                    ClientId = "Client.Code.CustomizeUri",
                    ClientName = "Client With Grant Type Code using default callback Uris",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris = {
                        "http://localhost:5006/fantastic-url-for-redirect",
                        "http://localhost:5006/wonderful-link-for-popup-login",
                        "http://localhost:5006/sign-out-popup-here",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5006/", "http://localhost:5006/sign-out-popup-here" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api"
                    }
                },

                new Client
                {
                    ClientId = "Client.Code.Complex",
                    ClientName = "A more complex example with variety claims, custom ClaimParser, automatic renew, ect.",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris = {
                        "http://localhost:5002/signin-popup-oidc",
                        "http://localhost:5002/signin-callback-oidc",
                        "http://localhost:5002/silent-callback-oidc",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5002/" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        "api_role",
                        "api",
                    },
                    AllowOfflineAccess = true,
                    // when AccessTokenLifetime is a low value (maybe <= 50, I'm not sure). Oidc-client do the signin silent very often, it's so weird.
                    AccessTokenLifetime = 100,
                },


        };
    }
}