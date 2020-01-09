angular.module('time').controller('AssignEvalCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.config = {};
    $scope.config.selectAll = false;
    $scope.course = {};
    $scope.course.projects = {};
    $scope.course.evaluations = {};

    $scope.load = function () {
        $scope.courseID = $routeParams.ID;

        if (!$scope.courseID) window.history.back();
        $scope.course.courseID = $scope.courseID;

        usSpinnerService.spin('spinner');
        $http.post("/Home/GetProjects", { courseID: $scope.courseID })
            .then(function (response) {
                usSpinnerService.stop('spinner');
                $.each(response.data, function (index, project) {
                    $scope.course.projects[project.projectID] = project;
                });
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Failed to retrieve course. Using Dummy Data.");

                //Dummy Data
                $scope.course = {
                    courseID: $scope.courseID,
                    courseName: 'Test Course'
                };
                $scope.course.projects = {
                    0: {
                        projectID: 1,
                        projectName: 'Test Project 1',
                            isActive: true,
                            isSelected: false
                    },
                    1: {
                        projectID: 2,
                        projectName: 'Test Project 2',
                        isActive: true,
                        isSelected: false
                    },
                    2: {
                        projectID: 3,
                        projectName: 'TestProject 3',
                        isActive: true,
                        isSelected: false
                    }
                };
                $scope.course.evaluations = {
                    0: {
                        evalTemplateID: 1,
                        templateName: "Test Evalation Template 1",
                        isSelected: false
                    },
                    1: {
                        evalTemplateID: 2,
                        templateName: "Test Evalation Template 2",
                        isSelected: false
                    },
                    2: {
                        evalTemplateID: 3,
                        templateName: "Test Evalation Template 3",
                        isSelected: false
                    }
                }
            });

        usSpinnerService.spin('spinner');
        $http.post("/Home/GetTemplates", { courseID: $scope.courseID })
            .then(function (response) {
                usSpinnerService.stop('spinner');
                $.each(response.data, function (index, eval) {
                    $scope.course.evaluations[eval.evalTemplateID] = eval;
                });
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Failed to retrieve evaluations.");
            });

        $scope.hasProjects = function () {
            if ($scope.course.projects) return !angular.equals([], $scope.course.projects);
            return false;
        };

        $scope.toggleSelectAll = function () {
            if ($scope.config.selectAll) {
                for (i = 0; i < Object.keys($scope.course.projects).length; i++) {
                    $scope.course.projects[i].isSelected = true;
                }
            } else {
                for (i = 0; i < Object.keys($scope.course.projects).length; i++) {
                    $scope.course.projects[i].isSelected = false;
                }
            }
        };

        $scope.uncheckSelectAll = function (isSelected) {
            if (!isSelected) $scope.config.selectAll = false;
        };

        $scope.assignEvaluation = function () {
            projectIDs = [];
            for (projectID in $scope.course.projects) {
                if ($scope.course.projects[projectID].isSelected) projectIDs.push($scope.course.projects[projectID].projectID);
            }

            evalTemplateID = 0;
            for (templateID in $scope.course.evaluations) {
                if ($scope.course.evaluations[templateID].isSelected) evalTemplateID = $scope.course.evaluations[templateID].evalTemplateID;
            }

            if (projectIDs.length < 1) {
                toastr["error"]("You must select at least one project to assign the evaluation to.");
                return;
            }
            if (evalTemplateID === 0) {
                toastr["error"]("You must select an evaluation to assign to the selected projects.");
                return;
            }

            if (confirm('Are you sure you want to assign this evaluation?')) {
                var projectIDs = [];
                var evalTemplateID = 0;
                for (projectID in $scope.course.projects) {
                    if ($scope.course.projects[projectID].isSelected) projectIDs.push(projectID);
                }
                for (templateID in $scope.course.evaluations) {
                    if ($scope.course.evaluations[templateID].isSelected) {
                        evalTemplateID = templateID;
                        break;
                    }
                }
                usSpinnerService.spin('spinner');
                $http.post("/Home/AssignEvals", { projectIDs: projectIDs, evalTemplateID: evalTemplateID })
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        toastr["success"]("Evaluation assigned.");
                        $location.path('/course/' + $scope.courseID);
                    }, function () {
                        usSpinnerService.stop('spinner');
                        toastr["error"]("Failed to assign the evaluation.");
                    });
            } else {
                // Do nothing!
            }
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