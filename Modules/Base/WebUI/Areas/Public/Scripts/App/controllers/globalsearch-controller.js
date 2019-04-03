(function (angular) {
    function globalSearchController($scope, $log, $routeParams, UtilsService, MapService, MapProviderService, LAYERS_VIEW_DEFS, GeoItem) {
        var self = this;

        this.opened = true;
        this.loading = false;
        this.selected = -1;
        this.page = 1;
        this.count = 0;
        this.size = 15;
        this.pages = 0;
        this.data = [];

        this.defaultIcon = LAYERS_VIEW_DEFS.DEF_ICON;
        this.defaultColor = LAYERS_VIEW_DEFS.DEF_ICON_COLOR;

        var openPopup = function (latlng) {
            var item = self.data[self.selected];
            if (angular.isDefined(item)) {
                MapService.settings.popupOpen(item.LayerId, new GeoItem(item.GeoObject), latlng);
            }
        };

        var closePopup = function () {
            var item = self.data[self.selected];
            if (angular.isDefined(item)) {
                MapService.settings.popupClose();
            }
        };

        this.isLoad = function () {
            return this.loading || MapService.settings.isLoading();
        };

        this.focus = function (item, index) {
            if (self.isLoad()) return;

            if (self.selected !== index) {
                self.selected = index;

                //debugger;

                var pos = UtilsService.layers.createCenter(item.GeoObject.Geometry);

                MapProviderService.setCurrentPosition(pos.bounds, pos.latlng, pos.zoom);

                console.log('Global:', item, pos);

                openPopup(pos.latlng);
            } else {
                //unselect
                closePopup();
                self.selected = -1;
            }
        };

        this.update = function () {
            this.loading = true;

            UtilsService.data.getGlobalSearchList($routeParams.query, this.page).then(function(data) {
                self.selected = -1;
                self.data = data.Items;
                self.page = data.Page;
                self.count = data.TotalCount;
                self.pages = data.TotalPages;
                self.size = data.PageSize;

                self.loading = false;

                $scope.$applyAsync();
            }, function (err) {
                self.loading = false;
                $scope.$applyAsync();
            });
        };

        this.getColor = function(layerId) {
            var config = UtilsService.views.getViewConfig(layerId);
            return config && config.Style && config.Style.Color ? config.Style.Color : this.defaultColor;
        };

        this.getIcon = function(layerId) {
            var config = UtilsService.views.getViewConfig(layerId);
            return config && config.Style && config.Style.Icon ? config.Style.Icon : this.defaultIcon;
        };

        this.update();
    };

    angular.module('MapApp')
       .controller('GlobalSearchController', globalSearchController);

    globalSearchController.$inject = ['$scope', '$log', '$routeParams', 'UtilsService', 'MapService', 'MapProviderService', 'LAYERS_VIEW_DEFS', 'GeoItem'];
})(window.angular);
