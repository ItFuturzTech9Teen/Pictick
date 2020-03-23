myApp.controller('OrganizationLinkController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.AlbumId = 0;
    $scope.DataList = [];
    $scope.StudioList = [];
    $scope.valueData = false;
    $scope.showStudio = false;

    var LoginUser = storage.get(Login_User);
    if (LoginUser != null && LoginUser != undefined) {
        if (LoginUser.Role == "Studio")
            $scope.StudioId = LoginUser.StudioId;
        else
            $scope.showStudio = true;
    }

    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetStudioSocialLinkList($scope.StudioId);
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
        HomeService.SaveStudioSocialLink($scope);
        $('#myModal').modal('hide');
    }

    $scope.EditData = function (data) {
        $('#myModal').modal('toggle');
        $scope.Id = data.Id;
        $scope.Title = data.Title;
        $scope.Link = data.Link;
        $scope.Image = data.Image;
    }


    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteStudioSocialLink(id, $scope);
        }
    }


    //$scope.Clear = function () {
    //    $scope.Id = 0;
    //    $scope.StateId = 0;
    //    $scope.CityId = 0;
    //    $scope.Name = "";
    //    $scope.Mobile = "";
    //    $scope.Email = "";
    //    $scope.UserName = "";
    //    $scope.About = "";
    //    $scope.Website = "";
    //    $scope.StudioOwner = "";
    //    $scope.PinCode = "";
    //    $scope.Services = "";
    //    $scope.Address = "";
    //}
    //$scope.Clear();

}]);