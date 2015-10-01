(function(){
	
	angular.module('app').controller('HomeController', HomeController);

	function HomeController($scope, configService){
		
		$scope.environmentSettingsFound = configService.environment != null;
	}
	
}());