angular.module('time').controller('ManageEvalsCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', '$route', function ($scope, $http, $routeParams, $location, usSpinnerService, $route) {
    $scope.loaded = false;
    $scope.config = {};
    $scope.config.currentTemplate = 0;
    $scope.config.instructorID = 0;
    $scope.evaulations = {};
    $scope.instructors = {};

    $scope.load = function () {
        $scope.userID = $routeParams.ID;
        $scope.config.instructorID = $scope.userID;
        if (!$scope.userID) window.history.back();
        
        $scope.loadTemplates = function () {
            $scope.evaluations = {};
            $scope.config.currentTemplate = 0;
            usSpinnerService.spin('spinner');
            $http.post("/Home/GetTemplatesForInstructor", { userID: $scope.config.instructorID })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    $.each(response.data, function (index, template) {
                        if ($scope.config.currentTemplate === 0) $scope.config.currentTemplate = template.evalTemplateID;
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
        }

        if ($scope.$parent.user.type === 'A') {
            usSpinnerService.spin('spinner');
            $http.get("/Home/GetUsers")
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
            $http.post("/Home/SaveTemplateName", { evalTemplateID: evalTemplate.evalTemplateID, templateName: evalTemplate.templateName })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    toastr["success"]("Saved template name.");
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to save template name.");
                });
        }

        $scope.hasEvaluationTemplates = function () {
            return !angular.equals({}, $scope.evaluations);
        }

        $scope.createBlankEvaluation = function () {
            if (confirm('Are you sure you want to create a new template?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Home/CreateTemplate", { userID: $scope.config.instructorID })
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
        }

        $scope.createCategory = function () {
            usSpinnerService.spin('spinner');
            $http.post("/Home/CreateCategory", { evalTemplateID: $scope.config.currentTemplate })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    toastr["success"]("Created category.");
                    $scope.evaluations[$scope.config.currentTemplate].categories[response.data] = {
                        evalTemplateQuestionCategoryID: response.data,
                        categoryName: "New Category",
                        evalTemplateID: $scope.config.currentTemplate
                    };
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to create a new category.");
                });
        }

        $scope.deleteCategory = function (category) {
            if (confirm('Are you sure you want to delete this category?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Home/DeleteCategory", { evalTemplateQuestionCategoryID: category.evalTemplateQuestionCategoryID })
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
        }

        $scope.deleteQuestion = function (question) {
            if (confirm('Are you sure you want to delete this question?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Home/DeleteQuestion", { evalTemplateID: $scope.config.currentTemplate, evalTemplateQuestionID: question.evalTemplateQuestionID })
                    .then(function (response) {
                        usSpinnerService.stop('spinner');
                        toastr["success"]("Question deleted.");
                        delete $scope.evaluations[$scope.config.currentTemplate].templateQuestions[question.evalTemplateQuestionID];
                    }, function () {
                        usSpinnerService.stop('spinner');
                        toastr["error"]("Failed to delete the question.");
                    });
            } else {
                // Do nothing!
            }
        }

        $scope.saveCategory = function (category) {
            $http.post("/Home/SaveCategory", category)
                .then(function (response) {
                    toastr["success"]("Category saved.");
                }, function () {
                    toastr["error"]("Failed to save the category.");
                });
        }

        $scope.saveQuestion = function (question) {
            if (question.number === null || isNaN(question.number)) { question.number = 0 };
            $http.post("/Home/SaveQuestion", question)
                .then(function (response) {
                    toastr["success"]("Question saved.");
                }, function () {
                    toastr["error"]("Failed to save the question.");
                });
        }

        $scope.addQuestion = function (categoryID) {
            $http.post("/Home/CreateTemplateQuestion", { evalTemplateID: $scope.config.currentTemplate, evalTemplateQuestionCategoryID: categoryID })
                .then(function (response) {
                    usSpinnerService.stop('spinner');
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