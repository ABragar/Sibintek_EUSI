(function (angular) {
    function globalSearchService($log, $timeout, DataService, MapService, UtilsService, MapProviderService, GeoItem) {

        var service = function () {

            var self = this;

            this.params = {
                query: '',
                layers: [],
                layerMemory: [],
                loading: false,
                active: false,
                selected: -1,
                page: 1,
                count: 0,
                size: 0,
                pages: 0,
                data: []
            };

            var restoreDefaults = function() {
                self.params.query = '';
                self.params.loading = false;
                self.params.active = false;
                self.params.selected = -1;
                self.params.page = 1;
                self.params.count = 0;
                self.params.size = 0;
                self.params.pages = 0;
                self.params.data = [];
                
                self.restoreLayers();
            };

            var openPopup = function (latlng) {
                var item = self.params.data[self.params.selected];
                if (angular.isDefined(item)) {
                    MapService.settings.popupOpen(item.LayerId, new GeoItem(item.GeoObject), latlng);
                }
            };

            var closePopup = function () {
                MapService.settings.popupClose();
            };

            this.rememberLayers = function () {
                //debugger;
                if (!this.params.layerMemory.length) {
                    this.params.layerMemory = UtilsService.views.viewConfigFlat.filter(function (config) {
                        return config.Checked;
                    }).map(function(config) {
                        return config.LayerId;
                    });

                    UtilsService.views.viewConfigFlat.forEach(function (config) {
                        config.Checked = false;
                        MapService.layers.update(config);
                    });
                }
            };

            this.restoreLayers = function() {
                this.params.layerMemory.forEach(function (layerId) {
                    var config = UtilsService.views.getViewConfig(layerId);
                    config.Checked = true;
                    MapService.layers.update(config);
                });

                this.params.layerMemory = [];
            };

            this.isLoading = function () {
                return this.params.loading || MapService.settings.isLoading();
            };

            this.search = function (layerIds) {
                this.rememberLayers();
                this.params.loading = true;
                this.params.active = true;

                if (angular.isDefined(layerIds)) {
                    this.params.layers = layerIds;
                }

                DataService.globalsearch(this.params.layers, this.params.query, this.params.page).then(function (data) {
                    MapService.layers.globalSearchCreate(data.Items);

                    $timeout(function () {
                        self.params.selected = -1;
                        self.params.data = data.Items;
                        self.params.page = data.Page;
                        self.params.count = data.TotalCount;
                        self.params.pages = data.TotalPages;
                        self.params.size = data.PageSize;
                        closePopup();
                        self.params.loading = false;
                    }, 0);
                }, function (err) {
                    closePopup();
                    self.params.loading = false;
                });
            };

            this.focus = function(item, index) {
                if (self.isLoading()) return;

                if (self.params.selected !== index) {
                    self.params.selected = index;

                    var pos = UtilsService.layers.createCenter(item.GeoObject.Geometry);

                    MapProviderService.setCurrentPosition(pos.bounds, pos.latlng, pos.zoom);

                    //console.log('Global:', item, pos);

                    openPopup(pos.latlng);
                } else {
                    //unselect
                    closePopup();
                    self.params.selected = -1;
                }
            };

            this.dispose = function () {
                closePopup();
                restoreDefaults();
                MapService.layers.globalSearchClear();
            };
        };

        return new service();
    };

    globalSearchService.$inject = ['$log', '$timeout', 'DataService', 'MapService', 'UtilsService', 'MapProviderService', 'GeoItem'];

    angular.module('MapApp').
        factory('GlobalSearchService', globalSearchService);

})(window.angular);