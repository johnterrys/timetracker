angular.module('time').controller('UsersCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.search = '';
    $scope.loaded = false;
    $scope.load = function() {
        usSpinnerService.spin('spinner');
        $http.get("/Home/GetUsers")
            .then(function (response) {
                $scope.users = {};
                //Setting users to be in the index of their userID
                $.each(response.data, function (index, user) {
                    $scope.users[user.userID] = user;
                });
                usSpinnerService.stop('spinner');
                $scope.loaded = true;
                if (response.status === 204) {
                    toastr["error"]("Not an Admin.");
                    window.history.back();
                }
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Failed to get users.");
                $location.path('/dashboard');
            });

        $scope.saveUser = function (user) {
            $http.post("/Home/ChangeUser", user)
                .then(function (response) {
                    toastr["success"]("User saved.");
                }, function (response) {
                    if (response.status === 500) {
                        toastr["error"]("Failed to save user, query error.");
                    } else if (response.status === 401) {
                        toastr["error"]("Unauthorized to make changes to this user.");
                    } else {
                        toastr["error"]("Failed to save user, unknown error.");
                    }
                });
        }
    };

    //Standard login check, if there is a user, load the page, if not, redirect to login
    usSpinnerService.spin('spinner');
    $http.get("/Home/CheckSession")
        .then(function (response) {
            usSpinnerService.stop('spinner');
            $scope.$parent.user = response.data;
            if ($scope.$parent.user.type !== 'A') {
                toastr["error"]("Not Admin.");
                $location.path('/dashboard');
            }
            $scope.$parent.loaded = true;
            $scope.load();
        }, function () {
            usSpinnerService.stop('spinner');
            toastr["error"]("Not logged in.");
            $location.path('/login');
        });
}]);