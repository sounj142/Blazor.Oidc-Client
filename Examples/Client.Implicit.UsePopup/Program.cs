﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect;

namespace Client.Implicit.UsePopup
{
	public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            ConfigureServices(builder.Services);

            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }

		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthorizationCore(options => { })
				.AddBlazoredOpenIdConnect(options =>
				{
					options.Authority = "http://localhost:5000/";

					options.ClientId = "Client.Implicit.UsePopup";
					options.ResponseType = "token id_token";

					options.SignedInCallbackUri = null;
					options.PopupSignInRedirectUri = "/signin-popup-oidc";
					options.PopupSignOutRedirectUri = "/signout-popup-oidc";

					options.Scope.Add("openid");
					options.Scope.Add("profile");
					options.Scope.Add("api");
				});
		}
	}
}
