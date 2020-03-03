﻿using HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
    public static class ServiceCollectionExtensions
	{
        public static IServiceCollection AddBlazoredOpenIdConnect(this IServiceCollection services,
            Action<OpenIdConnectOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<OpenIdConnectOptions>>().Value);

            services.AddSingleton(resolver => {
                var authOptions = resolver.GetRequiredService<OpenIdConnectOptions>();
                var navigationManager = resolver.GetRequiredService<NavigationManager>();
                return Utils.CreateClientOptionsConfigData(authOptions, navigationManager);
            });

            return services
                .AddScoped<AuthenticationStateProvider, BlazorAuthenticationStateProvider>()
                .AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
