(function (angular) {
    function mnemonicController($scope, $log, $routeParams, $timeout, DataService, UtilsService, MapProviderService, MapService, deviceDetector, LAYERS_VIEW_DEFS, GeoItem) {
        var self = this;

        var loadTimer;

        this.isMobile = deviceDetector.isMobile();
        this.opened = true;
        this.defaultIcon = LAYERS_VIEW_DEFS.DEF_ICON;
        this.defaultColor = LAYERS_VIEW_DEFS.DEF_ICON_COLOR;
        this.search = '';
        this.selected = -1;

        this.data = [];
        this.page = 1;
        this.count = 0;
        this.pages = 0;
        this.size = 15;
        this.loading = true;

        this.config = UtilsService.views.getViewConfig($routeParams.layerId);

        $log.info('Params:', $routeParams);

        var openPopup = function (latlng) {
            var item = self.data[self.selected];
            if (angular.isDefined(item)) {
                MapService.settings.popupOpen(self.config.LayerId, new GeoItem(item), latlng);
            }
        };

        var closePopup = function () {
            var item = self.data[self.selected];
            if (angular.isDefined(item)) {
                MapService.settings.popupClose();
            }
        };

        var load = function () {
            self.loading = true;

            var filters = UtilsService.views.getFilterProperties(self.config.LayerId);

            DataService.loadLayerList(self.config.LayerId, self.page, self.search, filters).then(function (data) {
                $log.info('Mnemonic list:', data);

                self.selected = -1;
                self.data = data.Items;
                self.page = data.Page;
                self.count = data.TotalCount;
                self.pages = data.TotalPages;
                self.size = data.PageSize;

                self.loading = false;
            },
                function (err) {
                    $log.error('Load mnemonic list error...');
                });
        };

        this.isSearchable = function () {
            return true;
        };

        this.isLoad = function () {
            return this.loading || MapService.settings.isLoading();
        };

        this.update = function (page) {
            if (self.isLoad()) return;

            self.page = angular.isDefined(page) ? page : self.page;
            load();
        };

        this.forceupdate = function() {
            if (self.isLoad()) return;

            $timeout.cancel(loadTimer);

            loadTimer = $timeout(function () {
                self.update(1);
            }, 500);
        };

        this.focus = function (item, index) {
            if (self.isLoad()) return;

            if (self.selected !== index) {
                self.selected = index;

                if (self.config.IsLoadable && self.config.IsVisible()) {
                    //debugger;
                    var pos = UtilsService.layers.createCenter(item.Geometry);

                    MapProviderService.setCurrentPosition(pos.bounds, pos.latlng, pos.zoom);

                    openPopup(pos.latlng);
                }
            } else {
                //unselect
                closePopup();
                self.selected = -1;
            }
        };

        load();
    };

    angular.module('MapApp')
       .controller('MnemonicListController', mnemonicController);

    mnemonicController.$inject = ['$scope', '$log', '$routeParams', '$timeout', 'DataService', 'UtilsService', 'MapProviderService', 'MapService', 'deviceDetector', 'LAYERS_VIEW_DEFS', 'GeoItem'];
})(window.angular);
