(function() {

	angular.module('app').controller('loginController', function ($scope, $http, $state, authenticationService) {

		$scope.login = function(email, password, rememberme) {
			
			authenticationService.signIn(email, password);
			
			//	console.log(rememberme)
			
			
			//=== test functionality for loginproces ===
				// if (emailaddress == "consument@courta.nl" && password == "Welkom01") {
				// 	$state.go("root.consumer.advertisements");
				// } else if (emailaddress == "makelaar@courta.nl" && password == "Welkom01") {
				// 	$state.go("root.broker.marketplace");
				// }
				// else {
				// 	$scope.message = "Het emailadres en wachtwoord komen niet overeen. Probeer het nogmaals s.v.p."
				// }
			//==========================================
		};
	});

}());