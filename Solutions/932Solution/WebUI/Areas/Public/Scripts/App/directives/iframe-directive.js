(function (angular) {
    function iframe() {
        return {
            restrict: 'EA',
            scope: {
                src: '=ngFrameUrl',
                height: '@ngHeight',
                width: '@ngWidth',
                onload: '=ngLoad'
            },
            replace: true,
            transclude: true,
            template: '<iframe class="frame" ng-style="{\'height\': height }" width="{{width}}" frameborder="0" border="0" marginwidth="0" marginheight="0" src="{{src}}"></iframe>',
            //controller: function($scope) {
            //    debugger;
            //},
            link: function ($scope, $element, $attrs) {
                $element.bind('load', function (event) {
                    $scope.onload();
                });
            }
        };
    };

    angular.module('ngIFrame', [])
        .directive('ngFrame', iframe);

})(window.angular);