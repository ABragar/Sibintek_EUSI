(function (angular) {
    function mainController($rootScope, $scope, $log, UtilsService, MapService, GlobalSearchService, $location, NavigationService, deviceDetector, WidgetService, MapProviderService, GeoItem, LAYERS_PARAM_DEFS, MapTilesFactory) {
        var self = this;

        $log.info('Device:', deviceDetector);

        this.isMobile = deviceDetector.isMobile();
        this.menuOpened = deviceDetector.isDesktop();

        this.layersOpened = false;
        this.searchVisible = deviceDetector.isDesktop();
        
        this.globalSearchParams = GlobalSearchService.params;

        this.navigationOptions = NavigationService.options;

        this.ftsConfigs = UtilsService.views.getFTSConfigs();

        this.isSimple = UtilsService.views.isSimple();

        this.widgets = WidgetService.getWidgets(true);

        this.tiles = MapTilesFactory.createTiles();

        this.currentTile = this.tiles.filter(function(tile) {
            return !tile.overlay && tile.checked;
        })[0];

        this.switchMenu = function() {
            this.menuOpened = !this.menuOpened;
        };

        this.isLoading = function () {
            return MapService.settings.isLoading();
        };

        this.switchSearchVisibility = function() {
            self.searchVisible = !self.searchVisible;
        };

        this.search = function (data) {
            $log.info('Global search:', data);
            if (data.type === 'layer') {
                GlobalSearchService.params.page = 1;
                GlobalSearchService.search([data.name]);
            }
        };

        this.switchNavigation = function () {
            NavigationService.switch();
        };

        this.switchLayers = function() {
            this.layersOpened = !this.layersOpened;
        }

        this.layersEnable = function() {
            return MapProviderService.mapIsInit();
        };

        this.switchTile = function (tile) {
            if (tile.overlay) {
                $log.info('Tilelayer overlay:', tile);

                if (tile.checked) {
                    MapService.settings.tileAdd(tile);
                } else {
                    MapService.settings.tileRemove(tile);
                }
            }
        };

        this.searchSelect = function (data) {
            $log.info('Search data:', data);

            switch (data.Type) {
                case 'kladr':
                    UtilsService.layers.removeKladrPopup();

                    MapProviderService.setCurrentPosition(data.BBox, data.LatLng, -1, false);

                    UtilsService.layers.createKladrPopup(MapProviderService.getMap(), data.LatLng, data.Title);
                    break;
                case 'layer':
                    GlobalSearchService.rememberLayers();
                    
                    MapService.layers.globalSearchCreate([data]);

                    var config = UtilsService.views.getViewConfig(data.LayerId);

                    var layerKey = config.LayerId + '_' + LAYERS_PARAM_DEFS.SEARCH_PREFIX;

                    var dataItem = MapService.data.getItem(data.GeoObject.ID, layerKey);

                    var pos = UtilsService.layers.createCenter(dataItem.Geometry);

                    MapProviderService.setCurrentPosition(pos.bounds, pos.latlng, pos.zoom);

                    MapService.settings.popupOpen(config.LayerId, dataItem, pos.latlng);

                    if (config.Lazy !== 0 && !dataItem.LazyLoaded()) {
                        UtilsService.views.loadLazyProperties(dataItem, config.LayerId);
                    }
                    break;;
                default:
            }
        };

        this.clearSearch = function() {
            //MapService.layers.globalSearchClear();
            //GlobalSearchService.restoreLayers();
            //self.switchSearchVisibility();
            GlobalSearchService.dispose();
        };

        $scope.$watch(function() {
            return MapProviderService.mapIsInit();
        }, function(value) {
            if (value) {
                angular.forEach(self.tiles.filter(function(tile) { return tile.overlay; }), function(tile) {
                    $log.info('Tilelayer overlay init:', tile);

                    if (tile.checked) {
                        MapService.settings.tileAdd(tile);
                    }
                });

                MapService.settings.tileAdd(self.currentTile);
            }
        }, true);

        $scope.$watch(function () {
            return self.currentTile;
        }, function (newValue, oldValue) {
            $log.info('Tilelayer current:', newValue, oldValue);
            MapService.settings.tileRemove(oldValue);
            MapService.settings.tileAdd(newValue);

            angular.forEach(self.tiles.filter(function (tile) { return tile.overlay && tile.checked; }), function (tile) {
                $log.info('Tilelayer on switch:', tile);
                tile.layer.bringToFront();
            });
        });
    }

    angular.module('MapApp')
       .controller('MainController', mainController);

    mainController.$inject = ['$rootScope', '$scope', '$log', 'UtilsService', 'MapService', 'GlobalSearchService', '$location', 'NavigationService', 'deviceDetector', 'WidgetService', 'MapProviderService', 'GeoItem', 'LAYERS_PARAM_DEFS', 'MapTilesFactory'];
})(window.angular);