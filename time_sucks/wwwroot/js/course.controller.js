angular.module('time').controller('CourseCtrl',['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.course = {};
    $scope.course.users = {};
    $scope.course.projects = {};
    $scope.config = {};
    $scope.config.showInactiveProjects = false;

    $scope.load = function() {
        $scope.courseID = $routeParams.ID;

        if (!$scope.courseID) $location.path('/courses');

        usSpinnerService.spin('spinner');
        $http.post("/Home/GetCourse", { courseID: $scope.courseID })
            .then(function (response) {
                usSpinnerService.stop('spinner');
                $scope.course.courseID = response.data.courseID;
                $scope.course.courseName = response.data.courseName;
                $scope.course.description = response.data.description;
                $scope.course.instructorID = response.data.instructorID;
                $scope.course.isActive = response.data.isActive;
                $.each(response.data.users, function (index, user) {
                    $scope.course.users[user.userID] = user;
                });
                $.each(response.data.projects, function (index, project) {
                    $scope.course.projects[project.projectID] = project;
                });
                if (!$scope.course.users) $scope.course.users = null;
                if (!$scope.course.projects) $scope.course.projects = null;
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Failed retrieving course.");
            });

        $scope.createProject = function () {
            usSpinnerService.spin('spinner');
            $http.post("/Home/AddProject", { courseID: $scope.course.courseID })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    $location.path('/project/'+response.data);
                }, function (response) {
                    usSpinnerService.stop('spinner');
                    if (response.status === 401) toastr["error"]("Unauthorized to create a project on this course.");
                    else toastr["error"]("Failed to create project, unknown error.");
                });
        }

        $scope.saveCourse = function () {
            usSpinnerService.spin('spinner');
            $http.post("/Home/SaveCourse",
                {
                    courseID: $scope.course.courseID,
                    courseName: $scope.course.courseName,
                    isActive: $scope.course.isActive,
                    description: $scope.course.description,
                    instructorID: $scope.course.instructorID
                })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    toastr["success"]("Saved course.");
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed saving course.");
                });
        }

        $scope.saveUserInCourse = function (userID, isActive) {
            usSpinnerService.spin('spinner');
            $http.post("/Home/SaveUserInCourse", { userID: userID, courseID: $scope.courseID, isActive: isActive})
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    toastr["success"]("Updated the user in this course.");
                }, function (response) {
                    usSpinnerService.stop('spinner');
                    if (response.status === 500) toastr["error"]("Failed to update the user in this course, query error.");
                    else if (response.status === 401) toastr["error"]("Unathorized to update the user in this course.");
                    else toastr["error"]("Failed to update the user in this course, unknown error.");
                });
        }

        $scope.joinCourse = function () {
            usSpinnerService.spin('spinner');
            $http.post("/Home/JoinCourse", { courseID: $scope.courseID })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    $scope.course.users[$scope.$parent.user.userID] = {
                        userID: $scope.$parent.user.userID,
                        firstName: $scope.$parent.user.firstName,
                        lastName: $scope.$parent.user.lastName,
                        isActive: false
                    };
                    toastr["info"]("You've requested to join the course. The instructor must accept your request before you can join any groups.");
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to join course.");
                });
        }

        $scope.leaveCourse = function () {
            if (confirm('Are you sure you want to leave this course? If you would like to rejoin the course later, you must contact the instructor to be added back into it.')) {
                usSpinnerService.spin('spinner');
                $http.post("/Home/LeaveCourse", { courseID: $scope.courseID })
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        $scope.course.users[$scope.$parent.user.userID].isActive = false;
                        toastr["info"]("You've left the course. Contact the Instructor if you want to be reactivated in the course.");
                    }, function (response) {
                        usSpinnerService.stop('spinner');
                        if (response.status === 500) toastr["error"]("Failed to leave the course, query error.");
                        else if (response.status === 401) toastr["error"]("Failed to leave the course, user not in the course.");
                        else toastr["error"]("Failed to leave the course, unknown error.");
                    });
            } else {
                // Do nothing!
            }
        }

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
        }

        $scope.userInCourse = function () {
            //Checks that the current user is listed in the current course.
            //Should be used to hide the Join button if it's true
            var inCourse = false;
            if (!$scope.course) return false;
            $.each($scope.course.users, function (index, user) {
                if (user.userID === $scope.$parent.user.userID ) {
                    inCourse = true;
                }
            });
            return inCourse;
        }

        $scope.userActiveInCourse = function () {
            //Checks that the current user is listed in the current course.
            //Should be used to hide the Join button if it's true
            var inCourse = false;
            if (!$scope.course) return false;
            $.each($scope.course.users, function (index, user) {
                if (user.userID === $scope.$parent.user.userID) {
                    if (user.isActive) inCourse = true;
                }
            });
            return inCourse;
        }

        $scope.hasProjects = function () {
            if ($scope.course) return !angular.equals([], $scope.course.projects);
            return false;
        };

        $scope.hasUsers = function () {
            if ($scope.course) return !angular.equals([], $scope.course.users);
            return false;
        };

        $scope.assignEval = function () {
            if ($scope.$parent.user.type === 'A' || $scope.$parent.user.userID === $scope.course.instructorID) $location.path('/assignEval/' + $scope.courseID);
        };

        $scope.loaded = true;
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