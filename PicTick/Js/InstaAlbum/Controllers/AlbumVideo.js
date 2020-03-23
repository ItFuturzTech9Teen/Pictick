myApp.controller('AlbumVideoController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.AlbumId = 0;
    $scope.DataList = [];
    $scope.StudioList = [];
    $scope.AlbumList = [];

    $scope.valueData = false;
    $scope.showStudio = false;

    var LoginUser = storage.get(Login_User);
    if (LoginUser != null && LoginUser != undefined) {
        if (LoginUser.Role == "Studio")
            $scope.StudioId = LoginUser.StudioId;
        else
            $scope.showStudio = true;
    }

    $scope.GetAlbumList = function () {
        showLoader();
        var dataList = HomeService.GetAlbumList($scope.StudioId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.AlbumList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }
    if ($scope.StudioId > 0)
        $scope.GetAlbumList();

    $scope.GetStudioList = function () {
        showLoader();
        var dataList = HomeService.GetStudioList();
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.StudioList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }
    $scope.GetStudioList();

    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetAlbumVideoList($scope.AlbumId);
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

    $scope.SaveData = function () {
        HomeService.SaveAlbumVideoData($scope);
    }

    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteAlbumVideo(id, $scope);
        }
    }

    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.Image = "";
        $scope.Title = "";
    }
    $scope.Clear();

}]);