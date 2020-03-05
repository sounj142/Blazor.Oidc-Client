using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect;
using Client.Code.Complex.Auth;

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
			services.AddAuthorizationCore(options => { })
				//.AddBlazoredOpenIdConnect(options =>
				.AddBlazoredOpenIdConnect<User, CustomClaimsParser>(options =>
				{
					options.Authority = "http://localhost:5000/";

					options.ClientId = "Client.Code.Complex";
					options.ResponseType = "code";

					options.AutomaticSilentRenew = true;

					options.PopupRedirectUri = "/signin-popup-oidc";
					options.SignedInCallbackUri = "/signin-callback-oidc";
					options.SilentRedirectUri = "/silent-callback-oidc";

					options.Scope.Add("openid");
					options.Scope.Add("profile");
					options.Scope.Add("email");
					options.Scope.Add("address");
					options.Scope.Add("api_role");
					options.Scope.Add("api");
					options.Scope.Add("offline_access");
				});
		}
	}
}
