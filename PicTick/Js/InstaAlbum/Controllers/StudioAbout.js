myApp.controller('StudioAboutController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.DataList = [];
    $scope.valueData = false;


    var LoginUser = storage.get(Login_User);
    if (LoginUser != null && LoginUser != undefined) {
        if (LoginUser.Role == "Studio")
            $scope.StudioId = LoginUser.StudioId;
        else
            $scope.showStudio = true;
    }

    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetStudioAboutList($scope.StudioId);
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
        HomeService.SaveStudioAboutData($scope);
    }

    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteStudioAbout(id, $scope);
        }
    }

    $scope.EditData = function (data) {
        $scope.Id = data.Id;
        $scope.Title = data.Title;
        $scope.Description = data.Description;
    }

    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.Title = "";
        $scope.Description = "";
    }
    $scope.Clear();

}]);