myApp.controller('ClientGalleryController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.GalleryId = 0;
    $scope.AlbumId = 0;
    $scope.GalleryData = {};
    $scope.AlbumPhotoList = [];
    $scope.SelectedList = [];
    $scope.DataList = [];
    $scope.StudioList = [];
    $scope.valueData = false;
    $scope.showStudio = false;


    $scope.Paramiters = function () {
        var url = window.location.href;
        var locationArray = url.split('/');
        if (locationArray.length > 4) {
            var id = locationArray[locationArray.length - 1];
            $scope.GalleryId = id;
        }
    }
    $scope.Paramiters();

    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetGalleryById($scope.GalleryId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.GalleryData = data.Data;
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

    $scope.GetAlbumData = function () {
        showLoader();
        var dataList = HomeService.GetAlbumList($scope.GalleryId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.DataList = data.Data;
                    if ($scope.DataList.length > 0) {
                        $scope.AlbumId = $scope.DataList[0].Id;
                        $scope.GetAlbumPhotoList();
                    }
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }
    $scope.GetAlbumData();

    $scope.GetAlbumPhotoList = function () {
        showLoader();
        var dataList = HomeService.GetAlbumPhotoList($scope.AlbumId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.AlbumPhotoList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }

    $scope.SelectPhoto = function (data) {
        showLoader();
        var flag = false;
      
        angular.forEach($scope.SelectedList, function (value, key) {
            if (value.Id == data.Id) {
                $scope.SelectedList[key].IsSelected = !$scope.SelectedList[key].IsSelected;
                flag = true;
            }
        });

        if (flag == false) {
            data.IsSelected = !data.IsSelected;
            var SelectedData = { "Id": data.Id, "IsSelected": data.IsSelected };
            $scope.SelectedList.push(SelectedData);

        }
    }

    $scope.UpdatePhotoSelection = function () {
        HomeService.UpdateAlbumSelection($scope);
    }

    $scope.SetClass = function (data, index) {
        var myEl = angular.element(document.querySelector('#parent' + index));
       
        if (data.IsSelected == true) {
            $scope.SelectPhoto(data);
            myEl.addClass('approved');
            //this_link.find('i').removeClass().addClass('fa fa-minus-circle');
        }
        else {
            //item_container.addClass('unviewed');
            myEl.addClass('unviewed');
            //$scope.SelectPhoto(data);
            //this_link.find('i').removeClass().addClass('fa fa-check-circle');
        }
    }

}]);