myApp.controller('DashBoardController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.StudioId = 0;
    $scope.DataList = [];
    $scope.DashboardCountList = [];
    $scope.StudioCount = 0;
    $scope.CustomerCount = 0;
    $scope.EventCount = 0;
    $scope.PortfolioCount = 0;
    $scope.StudioPer = "";
    $scope.CustomerPer = "";
    $scope.EventPer = "";
    $scope.PortfolioPer = "";
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

    $scope.GetDashboardCount = function () {
        showLoader();
        var dataList = HomeService.GetDashboardCount($scope.StudioId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.DashboardCountList = data.Data;
                    $scope.StudioCount = $scope.DashboardCountList[0].StudioCount;
                    $scope.CustomerCount = $scope.DashboardCountList[0].CustomerCount;
                    $scope.EventCount = $scope.DashboardCountList[0].EventCount;
                    $scope.PortfolioCount = $scope.DashboardCountList[0].PortfolioCount;
                    $scope.StudioPer = $scope.DashboardCountList[0].StudioPer;
                    $scope.CustomerPer = $scope.DashboardCountList[0].CustomerPer;
                    $scope.EventPer = $scope.DashboardCountList[0].EventPer;
                    $scope.PortfolioPer = $scope.DashboardCountList[0].PortfolioPer;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }
    $scope.GetDashboardCount();

}]);