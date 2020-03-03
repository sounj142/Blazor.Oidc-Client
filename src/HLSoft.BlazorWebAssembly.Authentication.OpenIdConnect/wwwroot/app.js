(function () {
	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect = {};
	var mgr = null;

	function setConfigOidc(config) {
		sessionStorage.setItem('_configOidc', JSON.stringify(config));
	}

	function getConfigOidc() {
		var str = sessionStorage.getItem('_configOidc');
		return str ? JSON.parse(str) : null;
	}

	function getParameterByName(name, url) {
		if (!url) url = window.location.href;
		name = name.replace(/[\[\]]/g, '\\$&');
		var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
			results = regex.exec(url);
		if (!results) return null;
		if (!results[2]) return '';
		return decodeURIComponent(results[2].replace(/\+/g, ' '));
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.configOidc = function (config, overrideOldConfig) {
		if (!mgr || overrideOldConfig) {
			if (config && config.client_id) {
				setConfigOidc(config);
			}
			else {
				config = getConfigOidc();
			}
			mgr = new Oidc.UserManager(config);
		}
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.signinRedirect = function () {
		sessionStorage.setItem('_returnUrl', window.location.href);
		return mgr.signinRedirect();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.signoutRedirect = function () {
		return mgr.signoutRedirect();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.getUser = function () {
		return mgr.getUser();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.signinPopup = function () {
		return mgr.signinPopup().then(() => {
			setTimeout(() => {
				window.location.reload();
			});
		}, error => {
			console.error(error);
		});
	}

	// renew Token
	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.signinSilent = function () {
		return mgr.signinSilent();
	}

	function createUserManager() {
		return getParameterByName('session_state') && window.location.href.indexOf('?') > 0
			? new Oidc.UserManager({ loadUserInfo: true, response_mode: "query" })
			: new Oidc.UserManager();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.processSigninCallback = function () {
		var mgr = createUserManager();
		var returnUrl = sessionStorage.getItem('_returnUrl') || '/';
		sessionStorage.removeItem('_returnUrl');

		mgr.signinRedirectCallback().then(() => {
			window.history.replaceState({}, window.document.title, window.location.origin + window.location.pathname);
			window.location = returnUrl;
		}, error => {
			console.error(error);
			window.location = returnUrl;
		});
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.processSilentCallback = function () {
		var mgr = createUserManager();
		
		mgr.signinSilentCallback('/').catch(error => {
			console.error(error);
		});
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.processSigninPopup = function () {
		var mgr = createUserManager();
		mgr.signinPopupCallback();
	}
})();