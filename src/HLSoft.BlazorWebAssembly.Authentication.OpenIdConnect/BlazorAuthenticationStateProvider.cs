﻿using HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect.Models;
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
		private readonly AuthenticationEventHandler _authenticationEventHandler;


		public BlazorAuthenticationStateProvider(IJSRuntime jsRuntime, NavigationManager navigationManager, 
			ClientOptions clientOptions, IClaimsParser<TUser> claimsParser, AuthenticationEventHandler authenticationEventHandler)
		{
			_jsRuntime = jsRuntime;
			_navigationManager = navigationManager;
			_clientOptions = clientOptions;
			_claimsParser = claimsParser;
			_authenticationEventHandler = authenticationEventHandler;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			await ProcessPreviousActionCode();
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

		private async Task ProcessPreviousActionCode()
		{
			var previousActionCode = await Utils.GetAndRemoveSessionStorageData(_jsRuntime, "_previousActionCode");
			switch (previousActionCode)
			{
				case Constants.SignedInSuccess:
					_authenticationEventHandler.NotifySignInSuccess();
					break;
				case Constants.SignedOutSuccess:
					_authenticationEventHandler.NotifySignOutSuccess();
					break;
			}
		}

		private async Task<bool> HandleSigninCallbackUri()
		{
			if (CurrentUriIs(_clientOptions.redirect_uri))
			{
				string returnUrl = null;
				try
				{
					returnUrl = await Utils.GetAndRemoveSessionStorageData(_jsRuntime, "_returnUrl");
					await _jsRuntime.InvokeVoidAsync(Constants.ProcessSigninCallback);
				}
				catch (Exception err)
				{
					_authenticationEventHandler.NotifySignInFail(err);
				}

				await Utils.SetSessionStorageData(_jsRuntime, "_previousActionCode", Constants.SignedInSuccess);
				_navigationManager.NavigateTo(returnUrl ?? _clientOptions.post_logout_redirect_uri, true);

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
					_authenticationEventHandler.NotifySilentRefreshTokenSuccess();
				}
				catch (Exception err)
				{
					_authenticationEventHandler.NotifySilentRefreshTokenFail(err);
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
				catch (Exception err)
				{
					_authenticationEventHandler.NotifySignInFail(err);
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
				catch (Exception err)
				{
					_authenticationEventHandler.NotifySignOutFail(err);
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
