(function() {

	angular.module('app').controller('logoutController', function ($scope, $http, $state, $timeout) {
		
		$scope.message = "U bent succesvol uitgelogd! U wordt over 5 seconden doorgestuurd naar onze website."
		
		$timeout(function (){
			$state.go("root.authentication.login");
		}, 5000);
		
	});
	
}());