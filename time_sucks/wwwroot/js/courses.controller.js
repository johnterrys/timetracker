angular.module('time').controller('CoursesCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.search = '';
    $scope.config = {};
    $scope.config.showInactiveCourses = false;

    $scope.load = function () {

        usSpinnerService.spin('spinner');
        $http.get("/Home/GetCourses")
            .then(function (response) {
                usSpinnerService.stop('spinner');
                $scope.courses = response.data;
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Error retrieving courses.");
            });

        $scope.createCourse = function () {
            $http.post("/Home/AddCourse", $scope.user)
                .then(function (response) {
                    $location.path('/course/'+response.data);
                }, function (response) {
                    if (response.status === 401) toastr["error"]("Unauthorized to create a course.");
                    else toastr["error"]("Failed to create course, unknown error.");
                });
        };

        $scope.loaded = true;
    }

    //  Helper method for sorting courses by logged in instructor/admin, then alphabetically.
    $scope.courseSort = function (c1, c2) {
        //  If the user is an instructor or admin, this will place their classes at the top of the list
        if ($scope.$parent.user.type === 'A' || $scope.$parent.user.type === 'I') {
            
            if (c1.value === $scope.$parent.user.firstName + ' ' + $scope.$parent.user.lastName) {
                return -1;
            }
            else if (c2.value === $scope.$parent.user.firstName + ' ' + $scope.$parent.user.lastName) {
                return 1;
            }
        }
        //  This will alphabetically sort the courses by teacher.  I added an ORDER BY to the getCourses method
        //  to make it assist with organizing the courses in alphabetical order.
        return (c1.value.toLowerCase() > c2.value.toLowerCase()) ? 1 : -1;
    } 
    
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