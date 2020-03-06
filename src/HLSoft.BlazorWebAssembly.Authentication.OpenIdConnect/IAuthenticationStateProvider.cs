using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	public interface IAuthenticationStateProvider
	{
		event AuthenticationStateChangedHandler AuthenticationStateChanged;
		Task<AuthenticationState> GetAuthenticationStateAsync();
		void NotifyAuthenticationStateChanged();
	}
}
