(function() {

	angular.module('app').controller('forgotpasswordController', function ($scope, $http) {

		$scope.forgotpassword = function(emailaddress) {
		
		//=== test functionality for forgot password proces ===
				$scope.message = "Het nieuwe wachtwoord is naar u opgestuurd. Volg de stappen in de email om opnieuw te kunnen inloggen."
		//=====================================================		
		};

	});

}());