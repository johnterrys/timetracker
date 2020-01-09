var app = angular.module('time', ['ngRoute', 'angularSpinner']); //Sets up the Angular app, and includes the ngRoute module

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider //Assigns the template file and controller for the routes
        .when("/", {
            templateUrl: "templates/login.html",
            controller: "LoginCtrl"
        })
        .when("/login", {
            templateUrl: "templates/login.html",
            controller: "LoginCtrl"
        })
        .when("/register", {
            templateUrl: "templates/register.html",
            controller: "RegisterCtrl"
        })
        .when("/dashboard", {
            templateUrl: "templates/dashboard.html",
            controller: "DashboardCtrl"
        })
        .when("/courses", {
            templateUrl: "templates/courses.html",
            controller: "CoursesCtrl"
        })
        .when("/course/:ID", {
            templateUrl: "templates/course.html",
            controller: "CourseCtrl"
        })
        .when("/project/:ID", {
            templateUrl: "templates/project.html",
            controller: "ProjectCtrl"
        })
        .when("/group/:ID", {
            templateUrl: "templates/group.html",
            controller: "GroupCtrl"
        })
        .when("/users", {
            templateUrl: "templates/users.html",
            controller: "UsersCtrl"
        })
        .when("/user/:ID", {
            templateUrl: "templates/user.html",
            controller: "UserCtrl"
        })
        .when("/assignEval/:ID", {
            templateUrl: "templates/assignEval.html",
            controller: "AssignEvalCtrl"
        })
        .when("/viewEvals/:ID", {
            templateUrl: "templates/viewEvals.html",
            controller: "ViewEvalsCtrl"
        })
        .when("/eval/:ID", {
            templateUrl: "templates/eval.html",
            controller: "EvalCtrl"
        })
        .when("/manageEvals/:ID", {
            templateUrl: "templates/manageEvals.html",
            controller: "ManageEvalsCtrl"
        })
        .when("/adminEvals", {
            templateUrl: "templates/adminEvals.html",
            controller: "AdminEvalsCtrl"
        })
        .when("/viewEval/:ID", {
            templateUrl: "templates/viewEval.html",
            controller: "ViewEvalCtrl"
        })
        .otherwise("/");
}]);

app.config(['usSpinnerConfigProvider', function (usSpinnerConfigProvider) {
    usSpinnerConfigProvider.setDefaults({ color: '#18BC9C', radius: 15 });
}]);

app.filter('orderObjectBy', function () {
    return function (items, field, reverse) {
        var filtered = [];
        angular.forEach(items, function (item) {
            filtered.push(item);
        });
        filtered.sort(function (a, b) {
            return (a[field] > b[field] ? 1 : -1);
        });
        if (reverse) filtered.reverse();
        return filtered;
    };
});

app.directive('stringToNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (value) {
                return '' + value;
            });
            ngModel.$formatters.push(function (value) {
                return parseFloat(value);
            });
        }
    };
});