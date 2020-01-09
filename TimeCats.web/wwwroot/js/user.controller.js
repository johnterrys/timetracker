angular.module('time').controller('UserCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.userID = $routeParams.ID;
    $scope.viewUser = {};
    $scope.viewUser.currentPassword = '';
    $scope.viewUser.newPassword = '';
    $scope.viewUser.repeatPassword = '';

    if (!$scope.userID) $location.path('/users');

    $scope.load = function () {
        usSpinnerService.spin('spinner');
        $http.post("/Home/GetUser", { userID: $scope.userID })
            .then(function (response) {
                $scope.viewUser = response.data;
                usSpinnerService.stop('spinner');
                $scope.loaded = true;
                if (response.status === 204) {
                    toastr["error"]("Unauthorized to view user.");
                    window.history.back();
                }
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Failed to get user.");
                $location.path('/dashboard');
            });

        $scope.saveUser = function () {
            $http.post("/Home/ChangeUser", $scope.viewUser)
                .then(function (response) {
                    toastr["success"]("User saved.");
                }, function (response) {
                    if (response.status === 500) {
                        toastr["error"]("Failed to save user, query error.");
                    } else if (response.status === 401) {
                        toastr["error"]("Unauthorized to make changes to this user.");
                    } else if (response.status === 403) {
                        toastr["error"]("Username already exists, choose a new username. No changes saved!");
                    } else if (response.status === 400) {
                        toastr["error"]("Must enter a username. No changes saved!");
                    } else {
                        toastr["error"]("Failed to save user, unknown error.");
                    }
                });
        };

        $scope.changePassword = function () {
            if (!$scope.viewUser.username) {
                toastr['error']("Please enter a Username");
                return;
            } else if ($scope.viewUser.newPassword === '') {
                toastr["error"]("Please enter a Password");
                return;
            } else if ($scope.viewUser.repeatPassword !== $scope.viewUser.newPassword) {
                toastr["error"]("Your passwords don't match.");
                return;
            }

            if ($scope.$parent.user.type !== 'A' && !$scope.viewUser.currentPassword) {
                toastr["error"]("You must enter your current password.");
                return;
            }

            var tempuser = {
                userID: $scope.viewUser.userID
            };

            tempuser.password = CryptoJS.SHA256($scope.viewUser.currentPassword).toString(CryptoJS.enc.Hex);
            tempuser.newPassword = CryptoJS.SHA256($scope.viewUser.newPassword).toString(CryptoJS.enc.Hex);

            $http.post("/Home/ChangePassword", tempuser)
                .then(function (response) {
                    toastr["success"]("Password changed.");
                }, function (response) {
                    if (response.status === 500) {
                        toastr["error"]("Password incorrect.");
                    } else {
                        toastr["error"]("Failed to change password.");
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
            if ($scope.$parent.user.type !== 'A' && $scope.$parent.user.userID !== Number($scope.userID)) {
                toastr["error"]("Not Admin or the specified user.");
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