 (function(){
	
	angular.module('app').service('configService', ConfigService);

	function ConfigService(){
		
		this.authServer = {
			tokenEndPoint: 'http://localhost:50929/authenticate',
			refreshTokenEndPoint: 'http://localhost:50929/authenticate/refresh-token'
		}
	}
}());