angular.module('time').controller('GroupCtrl', ['$scope', '$http', '$routeParams', '$location', 'usSpinnerService', function ($scope, $http, $routeParams, $location, usSpinnerService) {
    $scope.loaded = false;
    $scope.group = {};
    $scope.group.users = {};

    //$scope.newNumber = 13; //TODO get rid of this

    $scope.load = function () {
        $scope.groupID = $routeParams.ID;

        if (!$scope.groupID) $location.path('/courses');
        //TODO Enable Group functionality, disable dummy data
        usSpinnerService.spin('spinner');
        $http.post("/Group/GetGroup", { groupID: $scope.groupID })
            .then(function (response) {
                $scope.group = {};
                $scope.group.evalID = response.data.evalID;
                $scope.group.groupID = response.data.groupID;
                $scope.group.groupName = response.data.groupName;
                $scope.group.isActive = response.data.isActive;
                $scope.group.projectID = response.data.projectID;
                $scope.group.users = {};
                //Setting users to be in the index of their userID
                $.each(response.data.users, function (index, user) {
                    $scope.group.users[user.userID] = {};
                    $scope.group.users[user.userID].firstName = user.firstName;
                    $scope.group.users[user.userID].lastName = user.lastName;
                    $scope.group.users[user.userID].isActive = user.isActive;
                    $scope.group.users[user.userID].userID = user.userID;
                    $scope.group.users[user.userID].timecards = {};
                    $.each(user.timecards, function (index, timecard) {
                        $scope.group.users[user.userID].timecards[timecard.timeslotID] = timecard;
                        $scope.group.users[user.userID].timecards[timecard.timeslotID].hours = '';
                    });
                });
                $scope.updateAllHours();
                $scope.updateChart();
                usSpinnerService.stop('spinner');
            }, function (response) {
                usSpinnerService.stop('spinner');
                if (response.status === 401) {
                    toastr["error"]("Unauthorized to view this group.");
                    window.history.back();
                } else {
                    toastr["error"]("Error getting group.");
                }
            });

        //makes a blank time card for every user?
        $.each($scope.group.users, function (userID, user) {
            $scope.group.users[userID].blank = {
                timeslotID: userID + '-blank',
                hours: '',
                isEdited: false,
                timeIn: '',
                timeOut: '',
                description: ''
            }
        });

        $scope.createTime = function (id, startTime = '') {
            var data = {
                userID: id,
                groupID: $scope.groupID,
                timeIn: startTime,
                isEdited: false,
                timeOut: "",
                description: ""
            };

            usSpinnerService.spin('spinner');
            $http.post("/Time/CreateTimeCard", data)
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    $scope.group.users[id].timecards[response.data] = {
                        timeslotID: response.data,
                        hours: "",
                        isEdited: false,
                        timeIn: startTime,
                        timeOut: "",
                        description: "",
                        userID: id,
                        groupID: $routeParams.ID
                    };
                    toastr["success"]("Timeslot created.");
                }, function () {
                    usSpinnerService.stop('spinner');
                    toastr["error"]("Failed to create time.");
                });
        };

        $scope.createTimeFromBlank = function (id) {
            var data = {
                userID: id,
                groupID: $scope.groupID,
                timeIn: $scope.group.users[id].blank.timeIn,
                timeOut: $scope.group.users[id].blank.timeOut,
                description: $scope.group.users[id].blank.description,
                isEdited: false

            };

            if ($scope.group.users[id].blank.timeIn === '' && $scope.group.users[id].blank.timeOut === '' && $scope.group.users[id].blank.description === '')
                return;

            //Time-in is now a required field
            if (data.timeIn > moment().format('MM/DD/YYYY h:mm:ss A')) {
                $scope.group.users[id].blank.timeIn = '';
                toastr["error"]("Invalid Start Time: Slot Not Created");
                return;
            }

            //Time-out is now a required field
            if (data.timeOut === '' || data.timeOut === undefined || data.timeOut === null) {
                $scope.group.users[id].blank.timeOut = '';
                toastr["error"]("Invalid End Time: Slot Not Created");
                return;
            }

            //Description is now a required field
            if (data.description === '' || data.description === undefined || data.description === null) {
                $scope.group.users[id].blank.description = '';
                toastr["error"]("Invalid Description: Slot Not Created");
                return;
            }

            usSpinnerService.spin('spinner');
            $http.post("/Time/CreateTimeCard", data)
                .then(function (response) {
                    usSpinnerService.stop('spinner');
                    $scope.group.users[id].timecards[response.data] = {
                        userID: id,
                        timeslotID: response.data,
                        hours: '',
                        isEdited: false,
                        timeIn: $scope.group.users[id].blank.timeIn,
                        timeOut: $scope.group.users[id].blank.timeOut,
                        description: $scope.group.users[id].blank.description,
                        groupID: $routeParams.ID
                    };


                    $scope.group.users[id].blank.timeIn = '';
                    $scope.group.users[id].blank.timeOut = '';
                    $scope.group.users[id].blank.description = '';
                    toastr["success"]("Timeslot created.");
                    // window.location.reload();
                    $scope.updateAllHours();
                    $scope.updateChart();

                },
                    function () {
                        usSpinnerService.stop('spinner');
                        toastr["error"]("Failed to create time.");
                    });
        };

        $scope.leaveGroup = function () {
            if (confirm('Are you sure you want to leave this group?')) {
                usSpinnerService.spin('spinner');
                $http.post("/Group/LeaveGroup", { groupID: $scope.groupID })
                    .then(function (response) {
                        if (response.status === 204) {
                            delete $scope.group.users[$scope.$parent.user.userID];
                        } else {
                            $scope.group.users[$scope.$parent.user.userID].isActive = false;
                        }
                        usSpinnerService.stop('spinner');
                        toastr["success"]("You left the group.");
                    }, function (response) {
                        usSpinnerService.stop('spinner');
                        if (response.status === 401) toastr["error"]("You are not part of this group.");
                        else toastr["error"]("Failed to leave the group, unknown error.");
                    });
            } else {
                // Do nothing!
            }
        };

        $scope.joinGroup = function () {
            usSpinnerService.spin('spinner');
            $http.post("/Group/JoinGroup", { groupID: $scope.groupID })
                .then(function (response) {
                    if (response.status === 204) {
                        $scope.group.users[$scope.$parent.user.userID].isActive = true;
                    } else {
                        $scope.group.users[$scope.$parent.user.userID] = {
                            userID: $scope.$parent.user.userID,
                            firstName: $scope.$parent.user.firstName,
                            lastName: $scope.$parent.user.lastName,
                            isActive: true,
                            timecards: {}
                        };
                    }
                    usSpinnerService.stop('spinner');
                    toastr["success"]("You have joined the group.");

                }, function (response) {
                    usSpinnerService.stop('spinner');
                    if (response.status === 401) toastr["error"]("You are not part of this course.");
                    else if (response.status === 403) toastr["error"]("You are already active in a different group. Please leave that group before joining a new one.");
                    else toastr["error"]("Failed to join the group, unknown error.");
                });
        };

        $scope.saveGroup = function () {
            $http.post("/Group/SaveGroup", {
                groupID: $scope.group.groupID,
                groupName: $scope.group.groupName,
                isActive: $scope.group.isActive,
                evalID: $scope.group.evalID,
                projectID: $scope.group.projectID
            })
                .then(function (response) {
                    toastr["success"]("Group saved.");
                }, function (response) {
                    if (response.status === 401) toastr["error"]("Unauthorized to change this group.");
                    else toastr["error"]("Failed to save group, unknown error.");
                });
        };

        $scope.userInGroup = function () {
            //Checks that the current user is listed in the current group.
            if (!$scope.$parent.user) return false;

            var inGroup = false;
            if (!$scope.group) return false;
            $.each($scope.group.users, function (index, user) {
                if (Number(user.userID) === Number($scope.$parent.user.userID)) {
                    inGroup = true;
                    //  Give the user the ability to change the group name when the name is set
                    //  to "New Group" or "new group" (which is the default group name)
                    if ($scope.group.groupName === "New Group" || $scope.group.groupName === "new group") {
                        document.getElementById("group_name").readOnly = false;
                    }
                }
            });
            return inGroup;
        };

        $scope.userActiveInGroup = function () {
            //Checks that the current user is listed in the current group.
            if (!$scope.$parent.user) return false;

            var inGroup = false;
            if (!$scope.group) return false;
            $.each($scope.group.users, function (index, user) {
                if (Number(user.userID) === Number($scope.$parent.user.userID) && user.isActive) {
                    inGroup = true;
                }
            });
            return inGroup;
        };

        $scope.hasUnfinishedBusiness = function () {
            var hasUnfinishedBusiness = false;
            if ($scope.userInGroup()) {
                $.each($scope.group.users[$scope.$parent.user.userID].timecards, function (index, time) {
                    if (time.timeIn !== '' && time.timeOut === '') hasUnfinishedBusiness = true;
                });
            }
            return hasUnfinishedBusiness;
        };

        //click start button 
        $scope.startTime = function () {
            if ($scope.userInGroup()) {
                $scope.createTime($scope.$parent.user.userID, moment().format('MM/DD/YYYY h:mm:ss A'));
            } else {
                toastr["error"]("The logged in user isn't a member of the group.");
            }
        };
        ///This happens when the user presses the clock iout button////
        $scope.endTime = function () {
            if ($scope.userInGroup()) {
                $.each($scope.group.users[$scope.$parent.user.userID].timecards, function (index, time) {
                    if (time.timeIn !== '' && time.timeOut === '' && time.description === '') {
                        $scope.group.users[$scope.$parent.user.userID].timecards[time.timeslotID].timeOut = moment().format('MM/DD/YYYY h:mm:ss A');

                        var startTime = moment($scope.group.users[$scope.$parent.user.userID].timecards[time.timeslotID].timeIn);
                        var endTime = moment($scope.group.users[$scope.$parent.user.userID].timecards[time.timeslotID].timeOut);
                        var duration = moment.duration(endTime.diff(startTime));
                        var hour = Math.floor(duration.asHours());
                        var minutes = Math.floor(duration.asMinutes() - (hour * 60));
                        var seconds = Math.floor(duration.asSeconds() - (hour * 3600) - (minutes * 60));

                        $scope.group.users[$scope.$parent.user.userID].timecards[time.timeslotID].hours = hour + ':' + minutes + ':' + seconds;

                        // $scope.group.users[$scope.$parent.user.userID].timecards[time.timeslotID].hours = moment.duration(
                        //     moment($scope.group.users[$scope.$parent.user.userID].timecards[time.timeslotID].timeOut).diff(
                        //         $scope.group.users[$scope.$parent.user.userID].timecards[time.timeslotID].timeIn)).asHours().toFixed(2);

                        //  $scope.updateAllHours();

                        $scope.saveTime($scope.$parent.user.userID, time.timeslotID);
                        $scope.group.users[$scope.$parent.user.userID].timecards[time.timeslotID].description = 'FILL ME OUT';
                        $scope.updateChart();


                        return false;
                    }
                });
            } else {
                toastr["error"]("The logged in user isn't a member of the group.");
            }
        };


        $scope.deleteTime = function (userID, timeslotID) {
            toastr["warning"]("Deleting...");
            $scope.group.users[userID].timecards[timeslotID].groupID = $routeParams.ID;
            $http.post("/Time/DeleteTimeCard", $scope.group.users[userID].timecards[timeslotID])
                .then(function (response) {
                    window.location.reload();
                });
        };



        $scope.saveTime = function (userID, timeslotID) {
            //if stuff is in the future throw error 
            if ($scope.group.users[userID].timecards[timeslotID].timeIn > moment().format('MM/DD/YYYY h:mm:ss A')) {
                toastr["error"]("TimeIn > Current Date");
                $scope.group.users[userID].timecards[timeslotID].timeIn = moment().format('MM/DD/YYYY h:mm:ss A');
            }
            if ($scope.group.users[userID].timecards[timeslotID].timeOut > moment().format('MM/DD/YYYY h:mm:ss A')) {
                toastr["error"]("TimeOut > Current Date");
                $scope.group.users[userID].timecards[timeslotID].timeOut = moment().format('MM/DD/YYYY h:mm:ss A');
            }

            //if your logged in time is before or equal to time out say no
            if ($scope.group.users[userID].timecards[timeslotID].timeIn > $scope.group.users[userID].timecards[timeslotID].timeOut || $scope.group.users[userID].timecards[timeslotID].timeIn === $scope.group.users[userID].timecards[timeslotID].timeOut) {
                if ($scope.group.users[userID].timecards[timeslotID].timeOut !== '') {
                    $scope.group.users[userID].timecards[timeslotID].timeOut = '';
                    $scope.group.users[userID].timecards[timeslotID].hours = '';

                    toastr["error"]("Invalid Time Input");
                }
            }

            $scope.group.users[userID].timecards[timeslotID].groupID = $routeParams.ID;


            $http.post("/Time/SaveTime", $scope.group.users[userID].timecards[timeslotID])
                .then(function (response) {
                    if ($scope.group.users[userID].timecards[timeslotID].timeIn === '' || $scope.group.users[userID].timecards[timeslotID].timeOut === '') {
                        $scope.group.users[userID].timecards[timeslotID].hours = '';
                    }
                    else {
                        //  math to get proper display of hours hh:mm:ss
                        var startTime = moment($scope.group.users[userID].timecards[timeslotID].timeIn);
                        var endTime = moment($scope.group.users[userID].timecards[timeslotID].timeOut);
                        var duration = moment.duration(endTime.diff(startTime));
                        var hour = Math.floor(duration.asHours());
                        var minutes = Math.floor(duration.asMinutes() - (hour * 60));
                        var seconds = Math.floor(duration.asSeconds() - (hour * 3600) - (minutes * 60));
                        var FormatedString = "00:00:00"
                        //hour + ':' + minutes + ':' + seconds;
                        //if hour is between 0 and 9 stick a zero in front 
                        //if minute is between 0 and 9 stick a zero in front 
                        //if seconds is between 0 and 9 stick a zero in front 

                        $scope.group.users[user.userID].timecards[time.timeslotID].hours = "00:00:00";

                        if (hour > 9) {
                            FormatedString = hour + ":";
                        }
                        else {
                            FormatedString = "0" + hour + ":";
                        }
                        if (minutes > 9) {
                            FormatedString += minutes + ":";
                        }
                        else {
                            FormatedString += "0" + minutes + ":";
                        }
                        if (seconds > 9) {
                            FormatedString += seconds;
                        }
                        else {
                            FormatedString += "0" + seconds;
                        }


                        $scope.group.users[user.userID].timecards[time.timeslotID].hours = FormatedString;

                        $scope.updateChart();


                        // $scope.group.users[userID].timecards[timeslotID].hours = moment.duration(
                        //     moment($scope.group.users[userID].timecards[timeslotID].timeOut).diff(
                        //         $scope.group.users[userID].timecards[timeslotID].timeIn)).asHours().toFixed(2);
                    }
                    // $scope.updateAllHours(); 
                    // $scope.updateChart();

                    toastr["info"]("Timeslot Updated.");
                }, function (response) {
                    if (response.status === 401) toastr["error"]("Unauthorized to edit this time entry.");
                    else if (response.status === 400) toastr["error"]("Failed to save time entry due to : negative time or " +
                        "clock out in the future.");
                    else toastr["error"](response.status.toString() + "Failed to save time entry, unknown error.");
                });

        };

        //this isnt used?
        // $scope.diffHours = function (timeIn, timeOut) {
        //     if (timeIn === '' || timeOut === '') return "0:0:0";
        //     return moment.duration(moment(timeOut).diff(timeIn)).asHours().toFixed(2);
        // };

        //Used to check whether the currently logged in user is trying to change their own time, or is an instructor
        $scope.isUser = function (id) {
            if (!$scope.$parent.user) return false;
            return (id === $scope.$parent.user.userID || $scope.$parent.user.type === 'A');
        };



        $scope.updateAllHours = function () {
            $.each($scope.group.users, function (index, user) {
                $.each(user.timecards, function (index, time) {
                    if (time.timeIn !== '' && time.timeOut !== '') {

                        var startTime = moment($scope.group.users[user.userID].timecards[time.timeslotID].timeIn);
                        var endTime = moment($scope.group.users[user.userID].timecards[time.timeslotID].timeOut);
                        var duration = moment.duration(endTime.diff(startTime));
                        var hour = Math.floor(duration.asHours());
                        var minutes = Math.floor(duration.asMinutes() - (hour * 60));
                        var seconds = Math.floor(duration.asSeconds() - (hour * 3600) - (minutes * 60));


                        var FormatedString = "00:00:00"
                        //hour + ':' + minutes + ':' + seconds;
                        //if hour is between 0 and 9 stick a zero in front 
                        //if minute is between 0 and 9 stick a zero in front 
                        //if seconds is between 0 and 9 stick a zero in front 

                        $scope.group.users[user.userID].timecards[time.timeslotID].hours = "00:00:00";

                        if (hour > 9) {
                            FormatedString = hour + ":";
                        }
                        else {
                            FormatedString = "0" + hour + ":";
                        }
                        if (minutes > 9) {
                            FormatedString += minutes + ":";
                        }
                        else {
                            FormatedString += "0" + minutes + ":";
                        }
                        if (seconds > 9) {
                            FormatedString += seconds;
                        }
                        else {
                            FormatedString += "0" + seconds;
                        }


                        $scope.group.users[user.userID].timecards[time.timeslotID].hours = FormatedString;

                        $scope.updateChart();

                        // $scope.group.users[user.userID].timecards[time.timeslotID].hours = moment.duration( 
                        //     moment($scope.group.users[user.userID].timecards[time.timeslotID].timeOut).diff( 
                        //         $scope.group.users[user.userID].timecards[time.timeslotID].timeIn)).asHours().toFixed(2);
                    }
                });
            });
        };

        $scope.completeEval = function () {
            $location.path('/eval/' + $scope.groupID);
        };

        $scope.viewEvals = function () {
            $location.path('/viewEvals/' + $scope.groupID);
        };

        var data = { //Data and labels are set in the setData function
            datasets: [{
                data: [],
                backgroundColor: [ //After 30 groups are included in a project, the color will just be gray, add more colors here if there can be more than 30 groups
                    '#2C3E50', '#3498DB', '#18BC9C', '#F39C12', '#e83e8c', '#6610f2', '#fd7e14', '#E74C3C', '#6f42c1', '#95a5a6',
                    '#2C3E50', '#3498DB', '#18BC9C', '#F39C12', '#e83e8c', '#6610f2', '#fd7e14', '#E74C3C', '#6f42c1', '#95a5a6',
                    '#2C3E50', '#3498DB', '#18BC9C', '#F39C12', '#e83e8c', '#6610f2', '#fd7e14', '#E74C3C', '#6f42c1', '#95a5a6'
                ]
            }],

            // These labels appear in the legend and in the tooltips when hovering different arcs
            labels: []

        };

        $scope.setData = function () {
            data.datasets[0].data = [];//set data to empty array 
            data.labels = []; //empty out labels 
            for (var u in $scope.group.users) { //loop through each user
                u = $scope.group.users[u];
                var hours = 0;
                for (var t in u.timecards) {//go through eacah time card
                    t = u.timecards[t];
                    //hours += Number(t.hours);//add on each hours for each user



                    var time = t.hours.split(":"); //[0] is hours, [1] is min 
                    if (time.length == 3) {
                        var addedHours = Number(time[0]) * 60; //grab hours and make it minutes
                        var addedMinutes = Number(time[1]); //get minutes
                        var addedSeconds = Number(time[2]); //get seconds 
                        var totalTime = addedHours + addedMinutes + addedSeconds;

                        hours += totalTime;

                    }
                    else {
                        var addedHours = Number(time[0]) * 60; //grab hours and make it minutes
                        var addedMinutes = Number(time[1]); //get minutes
                        var totalTime = addedHours + addedMinutes + addedSeconds;
                        hours += totalTime;
                    }


                }
                data.datasets[0].data.push(hours);
                data.labels.push(u.firstName + " " + u.lastName);
            }
        };

        $scope.setData();

        var ctx = $("#groupHours");

        var myChart = new Chart(ctx, {
            type: 'pie',
            data: data
        });

        $scope.updateChart = function () {
            //  The section below creates a display for a empty chart.
            var hours = 0;
            for (var u in $scope.group.users) {
                u = $scope.group.users[u];
                for (var t in u.timecards) {
                    t = u.timecards[t];

                    var time = t.hours.split(":"); //[0] is hours, [1] is min 

                    if (time.length == 3) {
                        var addedHours = Number(time[0]) * 60; //grab hours and make it minutes
                        var addedMinutes = Number(time[1]); //get minutes
                        var addedSeconds = Number(time[2]); //get seconds 
                        var totalTime = addedHours + addedMinutes + addedSeconds;

                        hours += totalTime;

                    }
                    else {
                        var addedHours = Number(time[0]) * 60; //grab hours and make it minutes
                        var addedMinutes = Number(time[1]); //get minutes
                        var totalTime = addedHours + addedMinutes + addedSeconds;
                        hours += totalTime;
                    }
                }
            }
            if (hours === 0) {
                document.getElementById("groupHours").style.visibility = "hidden";
                document.getElementById("noData").style.visibility = "visible";
            }
            else {
                document.getElementById("groupHours").style.visibility = "visible";
                document.getElementById("noData").style.visibility = "hidden";
            }

            $scope.setData();
            myChart.update();
        };

        $scope.updateAllHours();
        $scope.updateChart();

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