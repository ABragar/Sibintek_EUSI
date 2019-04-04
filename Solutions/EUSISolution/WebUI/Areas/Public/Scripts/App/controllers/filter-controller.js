(function (angular) {
    function filterController($scope, $log, $routeParams, UtilsService, MapService, CRUD_PARAM_DEFS, deviceDetector) {
        var self = this;

        this.isMobile = deviceDetector.isMobile();
        this.opened = true;

        //var layerId = $routeParams.layerId;

        var config = UtilsService.views.getViewConfig($routeParams.layerId);

        this.title = config.Title;

        this.filters = config.Filters;

        if (!config.Filters || !config.Filters.length) {
            $log.info('Config filters is empty, try to load:', config.Filters);
            UtilsService.views.loadFilterConfig(config).then(function (filters) {
                self.filters = filters;
            });
        } else {
            $log.info('Config filters already loaded:', config.Filters);
        }
        
        this.model = UtilsService.views.getFilter(config.LayerId);

        this.checkUIType = function(type, uitype) {
            return CRUD_PARAM_DEFS.FILTER_VIEW_PROP_TYPES.indexOf(type) !== -1 &&
                CRUD_PARAM_DEFS.FILTER_VIEW_UI_PROP_TYPES.indexOf(uitype) !== -1;
        };

        this.clearFilter = function () {
            //Clear all filters for config
            UtilsService.views.clearFilter(config.LayerId);

            if (config.IsLoadable) {
                //Set counter to 'need to update' state
                UtilsService.views.counterUpdatedChangeState(config.LayerId, false);

                //Update layer
                MapService.load.layer(config, true);
            } else {
                //Just update counter
                UtilsService.views.loadConfigCounter(config.LayerId);
            }

            var layerIds = UtilsService.views.getChildLayerIds(config.LayerId);

            if (layerIds.length) {
                angular.forEach(layerIds, function (childLayerId) {
                    var childConfig = UtilsService.views.getViewConfig(childLayerId);

                    if (childConfig.IsFilterable) {
                        //Clear all filters for config
                        UtilsService.views.clearFilter(childLayerId);

                        if (childConfig.IsLoadable && childConfig.IsDynamic) {
                            if (childConfig.Checked) {
                                //Set counter to 'need to update' state
                                UtilsService.views.counterUpdatedChangeState(childLayerId, false);

                                //Update layer
                                MapService.load.layer(childConfig);
                            } else {
                                //Just update counter
                                UtilsService.views.loadConfigCounter(childLayerId);

                                //Set layer to 'Need to update on visibility change' state
                                MapService.layers.remember(childLayerId);
                            }
                        } else {
                            UtilsService.views.loadConfigCounter(childLayerId);
                        }
                    }
                });
            }

            //TODO: Clear all parent filters if all child filters is clear

            //UtilsService.views.setFiltered(mnemonic, false);
        };

        this.updateFilter = function () {            
            //Create new filter string
            UtilsService.views.updateFilterProperties(config.LayerId);

            if (config.IsLoadable) {
                //Set counter to 'need to update' state
                UtilsService.views.counterUpdatedChangeState(config.LayerId, false);

                //Update layer
                MapService.load.layer(config, true);
            } else {
                //Just update counter
                UtilsService.views.loadConfigCounter(config.LayerId);
            }

            var layerIds = UtilsService.views.getChildLayerIds(config.LayerId);

            //$log.info('Try to update child filter:', mnemonics);

            if (layerIds.length) {
                angular.forEach(layerIds, function (childLayerIds) {                    
                    var childConfig = UtilsService.views.getViewConfig(childLayerIds);

                    if (childConfig.IsFilterable) {
                        //debugger;
                        //Prepare child filter properties with parent filter config
                        UtilsService.views.updateFilterProperties(childLayerIds, config.Filters, self.model);

                        if (childConfig.IsLoadable && childConfig.IsDynamic) {
                            if (childConfig.Checked) {
                                //Set counter to 'need to update' state
                                UtilsService.views.counterUpdatedChangeState(childLayerIds, false);
                                //Update layer
                                MapService.load.layer(childConfig);
                            } else {
                                //Just update counter
                                UtilsService.views.loadConfigCounter(childLayerIds);
                                //Set layer to 'Need to update on visibility change' state
                                MapService.layers.remember(childLayerIds);
                            }
                        } else {
                            UtilsService.views.loadConfigCounter(childLayerIds);
                        }
                    }
                });
            }

            //UtilsService.views.setFiltered(mnemonic, true);
        };
    };

    angular.module('MapApp')
       .controller('FilterController', filterController);

    filterController.$inject = ['$scope', '$log', '$routeParams', 'UtilsService', 'MapService', 'CRUD_PARAM_DEFS', 'deviceDetector'];
})(window.angular);
