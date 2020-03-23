myApp.controller('AlbumPhotoController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.AlbumId = 11;
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
        var dataList = HomeService.GetAlbumPhotoList($scope.AlbumId);
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
        HomeService.SaveAlbumPhotoData($scope);
    }

    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteAlbumPhoto(id, $scope);
        }
    }

    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.Image = "";
    }
    $scope.Clear();



    $(document).ready(function () {
        var dropbox;

        var oprand = {
            dragClass: "active",
            on: {
                load: function (e, file) {
                    // check file type
                    var imageType = /image.*/;
                    if (!file.type.match(imageType)) {
                        alert("File \"" + file.name + "\" is not a valid image file");
                        return false;
                    }

                    // check file size
                    if (parseInt(file.size / 1024) > 2050) {
                        alert("File \"" + file.name + "\" is too big.Max allowed size is 2 MB.");
                        return false;
                    }

                    create_box(e, file);
                },
            }
        };

        FileReaderJS.setupDrop(document.getElementById('dropbox'), oprand);

    });

    create_box = function (e, file) {
        var rand = Math.floor((Math.random() * 100000) + 3);
        var imgName = file.name; // not used, Irand just in case if user wanrand to print it.
        var src = e.target.result;

        var template = '<div class="eachImage" id="' + rand + '">';
        template += '<span class="preview" id="' + rand + '"><img src="' + src + '"><span class="overlay"><span class="updone"></span></span>';
        template += '</span>';
        template += '<div class="progress" id="' + rand + '"><span></span></div>';

        if ($("#dropbox .eachImage").html() == null)
            $("#dropbox").html(template);
        else
            $("#dropbox").append(template);

        // upload image
        upload(file, rand);
    }

    upload = function (file, rand) {
        // now upload the file
        var xhr = new Array();
        xhr[rand] = new XMLHttpRequest();
        var ProdImgData = new FormData();
        ProdImgData.append('Image', file);
        ProdImgData.append('AlbumId', $scope.AlbumId);

        xhr[rand].open("post", API_URL + '/HomeAPI/SaveAlbumPhoto', true);

        xhr[rand].upload.addEventListener("progress", function (event) {
            console.log(event);
            if (event.lengthComputable) {
                $(".progress[id='" + rand + "'] span").css("width", (event.loaded / event.total) * 100 + "%");
                $(".preview[id='" + rand + "'] .updone").html(((event.loaded / event.total) * 100).toFixed(2) + "%");
            }
            else {
                alert("Failed to compute file upload length");
            }
        }, false);

        xhr[rand].onreadystatechange = function (oEvent) {
            if (xhr[rand].readyState === 4) {
                if (xhr[rand].status === 200) {
                    $(".progress[id='" + rand + "'] span").css("width", "100%");
                    $(".preview[id='" + rand + "']").find(".updone").html("100%");
                    $(".preview[id='" + rand + "'] .overlay").css("display", "none");
                } else {
                    alert("Error : Unexpected error while uploading file");
                }
            }
        };

        //// Set headers
        //xhr[rand].setRequestHeader("Content-Type", "multipart/form-data");
        //xhr[rand].setRequestHeader("X-File-Name", file.name);
        //xhr[rand].setRequestHeader("X-File-Size", file.size);
        //xhr[rand].setRequestHeader("X-File-Type", file.type);

        // Send the file (doh)
        xhr[rand].send(ProdImgData);
    }













}]);