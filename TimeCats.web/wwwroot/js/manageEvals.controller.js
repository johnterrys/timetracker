angular.module('time').controller('ManageEvalsCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', '$route', function ($scope, $http, $routeParams, $location, usSpinnerService, $route) {
    $scope.loaded = false;
    $scope.config = {};
    $scope.config.currentTemplate = 0;
    $scope.config.instructorId = 0;
    $scope.evaluations = {};
    $scope.instructors = {};

    var lastEval = 0; 

    $scope.load = function () {
        $scope.userID = $routeParams.ID;
        $scope.config.instructorId = $scope.userID;
        if (!$scope.userID) window.history.back();

        if (lastEval = 0) {
            $scope.config.currentTemplateID = 0;
        }

        $scope.loadTemplates = function () {
            $scope.evaluations = {};
            usSpinnerService.spin('spinner');
            $http.post("/Eval/GetTemplatesForInstructor", { userID: $scope.config.instructorId })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    $.each(response.data, function (index, template) {
                        lastEval = template.evalTemplateID;
                        if ($scope.config.currentTemplate === 0) {
                            $scope.config.currentTemplate = template.evalTemplateID;
                        }
                        else
                        {
                            $scope.config.currentTemplate = lastEval;
                        }
                        $scope.evaluations[template.evalTemplateID] = {
                            evalTemplateID: template.evalTemplateID,
                            templateName: template.templateName,
                            userID: template.userID,
                            inUse: template.inUse
                        };
                        $scope.evaluations[template.evalTemplateID].categories = {
                            0: {
                                evalTemplateQuestionCategoryID: 0,
                                categoryName: "No Category"
                            }
                        };
                        $scope.evaluations[template.evalTemplateID].templateQuestions = {};
                        $.each(template.categories, function (index, category) {
                            $scope.evaluations[template.evalTemplateID].categories[category.evalTemplateQuestionCategoryID] = category;
                        });
                        $.each(template.templateQuestions, function (index, templateQuestion) {
                            $scope.evaluations[template.evalTemplateID].templateQuestions[templateQuestion.evalTemplateQuestionID] = templateQuestion;
                        });
                    });
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to retrieve instructor's evaluations. Using Dummy Data.");

                    //Dummy Data
                    $scope.evaluations = {
                        1: {
                            evalTemplateID: 1,
                            templateName: "Test Evalation Template 1",
                            inUse: true,
                            categories: {
                                1: {
                                    evalTemplateQuestionCategoryID: 1,
                                    categoryName: "Test Category 1"
                                },
                                2: {
                                    evalTemplateQuestionCategoryID: 2,
                                    categoryName: "Test Category 2"
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
                                    questionText: "Test question 4, what do you think?",
                                    number: 1
                                }
                            }
                        },
                        2: {
                            evalTemplateID: 2,
                            templateName: "Test Evalation Template 2",
                            inUse: false,
                            number: 1,
                            categories: {
                                3: {
                                    evalTemplateQuestionCategoryID: 3,
                                    categoryName: "Category 3"
                                },
                                4: {
                                    evalTemplateQuestionCategoryID: 4,
                                    categoryName: "Category 4"
                                },
                                5: {
                                    evalTemplateQuestionCategoryID: 5,
                                    categoryName: "Category 5"
                                }
                            },
                            templateQuestions: {
                                1: {
                                    evalTemplateQuestionID: 1,
                                    evalTemplateQuestionCategoryID: 3,
                                    questionType: 'N',
                                    questionText: "Test question 1, what do you think?",
                                    number: 2
                                },
                                2: {
                                    evalTemplateQuestionID: 2,
                                    evalTemplateQuestionCategoryID: 3,
                                    questionType: 'R',
                                    questionText: "Test question 2 (should be first), what do you think?",
                                    number: 1
                                },
                                3: {
                                    evalTemplateQuestionID: 3,
                                    evalTemplateQuestionCategoryID: 4,
                                    questionType: 'N',
                                    questionText: "Test question 3, what do you think?",
                                    number: 1
                                },
                                4: {
                                    evalTemplateQuestionID: 4,
                                    evalTemplateQuestionCategoryID: 4,
                                    questionType: 'R',
                                    questionText: "Test question 4, what do you think?",
                                    number: 2
                                },
                                5: {
                                    evalTemplateQuestionID: 4,
                                    evalTemplateQuestionCategoryID: 5,
                                    questionType: 'N',
                                    questionText: "Test question 4, what do you think?",
                                    number: 1
                                }
                            }
                        }
                    }
                });
        };

        if ($scope.$parent.user.type === 'A') {
            usSpinnerService.spin('spinner');
            $http.get("/User/GetUsers")
                .then(function (response) {
                    //Setting users to be in the index of their userID
                    $.each(response.data, function (index, user) {
                        if (user.type === 'I' || user.userID === $scope.$parent.user.userID)
                            $scope.instructors[user.userID] = user;
                    });
                    usSpinnerService.stop('spinner');
                    $scope.loadTemplates();
                    $scope.loaded = true;
                    if (response.status === 204) {
                        toastr["error"]("Not an Admin.");
                        window.history.back();
                    }
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to get instructors.");
                    $location.path('/dashboard');
                });
        } else if ($scope.$parent.user.type === 'I') {
            $scope.loadTemplates();
        } else {
            toastr["error"]("Not an Admin or Instructor.");
            window.history.back();
        }

        $scope.getResponse = function (evaluationID, evalID, evalTemplateQuestionID) {
            for (responseID in $scope.group.evaluations[evaluationID].responses) {
                if ($scope.group.evaluations[evaluationID].responses[responseID].evalID === evalID &&
                    $scope.group.evaluations[evaluationID].responses[responseID].evalTemplateQuestionID === evalTemplateQuestionID)
                    return $scope.group.evaluations[evaluationID].responses[responseID].response;
            }
            return '';
        };

        $scope.saveTemplateName = function (evalTemplate) {
            usSpinnerService.spin('spinner');
            $http.post("/Eval/SaveTemplateName", { evalTemplateID: evalTemplate.evalTemplateID, templateName: evalTemplate.templateName })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    toastr["success"]("Saved template name.");
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to save template name.");
                });
        };

        $scope.hasEvaluationTemplates = function () {
            return !angular.equals({}, $scope.evaluations);
        };

        $scope.createBlankEvaluation = function () {
            if (confirm('Are you sure you want to create a new template?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Eval/CreateTemplate", { userID: $scope.config.instructorId })
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        toastr["success"]("Created evaluation.");
                        $route.reload();
                    }, function () {
                        usSpinnerService.stop('spinner');
                        toastr["error"]("Failed to create a new evaluation.");
                    });
            } else {
                // Do nothing!
            }
        };

        $scope.createCategory = function () {
            usSpinnerService.spin('spinner');
            $http.post("/Eval/CreateCategory", { evalTemplateID: $scope.config.currentTemplate })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    toastr["success"]("Created category.");
                    $route.reload();
                    $scope.evaluations[$scope.config.currentTemplate].categories[response.data] = {
                        evalTemplateQuestionCategoryID: response.data,
                        categoryName: "New Category",
                        evalTemplateID: $scope.config.currentTemplate
                    };
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to create a new category.");
                });
        };

        $scope.deleteCategory = function (category) {
            if (confirm('Are you sure you want to delete this category?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Eval/DeleteCategory", { evalTemplateQuestionCategoryID: category.evalTemplateQuestionCategoryID })
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        toastr["success"]("Category deleted.");
                        delete $scope.evaluations[$scope.config.currentTemplate].categories[category.evalTemplateQuestionCategoryID];
                        for (evalTemplateQuestionID in $scope.evaluations[$scope.config.currentTemplate].templateQuestions) {
                            //  Deletes all questions that belong to this category.
                            if ($scope.evaluations[$scope.config.currentTemplate].templateQuestions[evalTemplateQuestionID].evalTemplateQuestionCategoryID === category.evalTemplateQuestionCategoryID) {
                                delete $scope.evaluations[$scope.config.currentTemplate].templateQuestions[evalTemplateQuestionID].evalTemplateQuestionCategoryID;
                            }
                        }
                    }, function () {
                        usSpinnerService.stop('spinner');
                        toastr["error"]("Failed to delete the category.");
                    });
            } else {
                // Do nothing!
            }
        };

        $scope.deleteQuestion = function (question) {
            if (confirm('Are you sure you want to delete this question?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Eval/DeleteQuestion", { evalTemplateID: $scope.config.currentTemplate, evalTemplateQuestionID: question.evalTemplateQuestionID })
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        $route.reload();
                        toastr["success"]("Question deleted.");
                        delete $scope.evaluations[$scope.config.currentTemplate].templateQuestions[question.evalTemplateQuestionID];
                    }, function () {
                        usSpinnerService.stop('spinner');
                        toastr["error"]("Failed to delete the question.");
                    });
            } else {
                // Do nothing!
            }
        };

        $scope.saveCategory = function (category) {
            $http.post("/Eval/SaveCategory", category)
                .then(function (response) {
                    toastr["success"]("Category saved.");
                    $route.reload();
                }, function () {
                    toastr["error"]("Failed to save the category.");
                });
        };

        $scope.saveQuestion = function (question) {
            if (question.number === null || isNaN(question.number)) { question.number = 0 }
            $http.post("/Eval/SaveQuestion", question)
                .then(function (response) {
                    toastr["success"]("Question saved.");
                    $route.reload();
                }, function () {
                    toastr["error"]("Failed to save the question.");
                });
        };

        $scope.addQuestion = function (categoryID) {
            $http.post("/Eval/CreateTemplateQuestion", { evalTemplateID: $scope.config.currentTemplate, evalTemplateQuestionCategoryID: categoryID })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    $route.reload();
                    toastr["success"]("Created question.");
                    $scope.evaluations[$scope.config.currentTemplate].templateQuestions[response.data] = {
                        evalTemplateQuestionCategoryID: categoryID,
                        evalTemplateID: $scope.config.currentTemplate,
                        evalTemplateQuestionID: response.data,
                        questionType: 'N',
                        questionText: '',
                        number: 0
                    };
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to create a new question.");
                });
        };

        //wb:INB - When user completes eval and clicks button they are rerouted to dashboard
        $scope.completeEval = function () {
            if (confirm("Are you finished editing this Evaluation? You may edit at a later time.")) {
                $location.path('/dashboard');
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
            if ($scope.$parent.user.type !== 'A' && $scope.$parent.user.type !== 'I') {
                toastr["error"]("Not Admin or Instructor");
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
