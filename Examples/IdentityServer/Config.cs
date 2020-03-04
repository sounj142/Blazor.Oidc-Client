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
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {

                //// JavaScript Client
                //new Client
                //{
                //    ClientId = "js",
                //    ClientName = "JavaScript Client",
                //    AllowedGrantTypes = GrantTypes.Code,
                //    RequirePkce = true,
                //    RequireClientSecret = false,

                //    RedirectUris =           { "http://localhost:5003/callback.html" },
                //    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                //    AllowedCorsOrigins =     { "http://localhost:5003" },

                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        "api"
                //    },

                //    //AllowOfflineAccess = true,
                //    //AccessTokenLifetime = 12,
                //},

                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "js Client",
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris = { "http://localhost:5003/callback.html", "http://localhost:5003/silent-callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/" },
                    //FrontChannelLogoutUri = "http://localhost:5003/silent-callback.html", // for testing identityserver on localhost

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api" },

                    //AllowedCorsOrigins =     { "http://localhost:5003" },
                },


                // JavaScript Client
                new Client
                {
                    ClientId = "blazor.webassembly.js",
                    ClientName = "Blazor Webassembly Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    AccessTokenType = AccessTokenType.Jwt,

                    RequireConsent = false,

                    RedirectUris = {
                        "http://localhost:5005/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/signin-callback-oidc.html",
                        "http://localhost:5005/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/silent-callback-oidc.html",
                        "http://localhost:5005/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/signin-popup-oidc.html",
                        "http://localhost:5005/signin-callback-oidc",
                        "http://localhost:5005/silent-callback-oidc",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5005/" },
                    //AllowedCorsOrigins =     { "http://localhost:5005" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api"
                    },

                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 100,
                },

                new Client
                {
                    ClientId = "blazor.webassembly.js1",
                    ClientName = "Blazor Webassembly Client",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AccessTokenType = AccessTokenType.Jwt,

                    RedirectUris = {
                        "http://localhost:5005/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/signin-callback-oidc.html",
                        "http://localhost:5005/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/silent-callback-oidc.html",
                        "http://localhost:5005/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/signin-popup-oidc.html",
                        "http://localhost:5005/signin-callback-oidc",
                        "http://localhost:5005/silent-callback-oidc",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5005/" },

                    //AllowedCorsOrigins = { "http://localhost:5005" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api"
                    },

                    AccessTokenLifetime = 100,
                    AllowOfflineAccess = true,
                },

                new Client
                {
                    ClientId = "Client.Code",
                    ClientName = "Client With Grant Type Code",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris = {
                        "http://localhost:5005/signin-popup-oidc",
                        "http://localhost:5005/signin-callback-oidc",
                        "http://localhost:5005/silent-callback-oidc",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5005/" },
                    AllowedCorsOrigins = { "http://localhost:5005" },

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
                    ClientId = "Client.Code.DefaultUri",
                    ClientName = "Client With Grant Type Code using default callback Uris",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris = {
                        "http://localhost:5006/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/signin-callback-oidc.html",
                        "http://localhost:5006/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/silent-callback-oidc.html",
                        "http://localhost:5006/_content/HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect/signin-popup-oidc.html",
                    },
                    PostLogoutRedirectUris = { "http://localhost:5006/" },
                    AllowedCorsOrigins = { "http://localhost:5006" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api"
                    }
                },

                // implicit (e.g. SPA or OIDC authentication)
                new Client
                {
                    ClientId = "implicit",
                    ClientName = "Implicit Client",
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris = { "http://localhost:4200/assets/signin-callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:4200/" },
                    FrontChannelLogoutUri = "http://localhost:4200/assets/silent-callback.html", // for testing identityserver on localhost

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api" },

                    //AllowedCorsOrigins =     { "http://localhost:4200" },
                    
                },

                ///////////////////////////////////////////
                // MVC Hybrid Flow Sample (Automatic Refresh)
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "mvc.tokenmanagement",
                    ClientName = "MVC Hybrid (with automatic refresh)",

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    AccessTokenLifetime = 12,

                    RedirectUris = { "http://localhost:5004/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5004/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5004/signout-callback-oidc" },

                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api"
                    },
                },

                ///////////////////////////////////////////
                // JS OIDC Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "js_oidc",
                    ClientName = "JavaScript OIDC Client",
                    //ClientUri = "http://identityserver.io",
                    //LogoUri = "https://pbs.twimg.com/profile_images/1612989113/Ki-hanja_400x400.png",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AccessTokenType = AccessTokenType.Jwt,

                    RedirectUris =
                    {
                        "http://localhost:7017/index.html",
                        "http://localhost:7017/callback.html",
                        "http://localhost:7017/silent.html",
                        "http://localhost:7017/popup.html"
                    },

                    PostLogoutRedirectUris = { "http://localhost:7017/index.html" },
                    AllowedCorsOrigins = { "http://localhost:7017" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api"
                    },

                    AccessTokenLifetime = 100,
                    AllowOfflineAccess = true,
                },

                ///////////////////////////////////////////
                // JS OAuth 2.0 Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "js_oauth",
                    ClientName = "JavaScript OAuth 2.0 Client",
                    // ClientUri = "http://identityserver.io",
                    //LogoUri = "https://pbs.twimg.com/profile_images/1612989113/Ki-hanja_400x400.png",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:28895/index.html" },
                    AllowedScopes = { "api" }
                },
        };
    }
}