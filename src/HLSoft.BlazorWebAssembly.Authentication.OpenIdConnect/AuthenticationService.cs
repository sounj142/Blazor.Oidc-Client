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

		public AuthenticationService(IJSRuntime jsRuntime, IAuthenticationStateProvider authenticationStateProvider)
		{
			_jsRuntime = jsRuntime;
			_authenticationStateProvider = authenticationStateProvider;
		}

		public async Task LogInAsync()
		{
			try
			{
				await _jsRuntime.InvokeVoidAsync(Constants.SigninRedirect);
			}
			catch (Exception err)
			{
				WriteErrorToConsole(err);
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
				WriteErrorToConsole(err);
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
				WriteErrorToConsole(err);
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
				WriteErrorToConsole(err);
			}
			_authenticationStateProvider.NotifyAuthenticationStateChanged();
		}

		private void WriteErrorToConsole(Exception err)
		{
			var errorMsg = err.Message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
					.FirstOrDefault()
					?.Trim();
			Console.WriteLine("Error: {0}", errorMsg);
		}
	}
}
