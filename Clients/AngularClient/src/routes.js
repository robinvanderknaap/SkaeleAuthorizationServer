(function(){

	var app = angular.module('app');

	app.config(['$stateProvider', '$urlRouterProvider', ConfigureRoutes]);

	function ConfigureRoutes($stateProvider, $urlRouterProvider){

		// For any unmatched url, redirect to home
  		$urlRouterProvider.otherwise("/");

  		$stateProvider
		  	.state('root', {
				views:{
					'header@':{
						templateUrl: "modules/shared/header/header.html"
					},
				}
		  	})
				.state('root.home', {
					url: "/",
					views:{
						'main@':{
							templateUrl: "modules/home/home.html",
							controller: "HomeController"
						}
					}
				})
				.state('root.user', {
					url: "/",
					views:{
						'main@':{
							templateUrl: "modules/user/user.html",
							controller: "UserController"
						}
					}
				})
				.state('root.authentication', {
					abstract: true,
					url: "/authentication",
					views: {
						'header@': {
							templateUrl: "",
							controller: ""
						}
					}
				})
	
					.state('root.authentication.login', {
						url: "/login",
						views: {
							"main@": {
								templateUrl: "modules/authentication/login.html",
								controller: "loginController"
							}
						}
					})
	}
}());