myApp.controller('HomeController', ['$scope', 'HomeService', 'AccountService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, AccountService, storage, $http, ipCookie) {

    var d = new Date();
    var n = d.getTime();  //n in ms
    $scope.APIBASEURL = API_BASE_URL;
    $scope.LoginId = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.IsAdmin = false;
    $scope.showStudio = false;
    $scope.expires = 14;
    $scope.expirationUnit = 'days';

    var s = "/Account/GetLoginSession";
    var promiseGet = $http.get(s);
    promiseGet.then(function (pl) {
        if (pl.data.Data === null) {
            storage.remove(Login_User);
            $http({
                url: '/Account/DestoryLoginSession',
                method: "POST",
                data: {},
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status, headers, config) {
                window.location.href = '/AdminLogin';
            })
        }
        else {
            $scope.LoginUser = storage.get(Login_User);
            if ($scope.LoginUser != null && $scope.LoginUser != undefined) {
                $scope.LoginId = $scope.LoginUser.Id;
            }
        }
    });

    var LoginUser = storage.get(Login_User);
    if (LoginUser != null && LoginUser != undefined) {
        if (LoginUser.Role == "Studio")
            $scope.StudioId = LoginUser.StudioId;
        else {
            $scope.showStudio = true;
            $scope.IsAdmin = true;
        }
    }

    var watermarkData = storage.get(WatermarkDetail);
    if ((watermarkData == undefined || watermarkData == null) && $scope.StudioId > 0) {
        showLoader();
        var dataList = HomeService.GetStudioDataById($scope.StudioId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    var waterMarkData = {
                        "WaterMark": data.Data.WaterMark,
                        "Position": data.Data.Position,
                        "FontName": data.Data.Font,
                        "FontStyle": data.Data.FontStyle,
                        "Opacity": data.Data.Opacity,
                        "FontSize": data.Data.FontSize,
                        "WaterMarkType": data.Data.WaterMarkType,
                        "WaterMarkImage": data.Data.WaterMarkImage
                    }
                    storage.set(WatermarkDetail, waterMarkData);
                    ipCookie(WatermarkDetail, waterMarkData, { expires: $scope.expires, expirationUnit: $scope.expirationUnit, path: '/' });
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }

    $scope.logOutUser = function () {
        ipCookie.remove('LoginUser', { path: '/' });
        storage.remove(Login_User);
        AccountService.AutoLogOut(LoginUser.Role);
    }

}]);