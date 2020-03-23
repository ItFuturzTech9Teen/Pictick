//Local Connection
var API_BASE_URL = "http://localhost:59498";
var API_URL = "http://localhost:59498/api";
var APP_URL = "http://localhost:59498";

//Server Connection
//var API_BASE_URL = "http://instaalbum.itfuturz.com";
//var API_URL = "http://instaalbum.itfuturz.com/api";
//var APP_URL = "http://instaalbum.itfuturz.com";

var myApp = angular.module('InstaAlbumModule', ['ngTagsInput', 'angular.filter', 'ngTouch', 'angularLocalStorage', 'ipCookie', 'dndLists', 'ui.bootstrap', 'ui.select', 'ngSanitize', 'bw.paging']);
var Login_User = "login_user";
var WatermarkDetail = "WatermarkDetail";

//function showtimeAgo() {
//    setTimeout(function () {
//        $("span.timeago").timeago();// Do something after 5 seconds
//    }, 300);
//}

function encrypt(string, key) {
    var result = "";
    for (i = 0; i < string.length; i++) {
        result += String.fromCharCode(key ^ string.charCodeAt(i));
    }
    return result;
}

function decrypt(string, key) {
    var result = "";
    for (i = 0; i < string.length; i++) {
        result += String.fromCharCode(key ^ string.charCodeAt(i));
    }
    return result;
}

var encryptKey = 'sf2018'; // only integer value allow 