angular.module('time').controller('LoginCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.user = {};
    $scope.password = '';
    var hashedPass = '';

    $scope.load = function () {
        $scope.login = function () {
            if ($scope.user.username === '') {
                toastr['error']("Please enter a Username");
                return;
            } else if ($scope.password === '') {
                toastr["error"]("Please enter a Password");
                return;
            }

            $scope.user.password = CryptoJS.SHA256($scope.password).toString(CryptoJS.enc.Hex);

            usSpinnerService.spin('spinner');
            $http.post("/Home/LoginUser", $scope.user)
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    if (response.status === 204) {
                        toastr["error"]("Username does not exist.");
                    } else {
                        $location.path('/dashboard'); //Changes to the dashboard URL for normal user
                    }
                }, function (response) {
                    usSpinnerService.stop('spinner');
                    if (response.status === 401) {
                        toastr["error"]("Password incorrect.");
                    } else if (response.status === 403) {
                        toastr["error"]("User account has been deactivated.");
                    }
                });
        };

        $scope.register = function () {
            $location.path('/register'); //Changes to the register URL
        };
        $scope.loaded = true;
        $("#username").focus(); //Focus on the username field for quicker login
    };
   
    usSpinnerService.spin('spinner');
    $http.get("/Home/CheckSession")
        .then(function (response) {
            usSpinnerService.stop('spinner');
            if (response.data === '') {
                $scope.load();
                return;
            }
            $scope.$parent.user = response.data;
            $scope.$parent.loaded = true;
            if ($scope.$parent.user.type === 'A' || $scope.$parent.user.type === 'I') $location.path('/courses');
            else $location.path('/dashboard');
        }, function () {
            usSpinnerService.stop('spinner');
            $scope.$parent.loaded = true;
            $scope.load();
        });
}]);