(function () {
	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect = {};
	let mgr = null;

	function setConfigOidc(config) {
		sessionStorage.setItem('_configOidc', JSON.stringify(config));
	}

	function getConfigOidc() {
		let str = sessionStorage.getItem('_configOidc');
		return str ? JSON.parse(str) : null;
	}

	function getParameterByName(name, url) {
		if (!url) url = window.location.href;
		name = name.replace(/[\[\]]/g, '\\$&');
		let regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
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
		//let result = mgr.getUser();
		//result.then(user => console.log(user));
		//return result;
		return mgr.getUser();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.signinPopup = function () {
		return mgr.signinPopup();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.signoutPopup = function () {
		return mgr.signoutPopup();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.signinSilent = function () {
		return mgr.signinSilent();
	}

	function createUserManager() {
		return getParameterByName('session_state') && window.location.href.indexOf('?') > 0
			? new Oidc.UserManager({ loadUserInfo: true, response_mode: "query" })
			: new Oidc.UserManager();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.processSigninCallback = function () {
		let mgr = createUserManager();
		let returnUrl = sessionStorage.getItem('_returnUrl');
		sessionStorage.removeItem('_returnUrl');
		return mgr.signinRedirectCallback().then(() => returnUrl);
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.processSilentCallback = function () {
		let mgr = createUserManager();
		return mgr.signinSilentCallback('/');
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.processSigninPopup = function () {
		let mgr = createUserManager();
		return mgr.signinPopupCallback();
	}

	window.HLSoftBlazorWebAssemblyAuthenticationOpenIdConnect.processSignoutPopup = function () {
		let mgr = createUserManager();
		mgr.signoutPopupCallback(false);
	}
})();