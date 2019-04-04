(function (angular) {
    function navigationService($log, $timeout, $cookies, $location, DataService, MapProviderService, UtilsService, MapService, GLOBAL_PARAM_DEFS, GeoItem) {

        var service = function () {

            var self = this;

            this.options = {
                enable: $cookies.get('navigation-option') === 'true',
                support: (function () {
                    return angular.isDefined(navigator.geolocation) && angular.isDefined(navigator.geolocation.getCurrentPosition);
                })(),
                settings: {
                    enableHighAccuracy: true
                }
            };

            $log.info('Navigation options:', this.options);

            this.isSupport = (function () {
                return self.options.support;
            })();

            var watcher;

            var onChangePosition = function (data) {
                $timeout(function () {
                    $log.info('Navigation:', data, L.latLng(data.coords.latitude, data.coords.longitude));
                    if (data && data.coords) {
                        var latLng = L.latLng(data.coords.latitude, data.coords.longitude);

                        var zoom = 17;
                        //var searchZoom = 10;

                        MapProviderService.setCurrentPosition([], latLng, zoom);

                        MapService.layers.updateNavigation(latLng.lat, latLng.lng);

                        var bound = UtilsService.layers.createBounds(latLng, GLOBAL_PARAM_DEFS.NAVIGATION_SEARCH_RADIUS);

                        if (!GLOBAL_PARAM_DEFS.NAVIGATION_LAYERS.length) return;

                        DataService.searchLayers(GLOBAL_PARAM_DEFS.NAVIGATION_LAYERS,
                            latLng.lat,
                            latLng.lng,
                            [
                                bound.getSouthWest().lat, bound.getSouthWest().lng, bound.getNorthEast().lat, bound
                                .getNorthEast().lng
                            ],
                            zoom,
                            true).then(function(response) {
                                $log.info('Navigation search:', response);

                                if (response) {
                                    var layerIds = Object.keys(response);

                                    if (layerIds.length) {
                                        var config = UtilsService.views.getViewConfig(layerIds[0]);

                                        if (config) {
                                            var dataItem = response[config.LayerId][0];

                                            if (dataItem) {
                                                dataItem = new GeoItem(dataItem);

                                                if (dataItem.IsGeometry()) {
                                                    var pos = UtilsService.layers.createCenter(dataItem.Geometry);

                                                    MapService.settings.popupOpen(config.LayerId, dataItem, pos.latlng);

                                                    $location.path('/detail/' + config.LayerId + '/' + dataItem.ID + '/show');
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            function(err) {
                                $log.error('Navigation search error...');
                            });
                    }
                });
            };

            var onChangePositionError = function (data) {
                $log.error('Navigation error...');
            };

            this.switch = function () {
                this.options.enable = !this.options.enable;

                $cookies.put('navigation-option', this.options.enable);

                if (this.isSupport) {
                    if (this.options.enable) {
                        navigator.geolocation.getCurrentPosition(onChangePosition, onChangePositionError, self.options.settings);

                        clearInterval(watcher);

                        watcher = setInterval(function () {
                            navigator.geolocation.getCurrentPosition(onChangePosition, onChangePositionError, self.options.settings);
                        }, GLOBAL_PARAM_DEFS.NAVIGATION_TIMEOUT);
                    } else {
                        clearInterval(watcher);
                        MapService.layers.clearNavigation();
                    }
                };
            };

            this.start = function () {
                this.options.enable = true;

                $cookies.put('navigation-option', this.options.enable);

                if (this.isSupport) {
                    navigator.geolocation.getCurrentPosition(onChangePosition, onChangePositionError, self.options.settings);

                    clearInterval(watcher);

                    watcher = setInterval(function () {
                        navigator.geolocation.getCurrentPosition(onChangePosition, onChangePositionError, self.options.settings);
                    }, GLOBAL_PARAM_DEFS.NAVIGATION_TIMEOUT);
                };
            };

            this.stop = function () {
                this.options.enable = false;

                $cookies.put('navigation-option', this.options.enable);

                if (this.isSupport) {
                    clearInterval(watcher);
                };

                MapService.layers.clearNavigation();
            };

        };

        return new service();
    };

    navigationService.$inject = ['$log', '$timeout', '$cookies', '$location', 'DataService', 'MapProviderService', 'UtilsService', 'MapService', 'GLOBAL_PARAM_DEFS', 'GeoItem'];

    angular.module('MapApp').
        factory('NavigationService', navigationService);

})(window.angular);