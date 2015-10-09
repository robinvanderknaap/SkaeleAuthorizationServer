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

				.state('root.authentication.logout', {
				    url: "/logout",
				    views: {
				        "main@": {
				            templateUrl: "modules/authentication/logout.html",
				            controller: "logoutController"
				        }
				    }
				})

				.state('root.authentication.forgot-password', {
				    url: "/wachtwoord-vergeten",
				    views: {
				        "main@": {
				            templateUrl: "modules/authentication/forgotpassword.html",
				            controller: "forgotpasswordController"
				        }
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