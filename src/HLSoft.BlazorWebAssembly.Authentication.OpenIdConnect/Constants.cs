namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	public static class Constants
	{
		private const string JavascriptPrefix = "HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.";
		public const string SigninRedirect = JavascriptPrefix + "signinRedirect";
		public const string SignoutRedirect = JavascriptPrefix + "signoutRedirect";
		public const string GetUser = JavascriptPrefix + "getUser";
		public const string ConfigOidc = JavascriptPrefix + "configOidc";
		public const string SigninSilent = JavascriptPrefix + "signinSilent";
		public const string ProcessSilentCallback = JavascriptPrefix + "processSilentCallback";
		public const string ProcessSigninCallback = JavascriptPrefix + "processSigninCallback";
		public const string ProcessSigninPopup = JavascriptPrefix + "processSigninPopup";
		public const string ProcessSignoutPopup = JavascriptPrefix + "processSignoutPopup";
		public const string SigninPopup = JavascriptPrefix + "signinPopup";
		public const string SignoutPopup = JavascriptPrefix + "signoutPopup";
	}
}
