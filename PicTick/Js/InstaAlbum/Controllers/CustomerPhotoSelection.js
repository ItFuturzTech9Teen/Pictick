myApp.controller('CustomerPhotoSelectionController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
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
        $scope.DataList = []; 
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
        var dataList = HomeService.GetSelectedAlbumPhotoList($scope.AlbumId);
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

    $scope.ExportFile = function () {
        if ($scope.DataList.length > 0) {
            var filename = "";
            for (j = 0; j < $scope.AlbumList.length; j++) {
                if ($scope.AlbumList[j].Id == $scope.AlbumId) {
                    filename = $scope.AlbumList[j].Name;
                }
            }


            var textToSend = "";
            for (var i = 0; i < $scope.DataList.length; i++) {
                var url = $scope.DataList[i].Photo;
                var arrayListData = url.split('/');
                var img = arrayListData[arrayListData.length - 1]
                if (i == 0)
                    textToSend += img;
                else
                    textToSend += "\r\n" + img;
            }

            textToSend = encodeURIComponent(textToSend);

            var element = document.createElement('a');
            element.setAttribute('href', 'data:text/plain;charset=utf-8,' + textToSend);
            element.setAttribute('download', filename);

            element.style.display = 'none';
            document.body.appendChild(element);

            element.click();

            document.body.removeChild(element);
        }
    }
}]);