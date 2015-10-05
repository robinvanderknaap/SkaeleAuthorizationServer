 (function(){
	
	// Load environment dependent settings
	angular.module('app').constant('environmentSettings', window.environmentSettings);
	
	angular.module('app').service('configService', ConfigService);

	function ConfigService(environmentSettings){
		
		// Settings which change per environment
		this.environment = environmentSettings;
				
		// Settings which do not change per environemt
		this.exampleLocalSetting = 5;
	}

}());