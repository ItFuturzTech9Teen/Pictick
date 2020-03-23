myApp.controller('StateController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
 
    $scope.Id = 0;
    //$scope.SocietyId = 0;
    $scope.DataList = [];


    //var LoginUser = storage.get(Login_User);
    //if (LoginUser != null && LoginUser != undefined) {
    //    if (LoginUser.Role == "Company")
    //        $scope.SocietyId = LoginUser.Id;
    //}

    $scope.GetData = function () {
        showLoader();
        var dataList = HomeService.GetStateData();
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
        HomeService.SaveStateData($scope);
    }

    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteState(id, $scope);
        }
    }

    $scope.EditData = function (data) {
        $scope.Id = data.Id;
        $scope.Name = data.Name;

    }

    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.Name = "";

    }
    $scope.Clear();

}]);