(function (angular) {
    angular.module('MapApp')
        .factory('GeoItem', ['$log', 'LazyProperties', 'eventEmitter', function ($log, LazyProperties, eventEmitter) {
            var GeoItem = function (data) {
                var lazyLoaded = false;
                var lazyProperties = null;

                this.ID = data.ID;
                this.Title = data.Title;
                this.Description = data.Description;
                this.Type = data.Type;
                this.Geometry = data.Geometry;
                this.Count = data.Count ? data.Count : 1;

                this.DistanceToPoint = 0;

                this.IsGeometry = (function() {
                    return !!data.Geometry;
                })();

                this.GeoType = (function () {
                    return data.Geometry && data.Geometry.type && data.Geometry.type.length ?
                        data.Geometry.type.toLowerCase() :
                        'undefined';
                })();

                this.IsPoint = function () {
                    return this.GeoType === 'point';
                };

                this.IsLinear = function () {
                    return (this.GeoType === 'linestring' || this.GeoType === 'multilinestring');
                };

                this.LazyLoaded = function() {
                    return lazyLoaded;
                };

                this.SetLazyProperties = function(properties) {
                    lazyLoaded = true;
                    lazyProperties = new LazyProperties(properties);
                    this.emit('OnLazyLoaded', lazyProperties);
                };

                this.GetLazyProperties = function() {
                    return lazyProperties;
                };

                this.GetStartDisposition = function() {
                    return lazyProperties && lazyProperties.StartDisposition ? lazyProperties.StartDisposition : 0;
                };
            };

            GeoItem.prototype.equals = function(geoItem) {
                return geoItem.ID === this.ID;
            };

            eventEmitter.inject(GeoItem);

            return GeoItem;
        }])
        .factory('GeoItemFactory', ['$log', 'GeoItem', function ($log, GeoItem) {
            var GeoItemFactory = function () {
                var self = this;

                this.CreateGeoItems = function(dataItems) {
                    var items = [];

                    if (dataItems && dataItems.length) {
                        for (var i = dataItems.length - 1; i >= 0; i--) {
                            items.push(new GeoItem(dataItems[i]));
                        }
                    }

                    return items;
                };
            };

            return new GeoItemFactory();
        }])
        .factory('LazyProperties', ['$log', 'LAZY_PROP_NAMES', function ($log, LAZY_PROP_NAMES) {
            var LazyProperties = function (data) {

                //$log.info('Lazy properties:', data);

                this.StartDisposition = data[LAZY_PROP_NAMES.StartDisposition];

            };

            return LazyProperties;
        }]);
})(window.angular);