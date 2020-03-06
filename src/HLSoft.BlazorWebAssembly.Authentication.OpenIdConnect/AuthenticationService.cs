using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	internal class AuthenticationService : IAuthenticationService
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly IAuthenticationStateProvider _authenticationStateProvider;
		private readonly AuthenticationEventHandler _authenticationEventHandler;
		private readonly NavigationManager _navigationManager;

		public AuthenticationService(IJSRuntime jsRuntime, IAuthenticationStateProvider authenticationStateProvider,
			AuthenticationEventHandler authenticationEventHandler, NavigationManager navigationManager)
		{
			_jsRuntime = jsRuntime;
			_authenticationStateProvider = authenticationStateProvider;
			_authenticationEventHandler = authenticationEventHandler;
			_navigationManager = navigationManager;
		}

		public async Task SignInAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "_returnUrl", _navigationManager.Uri);
				await _jsRuntime.InvokeVoidAsync(Constants.SigninRedirect);
			}
			catch (Exception err)
			{
				_authenticationEventHandler.NotifySignInFail(err);
			}
		}
		
		public async Task SignInPopupAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync(Constants.SigninPopup);
			}
			catch (Exception err)
			{
				_authenticationEventHandler.NotifySignInFail(err);
			}
			_authenticationStateProvider.NotifyAuthenticationStateChanged();
		}

		public async Task SignOutAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync(Constants.SignoutRedirect);
			}
			catch (Exception err)
			{
				_authenticationEventHandler.NotifySignOutFail(err);
			}
		}

		public async Task SignOutPopupAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync(Constants.SignoutPopup);
			}
			catch (Exception err)
			{
				_authenticationEventHandler.NotifySignOutFail(err);
			}
			_authenticationStateProvider.NotifyAuthenticationStateChanged();
		}
	}
}
