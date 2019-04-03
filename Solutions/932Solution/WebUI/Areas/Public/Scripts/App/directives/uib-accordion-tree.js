(function(angular) {

    var uibAccordionTree = function ($compile, $log) {
        return {
            restrict: 'E',
            terminal: true,
            scope: {
                items: '=ngModel',
                isOpen: '=ngIsOpen',
                ngItemChange: '=ngItemChange',
                ngDefIcon: '=ngDefIcon',
                ngItemCheckStyles: '=ngItemCheckStyles',
                ngDefColor: '=ngDefColor',
                ngItemCountClick: '=ngItemCountClick',
                ngItemIsLoading: '=ngItemIsLoading',
                ngItemIsFiltered: '=ngItemIsFiltered'
            },
            link: function($scope, $element, $attrs) {
                if (angular.isArray($scope.items)) {
                    $element.append('<uib-accordion close-others="true"><uib-accordion-node ng-repeat="item in items | exceptSerchable" ng-model="item" ng-is-open="isOpen" ng-def-icon="ngDefIcon" ng-def-color="ngDefColor" ng-item-count-click="ngItemCountClick" ng-item-check-styles="ngItemCheckStyles" ng-item-change="ngItemChange" ng-item-is-loading="ngItemIsLoading" ng-item-is-filtered="ngItemIsFiltered"></uib-accordion-node></uib-accordion>');
                }

                $compile($element.contents())($scope.$new());
            }
        };
    };

    var uibAccordionNode = function ($compile, $log) {
        return {
            restrict: 'E',
            terminal: true,
            scope: {
                item: '=ngModel',
                isOpen: '=ngIsOpen',
                ngItemChange: '=ngItemChange',
                ngDefIcon: '=ngDefIcon',
                ngDefColor: '=ngDefColor',
                ngItemCheckStyles: '=ngItemCheckStyles',
                ngItemCountClick: '=ngItemCountClick',
                ngItemIsLoading: '=ngItemIsLoading',
                ngItemIsFiltered: '=ngItemIsFiltered'
            },
            link: function ($scope, $element, $attrs) {
                var checkStyle;

                if ($scope.ngItemCheckStyles.length) {
                    checkStyle = $scope.ngItemCheckStyles[0];
                    if ($scope.ngItemCheckStyles.length > 1) {
                        $scope.ngItemCheckStyles.splice(0, 1);
                    }
                } else {
                    checkStyle = 'green';
                    $scope.ngItemCheckStyles = [checkStyle];
                }

                var panelClass = '{{::((item.Children.length ? \'\' : \'no-items\') + (item.Filterable ? \'\': \' no-filter\'))}}';

                var headingCheckbox = '<input ng-change="ngItemChange(item)" type="checkbox" ng-icheck ng-icheck-color="' + checkStyle + '" ng-icheck-theme="square" ng-indeterminate="item.IsIndeterminated()" ng-model="item.Checked"/>';
                var headingIcon = '<div class="map-menu-layer-icon" ng-class="{ \'layer-load\': ngItemIsLoading(item) }" ng-style="{ \'background-color\': item.Style && item.Style.Color ? item.Style.Color : ngDefColor }"><span class="{{::item.Style && item.Style.Icon ? item.Style.Icon : ngDefIcon }}"></span></div>';

                var countFilter = '<div class="map-layer-filter-counter" ng-class="{ \'no-arrow\': !item.Children.length }">' +
                                '<a ng-click="$event.stopPropagation()" ng-href="#/filter/{{item.LayerId}}" class="map-layer-filter" ng-if="::item.Filterable" ng-class="{ \'has-filter\': ngItemIsFiltered(item) }"><span class="glyphicon glyphicon-filter"></span></a>' +
                                '<span class="map-layer-count" ng-click="$event.stopPropagation(); ngItemCountClick(item)" ng-class="{\'clickable\' : item.Load}">{{item.GetCount()}}</span></div>';

                var head = headingCheckbox + headingIcon + '{{::item.Title}}<i class="panel-arrow" ng-if="item.Children.length"></i>' + countFilter;

                if ($scope.item.HasChildren) {
                    $element.append('<uib-accordion-group is-open="isOpen" panel-class="' + panelClass + '"><uib-accordion-heading>'
                        + head + '</uib-accordion-heading><uib-accordion-tree ng-model="item.Children" ng-is-open="false" ng-def-icon="ngDefIcon" ng-def-color="ngDefColor" ng-item-check-styles="ngItemCheckStyles" ng-item-count-click="ngItemCountClick" ng-item-change="ngItemChange" ng-item-is-loading="ngItemIsLoading" ng-item-is-filtered="ngItemIsFiltered"></uib-accordion-tree></uib-accordion-group>');
                } else {
                    $element.append('<uib-accordion-group is-open="isOpen" panel-class="' + panelClass + '"><uib-accordion-heading>'
                        + head + '</uib-accordion-heading></uib-accordion-group>');
                }

                $compile($element.contents())($scope.$new());
            }
        };
    }

    uibAccordionTree.$inject = ['$compile', '$log'];
    uibAccordionNode.$inject = ['$compile', '$log'];

    angular.module("uib-accordion-tree", [])
        .directive("uibAccordionTree", uibAccordionTree)
        .directive("uibAccordionNode", uibAccordionNode);


})(window.angular);