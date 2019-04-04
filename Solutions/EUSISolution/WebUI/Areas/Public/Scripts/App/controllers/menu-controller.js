(function (angular) {
    function menuController($scope, $log, $cookies, $location, UtilsService, MapService, CadastreService, GlobalSearchService, LAYERS_VIEW_DEFS) {

        var updateCookies = function(config) {
            var visibilities = $cookies.get('view-config-visibility') ? JSON.parse($cookies.get('view-config-visibility')) : [];
            var index = visibilities.indexOf(config.LayerId);

            if (config.Checked) {
                if (index === -1)
                    visibilities.push(config.LayerId);
            } else {
                if (index > -1) {
                    visibilities.splice(index, 1);
                }
            }

            $cookies.put('view-config-visibility', JSON.stringify(visibilities));
        };

        var changeVisibility = function (configs, checked) {
            for (var i = configs.length - 1; i >= 0; i--) {
                var config = configs[i];

                config.Checked = checked;

                updateCookies(config);

                MapService.layers.update(config);

                if(config.Children && config.Children.length)
                    changeVisibility(config.Children, checked);
            }
        };

        this.configs = UtilsService.views.viewConfig;
        this.layerOptions = MapService.data.options;
        this.defaultIcon = LAYERS_VIEW_DEFS.DEF_ICON;
        this.defaultColor = LAYERS_VIEW_DEFS.DEF_ICON_COLOR;

        this.globalSearchParams = GlobalSearchService.params;

        this.getColor = function (layerId) {
            var config = UtilsService.views.getViewConfig(layerId);
            return config && config.Style && config.Style.Color ? config.Style.Color : this.defaultColor;
        };

        this.getIcon = function (layerId) {
            var config = UtilsService.views.getViewConfig(layerId);
            return config && config.Style && config.Style.Icon ? config.Style.Icon : this.defaultIcon;
        };

        this.visibilityChange = function(config) {
            changeVisibility([config], config.Checked);
        };

        this.hasVisible = function(config) {
            var hasVisible = false;

            var layerIds = UtilsService.views.getChildLayerIds(config.LayerId);

            if (layerIds.length) {
                angular.forEach(layerIds, function(layerId) {
                    var childConfig = UtilsService.views.getViewConfig(layerId);
                    if (childConfig.Checked) hasVisible = true;
                });
            }

            return hasVisible;
        };
        
        this.isLoading = function(config) {
            return MapService.settings.isLayerLoading(config.LayerId);
        };

        this.isFiltered = function(config) {
            return UtilsService.views.isFiltered(config.LayerId);
        };

        this.showItems = function (config) {
            if (config.IsLoadable && !config.IsSearchable) {

                if (!config.IsVisible()) {
                    config.Checked = true;
                    MapService.layers.update(config);
                }

                $log.info('Show items:', config);
                $location.path('/mnemoniclist/' + config.LayerId);
            }
        };

        this.isKadastre = function () {
            return CadastreService.isVisible() && !this.globalSearchParams.active;
        };

        this.isGlobalSearch = function() {
            return this.globalSearchParams.active;
        };

        //this.getSearchData = function() {
        //    return GlobalSearchService.data;
        //};

        //this.getSearchSelected = function() {
        //    return GlobalSearchService.selected;
        //};

        this.getSearchIsLoading = function() {
            return GlobalSearchService.isLoading();
        };

        //this.searchPages = function() {
        //    return GlobalSearchService.pages;
        //};

        //this.searchCount = function () {
        //    return GlobalSearchService.count;
        //};

        //this.searchPage = function () {
        //    return GlobalSearchService.page;
        //};

        //this.searchSize = function () {
        //    return GlobalSearchService.size;
        //};

        this.searchFocus = function (item, index) {
            GlobalSearchService.focus(item, index);
        };

        this.searchUpdate = function() {
            GlobalSearchService.search();
        };

        this.searchDispose = function() {
            GlobalSearchService.dispose();
        };
    };

    angular.module('MapApp')
       .controller('MenuController', menuController);

    menuController.$inject = ['$scope', '$log', '$cookies', '$location', 'UtilsService', 'MapService', 'CadastreService', 'GlobalSearchService', 'LAYERS_VIEW_DEFS'];
})(window.angular);