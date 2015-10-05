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
		    .state('root.about', {
		      url: "/about",
			  views:{
					'main@':{
						templateUrl: "modules/about/about.html",
			  			controller: "AboutController"
					}
				}
		    })
	}
}());