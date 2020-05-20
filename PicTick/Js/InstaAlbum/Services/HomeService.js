myApp.service('HomeService', ['$http', 'storage', 'ipCookie', function ($http, storage, ipCookie) {
    var _selfService = this;

    this.FormatDateTime = function (date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'         
        minutes = minutes < 10 ? '0' + minutes : minutes; var strTime = hours + ':' + minutes + ' ' + ampm;
        return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
    }

    //Satart Dashboard
    this.GetDashboardCount = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetDashboardCount?StudioId=' + studioId);
    }

    this.GetDashboardAlbumList = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetDashboardAlbumList?StudioId=' + studioId);
    }

    this.GetDashboardGalleryList = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetDashboardGalleryList?StudioId=' + studioId);
    }

    //End Dashboard

    //Start Studio Service
    this.GetStudioList = function () {
        return $http.get(API_URL + '/HomeAPI/GetStudioList');
    }

    this.SaveStudioData = function (scope) {
        var jsonData = {
            "Id": scope.Id, "Name": scope.Name, "Mobile": scope.Mobile,
            "UserName": scope.UserName, "Password": scope.Password
        };

        showLoader();
        $http({
            url: API_URL + '/HomeAPI/SaveStudio',
            method: "Post",
            data: jsonData,
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    showSuccessMessage("Data Saved Successfully");
                    scope.GetData();
                    hideLoader();
                    scope.Clear();
                }
                else {
                    showWarningMessage("Data Not Save");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }).error(function (error) {
            showErrorMessage("Some Error Occure");
            hideLoader();
        })
    }

    this.UpdateStudioWaterMarkData = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile != null && scope.myFile.length > 0)
            ProdImgData.append('WaterMarkImage', scope.myFile[0]);
        ProdImgData.append('Id', scope.StudioId);
        ProdImgData.append('WaterMark', scope.WaterMark);
        ProdImgData.append('Position', scope.Position);
        ProdImgData.append('Font', scope.FontName);
        ProdImgData.append('FontStyle', scope.FontStyle);
        ProdImgData.append('Opacity', scope.Opacity);
        ProdImgData.append('FontSize', scope.FontSize);
        ProdImgData.append('WaterMarkType', scope.WaterMarkType);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/UpdateStudioWaterMark', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        hideLoader();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.DeleteStudio = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteStudio?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End Studio Service

    //Start Studio About Service
    this.GetStudioAboutList = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetStudioAboutList?studioId=' + studioId);
    }

    this.SaveStudioAboutData = function (scope) {
        var jsonData = {
            "Id": scope.Id, "Title": scope.Title, "Description": scope.Description, "StudioId": scope.StudioId
        };

        showLoader();
        $http({
            url: API_URL + '/HomeAPI/SaveStudioAbout',
            method: "Post",
            data: jsonData,
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    showSuccessMessage("Data Saved Successfully");
                    scope.GetData();
                    hideLoader();
                    scope.Clear();
                }
                else {
                    showWarningMessage("Data Not Save");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }).error(function (error) {
            showErrorMessage("Some Error Occure");
            hideLoader();
        })
    }

    this.DeleteStudioAbout = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteStudioAbout?id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End Studio About Service

    //Start Gallery Service
    this.GetGalleryList = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetGalleryList?StudioId=' + studioId);
    }

    this.SaveGallery = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile != null && scope.myFile.length > 0)
            ProdImgData.append('Image', scope.myFile[0]);
        ProdImgData.append('Id', scope.Id);
        ProdImgData.append('Title', scope.Title);
        ProdImgData.append('GalleryPin', scope.GalleryPin);
        ProdImgData.append('SelectionPin', scope.SelectionPin);
        ProdImgData.append('AllowDownload', scope.AllowDownload);
        ProdImgData.append('WatterMark', scope.WatterMark);
        ProdImgData.append('StudioId', scope.StudioId);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/SaveGallery', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        scope.GetData();
                        hideLoader();
                        scope.Clear();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.DeleteGallery = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteGallery?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End Gallery Service

    //Start Album Service
    this.GetAlbumList = function (galleryId) {
        return $http.get(API_URL + '/HomeAPI/GetAlbumList?GalleryId=' + galleryId);
    }

    this.SaveAlbumData = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile != null && scope.myFile.length > 0)
            ProdImgData.append('Image', scope.myFile[0]);
        ProdImgData.append('Id', scope.Id);
        ProdImgData.append('Name', scope.Name);
        ProdImgData.append('Date', _selfService.FormatDateTime(scope.Date));
        ProdImgData.append('GalleryId', scope.GalleryId);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/SaveAlbum', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        scope.GetData();
                        hideLoader();
                        scope.Clear();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.DeleteAlbum = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteAlbum?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }

    this.UpdateAlbumSelection = function (scope) {
        showLoader();
        $http({
            url: API_URL + '/HomeAPI/UpdateAlbumSelection',
            method: "Post",
            data: scope.SelectedList,
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    showSuccessMessage("Data Saved Successfully");
                    //scope.GetData();
                    hideLoader();
                    //scope.Clear();
                }
                else {
                    showWarningMessage("Data Not Save");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }).error(function (error) {
            showErrorMessage("Some Error Occure");
            hideLoader();
        })
    }

    //End Album Service

    //Start Customer Service
    this.GetCustomerList = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetCustomerList?studioId=' + studioId);
    }

    this.GetCustomerAlbumList = function (customerId) {
        return $http.get(API_URL + '/HomeAPI/GetCustomerAlbumList?customerId=' + customerId);
    }

    this.GetCustomerGalleryList = function (customerId, studioId) {
        return $http.get(API_URL + '/HomeAPI/GetCustomerGalleryList?customerId=' + customerId + '&studioId=' + studioId);
    }

    this.SaveCustomerData = function (scope) {
        var jsonData = {
            "Id": scope.Id, "Name": scope.Name, "Mobile": scope.Mobile, "StudioId": scope.StudioId,
            "Email": scope.Email, "UserName": scope.UserName, "Password": scope.Password
        };
        showLoader();
        $http({
            url: API_URL + '/HomeAPI/SaveCustomer',
            method: "Post",
            data: jsonData,
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    showSuccessMessage("Data Saved Successfully");
                    scope.GetData();
                    hideLoader();
                    scope.Clear();
                }
                else {
                    showWarningMessage("Data Not Save");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }).error(function (error) {
            showErrorMessage("Some Error Occure");
            hideLoader();
        })
    }

    this.SaveCustomerGalleryData = function (scope, customerId, galleryId) {
        var jsonData = {
            "Id": 0, "CustomerId": customerId, "GalleryId": galleryId
        };
        showLoader();
        $http({
            url: API_URL + '/HomeAPI/SaveCustomerAlbum',
            method: "Post",
            data: jsonData,
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    showSuccessMessage("Data Saved Successfully");
                    scope.GetData();
                    hideLoader();
                    scope.Clear();
                }
                else {
                    showWarningMessage("Data Not Save");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }).error(function (error) {
            showErrorMessage("Some Error Occure");
            hideLoader();
        })
    }

    this.DeleteCustomer = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteCustomer?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End Customer Service

    //Start Album Photo Service
    this.GetAlbumPhotoList = function (albumId) {
        return $http.get(API_URL + '/HomeAPI/GetAlbumPhotoList?AlbumId=' + albumId);
    }

    this.GetSelectedAlbumPhotoList = function (albumId) {
        return $http.get(API_URL + '/HomeAPI/GetSelectedAlbumPhotoList?AlbumId=' + albumId);
    }

    this.SaveAlbumPhotoData = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile.length > 0) {
            for (var i = 0; i < scope.myFile.length; i++) {
                ProdImgData.append('Image' + i, scope.myFile[i]);
            }
        }

        ProdImgData.append('AlbumId', scope.AlbumId);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/SaveAlbumPhoto', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        scope.GetData();
                        hideLoader();
                        scope.Clear();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.DeleteAlbumPhoto = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteAlbumPhoto?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetPhotoFromServer(scope.AlbumId);
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End Album Photo Service

    //Start Album Photo Service
    this.GetAlbumVideoList = function (albumId) {
        return $http.get(API_URL + '/HomeAPI/GetAlbumVideoList?AlbumId=' + albumId);
    }

    this.SaveAlbumVideoData = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile.length > 0) {
            for (var i = 0; i < scope.myFile.length; i++) {
                ProdImgData.append('File' + i, scope.myFile[i]);
            }
        }
        ProdImgData.append('Title' + i, scope.Title);
        ProdImgData.append('AlbumId', scope.AlbumId);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/SaveAlbumVideo', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        scope.GetData();
                        hideLoader();
                        scope.Clear();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.DeleteAlbumVideo = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteAlbumVideo?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End Album Photo Service


    //Start Category Service
    this.GetCategoryList = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetCategoryList?studioId=' + studioId);
    }

    this.SaveCategoryData = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile != null && scope.myFile.length > 0)
            ProdImgData.append('Image', scope.myFile[0]);
        ProdImgData.append('Id', scope.Id);
        ProdImgData.append('Title', scope.Title);
        ProdImgData.append('StudioId', scope.StudioId);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/SaveCategoryData', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        scope.GetData();
                        hideLoader();
                        scope.Clear();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.DeleteCategory = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteCategory?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End Category Service

    //Start Portfolio Service
    this.GetPortfolioList = function (categoryId) {
        return $http.get(API_URL + '/HomeAPI/GetPortfolioList?CategoryId=' + categoryId);
    }

    this.SavePortfolioData = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile.length > 0) {
            for (var i = 0; i < scope.myFile.length; i++) {
                ProdImgData.append('Image' + i, scope.myFile[i]);
            }
        }

        ProdImgData.append('CategoryId', scope.CategoryId);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/SavePortfolio', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        scope.GetData();
                        hideLoader();
                        scope.Clear();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.DeletePortfolio = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeletePortfolio?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetPhotoFromServer(scope.CategoryId);
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }

    //End Portfolio Service

    //Start State Service
    this.GetStateData = function () {
        return $http.get(API_URL + '/HomeAPI/GetStates');
    }

    this.SaveStateData = function (scope) {
        var jsonData = {
            "Id": scope.Id, "Name": scope.Name
        };
        showLoader();
        $http({
            url: API_URL + '/HomeAPI/SaveState',
            method: "Post",
            data: jsonData,
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    showSuccessMessage("Data Saved Successfully");
                    scope.GetData();
                    hideLoader();
                    scope.Clear();
                }
                else {
                    showWarningMessage("Data Not Save");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }).error(function (error) {
            showErrorMessage("Some Error Occure");
            hideLoader();
        })
    }

    this.DeleteState = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteState?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End State Service


    //Start City Service
    this.GetStateList = function () {
        return $http.get(API_URL + '/HomeAPI/GetStates');
    }

    this.GetCityData = function (stateId) {
        return $http.get(API_URL + '/HomeAPI/GetCity?stateId=' + stateId);
    }

    this.SaveCityData = function (scope) {
        var jsonData = {
            "Id": scope.Id, "Name": scope.Name, "StateId": scope.StateId
        };
        showLoader();
        $http({
            url: API_URL + '/HomeAPI/SaveCity',
            method: "Post",
            data: jsonData,
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    showSuccessMessage("Data Saved Successfully");
                    scope.GetData();
                    hideLoader();
                    scope.Clear();
                }
                else {
                    showWarningMessage("Data Not Save");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }).error(function (error) {
            showErrorMessage("Some Error Occure");
            hideLoader();
        })
    }

    this.DeleteCity = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteCity?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }
    //End City Service

    //Start Organization Service
    this.GetStateList = function () {
        return $http.get(API_URL + '/HomeAPI/GetStates');
    }

    this.GetCityList = function (stateId) {
        return $http.get(API_URL + '/HomeAPI/GetCity?stateId=' + stateId);
    }

    this.UpdatestudioData = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile != null && scope.myFile.length > 0)
            ProdImgData.append('StudioLogo', scope.myFile[0]);
        ProdImgData.append('Id', scope.StudioId);
        ProdImgData.append('Name', scope.Name);
        ProdImgData.append('Mobile', scope.Mobile);
        ProdImgData.append('Email', scope.Email);
        ProdImgData.append('About', scope.About);
        ProdImgData.append('Services', scope.Services);
        ProdImgData.append('Website', scope.Website);
        ProdImgData.append('StudioOwner', scope.StudioOwner);
        ProdImgData.append('InviteMessage', scope.InviteMessage);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/UpdatestudioData', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        scope.GetData();
                        hideLoader();
                        scope.Clear();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.GetStudioDataById = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetStudioDataById?studioId=' + studioId);
    }

    //End Organization Service

    //Start AppointmentSlot Service
    this.GetAppointmentSlotList = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetAppointmentSlotList?studioId=' + studioId);
    }

    this.SaveAppointmentSlotData = function (scope) {
        var jsonData = {
            "Id": scope.Id, "Title": scope.Title, "StudioId": scope.StudioId
        };

        showLoader();
        $http({
            url: API_URL + '/HomeAPI/SaveAppointmentSlot',
            method: "Post",
            data: jsonData,
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            if (data.IsSuccess === true) {
                if (data.Data !== null && data.Data !== undefined) {
                    showSuccessMessage("Data Saved Successfully");
                    scope.GetData();
                    hideLoader();
                    scope.Clear();
                }
                else {
                    showWarningMessage("Data Not Save");
                    hideLoader();
                }
            }
            else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }).error(function (error) {
            showErrorMessage("Some Error Occure");
            hideLoader();
        })
    }

    this.DeleteAppointmentSlot = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteAppointmentSlot?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }

    //End AppointmentSlot Service

    //Start AppointmentSlot Service

    this.GetAppointmentList = function (studioId, fromDate) {
        return $http.get(API_URL + '/HomeAPI/GetAppointmentList?studioId=' + studioId + '&fromDate=' + _selfService.FormatDateTime(new Date(fromDate)));
    }

    //End AppointmentSlot Service


    //Start Organization Service

    this.GetStudioSocialLinkList = function (studioId) {
        return $http.get(API_URL + '/HomeAPI/GetStudioSocialLinkList?studioId=' + studioId);
    }

    this.SaveStudioSocialLink = function (scope) {
        showLoader();
        var ProdImgData = new FormData();
        if (scope.myFile != null && scope.myFile.length > 0)
            ProdImgData.append('Image', scope.myFile[0]);
        ProdImgData.append('Id', scope.Id);
        ProdImgData.append('Title', scope.Title);
        ProdImgData.append('Link', scope.Link);
        ProdImgData.append('StudioId', scope.StudioId);

        var xhr = new XMLHttpRequest;
        xhr.open('POST', API_URL + '/HomeAPI/SaveStudioSocialLink', true);
        xhr.onload = function handler() {
            hideLoader();
            if (this.status === 200) {
                var data = JSON.parse(this.responseText);
                if (data.IsSuccess === true) {
                    if (data.Data !== null && data.Data !== undefined) {
                        showSuccessMessage("Data Saved Successfully");
                        scope.GetData();
                        hideLoader();
                        scope.Clear();
                    }
                    else {
                        showWarningMessage("Data Not Save");
                        hideLoader();
                    }
                }
                else {
                    showErrorMessage("Some Error Occure");
                    hideLoader();
                }
            } else {
                showErrorMessage("Some Error Occure");
                hideLoader();
            }
        }
        xhr.send(ProdImgData);
    }

    this.DeleteStudioSocialLink = function (Id, scope) {
        showLoader();
        var deletedData = $http.get(API_URL + '/HomeAPI/DeleteStudioSocialLink?Id=' + Id);
        deletedData.then(function (data) {
            if (data.data.IsSuccess === true) {
                showSuccessMessage("Data Delete Successfully");
                scope.GetData();
                hideLoader();
            }
            else {
                showWarningMessage("Something went wrong");
                hideLoader();
            }
        })
    }

    //End Organization Service



    // Start Client Gallery

    this.GetGalleryById = function (galleryId) {
        return $http.get(API_URL + '/HomeAPI/GetGalleryById?galleryId=' + galleryId);
    }

    this.GetFontList = function () {
        return $http.get(API_URL + '/HomeAPI/GetFontList');
    }

    //End Client Gallery

}]);