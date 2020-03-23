myApp.filter('html', ['$sce', function ($sce) {
    return function (text) {
        return $sce.trustAsHtml(text);
    };
}]);

myApp.filter('htmlToPlaintext', ['$sce', function ($sce) {
    return function (text) {
        return text ? String(text).replace(/<[^>]+>/gm, '') : '';
    };
}]);

myApp.filter('upperCase', function () {
    return function (input) {
        return (!!input) ? input.charAt(0).toUpperCase() + input.substr(1).toUpperCase() : '';
    }
});

myApp.filter('TimeFilter', function () {
    return function (time) {
        time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [time];

        if (time.length > 1) { // If time format correct
            time = time.slice(1);  // Remove full string match value
            time[5] = +time[0] < 12 ? 'AM' : 'PM'; // Set AM/PM
            time[0] = +time[0] % 12 || 12; // Adjust hours
        }
        return time.join('');
    };
})