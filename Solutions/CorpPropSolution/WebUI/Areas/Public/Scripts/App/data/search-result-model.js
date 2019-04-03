(function (angular, L) {
    angular.module('MapApp')
        .factory('SearchResult',
        [
            '$log', 'LAYERS_PARAM_DEFS', function ($log, LAYERS_PARAM_DEFS) {
                var SearchResult = function (data) {
                    var self = this;

                    this.Title = data.Title;
                    this.Type = data.Type;

                    this.BBox = (function () {
                        var layer, bounds;

                        if (data.BBox && data.BBox.length === 4) {
                            return L.latLngBounds(L.latLng(data.BBox[0], data.BBox[1]), L.latLng(data.BBox[2], data.BBox[3]));;
                        };

                        if (data.GeoObject && data.GeoObject.Geometry) {
                            layer = L.geoJson(data.GeoObject.Geometry);
                            bounds = layer.getBounds();

                            return bounds;
                        };

                        return null;
                    })();

                    this.LatLng = (function () {
                        if (self.BBox) {
                            return self.BBox.getCenter();
                        };
                        return null;
                    })();

                    this.GeoObject = data.GeoObject ? data.GeoObject : null;

                    this.LayerId = (function () {
                        return angular.isDefined(data.LayerId) ? data.LayerId : LAYERS_PARAM_DEFS.KLADRADDRESS_LAYERID;
                    })();
                };

                return SearchResult;
            }
        ])
        .factory('SearchResultFactory',
        [
            '$log', 'SearchResult', function ($log, SearchResult) {
                var SearchResultFactory = function() {
                    this.CreateResultByLayers = function(typeName, dataItems) {
                        var items = [];

                        if (dataItems && dataItems.length) {
                            for (var i = dataItems.length - 1; i >= 0; i--) {
                                items.push(new SearchResult({
                                    Title: dataItems[i].GeoObject.Title,
                                    LayerId: dataItems[i].LayerId,
                                    Type: typeName,
                                    GeoObject: dataItems[i].GeoObject
                                }));
                                $log.info(dataItems[i]);
                            }
                        }

                        return items;
                    };

                    this.CreateResultByYandex = function (typeName, dataItems) {
                        var items = [];

                        if (dataItems && dataItems.length) {
                            for (var i = dataItems.length - 1; i >= 0; i--) {
                                items.push(new SearchResult({
                                    Title: dataItems[i].description + ', ' + dataItems[i].name,
                                    Type: typeName,
                                    GeoObject: {
                                        Description: dataItems[i].description + ', ' + dataItems[i].name,
                                        ID: -1,
                                        Title: dataItems[i].description + ', ' + dataItems[i].name,
                                        Type: 1,
                                        Geometry: {
                                            coordinates: dataItems[i].Point.pos.split(' ').reverse(),
                                            crs: {
                                                properties: {
                                                    name: 'EPSG:4326'
                                                },
                                                type: 'name'
                                            },
                                            type: "Point"
                                        }
                                    },
                                    BBox: dataItems[i].boundedBy.Envelope.upperCorner.split(' ').reverse().concat(dataItems[i].boundedBy.Envelope.lowerCorner.split(' ').reverse())
                                }));
                                //$log.info(dataItems[i]);
                            }
                        }

                        return items;
                    };
                };


               

                return new SearchResultFactory();
            }
        ]);
})(window.angular, window.L);