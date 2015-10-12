(function () {

	angular.module('app').controller('UserController', UserController);

	function UserController($scope, authenticationService) {
		$scope.user = authenticationService.currentUser();
	}

} ());