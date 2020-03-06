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

		public AuthenticationService(IJSRuntime jsRuntime, IAuthenticationStateProvider authenticationStateProvider,
			AuthenticationEventHandler authenticationEventHandler)
		{
			_jsRuntime = jsRuntime;
			_authenticationStateProvider = authenticationStateProvider;
			_authenticationEventHandler = authenticationEventHandler;
		}

		public async Task LogInAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync(Constants.SigninRedirect);
			}
			catch (Exception err)
			{
				_authenticationEventHandler.NotifyLoginFail(err);
			}
		}
		
		public async Task LogInPopupAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync(Constants.SigninPopup);
			}
			catch (Exception err)
			{
				_authenticationEventHandler.NotifyLoginFail(err);
			}
			_authenticationStateProvider.NotifyAuthenticationStateChanged();
		}

		public async Task LogOutAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync(Constants.SignoutRedirect);
			}
			catch (Exception err)
			{
				_authenticationEventHandler.NotifyLogoutFail(err);
			}
		}

		public async Task LogOutPopupAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync(Constants.SignoutPopup);
			}
			catch (Exception err)
			{
				_authenticationEventHandler.NotifyLogoutFail(err);
			}
			_authenticationStateProvider.NotifyAuthenticationStateChanged();
		}
	}
}
