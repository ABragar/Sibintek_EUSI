(function (angular) {
    function searchController($scope, $log, UtilsService, MapService) {

        this.opened = true;

        this.data = MapService.data.data;
        this.options = MapService.data.options;

        this.layerIds = UtilsService.views.getLayerIds(true);

        this.switchAll = function() {
            var value = !this.hasChecked();
            var keys;
            for (var i = this.layerIds.length - 1; i >= 0; i--) {
                if (this.options[this.layerIds[i]]) {
                    keys = Object.keys(this.options[this.layerIds[i]]);
                    if (keys && keys.length > 1) {
                        for (var key = keys.length - 1; key >= 0; key--) {
                            if (keys[key] !== 'config') {
                                this.options[this.layerIds[i]][keys[key]] = value;
                            }
                        }
                    }
                }
            }
        };

        this.hasChecked = function () {
            var hasChecked = false;
            var keys;
            for (var i = this.layerIds.length - 1; i >= 0; i--) {
                if (this.options[this.layerIds[i]]) {
                    keys = Object.keys(this.options[this.layerIds[i]]);
                    if (keys && keys.length > 1) {
                        for (var key = keys.length - 1; key >= 0; key--) {
                            if (keys[key] !== 'config' && this.options[this.layerIds[i]][keys[key]]) {
                                hasChecked = true;
                                break;
                            }
                        }
                    }
                }
            }
            return hasChecked;
        };

        this.isSearchData = function(layerId) {
            return this.layerIds.indexOf(layerId) >= 0;
        };

        this.getColor = function(layerId) {
            var config = UtilsService.views.getViewConfig(layerId);
            return angular.isDefined(config.Style.Color) && angular.isString(config.Style.Color) ? config.Style.Color : UtilsService.layers.getDefaultColor();
        };

        this.getIcon = function (layerId) {
            var config = UtilsService.views.getViewConfig(layerId);
            return angular.isDefined(config.Style.Icon) && angular.isString(config.Style.Icon) ? config.Style.Icon : UtilsService.layers.getDefaultIcon();
        };


        this.getTitle = function(title, layerId) {
            var config = UtilsService.views.getViewConfig(layerId);
            return angular.isDefined(title) && angular.isString(title) ? title : config.Title;
        };
    };

    angular.module('MapApp')
       .controller('SearchController', searchController);

    searchController.$inject = ['$scope', '$log', 'UtilsService', 'MapService'];
})(window.angular);
