app.factory("userService", ["$http", UserService]);
function UserService($http) {
    return {
        get: getQuery,
        post: postQuery,
        validate: validateUser,
        logout: logoutUser
    };

    function getQuery(data) {
        return $http.get("api/Data/Get", { params: data });
    }

    function postQuery(data) {
        return $http.post("api/Data/Post", data);
    }

    function validateUser(data) {
        return $http.post("api/Account/validate", data);
    }

    function logoutUser()
    {
        return $http.post("api/Account/logout");
    }
}