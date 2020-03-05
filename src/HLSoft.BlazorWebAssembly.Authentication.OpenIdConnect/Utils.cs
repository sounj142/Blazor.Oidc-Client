using HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	internal static class Utils
	{
		internal static ValueTask ConfigOidcAsync(IJSRuntime jsRuntime, ClientOptions clientOptions, bool overrideOldConfig = false)
		{
			return jsRuntime.InvokeVoidAsync(Constants.ConfigOidc, clientOptions, overrideOldConfig);
		}

		internal static ClientOptions CreateClientOptionsConfigData(OpenIdConnectOptions authOption, NavigationManager navigationManager)
		{
			return new ClientOptions
			{
				authority = authOption.Authority,
				client_id = authOption.ClientId,
				redirect_uri = GetAbsoluteUri(authOption.SignedInCallbackUri, navigationManager),
				silent_redirect_uri = GetAbsoluteUri(authOption.SilentRedirectUri, navigationManager),
				response_type = authOption.ResponseType,
				scope = string.Join(" ", authOption.Scope.Distinct()),
				post_logout_redirect_uri = GetAbsoluteUri(authOption.SignedOutRedirectUri, navigationManager),
				popup_redirect_uri = GetAbsoluteUri(authOption.PopupRedirectUri, navigationManager),
				loadUserInfo = authOption.LoadUserInfo,
				automaticSilentRenew = authOption.AutomaticSilentRenew,
				monitorAnonymousSession = authOption.MonitorAnonymousSession,
				revokeAccessTokenOnSignout = authOption.RevokeAccessTokenOnSignout,
				filterProtocolClaims = authOption.FilterProtocolClaims,
				popupWindowFeatures = authOption.PopupWindowFeatures
			};
		}

		private static string GetAbsoluteUri(string uri, NavigationManager navigationManager)
		{
			if (uri == null) return null;
			return uri.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || uri.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
				? uri
				: navigationManager.ToAbsoluteUri(uri).AbsoluteUri;
		}
	}
}
