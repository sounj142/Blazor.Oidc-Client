using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	internal class AuthenticationService : IAuthenticationService
	{
		private readonly IJSRuntime _jsRuntime;

		public AuthenticationService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task LogInAsync()
		{
			await _jsRuntime.InvokeVoidAsync(Constants.SigninRedirect);
		}
		
		public async Task LogInPopupAsync()
		{
			await _jsRuntime.InvokeVoidAsync(Constants.SigninPopup);
		}

		public async Task LogOutAsync()
		{
			await _jsRuntime.InvokeVoidAsync(Constants.SignoutRedirect);
		}

		public async Task LogOutPopupAsync()
		{
			await _jsRuntime.InvokeVoidAsync(Constants.SignoutPopup);
		}
	}
}
