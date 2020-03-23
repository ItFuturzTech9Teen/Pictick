myApp.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;
            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files);
                });
            });
        }
    };
}]);

myApp.directive('exportToPdf', function () {
    return {
        restrict: 'E',
        scope: {
            elemId: '@'
        },
        template: '<button data-ng-click="exportToPdf()">Export to PDF</button>',
        link: function (scope, elem, attr) {
            scope.exportToPdf = function () {
                var doc = new jsPDF();
                console.log('elemId 12312321', scope.elemId);
                doc.fromHTML(
                document.getElementById(scope.elemId).innerHTML, 15, 15, {
                    'width': 170
                });
                doc.save('a4.pdf')
            }
        }
    }
});

//app.directive('backImg', function () {
//    return function (scope, element, attrs) {
//        attrs.$observe('backImg', function (value) {
//            element.css({
//                'background-image': 'url(' + value + ')',
//                'background-size': 'cover'
//            });
//        });
//    };
//});