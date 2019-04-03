(function (angular, L) {
    function angularKladr($log, $compile, $q, format, $http, deviceDetector, SearchResultFactory) {
        var geoItemsSearch = function (layerIds, query) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'GET',
                    url: format('/Public/Map/FullTextSearchInLayers?searchStr={query}&layerIds={layerIds}&page=1&pageSize=10', { layerIds, query })
                }).success(function (response) {
                    resolve(response);
                }).error(function (err) {
                    $log.error('Global search load error...');
                    reject(err);
                });
            });
        };

        var yandexSearch = function (address) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'GET',
                    url: format('https://geocode-maps.yandex.ru/1.x/?format={format}&geocode={address}', { format: 'json', address })
                }).success(function (response) {
                    resolve(response);
                }).error(function (err) {
                    $log.error('Yandex search load error...');
                    reject(err);
                });
            });
        };

        return {
            restrict: 'EA',
            scope: {
                onSelect: '=ngOnSelect',
                onSearch: '=ngOnSearch',
                onClose: '=ngOnClose',
                onHide: '=ngOnHide',
                configs: '=ngSearchConfigs',
                query: '=ngQuery'
            },
            replace: true,
            transclude: true,
            //require: "ngQuery",
            template: '<div class="kladr">\
            <div class="btn-group kladr-category" uib-dropdown is-open="configopened">\
            <button type="button" class="btn btn-title">\
            {{selected.title.length > 13 ? selected.title.substring(0, 10) + "..." : selected.title }}\
            </button>\
            <button type="button" class="btn btn-cat" uib-dropdown-toggle>\
            <span class="caret"></span>\
            <span class="sr-only">Split button</span>\
            </button>\
            <ul class="dropdown-menu" uib-dropdown-menu role="menu" aria-labelledby="split-button">\
            <li ng-click="onChange(option)" ng-class="{ selected: selected == option }" ng-repeat="option in options" role="menuitem">\
            <a href="#">{{option.title}}</a>\
            </li>\
            </ul>\
            </div>\
            <button class="btn btn-back" ng-if="hideIsVisible" ng-click="hide()">\
            <i class="fa fa-arrow-left"></i>\
            </button>\
            <form class="kladr-form" ng-submit="search()">\
            <input type="text" class="kladr-search" placeholder="Поиск..." ng-model="query" />\
            <button type="submit" class="btn btn-primary kladr-btn">\
            <i class="fa fa-search"></i>\
            </button>\
            </form>\
            <button ng-hide="!items.length || !query.length || configopened" ng-click="close()" class="kladr-close">×</button>\
            <perfect-scrollbar class="kladr-autocomplete" ng-hide="!items.length || !query.length || configopened">\
            <ul>\
            <li ng-repeat="item in items">\
            <a href="#" ng-click="select(item)">{{item.Title}}</a>\
            </li>\
            </ul>\
            </perfect-scrollbar>\
            </div>',
            controller: function ($scope, $element) {
                var loadLayers = function () {
                    geoItemsSearch([$scope.selected.name], $scope.query).then(function (data) {
                        $scope.items = [];

                        if (data && data.Items && data.Items.length) {
                            $scope.items = SearchResultFactory.CreateResultByLayers($scope.selected.type, data.Items);

                            $log.info('Layers:', $scope.items);

                            $scope.$applyAsync();
                        }
                    },
                        function (err) {
                            $log.error(err);
                        });
                };

                var loadKladr = function () {
                    yandexSearch($scope.query).then(function (data) {
                        $scope.items = [];

                        if (data && data.response && data.response.GeoObjectCollection && data.response.GeoObjectCollection.featureMember) {
                            $scope.items = SearchResultFactory.CreateResultByYandex($scope.selected.type, data.response.GeoObjectCollection.featureMember.map(function(item) {
                                return item.GeoObject;
                            }));

                            $log.info('Yandex:', $scope.items);

                            $scope.$applyAsync();
                        }
                        
                    }, function (err) {
                        $log.error(err);
                        });
                };

                var reSearch = function () {
                    if ($scope.query.length >= 3) {
                        switch ($scope.selected.type) {
                            case 'layer':
                                loadLayers();
                                return;
                            case 'kladr':
                                loadKladr();
                                return;
                        }
                    }

                    $scope.items = [];
                };

                $scope.configopened = false;

                $scope.options = [
                    {
                        name: 'kladr-search',
                        type: 'kladr',
                        title: 'По адресу'
                    }
                ];

                if ($scope.configs && $scope.configs.length) {
                    angular.forEach($scope.configs, function (config) {
                        $scope.options.push({
                            name: config.LayerId,
                            type: 'layer',
                            title: config.Title
                        });
                    });
                }

                $scope.selected = $scope.options[0];

                $scope.items = [];

                $scope.select = function (item) {
                    $scope.onSelect(item);
                };

                $scope.close = function () {
                    $scope.items = [];
                    $scope.onClose();
                    //$scope.query = '';
                };

                $scope.hideIsVisible = deviceDetector.isMobile();

                $scope.hide = function() {
                    $scope.onHide();
                };


                $scope.search = function() {
                    switch ($scope.selected.type) {
                        case 'layer':
                            $scope.onSearch($scope.selected);
                            return;
                        case 'kladr':
                            if ($scope.items && $scope.items.length) {
                                $scope.select($scope.items[0]);
                            }
                            return;
                    }
                };

                $scope.onChange = function (option) {
                    $scope.selected = option;

                    reSearch();
                };

                $scope.$watch(function () {
                    return $scope.query;
                }, function (query) {
                    reSearch();
                }, true);
            },
            link: function ($scope, $element) {
                //$element.find('input[type=text]:eq(0)').kladr({
                //    oneString: true,
                //    receive: function (data) {
                //        if (data && data.length && $scope.selected.type === 'kladr') {
                //            $scope.items = [];

                //            angular.forEach(data, function (item) {
                //                $scope.items.push({
                //                    title: item.fullName,
                //                    type: $scope.selected.type,
                //                    item: item
                //                });
                //            });

                //            $scope.$applyAsync();
                //        }
                //    }
                //});
            }
        };
    };

    angularKladr.$inject = ['$log', '$compile', '$q', 'format', '$http', 'deviceDetector', 'SearchResultFactory'];

    angular.module('jKladr', [])
        .directive('ngKladr', angularKladr);

})(window.angular, window.L);