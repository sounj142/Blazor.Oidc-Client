using Microsoft.JSInterop;
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
			await _jsRuntime.InvokeVoidAsync(Constants.SigninRedirect);
		}
		
		public async Task LogInPopupAsync()
		{
			await _jsRuntime.InvokeVoidAsync(Constants.SigninPopup);
			_authenticationStateProvider.NotifyAuthenticationStateChanged();
		}

		public async Task LogOutAsync()
		{
			await _jsRuntime.InvokeVoidAsync(Constants.SignoutRedirect);
		}

		public async Task LogOutPopupAsync()
		{
			await _jsRuntime.InvokeVoidAsync(Constants.SignoutPopup);
			_authenticationStateProvider.NotifyAuthenticationStateChanged();
		}
	}
}
