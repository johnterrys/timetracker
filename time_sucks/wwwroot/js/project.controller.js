angular.module('time').controller('ProjectCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.config = {};
    $scope.config.showInactiveGroups = false;

    $scope.load = function() {

        $scope.projectID = $routeParams.ID;

        if (!$scope.projectID) $location.path('/courses');

        usSpinnerService.spin('spinner');
        $http.post("/Home/GetProject", { projectID: $scope.projectID })
            .then(function (response) {
                $scope.project = {}
                $scope.project.projectID = response.data.projectID;
                $scope.project.projectName = response.data.projectName;
                $scope.project.isActive = response.data.isActive;
                $scope.project.description = response.data.description;
                $scope.project.courseID = response.data.courseID;
                $scope.project.groups = {};
                $.each(response.data.groups, function (index, group) {
                    $scope.project.groups[group.groupID] = {}
                    $scope.project.groups[group.groupID].evalID = group.evalID;
                    $scope.project.groups[group.groupID].groupID = group.groupID;
                    $scope.project.groups[group.groupID].groupName = group.groupName;
                    $scope.project.groups[group.groupID].isActive = group.isActive;
                    $scope.project.groups[group.groupID].users = {};
                    //Setting users to be in the index of their userID
                    $.each(group.users, function (index, user) {
                        $scope.project.groups[group.groupID].users[user.userID] = {};
                        $scope.project.groups[group.groupID].users[user.userID].firstName = user.firstName;
                        $scope.project.groups[group.groupID].users[user.userID].lastName = user.lastName;
                        $scope.project.groups[group.groupID].users[user.userID].isActive = user.isActive;
                        $scope.project.groups[group.groupID].users[user.userID].userID = user.userID;
                        $scope.project.groups[group.groupID].users[user.userID].timecards = {};
                        $.each(user.timecards, function (index, timecard) {
                            $scope.project.groups[group.groupID].users[user.userID].timecards[timecard.timeslotID] = timecard;
                            $scope.project.groups[group.groupID].users[user.userID].timecards[timecard.timeslotID].hours = "";
                        });
                    });
                });
                
                $scope.updateAllHours();
                $scope.updateChart();
                usSpinnerService.stop('spinner');
            }, function (response) {
                usSpinnerService.stop('spinner');
                if (response.status === 401) toastr["error"]("Unauthorized to view this project.");
                else toastr["error"]("Failed to load project, unknown error.");
            });

        $scope.createGroup = function () {
            $http.post("/Home/CreateGroup", {
                projectID: $scope.projectID
            })
                .then(function (response) {
                    $location.path('/group/'+response.data);
                }, function (response) {
                    if (response.status === 401) toastr["error"]("Unauthorized to create a group on this project.");
                    else if (response.status === 403) toastr["error"]("You are already part of a group on this project. Please leave the group before creating a new one.");
                    else toastr["error"]("Failed to create group, unknown error.");
                });
        }

        $scope.saveProject = function () {
            $http.post("/Home/SaveProject", {
                projectID: $scope.project.projectID,
                projectName: $scope.project.projectName,
                description: $scope.project.description,
                isActive: $scope.project.isActive
            })
                .then(function (response) {
                    toastr["success"]("Saved group.");
                }, function () {
                    toastr["error"]("Failed to save group.");
                });
        }

        $scope.updateAllHours = function () {
            $.each($scope.project.groups, function (index, group) {
                $.each(group.users, function (index, user) {
                    $.each(user.timecards, function (index, time) {
                        if (time.timeIn !== '' && time.timeOut !== '') {
                            $scope.project.groups[group.groupID].users[user.userID].timecards[time.timeslotID].hours = moment.duration(
                                moment($scope.project.groups[group.groupID].users[user.userID].timecards[time.timeslotID].timeOut).diff(
                                    $scope.project.groups[group.groupID].users[user.userID].timecards[time.timeslotID].timeIn)).asHours().toFixed(2);
                        }
                    });
                });
            });
        }

        var data = { //Data and labels are set in the setData function
            datasets: [{
                data: [],
                backgroundColor: [ //After 30 groups are included in a project, the color will just be gray, add more colors here if there can be more than 30 groups
                    '#2C3E50', '#3498DB', '#18BC9C', '#F39C12', '#e83e8c', '#6610f2', '#fd7e14', '#E74C3C', '#6f42c1', '#95a5a6',
                    '#2C3E50', '#3498DB', '#18BC9C', '#F39C12', '#e83e8c', '#6610f2', '#fd7e14', '#E74C3C', '#6f42c1', '#95a5a6',
                    '#2C3E50', '#3498DB', '#18BC9C', '#F39C12', '#e83e8c', '#6610f2', '#fd7e14', '#E74C3C', '#6f42c1', '#95a5a6',
                ]
            }],

            // These labels appear in the legend and in the tooltips when hovering different arcs
            labels: []
            
        };

        $scope.setData = function () {
            if (!$scope.project || !$scope.project.groups) return;
            data.datasets[0].data = [];
            data.labels = [];
            for (var g in $scope.project.groups) {
                g = $scope.project.groups[g];
                if (!g.isActive && !$scope.config.showInactiveGroups) continue;
                var hours = 0;
                for (var u in g.users) {
                    u = g.users[u];
                    for (var t in u.timecards) {
                        t = u.timecards[t];
                        hours += Number(t.hours);
                    }
                }
                data.datasets[0].data.push(hours);
                data.labels.push(g.groupName);
            }
        }

        $scope.setData();

        var ctx = $("#projectHours");

        var myChart = new Chart(ctx, {
            type: 'pie',
            data: data
        });

        $scope.updateChart = function () {
            //  The section below creates a display for a empty chart.            
            var hours = 0;

            for (var g in $scope.project.groups) {
                g = $scope.project.groups[g];
                if (!g.isActive && !$scope.config.showInactiveGroups) continue;

                for (var u in g.users) {
                    u = g.users[u];
                    for (var t in u.timecards) {
                        t = u.timecards[t];
                        hours += Number(t.hours);
                    }
                }
            }
            if (hours === 0) {
                document.getElementById("projectHours").style.visibility = "hidden";
                document.getElementById("noProjectData").style.visibility = "visible";
            }
            else {
                document.getElementById("projectHours").style.visibility = "visible";
                document.getElementById("noProjectData").style.visibility = "hidden";
            }
            
            $scope.setData();
            myChart.update();
        }
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