using HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	public class BlazorAuthenticationStateProvider<TUser> : AuthenticationStateProvider, IAuthenticationStateProvider where TUser: class
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly ClientOptions _clientOptions;
		private readonly NavigationManager _navigationManager;
		private readonly IClaimsParser<TUser> _claimsParser;

		public BlazorAuthenticationStateProvider(IJSRuntime jsRuntime, NavigationManager myNavigationManager, 
			ClientOptions clientOptions, IClaimsParser<TUser> claimsParser)
		{
			_jsRuntime = jsRuntime;
			_navigationManager = myNavigationManager;
			_clientOptions = clientOptions;
			_claimsParser = claimsParser;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			if (await HandleKnownUri())
			{
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}

			await Utils.ConfigOidcAsync(_jsRuntime, _clientOptions);

			var user = await _jsRuntime.InvokeAsync<TUser>(Constants.GetUser);
			Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(user));

			var claimsIdentity = _claimsParser.CreateIdentity(user);

			return new AuthenticationState(new ClaimsPrincipal(claimsIdentity));
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
			if (_navigationManager.Uri.StartsWith(_clientOptions.popup_post_logout_redirect_uri, StringComparison.OrdinalIgnoreCase))
			{
				await _jsRuntime.InvokeVoidAsync(Constants.ProcessSignoutPopup);
				return true;
			}
			return false;
		}

		public void NotifyAuthenticationStateChanged()
		{
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}
	}
}
