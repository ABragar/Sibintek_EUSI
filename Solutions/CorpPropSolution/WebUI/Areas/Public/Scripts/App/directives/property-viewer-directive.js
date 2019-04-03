(function (angular) {
    function isString(text) {
        return angular.isDefined(text) && angular.isString(text) && text.length;
    };

    function baseObjectOne() {
        return {
            restrict: 'AE',
            require: 'ngModel',
            scope: {
                model: '=ngModel'
            },
            replace: true,
            transclude: true,
            template: '<div class="baseobject-one"><div class="baseobject-one-icon" ng-if="model.Icon" ng-style="{ \'background-color\' : model.Icon.Color }"><span class="{{model.Icon.Value}}"><span></div><p class="baseobject-one-title">{{(isString(model.Title) ? model.Title : (isString(model.Name) ? model.Name : \'-\'))}}</p></div>',
            controller: function ($scope) {
                $scope.isString = function(title) {
                    return isString(title);
                };
            },
            //link: function ($scope, element, $attrs) {
            //    $scope.$watch('model', function(model) {
            //        var html = '-';

            //        if (model) {
            //            html = '';

            //            if (model.Icon) {
            //                html += '<div class=\'baseobject-icon\' style=\'background-color: ' + model.Icon.Color + '\'>' + '<i class=\'' + model.Icon.Value + '\'></i>' + '</div>';
            //            }

            //            html += '<p class=\'baseobject-title\'>' + (isString(model.Title) ? model.Title : (isString(model.Name) ? model.Name : '-')) + '</p>';
            //        }

            //        angular.element(element).html(html);
            //    });
            //}
        };
    }

    function baseEnum() {
        return {
            restrict: 'AE',
            require: 'ngModel',
            scope: {
                model: '=ngModel'
            },
            replace: true,
            transclude: true,
            template: '<div class="baseobject-one"><div class="baseobject-one-icon" ng-if="model" ng-style="{ \'background-color\' : model.Color }"><span class="{{model.Icon}}"><span></div><p class="baseobject-one-title">{{(isString(model.Title) ? model.Title : \'-\')}}</p></div>',
            controller: function ($scope) {
                $scope.isString = function (title) {
                    return isString(title);
                };

                //$scope.$watch('model',
                //    function(newVal, oldVal) {

                //    }, true);
            },
            //link: function ($scope, element, $attrs) {
            //    $scope.$watch('model', function (model) {
            //        var html = '-';

            //        if (model) {
            //            html = '';
            //            html += '<div class="baseobject-icon" style="background-color: ' + model.Color + '">' + '<i class="' + model.Icon + '"></i>' + '</div>';
            //            html += '<p class="baseobject-title">' + (isString(model.Title) ? model.Title : '-') + '</p>';
            //        }

            //        angular.element(element).html(html);
            //    });
            //}
        };
    }

    function baseDate($filter) {
        return {
            restrict: 'AE',
            require: 'ngModel',
            scope: {
                model: '=ngModel'
            },
            link: function ($scope, element, $attrs) {
                $scope.$watch('model', function(model) {
                    if (model) {
                        var dateTime = $filter('date')(model, 'dd.MM.yyyy HH:mm:ss');
                        angular.element(element).html(dateTime);
                    }
                });
            }
        };
    }

    angular.module('ngPropertyViewers', [])
        .directive('ngBaseobjectOne', baseObjectOne)
        .directive('ngBaseEnum', baseEnum)
        .directive('ngBaseDate', baseDate);

    baseDate.$inject = ['$filter'];

})(window.angular);