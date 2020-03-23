myApp.controller('LoginController', ['$scope', 'AccountService', 'storage', '$http', 'ipCookie', function ($scope, AccountService, storage, $http, ipCookie) {
    $scope.APIBASEURL = API_BASE_URL;
    $scope.UserName = "";
    $scope.Password = "";
    $scope.user = {};
    $scope.expires = 14;
    $scope.expirationUnit = 'days';

    $scope.checkCookie = function () {
        var user = ipCookie('LoginUser');
        if (user !== null && user !== undefined) {
            var userName = user.UserName;
            var password = user.Password;

            if (user.Role == "Admin")
                AccountService.ValidateAdmin(userName, password, $scope);
            else
                AccountService.StudioLogin(userName, password, $scope);
        }
    }

    $scope.saveCookie = function (cookie) {
        ipCookie('LoginUser', cookie, { expires: $scope.expires, expirationUnit: $scope.expirationUnit, path: '/' });
    }

    $scope.LoginAdmin = function () {
        showLoader();
        AccountService.ValidateAdmin($scope.UserName, $scope.Password, $scope);
    }

    $scope.LoginStudio = function () {
        showLoader();
        AccountService.StudioLogin($scope.UserName, $scope.Password, $scope);
    }

    $scope.LoginUser = function () {
        showLoader();
        AccountService.ValidateUser($scope.UserName, $scope.Password, $scope);
    }

    $scope.checkCookie();

}]);