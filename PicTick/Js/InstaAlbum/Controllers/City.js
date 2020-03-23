myApp.controller('CityController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
   
    $scope.Id = 0;
    $scope.StateId = 0;
    $scope.DataList = [];
    $scope.StateList = [];


    //var LoginUser = storage.get(Login_User);
    //if (LoginUser != null && LoginUser != undefined) {
    //    if (LoginUser.Role == "Company")
    //        $scope.SocietyId = LoginUser.Id;
    //}

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


    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetCityData($scope.StateId);
        dataList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.DataList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure ");
                hideLoader();
            }
        });
    }
    $scope.GetData();

    $scope.SaveData = function () {
        HomeService.SaveCityData($scope);
    }

    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteCity(id, $scope);
        }
    }

    $scope.EditData = function (data) {
        $scope.Id = data.Id;
        $scope.Name = data.Name;
        $scope.StateId = data.StateId;
    }

    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.Name = "";
        $scope.StateId = 0;

    }
    $scope.Clear();


}]);