angular.module('time').controller('DashboardCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.load = function () {
        $scope.groups = {};
        $scope.courses = {};
        $scope.courseToInactiveUsers = {};
        $scope.loadInactivePanel = false;
        $scope.responsesComplete = 0;

        $http.get("/Home/GetDashboard")
            .then(function (response) {
                $scope.groups = response.data;
            }, function () {
                toastr["error"]("Error retrieving dashboard groups.");
            });

        if ($scope.$parent.user.type === "I" || $scope.$parent.user.type === "A") {
            $http.post("/Home/GetInstructorCourses")
                .then(function (response) {
                    $scope.courses = response.data;
                    if ($scope.courses.length <= 0) {
                        $scope.loadInactivePanel = false;
                        $scope.loaded = true;
                        usSpinnerService.stop('spinner');
                    }
                    else {
                        for (i = 0; i < $scope.courses.length; i++) {
                            $http.post("/Home/GetInactiveStudentsInCourse", $scope.courses[i])
                                .then(function (response_inner) {
                                    var indexCourseID = response_inner.config.data.courseID;
                                    $scope.courseToInactiveUsers[indexCourseID] = response_inner.data;
                                    if (Object.keys($scope.courseToInactiveUsers).length > 0) {
                                        $scope.loadInactivePanel = true;
                                    }
                                    $scope.responsesComplete += 1;
                                    if ($scope.responsesComplete == $scope.courses.length) {
                                        $scope.loaded = true;
                                        usSpinnerService.stop('spinner');
                                    }
                                }, function () {
                                    var indexCourseID = response_inner.config.data.courseID;
                                    $scope.responsesComplete += 1;
                                    if ($scope.responsesComplete == $scope.courses.length) {
                                        $scope.loaded = true;
                                        usSpinnerService.stop('spinner');
                                    }
                                    toastr["error"]("Error retrieving inactive students for course " + $scope.courses[indexCourseID].courseName);
                                });
                        }

                    }
                }, function () {
                    toastr["error"]("Error retrieving Dashboard courses");
                });
        }

        $scope.saveUserInCourse = function (courseID, userID, isActive) {
                usSpinnerService.spin('spinner');
                $http.post("/Home/SaveUserInCourse", { userID: userID, courseID: courseID, isActive: isActive })
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        toastr["success"]("Updated the user in this course.");
                    }, function (response) {
                        usSpinnerService.stop('spinner');
                        if (response.status === 500) toastr["error"]("Failed to update the user in this course, query error.");
                        else if (response.status === 401) toastr["error"]("Unathorized to update the user in this course.");
                        else toastr["error"]("Failed to update the user in this course, unknown error.");
                    });
        };
        $scope.deleteUserFromCourse = function (userID) {
            if (confirm('Are you sure you want to delete this user from the course?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Home/DeleteUserFromCourse", { userID: userID, courseID: $scope.courseID })
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        delete $scope.course.users[userID];
                        toastr["info"]("Deleted the user from the course.");
                    }, function (response) {
                        usSpinnerService.stop('spinner');
                        if (response.status === 500) toastr["error"]("Failed to delete user from course, query error.");
                        else if (response.status === 401) toastr["error"]("Unauthorized to delete this user from the course.");
                        else toastr["error"]("Failed to delete user from course, unknown error.");
                    });
            } else {
                // Do nothing!
            }
        };


        if ($scope.$parent.user.type === "A" || $scope.$parent.user.type === "S") {
            $scope.loaded = true;
            usSpinnerService.stop('spinner');
        }
        //$scope.loaded = true;
    };

    //Standard login check, if there is a user, load the page, if not, redirect to login
    usSpinnerService.spin('spinner');
    $http.get("/Home/CheckSession")
        .then(function (response) {
//            usSpinnerService.stop('spinner');
            $scope.$parent.user = response.data;
            $scope.$parent.loaded = true;
            $scope.load();
        }, function () {
            usSpinnerService.stop('spinner');
            toastr["error"]("Not logged in.");
            $location.path('/login');
        });
}]);