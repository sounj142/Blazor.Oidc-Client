using HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	public class BlazorAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly ClientOptions _clientOptions;
		private readonly NavigationManager _navigationManager;

		public BlazorAuthenticationStateProvider(IJSRuntime jsRuntime, NavigationManager myNavigationManager, ClientOptions clientOptions)
		{
			_jsRuntime = jsRuntime;
			_navigationManager = myNavigationManager;
			_clientOptions = clientOptions;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			if (await HandleKnownUri())
			{
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}

			await Utils.ConfigOidcAsync(_jsRuntime, _clientOptions);

			var user = await _jsRuntime.InvokeAsync<object>(Constants.GetUser);
			var claims = ParseClaims(user);
			return claims.Count == 0 ? new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))
				: new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer")));
		}

		private async Task<bool> HandleKnownUri()
		{
			if (_navigationManager.Uri.StartsWith(_clientOptions.redirect_uri, StringComparison.OrdinalIgnoreCase))
			{
				await _jsRuntime.InvokeVoidAsync(Constants.ProcessSigninCallback);
				return true;
			}
			if (_navigationManager.Uri.StartsWith(_clientOptions.silent_redirect_uri, StringComparison.OrdinalIgnoreCase))
			{
				await _jsRuntime.InvokeVoidAsync(Constants.ProcessSilentCallback);
				return true;
			}
			if (_navigationManager.Uri.StartsWith(_clientOptions.popup_redirect_uri, StringComparison.OrdinalIgnoreCase))
			{
				await _jsRuntime.InvokeVoidAsync(Constants.ProcessSigninPopup);
				return true;
			}
			return false;
		}

		private IList<Claim> ParseClaims(object claims)
		{
			var result = new List<Claim>();
			if (claims == null) 
				return result;
			var claimsObj = (JsonElement)claims;
			if (claimsObj.ValueKind != JsonValueKind.Object) 
				return result;

			ParseClaims(claimsObj, result, 1);

			return result;
		}

		private void ParseClaims(JsonElement jsonElem, IList<Claim> claims, int level)
		{
			foreach(var item in jsonElem.EnumerateObject())
			{
				switch (item.Value.ValueKind)
				{
					case JsonValueKind.Null:
					case JsonValueKind.Undefined:
						break;
					case JsonValueKind.Array:
						break;
					case JsonValueKind.Object:
						if (level < 3)
						{
							ParseClaims(item.Value, claims, level + 1);
						}
						break;
					default:
						claims.Add(new Claim(item.Name, item.Value.ToString()));
						break;
				}
			}
		}
	}
}
