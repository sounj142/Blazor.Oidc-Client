﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect;
using Client.Code.Complex.Auth;
using Microsoft.AspNetCore.Authorization;
using MatBlazor;

namespace Client.Code.Complex
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
			services.AddAuthorizationCore(options =>
			{
				options.AddPolicy(
					Policies.CanManageWeatherForecast,
					new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.RequireClaim("api_role", "Admin")
						.Build());
			});


			//services.AddBlazoredOpenIdConnect(options => // switch to this line to use default ClaimsParser
			services.AddBlazoredOpenIdConnect<User, CustomClaimsParser>(options => // note: don't use this config with External Google/IdentityServer, the User class is not compatible with claims from these source
			{
				options.Authority = "http://localhost:5000/";

				options.ClientId = "Client.Code.Complex";
				options.ResponseType = "code";

				options.AutomaticSilentRenew = true;

				options.PopupSignInRedirectUri = "/signin-popup-oidc";
				options.SignedInCallbackUri = "/signin-callback-oidc";
				options.SilentRedirectUri = "/silent-callback-oidc";

				options.WriteErrorToConsole = true;

				options.Scope.Add("openid");
				options.Scope.Add("profile");
				options.Scope.Add("email");
				options.Scope.Add("address");
				options.Scope.Add("api_role");
				options.Scope.Add("api");
				options.Scope.Add("offline_access");
			});

			services.AddMatToaster(config =>
			{
				config.Position = MatToastPosition.BottomRight;
				config.PreventDuplicates = true;
				config.NewestOnTop = true;
				config.ShowCloseButton = true;
				config.MaximumOpacity = 95;
				config.VisibleStateDuration = 3000;
			});
		}
	}
}
