(function (angular) {
    function mapController($scope, $log, UtilsService, MapService, MapProviderService, $location, NavigationService, MapControlsFactory) {

        var navigationStartKeyLocker = true;

        var needToMove = UtilsService.common.isMoveURL($location.$$path);

        this.data = {
            layers: MapService.data.layers,
            options: MapService.data.options
        };

        this.position = MapProviderService.getCurrentPosition();

        this.controls = [];
        this.controls.push(MapControlsFactory.createWeater());
        this.controls.push(MapControlsFactory.createFocus(!needToMove));
        this.controls.push(MapControlsFactory.createCopyRight());
        this.controls.push(MapControlsFactory.createEasyPrint());

        this.events = {
            'OnMapClick': function (latlng, zoom) {
                MapService.load.search(latlng, zoom);
            },
            'OnZoomEnd': function (bounds, center, zoom, point) {
                //All map changes (zoom\move) call 'OnMoveEnd' event
            },
            'OnMoveEnd': function (bounds, center, zoom, point) {
                MapProviderService.setCurrentPosition(bounds, {
                    lat: center.lat,
                    lng: center.lng
                }, zoom, true);

                //var currentBounds = MapProviderService.getUpdatedBoundsAndZoom();

                $log.info('Move event:', point, center, zoom, bounds);

                //if (currentBounds.zoom !== zoom || MapService.settings.checkDisplacement(point)) {
                //    MapService.load.layers(bounds, zoom);
                //}

                MapService.load.layers(UtilsService.layers.rotateBounds(bounds), zoom);
            },
            'OnLoad': function (bounds, center, zoom, point, map) {
                MapProviderService.setMap(map);
                MapService.settings.setDisplacementPoint(point);

                //MapProviderService.setUpdatedBoundsAndZoom(bounds, zoom); //Not necessarily
                MapService.load.border();
            },
            'OnMouseMove': function (latlng, zoom) {
                MapService.layers.searchLine(latlng, zoom);
            }
        };

        //Update counters for not Loadable layers
        UtilsService.views.loadConfigCounters(true);

        //$scope.$on('loadState', function (event, state) {
        //    $log.info('Map conroller: On load event broadcasted:', state);
        //});

        $scope.$watch(function () {
            return MapService.settings.isLoading();
        }, function (newValue, oldValue) {
            $log.info('Loading change: ' + oldValue + ' > ' + newValue);

            if (newValue === false && oldValue === true) {
                $log.info('End loading ...');

                MapService.settings.popupCheck();

                if (NavigationService.options.enable && navigationStartKeyLocker) {
                    navigationStartKeyLocker = false;
                    NavigationService.start();
                }
            }
        }, true);

    };

    angular.module('MapApp')
       .controller('MapController', mapController);

    mapController.$inject = ['$scope', '$log', 'UtilsService', 'MapService', 'MapProviderService', '$location', 'NavigationService', 'MapControlsFactory'];
})(window.angular);