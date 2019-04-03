(function (angular, $) {
    function angularFancybox($log, $compile, format) {
        return {
            restrict: 'EA',
            scope: {
                model: '=ngModel',
                //dataItems: []
            },
            replace: true,
            transclude: true,
            require: "ngModel",
            template: '<div class="gallery-button" ng-style="{ \'background-image\': \'url(/Files/GetImage?id=\' + (isDefined() ? dataItems[0].FileID : null) + \'&width=300&height=300&defImage=NoImage)\' }" ng-click="show()"></div>',
            controller: function ($scope, $element) {
                $scope.dataItems = [];

                $scope.isDefined = function () {
                    return (angular.isArray($scope.model) && $scope.model.length) || $scope.model;
                };

                var init = function() {
                    $scope.dataItems = $scope.isDefined() ? (angular.isArray($scope.model) ? $scope.model : [$scope.model]) : [];
                };

                $scope.show = function (index) {
                    if (!$scope.isDefined()) return;

                    index = angular.isDefined(index) ? index : 0;

                    var items = $scope.dataItems.map(function(image) {
                        return {
                            src: format('/Files/GetImage?id={0}', image.FileID),
                            type: 'image',
                            opts: {
                            }
                        };
                    });

                    //$log.info(items);

                    $.fancybox.open(items, null, index);
                };

                //init();

                $scope.$watch(function () {
                    return $scope.model;
                }, function (newValue, oldValue) {
                    if (angular.isDefined(newValue) && newValue !== oldValue) {
                        init();
                    }
                });
            },
            link: function ($scope, $element) {

            }
        };
    };

    angularFancybox.$inject = ['$log', '$compile', 'format'];

    angular.module('jFancybox', [])
        .directive('ngFancybox', angularFancybox);

})(window.angular, window.jQuery);