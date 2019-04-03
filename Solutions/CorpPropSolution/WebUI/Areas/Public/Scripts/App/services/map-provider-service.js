(function (angular) {
    function mapProviderService($log) {

        var service = function () {
            var self = this;

            var settings = {
                lastUpdateBoundsAndZoom: {
                    bounds: [],
                    zoom: 0
                },
                currentPosition: {
                    bounds: [],
                    position: {
                        lat: 0,
                        lng: 0
                    },
                    zoom: 0,
                    setview: false
                }
            };

            this.setUpdatedBoundsAndZoom = function(b, z) {
                settings.lastUpdateBoundsAndZoom.bounds = b;
                settings.lastUpdateBoundsAndZoom.zoom = z;
            };

            this.getUpdatedBoundsAndZoom = function() {
                return {
                    bounds: settings.lastUpdateBoundsAndZoom.bounds,
                    zoom: settings.lastUpdateBoundsAndZoom.zoom
                }
            };

            this.setCurrentPosition = function (bounds, pos, z, setview) {
                settings.currentPosition.bounds = bounds;
                settings.currentPosition.position.lat = pos.lat;
                settings.currentPosition.position.lng = pos.lng;
                settings.currentPosition.zoom = z;
                settings.currentPosition.setview = angular.isDefined(setview) ? setview : false;
            };

            this.getCurrentPosition = function () {
                return settings.currentPosition;
            };

            this.map = null;

            this.mapIsInit = function () {
                return self.map !== null;
            };

            this.setMap = function (m) {
                self.map = m;
            };

            this.getMap = function () {
                return self.map;
            };

            this.removeLayer = function (layer) {
                if (self.mapIsInit()) {
                    self.map.removeLayer(layer);
                }
            };
        };

        return new service();
    };

    mapProviderService.$inject = ['$log'];

    angular.module('MapApp').
        factory('MapProviderService', mapProviderService);

})(window.angular);