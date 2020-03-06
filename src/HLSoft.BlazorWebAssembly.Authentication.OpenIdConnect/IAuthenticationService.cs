using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	public interface IAuthenticationService
	{
		Task LogInAsync();
		Task LogOutAsync();
		Task LogInPopupAsync();
		Task LogOutPopupAsync();
	}
}
