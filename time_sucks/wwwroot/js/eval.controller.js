angular.module('time').controller('EvalCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.config = {};
    $scope.currentEval = 0;
    $scope.group = {};
    $scope.users = {};
    $scope.evaluation = {};
    $scope.evaluation.templateQuestions = {};
    $scope.evaluation.categories = {};
    $scope.evaluation.responses = [];

    $scope.responsecounter = 1;

    $scope.load = function () {
        $scope.groupID = $routeParams.ID;
        if (!$scope.groupID) window.history.back();
        usSpinnerService.spin('spinner');
        $http.post("/Home/GetLatestIncompleteEvaluation", { groupID: $scope.groupID })
            .then(function (response) {
                usSpinnerService.stop('spinner');

                if (response.status === 204) {
                    toastr["error"]("There isn't currently an evaluation for you to complete.");
                    window.history.back();
                }

                $scope.evaluation.evalID = response.data.evalID;
                $scope.evaluation.templateName = response.data.templateName;
                $scope.evaluation.number = response.data.number;
                $.each(response.data.categories, function (index, category) {
                    $scope.evaluation.categories[category.evalTemplateQuestionCategoryID] = category;
                });
                $.each(response.data.templateQuestions, function (index, templateQuestion) {
                    $scope.evaluation.templateQuestions[templateQuestion.evalTemplateQuestionID] = templateQuestion;
                });
                $.each(response.data.users, function (index, user) {
                    $scope.users[user.userID] = user;
                });
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Failed to retrieve evaluation. Using Dummy Data.");

                //Dummy Data
                $scope.group = {
                    groupID: $scope.groupID,
                    groupName: 'Test Group'
                }
                $scope.evaluation = {
                    evalID: 1, //Should we randomize this number on the back end?
                    templateName: "Test Evalation",
                    number: 1,
                    categories: {
                        1: {
                            evalTemplateQuestionCategoryID: 1,
                            categoryName: "Category 1"
                        },
                        2: {
                            evalTemplateQuestionCategoryID: 2,
                            categoryName: "Category 2"
                        }
                    },
                    templateQuestions: {
                        1: {
                            evalTemplateQuestionID: 1,
                            evalTemplateQuestionCategoryID: 1,
                            questionType: 'N',
                            questionText: "Test question 1, what do you think?",
                            number: 2
                        },
                        2: {
                            evalTemplateQuestionID: 2,
                            evalTemplateQuestionCategoryID: 1,
                            questionType: 'R',
                            questionText: "Test question 2 (should be first), what do you think?",
                            number: 1
                        },
                        3: {
                            evalTemplateQuestionID: 3,
                            evalTemplateQuestionCategoryID: 2,
                            questionType: 'N',
                            questionText: "Test question 3, what do you think?",
                            number: 1
                        },
                        4: {
                            evalTemplateQuestionID: 4,
                            evalTemplateQuestionCategoryID: 2,
                            questionType: 'R',
                            questionText: "Test question 4, what do you think?",
                            number: 2
                        },
                        5: {
                            evalTemplateQuestionID: 4,
                            evalTemplateQuestionCategoryID: 0,
                            questionType: 'N',
                            questionText: "Test question 5, what do you think?",
                            number: 1
                        }
                    }
                }

                //create empty responses
                tempID = 1;
                $scope.evaluation.responses = {};
                for (questionID in $scope.evaluation.templateQuestions) {
                    for (userID in $scope.users) {
                        $scope.evaluation.responses[tempID] = {
                            evalID: $scope.evaluation.evalID,
                            evalTemplateQuestionID: $scope.evaluation.templateQuestions[questionID].evalTemplateQuestionID,
                            response: "",
                            userID: $scope.users[userID].userID
                        }
                        tempID++;
                    }
                }
            });

        $scope.getResponse = function (evalTemplateQuestionID, userID) {
            for (responseID in $scope.evaluation.responses) {
                if ($scope.evaluation.responses[responseID].userID === userID &&
                    $scope.evaluation.responses[responseID].evalTemplateQuestionID === evalTemplateQuestionID)
                    return $scope.evaluation.responses[responseID];
            }

            return $scope.evaluation.responses.push({
                evalID: $scope.evaluation.evalID,
                evalTemplateQuestionID: evalTemplateQuestionID,
                response: "",
                userID: userID
            });
        };

        $scope.completeEvaluation = function () {
            if (confirm('Are you sure you want to submit this evaluation?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Home/CompleteEvaluation", $scope.evaluation.responses )
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        toastr["success"]("Evaluation completed.");
                        $location.path('/group/' + $scope.groupID)
                    }, function () {
                        usSpinnerService.stop('spinner');
                        toastr["error"]("Failed to complete evaluation.");
                    });
            } else {
                // Do nothing!
            }
        }

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