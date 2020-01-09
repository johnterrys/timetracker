angular.module('time').controller('ViewEvalsCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.config = {};
    $scope.currentEval = 0;
    $scope.currentUser = 0;
    $scope.group = {};
    $scope.group.users = {};
    $scope.group.evaluations = {};

    $scope.load = function () {
        $scope.groupID = $routeParams.ID;

        if (!$scope.groupID) window.history.back();

        usSpinnerService.spin('spinner');

        $http.post("/Home/GetUsersForGroup", { groupID: $scope.groupID })
            .then(function (response) {
                $.each(response.data, function (index, user) {
                    $scope.group.users[user.userID] = user;
                });
                var inGroup = false;
                for (userID in $scope.group.users) {
                    if ($scope.group.users[userID].userID === $scope.$parent.user.userID)
                        inGroup = true;
                }
                if (inGroup) {
                    $scope.currentUser = $scope.$parent.user.userID;
                } else {
                    for (userID in $scope.group.users) {
                        $scope.currentUser = $scope.group.users[userID].userID;
                        break;
                    }
                }
                $scope.loadEvals();
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Failed to retrieve group users.");
            });

        $scope.loadEvals = function () {
            usSpinnerService.spin('spinner');
            $http.post("/Home/GetAllCompleteEvaluations", { groupID: $scope.groupID, userID: $scope.currentUser })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    $.each(response.data, function (index, eval) {
                        if ($scope.currentEval < eval.number) $scope.currentEval = eval.number;
                        $scope.group.evaluations[eval.number] = {};
                        $scope.group.evaluations[eval.number].evalID = eval.evalTemplateID;
                        $scope.group.evaluations[eval.number].number = eval.number;
                        $scope.group.evaluations[eval.number].categories = {};
                        $scope.group.evaluations[eval.number].templateQuestions = {};
                        $scope.group.evaluations[eval.number].responses = {};
                        $scope.group.evaluations[eval.number].evals = {};

                        $.each(eval.evals, function (index, evalColumn) {
                            $scope.group.evaluations[eval.number].evals[evalColumn.evalID] = evalColumn;
                            /****Jamison Edit******/
                            $scope.group.evaluations[eval.number].evals[evalColumn.evalID].totalValue = 0;
                            $scope.group.evaluations[eval.number].evals[evalColumn.evalID].userAvgerage = 0;
                            /**End Edit**/
                        });
                        $.each(eval.categories, function (index, category) {
                            $scope.group.evaluations[eval.number].categories[category.evalTemplateQuestionCategoryID] = category;
                        });
                        $.each(eval.templateQuestions, function (index, templateQuestion) {
                            $scope.group.evaluations[eval.number].templateQuestions[templateQuestion.evalTemplateQuestionID] = templateQuestion;
                        });
                        $.each(eval.responses, function (index, response) {
                            /**Jamison Edit**/
                            //check to see if a response is text or an int... if int, add to the total count
                            if (Number.isInteger(parseInt(response.response, 10)) == true) {
                                $scope.group.evaluations[eval.number].evals[response.evalID].totalValue += parseInt(response.response, 10);
                            }
                            /**End Edit**/
                            $scope.group.evaluations[eval.number].evals[response.evalID].userAvgerage = parseFloat(response.userAvgerage);
                            $scope.group.evaluations[eval.number].responses[response.evalResponseID] = response;
                        });
                                               
                    });




                    $scope.group.testEvaluations = {
                        1: {
                            evalTemplateID: 1,
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
                                    evalTemplateQuestionID: 5,
                                    evalTemplateQuestionCategoryID: 0,
                                    questionType: 'N',
                                    questionText: "Test question 4, what do you think?",
                                    number: 1
                                }
                            },
                            evals: {
                                1: {
                                    evalID: 1,
                                    firstName: "Team",
                                    lastName: "Member"
                                },
                                2: {
                                    evalID: 2,
                                    firstName: "Team",
                                    lastName: "Member"
                                }
                            },
                            responses: {
                                1: {
                                    evalResponseID: 1,
                                    evalID: 1,
                                    evalTemplateQuestionID: 1,
                                    response: "5",
                                    userID: 1
                                },
                                2: {
                                    evalResponseID: 2,
                                    evalID: 2,
                                    evalTemplateQuestionID: 1,
                                    response: "4",
                                    userID: 1
                                },
                                3: {
                                    evalResponseID: 3,
                                    evalID: 1,
                                    evalTemplateQuestionID: 2,
                                    response: "Hello! This is a response that has been thought out and considered. It takes up more space, because they wanted to be detailed and make a coherent argument. Hopefully it illustrates what a long response could be.",
                                    userID: 21
                                },
                                4: {
                                    evalResponseID: 4,
                                    evalID: 2,
                                    evalTemplateQuestionID: 2,
                                    response: "Hello! This is a response that has been thought out and considered. It takes up more space, because they wanted to be detailed and make a coherent argument. Hopefully it illustrates what a long response could be.",
                                    userID: 1
                                },
                                5: {
                                    evalResponseID: 5,
                                    evalID: 1,
                                    evalTemplateQuestionID: 3,
                                    response: "5",
                                    userID: 1
                                },
                                6: {
                                    evalResponseID: 6,
                                    evalID: 2,
                                    evalTemplateQuestionID: 3,
                                    response: "4",
                                    userID: 1
                                },
                                7: {
                                    evalResponseID: 7,
                                    evalID: 1,
                                    evalTemplateQuestionID: 4,
                                    response: "Hello! This is a response that has been thought out and considered. It takes up more space, because they wanted to be detailed and make a coherent argument. Hopefully it illustrates what a long response could be.",
                                    userID: 1
                                },
                                8: {
                                    evalResponseID: 8,
                                    evalID: 2,
                                    evalTemplateQuestionID: 4,
                                    response: "Hello! This is a response that has been thought out and considered. It takes up more space, because they wanted to be detailed and make a coherent argument. Hopefully it illustrates what a long response could be.",
                                    userID: 1
                                }
                            }
                        },
                        2: {
                            evalTemplateID: 1,
                            number: 2,
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
                                    evalTemplateQuestionID: 5,
                                    evalTemplateQuestionCategoryID: 0,
                                    questionType: 'N',
                                    questionText: "Test question 4, what do you think?",
                                    number: 1
                                }
                            },
                            evals: {
                                1: {
                                    evalID: 1,
                                    firstName: "Logan",
                                    lastName: "Brown"
                                },
                                2: {
                                    evalID: 2,
                                    firstName: "Tashi",
                                    lastName: "Aguilar"
                                }
                            },
                            responses: {
                                1: {
                                    evalResponseID: 1,
                                    evalID: 1,
                                    evalTemplateQuestionID: 1,
                                    response: "3",
                                    userID: 1
                                },
                                2: {
                                    evalResponseID: 2,
                                    evalID: 2,
                                    evalTemplateQuestionID: 1,
                                    response: "2",
                                    userID: 1
                                },
                                3: {
                                    evalResponseID: 3,
                                    evalID: 1,
                                    evalTemplateQuestionID: 2,
                                    response: "Hello!",
                                    userID: 21
                                },
                                4: {
                                    evalResponseID: 4,
                                    evalID: 2,
                                    evalTemplateQuestionID: 2,
                                    response: "Goodbye!",
                                    userID: 1
                                },
                                5: {
                                    evalResponseID: 5,
                                    evalID: 1,
                                    evalTemplateQuestionID: 3,
                                    response: "2",
                                    userID: 1
                                },
                                6: {
                                    evalResponseID: 6,
                                    evalID: 2,
                                    evalTemplateQuestionID: 3,
                                    response: "3",
                                    userID: 1
                                },
                                7: {
                                    evalResponseID: 7,
                                    evalID: 1,
                                    evalTemplateQuestionID: 4,
                                    response: "God Morgen!",
                                    userID: 1
                                },
                                8: {
                                    evalResponseID: 8,
                                    evalID: 2,
                                    evalTemplateQuestionID: 4,
                                    response: "God Natt",
                                    userID: 1
                                }
                            }
                        }
                    }

                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to retrieve group evaluations.");
                });
        }
        

        $scope.getResponse = function (number, evalID, evalTemplateQuestionID) {
            for (responseID in $scope.group.evaluations[number].responses) {
                if ($scope.group.evaluations[number].responses[responseID].evalID === evalID &&
                    $scope.group.evaluations[number].responses[responseID].evalTemplateQuestionID === evalTemplateQuestionID)
                    return $scope.group.evaluations[number].responses[responseID].response;
            }
            return '';
        };
        
        //  Get user average score per eval
        $scope.getUserAvg = function (number, evalID) {
            return $scope.group.evaluations[number].evals[evalID].userAvgerage;
        };

        //  Get Difference
        $scope.getDiff = function (number, evalID) {
            return $scope.group.evaluations[number].evals[evalID].totalValue - $scope.group.evaluations[number].evals[evalID].userAvgerage;
        };

        $scope.calculateCategoryTotal = function (categoryID, evalID) {
            return '';
        };

        /**Jamison Edit**/
        $scope.getTotal = function (number, evalID) {
            return $scope.group.evaluations[number].evals[evalID].totalValue;
        };
        /**End Edit**/



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