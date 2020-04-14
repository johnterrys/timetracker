angular.module('time').controller('CoursesCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.search = '';
    $scope.config = {};
    $scope.config.showInactiveCourses = false;

    $scope.load = function () {

        usSpinnerService.spin('spinner');
        $http.get("/Course/GetCourses")
            .then(function (response) {
                usSpinnerService.stop('spinner');
                $scope.courses = response.data;
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Error retrieving courses.");
            });

        $scope.createCourse = function () {
            $http.post("/Course/AddCourse", $scope.user)
                .then(function (response) {
                    $location.path('/course/' + response.data);
                }, function (response) {
                    if (response.status === 401) toastr["error"]("Unauthorized to create a course.");
                    else toastr["error"]("Failed to create course, unknown error.");
                });
        };

        $scope.loaded = true;
    };

    //Standard login check, if there is a user, load the page, if not, redirect to login
    usSpinnerService.spin('spinner');
    $http.get("/Home/CheckSession")
        .then(function (response) {
            usSpinnerService.stop('spinner');
            $scope.$parent.user = response.data;
            $scope.$parent.loaded = true;
            $scope.load();
        }, function () {
            usSpinnerService.stop('spinner');
            toastr["error"]("Not logged in.");
            $location.path('/login');
        });
}]);
