(function(){
	
	angular.module('app').controller('AboutController', AboutController);

	function AboutController($scope, $http){

		var usernames = ['robinvanderknaap','remjer','ericsmal'];
		
		$scope.teamMembers = [];
		
		angular.forEach(usernames, function(username){
			$http.get('https://api.github.com/users/' + username).then(function(response){
				$scope.teamMembers.push(response.data);
			});
		});
	}
}());