(function () {

    angular.module('app').service('authenticationService', AuthenticationService);

    function AuthenticationService($http, $state, configService, $localStorage) {

        this.signIn = function (username, password) {

            $http.post(configService.authServer.tokenEndPoint, { username: username, password: password })
                    
                // Success
                .then(function (response) {
                    $localStorage.accessToken = response.data.accessToken;
                    $state.go('root.home');
                })
                
                // Error
                .then(function () {
                })
        }
            
        
        this.currentUser = getUserFromToken;

        function getUserFromToken() {
            var token = $localStorage.accessToken;
            var user = {};
            if (typeof token !== 'undefined') {
                    var encoded = token.split('.')[1];
                    user = JSON.parse(urlBase64Decode(encoded));
            }
            return user;
        }

        function urlBase64Decode(str) {
            var output = str.replace('-', '+').replace('_', '/');
            switch (output.length % 4) {
                case 0:
                    break;
                case 2:
                    output += '==';
                    break;
                case 3:
                    output += '=';
                    break;
                default:
                    throw 'Illegal base64url string!';
            }
            return window.atob(output);
        }
    }
} ());