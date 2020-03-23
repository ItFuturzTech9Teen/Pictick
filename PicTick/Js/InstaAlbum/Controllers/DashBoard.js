myApp.controller('DashBoardController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.StudioId = 0;
    $scope.DataList = [];
    $scope.IsAdmin = false;

    var LoginUser = storage.get(Login_User);
    if (LoginUser != null && LoginUser != undefined) {
        if (LoginUser.Role == "Studio")
            $scope.StudioId = LoginUser.Id;
        else
            $scope.IsAdmin = true;
    }

    //$scope.GetData = function () {
    //    showLoader();
    //    var dataList = HomeService.GetDashboardAlbumList($scope.StudioId);
    //    dataList.then(function (pl) {
    //        var data = pl.data;
    //        if (data.IsSuccess === true) {
    //            if (data.Data !== null && data.Data !== undefined) {
    //                $scope.DataList = data.Data;
    //                hideLoader();
    //            }
    //        }
    //        else {
    //            showErrorMessage("Some Error Occure");
    //            hideLoader();
    //        }
    //    });
    //}
    //$scope.GetData();

    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetDashboardGalleryList($scope.StudioId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.DataList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }
    $scope.GetData();

}]);