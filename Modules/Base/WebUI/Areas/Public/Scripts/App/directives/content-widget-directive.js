(function(angular) {
    function contentWidget() {
        return {
            restrict: 'EA',
            scope: {
                uid: '=ngUid',
                mnemonic: '=ngMnemonic',
                layerId: '=ngLayerId',
                template: '=ngWidgetTemplate',
                onload: '=ngLoad'
            },
            replace: true,
            transclude: true,
            template: '<div ng-bind-html="template"></div>',
            controller: function($scope, $element) {

                $scope.$watch('template', function (template) {

                    //console.log('Template:', $scope.template, $element);

                    var $wrapper = $element.find('*:not(script, style, link)').first();

                    if ($wrapper.length) {
                        $wrapper.on('complete', function () {
                            $scope.onload();
                        });

                        $wrapper.trigger('init', [$scope.uid, $scope.mnemonic, $scope.layerId]);
                    }
                });
            },
            link: function ($scope, $element) {

                
            }
        };
    };

    angular.module('ngWidget', [])
        .directive('ngWidget', contentWidget);

})(window.angular);