(function (angular, L) {
    angular.module('MapApp')
        .factory('MapControlsFactory',
        [
            '$log', 'LAYERS_PARAM_DEFS', 'GLOBAL_PARAM_DEFS', function ($log, LAYERS_PARAM_DEFS, GLOBAL_PARAM_DEFS) {
                var MapControlsFactory = function() {

                    this.createWeater = function () {
                        return new L.Control.Weather();
                    };

                    this.createFocus = function (focusOnInit) {
                        focusOnInit = angular.isDefined(focusOnInit) ? focusOnInit : true;

                        return new L.Control.Focus({
                            navigator: [
                                {
                                    name: GLOBAL_PARAM_DEFS.CURRENT_REGION_NAME,
                                    latlng: GLOBAL_PARAM_DEFS.CURRENT_REGION_POSITION,
                                    zoom: GLOBAL_PARAM_DEFS.CURRENT_REGION_ZOOM,
                                    distance: {
                                        title: GLOBAL_PARAM_DEFS.CURRENT_CENTER_POINT_NAME_TO,
                                        latlng: GLOBAL_PARAM_DEFS.CURRENT_CENTER_POINT_POSITION
                                    }
                                }
                            ],
                            focusOnInit: focusOnInit
                        });
                    };

                    this.createCopyRight = function() {
                        return new L.Control.PbaCopyright();
                    };

                    this.createEasyPrint = function() {
                        return L.easyPrint({
                            title: 'Печать',
                            position: 'topleft',
                            elementsToHide:
                                '.leaflet-control-container, .map-head .btn, .input-group-search, .map-tile-layers-panel, .map-layer-filter, .panel-arrow, .leaflet-popup-content .btn-group'
                        });
                    };

                };


                return new MapControlsFactory();
            }
        ]);
;})(window.angular, window.L);