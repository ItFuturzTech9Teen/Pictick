myApp.service('AccountService', ['$http', 'storage', 'ipCookie', function ($http, storage, ipCookie) {

    $http.defaults.headers.post["Content-Type"] = 'application/json';
    var expires = 14;
    var expirationUnit = 'days';

    var _selfService = this;

    //to get proper datetime in all system
    this.FormatDateTime = function (date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
    }

    //Session & Login
    this.SaveSession = function (sessionObject) {
        $http({
            url: '/Account/SetLoginSession',
            method: "POST",
            data: sessionObject,
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data, status, headers, config) {
            storage.set(Login_User, sessionObject);
            window.location.href = '/Dashboard';
        })
    }

    this.ValidateAdmin = function (userName, password, scope) {
        var Obj = { "UserName": userName, "Password": password, "Role": "Admin" }

        $http({
            url: API_URL + '/AccountAPI/ValidateAdmin?userName=' + userName + '&password=' + password,
            method: "GET"
        }).success(function (data, status, headers, config) {
            if (data.IsSuccess == true) {
                if (data.Data !== null && data.Data !== undefined && data.Data !== 0) {
                    scope.saveCookie(Obj);
                    var sessionObject = JSON.parse(JSON.stringify(data.Data));
                    _selfService.SaveSession(sessionObject);
                }
                else {
                    showWarningMessage("Invalid UserName Or Password");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }

    this.StudioLogin = function (userName, password, scope) {
        var Obj = { "UserName": userName, "Password": password, "Role": "Studio" }

        $http({
            url: API_URL + '/AccountAPI/StudioLogin?userName=' + userName + '&password=' + password,
            method: "GET"
        }).success(function (data, status, headers, config) {
            if (data.IsSuccess == true) {
                if (data.Data !== null && data.Data !== undefined && data.Data !== 0) {
                    scope.saveCookie(Obj);
                    var sessionObject = JSON.parse(JSON.stringify(data.Data));
                    _selfService.SaveSession(sessionObject);
                }
                else {
                    showWarningMessage("Invalid UserName Or Password");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }

    this.AutoLogOut = function (role) {
        $http({
            url: '/Account/DestoryLoginSession',
            method: "POST",
            data: {},
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data, status, headers, config) {
            storage.remove(Login_User);
            if (role == "Studio")
                window.location.href = '/StudioLogin';
            else
                window.location.href = '/AdminLogin';
        })
        return "";
    }
}]);