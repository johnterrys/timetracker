angular.module('time').controller('AdminEvalsCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.search = '';
    $scope.loaded = false;
    $scope.load = function() {
        usSpinnerService.spin('spinner');
        $http.get("/Home/GetAllEvaluations")
            .then(function (response) {
                $scope.evals = {};
                //Setting users to be in the index of their userID
                $.each(response.data, function (index, eval) {
                    $scope.evals[eval.evalID] = eval;
                });
                usSpinnerService.stop('spinner');
                $scope.loaded = true;
            }, function (response) {
                usSpinnerService.stop('spinner');
                if (response.status === 401) {
                    toastr["error"]("Not an Admin.");
                    window.history.back();
                } else {
                    toastr["error"]("Failed to get users.");
                    $location.path('/dashboard');
                }
            });

        $scope.saveEval = function (eval) {
            $http.post("/Home/SaveEval", eval)
                .then(function (response) {
                    toastr["success"]("Eval saved.");
                }, function (response) {
                    if (response.status === 500) {
                        toastr["error"]("Failed to save eval, query error.");
                    } else if (response.status === 401) {
                        toastr["error"]("Unauthorized to make changes to this eval.");
                    } else {
                        toastr["error"]("Failed to save eval, unknown error.");
                    }
                });
        }
    };

    //Standard login check, if there is a user, load the page, if not, redirect to login
    usSpinnerService.spin('spinner');
    $http.get("/Home/CheckSession")
        .then(function (response) {
            usSpinnerService.stop('spinner');
            $scope.$parent.user = response.data;
            if ($scope.$parent.user.type !== 'A') {
                toastr["error"]("Not Admin.");
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