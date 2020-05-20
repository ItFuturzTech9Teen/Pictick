myApp.controller('CustomerController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.AlbumId = 0;
    $scope.DataList = [];
    $scope.StudioList = [];
    $scope.AlbumList = [];
    $scope.CustomerGalleryList = [];
    $scope.SelectedCustomer = {};
    $scope.valueData = false;
    $scope.showStudio = false;

    var LoginUser = storage.get(Login_User);
    if (LoginUser != null && LoginUser != undefined) {
        if (LoginUser.Role == "Studio")
            $scope.StudioId = LoginUser.StudioId;
        else
            $scope.showStudio = true;
    }

    //$scope.GetAlbumList = function () {
    //    showLoader();
    //    var dataList = HomeService.GetAlbumList($scope.StudioId);
    //    dataList.then(function (pl) {
    //        var data = pl.data;
    //        if (data.IsSuccess === true) {
    //            if (data.Data !== null && data.Data !== undefined) {
    //                $scope.AlbumList = data.Data;
    //                hideLoader();
    //            }
    //        }
    //        else {
    //            showErrorMessage("Some Error Occure");
    //            hideLoader();
    //        }
    //    });
    //}
    //if ($scope.StudioId > 0)
    //    $scope.GetAlbumList();

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
        var dataList = HomeService.GetCustomerList($scope.StudioId);
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

    $scope.SetCustomerGallery = function (gallery) {
        var CustomerId = $scope.SelectedCustomer.Id;
        var GalleryId = gallery.Id;

        HomeService.SaveCustomerGalleryData($scope, CustomerId, GalleryId);
    }

    $scope.GetCustomerGalleryList = function (data) {
        $scope.CustomerGalleryList = [];
        showLoader();
        $scope.SelectedCustomer = data;
        var dataList = HomeService.GetCustomerGalleryList(data.Id, data.StudioId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.CustomerGalleryList = data.Data;
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
        HomeService.SaveCustomerData($scope);
        $('#myModal').modal('hide');
    }

    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteCustomer(id, $scope);
        }
    }

    $scope.EditData = function (data) {
        $('#myModal').modal('toggle');
        $scope.Id = data.Id;
        $scope.Name = data.Name;
        $scope.Mobile = data.Mobile;
        $scope.Email = data.Email;
        $scope.AlbumId = data.AlbumId;
        $scope.UserName = data.UserName;
        $scope.Password = data.Password;
    }

    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.AlbumId = 0;
        $scope.Name = "";
        $scope.Mobile = "";
        $scope.Email = "";
        $scope.UserName = "";
        $scope.Password = "";
    }
    $scope.Clear();

}]);