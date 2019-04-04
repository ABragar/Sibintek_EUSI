(function (angular, L) {
    angular.module('MapApp')
        .factory('MapTileLayer', ['$log', function ($log) {
                var MapTileLayer = function (data) {
                    this.name = angular.isDefined(data.name) ? data.name : 'Default layer name';
                    this.layer = data.layer;
                    this.overlay = angular.isDefined(data.overlay) ? data.overlay : false;
                    this.checked = angular.isDefined(data.checked) ? data.checked : false;
                };

                return MapTileLayer;
            }])
        .factory('MapTilesFactory',
        [
            '$log', '$timeout', 'LAYERS_PARAM_DEFS', 'GLOBAL_PARAM_DEFS', 'CadastreService', 'MapTileLayer', function ($log, $timeout, LAYERS_PARAM_DEFS, GLOBAL_PARAM_DEFS, CadastreService, MapTileLayer) {
                var MapTilesFactory = function () {
                    var self = this;

                    this.createTileLayer = function (url, options) {
                        options = angular.extend({
                            maxNativeZoom: 18,
                            minZoom: LAYERS_PARAM_DEFS.MIN_ZOOM,
                            maxZoom: LAYERS_PARAM_DEFS.MAX_ZOOM
                        }, (options || {}));

                        return L.tileLayer(url, options);
                    };

                    this.createOSM = function(checked) {
                        return new MapTileLayer({
                            name: 'Open street map',
                            layer: self.createTileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png'),
                            checked: checked
                        });
                    };

                    this.createMapQuest = function (checked) {
                        return new MapTileLayer({
                            name: 'Map quest',
                            layer: self.createTileLayer('http://otile4.mqcdn.com/tiles/1.0.0/osm/{z}/{x}/{y}.png'), //UNSECURE SSL
                            checked: checked
                        });
                    };

                    this.createOCM = function(checked) {
                        return new MapTileLayer({
                            name: 'Open cycle map',
                            layer: self.createTileLayer('http://{s}.tile.thunderforest.com/transport-dark/{z}/{x}/{y}.png'),
                            checked: checked
                        });
                    };

                    this.createMapBox = function (checked) {
                        return new MapTileLayer({
                            name: 'Map box',
                            layer: self.createTileLayer('http://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
                                id: 'mapbox.streets',
                                accessToken:
                                'pk.eyJ1IjoiYWx0YXJpY2thIiwiYSI6ImNpa3BidXNoaTExMWh1Mm02YTY1ZXlvZXkifQ.fRS4SvpNrcqc7ZFKQSUJxA'
                            }),
                            checked: checked
                        });
                    };

                    this.createArcGISDefault = function(checked) {
                        return new MapTileLayer({
                            name: 'ESRI ArcGIS',
                            layer: self.createTileLayer('http://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}'),
                            checked: checked
                        });
                    };

                    this.createArcGISAero = function (checked) {
                        return new MapTileLayer({
                            name: 'ESRI ArcGIS Aero',
                            layer: self.createTileLayer('http://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}'),
                            checked: checked
                        });
                    };

                    this.createYandexDefault = function(checked) {
                        return new MapTileLayer({
                            name: 'Яндекс',
                            layer: new L.Yandex('publicMap'),
                            checked: checked
                        });
                    };

                    this.createYandexHybrid = function (checked) {
                        return new MapTileLayer({
                            name: 'Яндекс',
                            layer: new L.Yandex('hybrid'),
                            checked: checked
                        });
                    };

                    this.createGoogleHybrid = function (checked) {
                        return new MapTileLayer({
                            name: 'Google Гибрид',
                            layer: new L.Google('HYBRID'),
                            checked: checked
                        });
                    };

                    this.createWeather = function(checked) {
                        return new MapTileLayer({
                            name: 'Погода', //NO SSL
                            layer: L.OWM.current({
                                intervall: 15,
                                imageLoadingUrl: '/areas/public/content/images/gears.svg',
                                lang: 'ru',
                                minZoom: 5,
                                appId: '9dab5710a991de7a46a842d941e845ae'
                            }),
                            checked: checked,
                            overlay: true
                        });
                    };

                    this.createCadastre = function(checked) {
                        return new MapTileLayer({
                            name: 'Кадастр',
                            layer: L.tileLayer
                                .Rosreestr('http://{s}.pkk5.rosreestr.ru/arcgis/rest/services/Cadastre/Cadastre/MapServer/export?dpi=96&transparent=true&format=png32&bbox={bbox}&size=1024,1024&bboxSR=102100&imageSR=102100&f=image',
                                {
                                    tileSize: 1024,
                                    attribution: 'Rosreestr',
                                    clickable: true,
                                    onAdd: function(map) {
                                        //$log.info('Overlay add:', map);
                                        $timeout(function() {
                                                CadastreService.visibility(true);
                                            },
                                            0);

                                        $('.leaflet-overlay-pane svg.leaflet-zoom-animated')
                                            .attr('class', 'leaflet-zoom-animated without-events');
                                    },
                                    onRemove: function(map) {
                                        //$log.info('Overlay remove:', map);
                                        $timeout(function() {
                                                CadastreService.visibility(false);
                                            },
                                            0);

                                        $('.leaflet-overlay-pane svg.leaflet-zoom-animated')
                                            .attr('class', 'leaflet-zoom-animated');
                                    }
                                }).on('click',
                                    function(e) {
                                        //$log.info('Overlay:', e);
                                        $timeout(function() {
                                                CadastreService.visibility(true);
                                            },
                                            0);

                                        CadastreService.search(e.latlng.lat, e.latlng.lng, e.target._map.getZoom());
                                        e.originalEvent.stopPropagation();
                                    }),
                            overlay: true,
                            checked: checked
                        });
                    };

                    this.createTiles = function() {
                        return [
                            this.createOSM(true),
                            //this.this.createMapQuest(),
                            this.createOCM(),
                            this.createMapBox(),
                            this.createArcGISDefault(),
                            this.createArcGISAero(),
                            this.createYandexDefault(),
                            //this.createYandexHybrid(),
                            this.createGoogleHybrid(),
                            this.createWeather(),
                            this.createCadastre()
                        ];
                    };
                };

                return new MapTilesFactory();
            }
        ]);
})(window.angular, window.L);