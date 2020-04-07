angular.module('time').controller('MasterCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.user = null;
    $scope.loaded = false;
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "200",
        "hideDuration": "500",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    $scope.collapse = function () {
        $('.navbar-collapse').collapse('hide');
    };

    $scope.userExists = function () {
        if ($scope.user !== null) {
            $('#navbarSupportedContent').removeClass('invisible');
            return true;
        }
        return false;
    };

    $scope.logout = function () {
        usSpinnerService.spin('spinner');
        $http.get("/User/Logout")
            .then(function (response) {
                $scope.user = null;
                usSpinnerService.stop('spinner');
                $location.path('/login');
            }, function () {
                usSpinnerService.stop('spinner');
                toastr["error"]("Failed to logout.");
            });
    }
}]);
