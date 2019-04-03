/* Map viewer 1.3.0 */

(function (angular, $, map) {
    'use strict';

    map.app = angular.module('MapApp', ['ngRoute', 'ngAnimate', 'ui.bootstrap',
        'perfect_scrollbar', 'ngICheck', 'ngPropertyViewers',
        'ngLeaflet', 'pageslide-directive', 'dropdown-multiselect', 'rzModule',
        'angularMoment', 'MapFilters', 'uib-accordion-tree', 'ngIFrame', 'ngWidget',
        'ngPano', 'ng.deviceDetector', 'ngCookies', 'angular-clipboard', 'rt.eventemitter', 'jKladr', 'jFancybox'])
        .config(['$routeProvider', '$compileProvider', function ($routeProvider, $compileProvider) {

            $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|javascript):/);

            $routeProvider.when('/', {})
                .when('/detail/:layerId/:id/:action', {
                    templateUrl: 'detail.container.html',
                    controller: 'DetailController',
                    controllerAs: 'detail'
                }).when('/panoram/:id', {
                    resolve: {
                        load: function ($routeParams, $uibModal) {
                            $uibModal.open({
                                animation: true,
                                backdrop: 'static',
                                windowClass: 'modal-fixed-size',
                                templateUrl: 'panoram.container.html',
                                controller: 'PanoramController',
                                controllerAs: 'panoram',
                                size: 'ls',
                                resolve: {
                                    panoram: function () {
                                        return $routeParams;
                                    }
                                }
                            });
                        }
                    }
                }).when('/content/:layerId/:id?', {
                    templateUrl: 'content.container.html',
                    controller: 'ContentController',
                    controllerAs: 'content'
                }).when('/clusterlist/:layerId/:id/:zoom', {
                    templateUrl: 'list.container.html',
                    controller: 'ClusterListController',
                    controllerAs: 'list'
                }).when('/mnemoniclist/:layerId', {
                    templateUrl: 'list.container.html',
                    controller: 'MnemonicListController',
                    controllerAs: 'list'
                }).when('/searchdetail', {
                    templateUrl: 'search.container.html',
                    controller: 'SearchController',
                    controllerAs: 'search'
                }).when('/filter/:layerId', {
                    templateUrl: 'filter.container.html',
                    controller: 'FilterController',
                    controllerAs: 'filter'
                }).when('/kadinfo/:code/:id', {
                    templateUrl: 'kaddetail.container.html',
                    controller: 'CadastreDetailController',
                    controllerAs: 'kadinfo'
                }).otherwise({
                    redirectTo: '/'
                });
        }])
        .run(['$rootScope', '$log', '$cookies', 'UtilsService', 'amMoment', 'VIEW_CONFIG', 'APP_SETTINGS', function ($rootScope, $log, $cookies, UtilsService, amMoment, VIEW_CONFIG, APP_SETTINGS) {

            $log.info('Application view configs:', VIEW_CONFIG);
            $log.info('Application settings:', APP_SETTINGS);

            amMoment.changeLocale('ru');

            var getVisibleLayerIds = function(configs) {
                var layerIds = [];

                if (configs && configs.length) {
                    configs.forEach(function (config) {
                        if (config.Checked) layerIds.push(config.LayerId);
                        layerIds = layerIds.concat(getVisibleLayerIds(config.Children));
                    });
                }

                return layerIds;
            };

            var updateVisibilities = function (configs, visibleLayerIds) {
                if (configs && configs.length) {
                    configs.forEach(function (config) {
                        config.Checked = visibleLayerIds.indexOf(config.LayerId) > -1;
                        updateVisibilities(config.Children, visibleLayerIds);
                    });
                }
            };

            var visibilities = $cookies.get('view-config-visibility') ? JSON.parse($cookies.get('view-config-visibility')) : null;

            if (!visibilities) {
                $cookies.put('view-config-visibility', JSON.stringify(getVisibleLayerIds(VIEW_CONFIG)));
            } else {
                updateVisibilities(VIEW_CONFIG, visibilities);
            }

            $log.info('Visibilities:', visibilities, VIEW_CONFIG);

            UtilsService.views.createConfig(VIEW_CONFIG);

            $log.info('View config settings:', UtilsService.views.viewConfigDict);
        }]);


    var loadViewConfigs = function() {
        return new Promise(function(resolve, reject) {
            $.ajax({
                type: 'GET',
                url: '/Public/Map/GetPublicLayers',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function(data) {
                    resolve(data);
                },
                error: function(err) {
                    reject(err);
                }
            });
        });
    };

    var loadSettings = function () {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: 'GET',
                url: '/Public/Map/GetPublicSettings',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
    };

    map.init = function (wrapper, viewType) {
        angular.module('MapApp').constant('VIEW_TYPE', viewType);

        Promise.all([loadViewConfigs(), loadSettings()]).then(function(results) {
            angular.module('MapApp').constant('VIEW_CONFIG', results[0]);
            angular.module('MapApp').constant('APP_SETTINGS', results[1].AppSettings);

            setTimeout(function () {
                angular.bootstrap('#' + wrapper, ['MapApp']);
            }, 2000);

        },
            function(errors) {
                console.log('Map init error:', errors);
            });
    };

})(window.angular, jQuery, window.mapapp || (window.mapapp = {}));

