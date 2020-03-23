myApp.controller('GalleryController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.DataList = [];
    $scope.StudioList = [];
    $scope.valueData = false;
    $scope.showStudio = false;

    var LoginUser = storage.get(Login_User);
    if (LoginUser != null && LoginUser != undefined) {
        if (LoginUser.Role === "Studio")
            $scope.StudioId = LoginUser.StudioId;
        else
            $scope.showStudio = true;
    }

    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetGalleryList($scope.StudioId);
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

    $scope.SaveData = function () {
        HomeService.SaveGallery($scope);
        $('#myModal').modal('hide');
    }

    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteGallery(id, $scope);
        }
    }

    $scope.EditData = function (data) {
        $('#myModal').modal('toggle');
        $scope.Id = data.Id;
        $scope.StudioId = data.StudioId;
        $scope.Title = data.Title;
        $scope.GalleryPin = data.GalleryPin;
        $scope.SelectionPin = data.SelectionPin;
        $scope.AllowDownload = data.AllowDownload;
        $scope.WatterMark = data.WatterMark;
    }

    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.Title = "";
        $scope.GalleryPin = "";
        $scope.SelectionPin = "";
        $scope.AllowDownload = false;
        $scope.WatterMark = "None";
        $scope.myFile = null;
    }
    $scope.Clear();

}]);