myApp.controller('AlbumController', ['$scope', 'HomeService', 'storage', '$http', 'ipCookie', function ($scope, HomeService, storage, $http, ipCookie) {
    $scope.Id = 0;
    $scope.MemberId = 0;
    $scope.StudioId = 0;
    $scope.GalleryId = 0;
    $scope.AlbumId = 0;
    $scope.DataList = [];
    $scope.AlbumPhotoList = [];
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
        var dataList = HomeService.GetAlbumList($scope.GalleryId);
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

    $scope.GetPhotoData = function (albumId) {
        $('#myPhotoModal').modal('toggle');
        $("#togList").click();
        var template = '<div class="text">Drop Images Here</div>';
        $("#dropbox").html(template);
        $scope.GetPhotoFromServer(albumId);
    }

    $scope.GetPhotoFromServer = function (albumId) {
        $scope.AlbumId = albumId;
        showLoader();
        var dataList = HomeService.GetAlbumPhotoList(albumId);
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

        showLoader();
        var dataPhtotList = HomeService.GetSelectedAlbumPhotoList(albumId);
        dataPhtotList.then(function (pl) {
            var data = pl.data;
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    $scope.SelectedPhotoList = data.Data;
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        });
    }

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

    $scope.ExportFile = function (albumId, count) {
        if (count != null && count > 0) {
            showLoader();
            var dataList = HomeService.GetSelectedAlbumPhotoList(albumId);
            dataList.then(function (pl) {
                var data = pl.data;
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        $scope.SelectedPhotoList = data.Data;
                        //Export File
                        if ($scope.SelectedPhotoList.length > 0) {
                            var filename = "";
                            for (j = 0; j < $scope.DataList.length; j++) {
                                if ($scope.DataList[j].Id == albumId) {
                                    filename = $scope.DataList[j].Name;
                                }
                            }

                            var textToSend = "";
                            for (var i = 0; i < $scope.SelectedPhotoList.length; i++) {
                                var url = $scope.SelectedPhotoList[i].Photo;
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

                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            });
        } else
            showWarningMessage("No Photo Selected Yet !");
    }

    $scope.OpenPopUp = function () {
        $scope.Clear();
    }

    $scope.SaveData = function () {
        HomeService.SaveAlbumData($scope);
        $('#myModal').modal('hide');
    }

    $scope.DeleteData = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteAlbum(id, $scope);
        }
    }

    $scope.DeletePhoto = function (id) {
        var result = confirm("Are you sure you want to delete this ?");
        if (result) {
            HomeService.DeleteAlbumPhoto(id, $scope);
        }
    }

    $scope.EditData = function (data) {
        //$('#myModal').click();
        //$('#myModal').modal('toggle');
        $scope.Id = data.Id;
        $scope.Name = data.Name;
        $scope.Date = new Date(data.Date);
    }

    $scope.Clear = function () {
        $scope.Id = 0;
        $scope.Name = "";
        $scope.Date = new Date();
    }
    $scope.Clear();

    //Upload Image

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
                    showSuccessMessage("Photo Saved Successfully");
                } else {
                    alert("Error : Unexpected error while uploading file");
                }
            }
        };

        // Send the file (doh)
        xhr[rand].send(ProdImgData);
    }

}]);