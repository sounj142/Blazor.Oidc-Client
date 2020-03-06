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
			Console.WriteLine("===========================BlazorAuthenticationStateProvider duoc tao ==========");
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
			//Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(user));

			var claimsIdentity = _claimsParser.CreateIdentity(user);

			return new AuthenticationState(new ClaimsPrincipal(claimsIdentity));
		}

		public void NotifyAuthenticationStateChanged()
		{
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		private async Task<bool> HandleKnownUri()
		{
			if (await HandleSigninCallbackUri()) return true;
			if (await HandleSilentCallbackUri()) return true;
			if (await HandleSigninPopupUri()) return true;
			if (await HandleSignoutPopupUri()) return true;

			return false;
		}

		private async Task<bool> HandleSigninCallbackUri()
		{
			if (CurrentUriIs(_clientOptions.redirect_uri))
			{
				string returnUrl = _clientOptions.post_logout_redirect_uri;
				try
				{
					returnUrl = await _jsRuntime.InvokeAsync<string>(Constants.ProcessSigninCallback);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine(e);
				}

				_navigationManager.NavigateTo(returnUrl, true);
				return true;
			}
			return false;
		}

		private async Task<bool> HandleSilentCallbackUri()
		{
			if (CurrentUriIs(_clientOptions.silent_redirect_uri))
			{
				try
				{
					await _jsRuntime.InvokeVoidAsync(Constants.ProcessSilentCallback);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine(e);
				}
				return true;
			}
			return false;
		}

		private async Task<bool> HandleSigninPopupUri()
		{
			if (CurrentUriIs(_clientOptions.popup_redirect_uri))
			{
				try
				{
					await _jsRuntime.InvokeVoidAsync(Constants.ProcessSigninPopup);
					await _jsRuntime.InvokeVoidAsync("window.close");
				}
				catch (Exception e)
				{
					Console.Error.WriteLine(e);
				}
				return true;
			}
			return false;
		}

		private async Task<bool> HandleSignoutPopupUri()
		{
			if (CurrentUriIs(_clientOptions.popup_post_logout_redirect_uri))
			{
				try
				{
					await _jsRuntime.InvokeVoidAsync(Constants.ProcessSignoutPopup);
					await _jsRuntime.InvokeVoidAsync("window.close");
				}
				catch (Exception e)
				{
					Console.Error.WriteLine(e);
				}
				return true;
			}
			return false;
		}

		private bool CurrentUriIs(string url)
		{
			return !string.IsNullOrEmpty(url) && _navigationManager.Uri.StartsWith(url, StringComparison.OrdinalIgnoreCase);
		}
	}
}
