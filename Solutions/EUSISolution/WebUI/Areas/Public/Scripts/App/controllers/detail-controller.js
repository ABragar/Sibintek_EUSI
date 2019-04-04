(function (angular) {
    function detailController($scope, $log, $routeParams, DataService, UtilsService, CRUDService, URLFactory, MapService, MapProviderService, WidgetService, deviceDetector, LAYERS_VIEW_DEFS, GeoItem) {

        var self = this;

        var openPopup = function (latlng) {
            MapService.settings.popupOpen(self.config.LayerId, new GeoItem(self.data), latlng);
        };

        this.isMobile = deviceDetector.isMobile();
        this.opened = true;
        this.isSimple = UtilsService.views.isSimple();
        this.data = {};
        this.config = UtilsService.views.getViewConfig($routeParams.layerId);
        this.defaultIcon = LAYERS_VIEW_DEFS.DEF_ICON;
        this.defaultColor = LAYERS_VIEW_DEFS.DEF_ICON_COLOR;
        this.dashboardUrl = '#';

        this.hasWidget = WidgetService.hasWidget(this.config, false);

        $log.info('Params:', $routeParams);

        DataService.loadDetailData(self.config.Mnemonic, $routeParams.id)
            .then(function (data) {
                $log.info('Detail data:', data);

                self.data = data;
                self.dashboardUrl = URLFactory.createObjectDashboardDetailUrl(self.data.ID, self.config.Mnemonic);

                if ($routeParams.action === 'move' && data.Location && data.Location.Disposition) {
                    if (self.config.IsLoadable && !self.config.IsVisible()) {
                        self.config.Checked = true;
                    }

                    var pos = UtilsService.layers.createCenter(data.Location.Disposition);

                    MapProviderService.setCurrentPosition(pos.bounds, pos.latlng, pos.zoom);

                    openPopup(pos.latlng);
                }
            },
            function (err) {
                $log.error('Load detail view error...');
            });

        this.hasValidTypes = function (properties) {
            return CRUDService.hasValidTypes(properties);
        };


        this.isFullColumn = function(propertyType) {
            return CRUDService.isFullColumn(propertyType);
        };

        this.getEnumValues = function (uitype) {
            var enums = CRUDService.getEnumValues(uitype);
            if (!CRUDService.isEnumLoaded(uitype)) {
                CRUDService.loadEnumValues(uitype);
            }
            return enums;
        };

        this.showCollection = function(properties, key) {
            $log.info('Show collection:', properties, key);
        };

    };

    angular.module('MapApp')
       .controller('DetailController', detailController);

    detailController.$inject = ['$scope', '$log', '$routeParams', 'DataService', 'UtilsService', 'CRUDService', 'URLFactory', 'MapService', 'MapProviderService', 'WidgetService', 'deviceDetector', 'LAYERS_VIEW_DEFS', 'GeoItem'];

})(window.angular);