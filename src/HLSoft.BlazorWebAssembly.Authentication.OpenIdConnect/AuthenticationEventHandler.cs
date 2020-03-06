using System;
using System.Linq;
using System.Threading.Tasks;

namespace HLSoft.BlazorWebAssembly.Authentication.OpenIdConnect
{
	public class AuthenticationEventHandler
	{
		public event EventHandler LoginSuccessEvent;
		public event EventHandler<string> LoginFailEvent;
		public event EventHandler LogoutSuccessEvent;
		public event EventHandler<string> LogoutFailEvent;

		public void NotifyLoginFail(Exception err)
		{
			Task.Run(() =>
			{
				ProcessException(err, LoginFailEvent);
			});
		}

		public void NotifyLogoutFail(Exception err)
		{
			Task.Run(() =>
			{
				ProcessException(err, LogoutFailEvent);
			});
		}

		private void ProcessException(Exception err, EventHandler<string> eventHandler)
		{
			var errorMsg = err.Message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
					.FirstOrDefault()
					?.Trim();
			eventHandler?.Invoke(this, errorMsg);
		}
	}
}
