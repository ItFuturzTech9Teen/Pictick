myApp.controller('OrganizationController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.AlbumId = 0;
    $scope.DataList = [];
    $scope.StudioList = [];
    $scope.CityList = [];
    $scope.FontList = [];
    $scope.AlbumList = [];
    $scope.StateList = [];
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

    $scope.GetFontList = function () {
        showLoader();
        var dataList = HomeService.GetFontList();
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.FontList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure ");
                hideLoader();
            }
        });
    }
    $scope.GetFontList();


    $scope.GetStudioDataById = function () {
        showLoader();
        var dataList = HomeService.GetStudioDataById($scope.StudioId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.Name = data.Data.Name;
                    $scope.Mobile = data.Data.Mobile;
                    $scope.Email = data.Data.Email;
                    $scope.UserName = data.Data.UserName;
                    $scope.Password = data.Data.Password;
                    $scope.About = data.Data.About;
                    $scope.Website = data.Data.Website;
                    $scope.StudioOwner = data.Data.StudioOwner;
                    $scope.StateId = data.Data.StateId;
                    $scope.CityId = data.Data.CityId;
                    $scope.PinCode = data.Data.PinCode;
                    $scope.Services = data.Data.Services;
                    $scope.Address = data.Data.Address;
                    $scope.WaterMark = data.Data.WaterMark;
                    $scope.Position = data.Data.Position;
                    $scope.FontName = data.Data.Font;
                    $scope.FontStyle = data.Data.FontStyle;
                    $scope.Opacity = data.Data.Opacity;
                    $scope.FontSize = data.Data.FontSize;
                    $scope.WaterMarkType = data.Data.WaterMarkType;
                    $scope.WaterMarkImage = data.Data.WaterMarkImage;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });

    }

    $scope.GetStudioDataById();

    $scope.GetStateList = function () {
        showLoader();
        var dataList = HomeService.GetStateList();
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.StateList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure ");
                hideLoader();
            }
        });
    }
    $scope.GetStateList();

    $scope.GetCityList = function () {
        showLoader();
        var dataList = HomeService.GetCityList($scope.StateId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.CityList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure ");
                hideLoader();
            }
        });
    }
    $scope.GetCityList();

    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetStudioList();
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

    $scope.SaveData = function () {
        HomeService.UpdatestudioData($scope);
    }

    $scope.UpdateStudioWaterMarkData = function () {
        HomeService.UpdateStudioWaterMarkData($scope);
    }

    //$scope.DeleteData = function (id) {
    //    var result = confirm("Are you sure you want to delete this ?");
    //    if (result) {
    //        HomeService.DeleteCustomer(id, $scope);
    //    }
    //}


    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.StateId = 0;
        $scope.CityId = 0;
        $scope.Name = "";
        $scope.Mobile = "";
        $scope.Email = "";
        $scope.UserName = "";
        $scope.About = "";
        $scope.Website = "";
        $scope.StudioOwner = "";
        $scope.PinCode = "";
        $scope.Services = "";
        $scope.Address = "";
        $scope.InviteMessage = "";
    }
    $scope.Clear();

}]);