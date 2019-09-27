var app = angular
    .module('myApp', [])
    .controller("myCtrl", ["$scope", "userService", function ($scope, userService) {
        Initilize();

        function Initilize() {
            $scope.loginStatus = "-1";
            $scope.isAuthenticatd = "0";
            $scope.StatusMsg = "Please Login to enter the application";
            $scope.loginStatusClass = "status-defult";
            $scope.action = "Save";
            $scope.userID = 0;
            $scope.user = {};
            $scope.users = [];
            $scope.Departments = [];
            LoadDepartments();
        }

        $scope.register = function () {
            var register = [{
                "ID": ($scope.action == "Save") ? -1 : $scope.user.ID,
                "Name": $scope.user.Name,
                "Password": $scope.user.Password,
                "Email": $scope.user.Email,
                "Department": $scope.user.sDepartment,
                "action": $scope.action
            }];

            var param = {
                "spName": "sp_createuser",
                "parameters": register
            };

            userService.post(param).then(function (response) {
                LoadAllUsers();
            }, function (error) {
            });
        }

        $scope.login = function () {

            var login = {
                "spName": "sp_validateuser",
                "parameters": [{
                    "UserName": $scope.username,
                    "Password": $scope.password
                }]
            };
            
            userService.validate(login).success(
                function (response) {
                    $scope.isAuthenticatd = response;
                    if (response == "1") {
                        $scope.loginStatus = true;
                        $scope.StatusMsg = "Login Success";
                        $scope.loginStatusClass = "status-success";
                        LoadAllUsers();
                    }
                    if (response == "0") {
                        $scope.loginStatus = true;
                        $scope.StatusMsg = "Login Failed";
                        $scope.loginStatusClass = "status-failure";
                    }
                },
                function (response) {

                });
        };

        $scope.logout = function ()
        {
            userService.logout().then(
                function (response)
                {
                    $scope.user = {};
                    $scope.isAuthenticatd = response;
                    $scope.StatusMsg = "Please Login to enter the application";
                    $scope.loginStatusClass = "status-defult";
                }
            );
        }

        $scope.multipleUser = function () {

            var users = [
                {
                    "ID": ($scope.action == "Save") ? -1 : $scope.user.ID,
                    "Name": "Aswanth",
                    "Password": "Aswanth",
                    "Email": "Aswanth@email.com",
                    "Department": "2",
                    "action": "Save"
                },
                {
                    "ID": ($scope.action == "Save") ? -1 : $scope.user.ID,
                    "Name": "Aiswarya",
                    "Password": "Aiswarya",
                    "Email": "Aiswarya@mail.com",
                    "Department": "3",
                    "action": "Save"
                }
            ];
            var data = {
                "spName": "sp_createuser",
                "parameters": users
            };

            userService.post(data).then(
                function (response) {
                    LoadAllUsers();
                },
                function (error) {

                }
            );
        };

        function LoadDepartments() {
            var objParam = {
                "spName": "sp_department",
                "parameters": []
            };
            userService.get(objParam).then(function (response) {
                $scope.Departments = JSON.parse(JSON.stringify(response.data.Table));
            });
        }

        function LoadAllUsers() {
            param = {
                "spName": "sp_alluser",
                "parameters": null
            };
            userService.get(param).then(function (response) {
                $scope.users = JSON.parse(JSON.stringify(response.data.Table));
            });
        }
    }]);