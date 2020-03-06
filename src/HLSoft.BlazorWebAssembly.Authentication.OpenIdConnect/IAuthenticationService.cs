using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	public interface IAuthenticationService
	{
		Task SignInAsync();
		Task SignOutAsync();
		Task SignInPopupAsync();
		Task SignOutPopupAsync();
	}
}
