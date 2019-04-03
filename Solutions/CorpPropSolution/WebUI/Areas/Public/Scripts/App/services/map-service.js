(function (angular) {
    function mapService($log, $location, $timeout, DataService, UtilsService, URLFactory, MapProviderService, LAYERS_PARAM_DEFS, GLOBAL_PARAM_DEFS, SignalService, GeoItemFactory, GeoItem) {

        var service = function () {

            var self = this;

            var lastBoundAndZoom = {};
            var reloadTimer;
            var loadMemory = {};
            var lastPoint = { x: 0, y: 0 };

            var loadLayer = function (config) {
                if (!angular.isDefined(self.data.options[config.LayerId])) {
                    self.data.options[config.LayerId] = {
                        config: {}
                    }
                }

                self.data.options[config.LayerId].config.loading = true;

                var filters = UtilsService.views.getFilterProperties(config.LayerId);

                var boundsAndZoom = MapProviderService.getUpdatedBoundsAndZoom();

                var bounds = boundsAndZoom.bounds;
                var zoom = boundsAndZoom.zoom;

                $log.info('Start request for layer (' + config.LayerId + '):', filters);

                //UtilsService.layers.popupDetach();

                DataService.loadLayer(config, filters, bounds, zoom)
                    .then(function (dataItems) {
                        $log.info('Get response for layer (' + config.LayerId + '):', dataItems);

                        var isUpdated = angular.isDefined(self.data.data[config.LayerId]);

                        self.data.data[config.LayerId] = GeoItemFactory.CreateGeoItems(dataItems);

                        var state = isUpdated ? 'refresh' : (config.Checked ? 'visible' : 'hidden');

                        self.data.layers[config.LayerId] = UtilsService.layers.createLayers(config, self.data.data[config.LayerId], self.load.search);

                        self.data.options[config.LayerId].config.state = state;
                        self.data.options[config.LayerId].config.clientCluster = config.IsClientCluster;
                        self.data.options[config.LayerId].config.serverCluster = config.IsServerCluster;
                        self.data.options[config.LayerId].config.clusterLevel = config.ServerClusteringMaxZoom;
                        self.data.options[config.LayerId].config.options = UtilsService.layers.createLayerOptions(config);

                        lastBoundAndZoom[config.LayerId] = {
                            bounds: [bounds[0], bounds[1], bounds[2], bounds[3]],
                            zoom: zoom
                        };

                        if (!UtilsService.views.counterUpdated(config.LayerId))
                            UtilsService.views.loadConfigCounter(config.LayerId);

                    }, function (reason) {
                        $log.error('Request for (' + config.LayerId + ') was aborted: ' + reason);
                        self.data.options[config.LayerId].config.loading = false;
                    });
            };

            var dataIsNew = function (dataItems, dataItem) {
                if (!dataItems || !dataItems.length) return true;

                return dataItems.filter(function (item) {
                    return item.equals(dataItem);
                }).length === 0;
            };

            var hasData = function (layerId) {
                return angular.isDefined(self.data.data[layerId]) && angular.isDefined(self.data.options[layerId]);
            };

            var mapChanged = function (layerId) {
                return !angular.equals(lastBoundAndZoom[layerId], MapProviderService.getUpdatedBoundsAndZoom());
            };

            var checkChildLoading = function (layerId) {
                var loading = false, childLayerIds, options;

                var layerIds = UtilsService.views.getChildLayerIds(layerId);

                for (var i = layerIds.length - 1; i >= 0; i--) {
                    childLayerIds = layerIds[i];
                    options = self.data.options[childLayerIds];

                    if ((angular.isDefined(options) && angular.isDefined(options.config) &&
                        options.config.loading) || checkChildLoading(childLayerIds)) {
                        loading = true;
                        break;;
                    }
                }

                return loading;
            };

            var updateTracking = function (tracks) {

                if (!GLOBAL_PARAM_DEFS.TRACK_LAYERS.length)
                    return;

                if (!tracks || !tracks.length)
                    return;

                for (var i = tracks.length - 1; i >= 0; i--) {
                    var track = tracks[i];
                    var layerId = '', dataItem = null;

                    for (var y = GLOBAL_PARAM_DEFS.TRACK_LAYERS.length - 1; y >= 0; y--) {
                        layerId = GLOBAL_PARAM_DEFS.TRACK_LAYERS[y];

                        //if (angular.isUndefined(self.data.data[mnemonic])) continue;

                        dataItem = self.data.getItem(track.ID, layerId);

                        if (dataItem && dataItem.IsPoint()) {
                            //exists = true;
                            dataItem.Geometry = track.Location.Disposition;
                            break;
                        }
                    }

                    if (dataItem && angular.isArray(self.data.layers[layerId])) {
                        for (var l = self.data.layers[layerId].length - 1; l >= 0; l--) {
                            var layer = self.data.layers[layerId][l];

                            if (layer.options.id === track.ID) {
                                if (layer.setLatLng) {

                                    var point = UtilsService.layers.createPoint(dataItem.Geometry);
                                    layer.setLatLng(point);

                                    var popup = UtilsService.layers.getPopup();
                                    if (popup && popup.layerId === layerId && popup.id === track.ID) {
                                        popup.popup.setLatLng(point);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            };


            this.data = {
                data: {},
                layers: {},
                options: {},
                getItem: function (id, layerId) {
                    var dataItem = null,
                        dataItems = self.data.data[layerId];

                    if (angular.isArray(dataItems)) {
                        dataItem = dataItems.filter(function (item) {
                            return item.ID === id;
                        })[0];
                    }

                    return dataItem;
                }
            };

            this.load = {
                border: function () {
                    DataService.loadBorder().then(function(response) {
                        self.data.data[LAYERS_PARAM_DEFS.BORDER_LAYERID] = new GeoItem(response);

                        var layer = UtilsService.layers.createBorder(self.data.data[LAYERS_PARAM_DEFS.BORDER_LAYERID], LAYERS_PARAM_DEFS.BORDER_LAYERID, self.load.search);

                        self.data.layers[LAYERS_PARAM_DEFS.BORDER_LAYERID] = [layer];

                        self.data.options[LAYERS_PARAM_DEFS.BORDER_LAYERID] = {
                            config: {
                                state: 'visible',
                                clientCluster: false,
                                serverCluster: false,
                                clusterLevel: -1,
                                options: {}
                            }
                        };
                    }, function(err) {
                        $log.error('Border load error...');
                    });
                },
                layers: function (bounds, zoom) {
                    if (angular.isDefined(bounds) && angular.isDefined(zoom)) {
                        MapProviderService.setUpdatedBoundsAndZoom(bounds, zoom);
                    }

                    if (angular.isDefined(reloadTimer)) {
                        $timeout.cancel(reloadTimer);
                    }

                    //First load start immediately
                    var firstLoad = Object.keys(self.data.data).length <= 0;

                    reloadTimer = $timeout(function () {
                        var config,
                            configs = UtilsService.views.getVisibleConfigs();

                        for (var i = configs.length - 1; i >= 0; i--) {
                            config = configs[i];

                            //Data never loaded or config is dynamic
                            if (!hasData(config.LayerId) || config.IsDynamic)
                                loadLayer(config);
                        }
                    }, firstLoad ? 0 : GLOBAL_PARAM_DEFS.LAYER_RELOAD_TIMEOUT);
                },
                layer: function (config, force) {
                    if (config.IsLoadable && (!hasData(config.LayerId) || config.IsDynamic || force))
                        loadLayer(config);
                },
                search: function (latlng, zoom, multiple) {
                    multiple = angular.isDefined(multiple) ? multiple : false;

                    //getNorthWest
                    //getSouthEast

                    var bound = UtilsService.layers.createBounds(latlng, (-7 + 1.5 * (zoom > 6 ? zoom : 6)) * (1 << (18 - zoom)));

                    $log.info('Bounded point:', bound, [bound.getNorthWest(), bound.getSouthEast()], zoom);

                    $log.info('lat lng:', latlng);

                    var layerIds = UtilsService.views.getLayerIds(true);

                    $log.info('Search layers ID:', layerIds);

                    if (layerIds.length) {
                        DataService.searchLayers(layerIds,
                                latlng.lat,
                                latlng.lng,
                                [
                                    bound.getSouthWest().lat, bound.getSouthWest().lng, bound.getNorthEast().lat, bound
                                    .getNorthEast().lng
                                ],
                                zoom,
                                !multiple)
                            .then(function (response) {
                                $log.info('Response by search:', response);

                                if (response) {
                                    var responseLayersIds = Object.keys(response);

                                    for (var i = responseLayersIds.length - 1; i >= 0; i--) {
                                        var layerId = responseLayersIds[i];
                                        var dataItems = GeoItemFactory.CreateGeoItems(response[layerId]);

                                        $log.info('Response by search bound:', dataItems, layerId);

                                        if (angular.isArray(dataItems) && dataItems.length) {

                                            var config = UtilsService.views.getViewConfig(layerId);

                                            var item, dataItem;

                                            if (!self.data.data[layerId] || !multiple) {

                                                var state = !multiple && self.data.data[layerId] ? 'refresh' : 'visible';

                                                self.data.data[layerId] = dataItems;

                                                self.data.layers[layerId] = UtilsService.layers.createLayers(config, dataItems);

                                                //Watch in leaflet directive
                                                self.data.options[layerId] = {
                                                    config: {
                                                        state: state,
                                                        clientCluster: config.IsClientCluster,
                                                        serverCluster: config.IsServerCluster,
                                                        options: UtilsService.layers.createLayerOptions(config),
                                                        clusterLevel: config.ServerClusteringMaxZoom
                                                    }
                                                };

                                                for (item = dataItems.length - 1; item >= 0; item--) {
                                                    dataItem = dataItems[item];
                                                    self.data.options[layerId][dataItem.ID] = true;
                                                }
                                            } else {
                                                for (item = dataItems.length - 1; item >= 0; item--) {
                                                    dataItem = new GeoItem(dataItems[item]);

                                                    if (dataIsNew(self.data.data[layerId], dataItem)) {

                                                        self.data.data[layerId].push(dataItem);

                                                        //Create layer method return array 0 to ..., then concat
                                                        self.data.layers[layerId] = self.data.layers[layerId].concat(UtilsService.layers.createLayer(config, dataItem));
                                                    }

                                                    self.data.options[layerId][dataItem.ID] = true;
                                                }

                                                self.data.options[layerId].config.state = 'visible';
                                            }
                                        }

                                    }

                                    if (multiple) $location.path('/searchdetail');
                                }
                                },
                                function(err) {
                                    $log.error('Layer search error...');
                            });
                    }
                }
            }

            this.layers = {
                get: function (layerId) {
                    return self.data.layers[layerId];
                },
                update: function (config, force) {
                    if (!config.IsLoadable || config.IsSearchable) return;

                    if (angular.isDefined(self.data.options[config.LayerId]) &&
                        angular.isDefined(self.data.options[config.LayerId].config)) {
                        self.data.options[config.LayerId].config.state = config.Checked ? 'visible' : 'hidden';
                    }

                    if (config.Checked && (!hasData(config.LayerId) || (mapChanged(config.LayerId) && !config.IsStatic) || force || loadMemory[config.LayerId])) {
                        self.layers.forget(config.LayerId);
                        loadLayer(config);
                    }
                },
                remember: function (layerId) {
                    loadMemory[layerId] = true;
                },
                forget: function (layerId) {
                    loadMemory[layerId] = false;
                },
                updateNavigation: function (lat, lng) {
                    self.data.data[LAYERS_PARAM_DEFS.NAVIGATION_LAYERID] = [lat, lng];

                    var layer = UtilsService.layers.createNavigator([lat, lng], LAYERS_PARAM_DEFS.NAVIGATION_LAYERID, self.load.search);

                    self.data.layers[LAYERS_PARAM_DEFS.NAVIGATION_LAYERID] = [layer];

                    if (angular.isUndefined(self.data.options[LAYERS_PARAM_DEFS.NAVIGATION_LAYERID])) {
                        self.data.options[LAYERS_PARAM_DEFS.NAVIGATION_LAYERID] = {
                            config: {
                                state: 'visible',
                                clientCluster: false,
                                serverCluster: false,
                                clusterLevel: -1,
                                options: {}
                            }
                        };
                    } else {
                        //refresh
                        self.data.options[LAYERS_PARAM_DEFS.NAVIGATION_LAYERID].config.state = 'refresh';
                    }
                },
                clearNavigation: function () {
                    self.data.data[LAYERS_PARAM_DEFS.NAVIGATION_LAYERID] = [];
                    self.data.layers[LAYERS_PARAM_DEFS.NAVIGATION_LAYERID] = [];
                    self.data.options[LAYERS_PARAM_DEFS.NAVIGATION_LAYERID].config.state = 'refresh';
                },
                searchLine: function (latlng, zoom) {
                    if (zoom < 10) return;

                    var popup = UtilsService.layers.getPopup();

                    if (popup.fixed) return;

                    if (MapProviderService.mapIsInit()) {
                        //searchLineLocker.enter(function (innerToken) {
                        //    lock.leave(innerToken);
                        //});

                        var layerIds = Object.keys(self.data.layers);

                        if (layerIds.length) {
                            var layers = [], layer;

                            angular.forEach(layerIds, function (layerId) {
                                if (layerId !== LAYERS_PARAM_DEFS.BORDER_LAYERID && self.data.layers[layerId] && self.data.layers[layerId].length) {
                                    for (var i = self.data.layers[layerId].length - 1; i >= 0; i--) {
                                        layer = self.data.layers[layerId][i];
                                        if (layer._map && (UtilsService.layers.isPolyline(layer) || UtilsService.layers.isMultiPolyline(layer)) &&
                                            !layer.options.fake) {
                                            layers.push(layer);
                                        }
                                    }
                                }
                            });

                            var data = L.GeometryUtil.closestLayerSnap(MapProviderService.map, layers, latlng, LAYERS_PARAM_DEFS.SNAP_RADIUS);

                            if (data && data.layer) {
                                var closest = L.GeometryUtil.closest(MapProviderService.map, data.layer, latlng);
                                var id = data.layer.options.id;
                                var layerId = data.layer.options.layerId;

                                var dataItem = self.data.getItem(id, layerId);

                                dataItem = dataItem ? dataItem : self.data.getItem(id, layerId + '_' + LAYERS_PARAM_DEFS.SEARCH_PREFIX);

                                if (dataItem) {
                                    dataItem.DistanceToPoint = UtilsService.common.getDistanceFromStart(data.layer, latlng);
                                    self.settings.popupOpen(layerId, dataItem, closest, true);
                                    data.layer.fire('mouseover');
                                    return;
                                }
                            }

                            if (popup.pointer)
                                self.settings.popupClose();
                        }
                    }
                },
                globalSearchCreate: function (data) {
                    var layerIds = [];

                    data.forEach(function (item) {
                        if (layerIds.indexOf(item.LayerId) === -1)
                            layerIds.push(item.LayerId);
                    });

                    var dataItems = {}, state, config, layerKey, newLayerIds = [];

                    layerIds.forEach(function (layerId) {
                        layerKey = layerId + '_' + LAYERS_PARAM_DEFS.SEARCH_PREFIX;

                        newLayerIds.push(layerKey);

                        config = UtilsService.views.getViewConfig(layerId);

                        dataItems = data.filter(function (item) {
                            return item.LayerId === layerId;
                        }).map(function (item) {
                            return new GeoItem(item.GeoObject);
                        });

                        state = self.data.data[layerKey] ? 'refresh' : 'visible';

                        self.data.data[layerKey] = dataItems; //GeoItemFactory.CreateGeoItems(dataItems);

                        self.data.layers[layerKey] = UtilsService.layers.createLayers(config, self.data.data[layerKey]);

                        //Watch in leaflet directive
                        self.data.options[layerKey] = {
                            config: {
                                state: state,
                                clientCluster: true,
                                serverCluster: false,
                                options: UtilsService.layers.createLayerOptions(config),
                                clusterLevel: config.ServerClusteringMaxZoom
                            }
                        };

                        $log.info('Global search:', self.data.data[layerKey], self.data.options[layerKey]);
                    });

                    angular.forEach(self.data.options, function (option, key) {
                        if (newLayerIds.indexOf(key) === -1 && key.split('_').length > 1 && key.split('_')[1] === LAYERS_PARAM_DEFS.SEARCH_PREFIX) {
                            self.data.data[key] = [];
                            self.data.layers[key] = [];
                            self.data.options[key].config.state = 'refresh';
                        }
                        $log.info('Global keys:', key, option);
                    });
                },
                globalSearchClear: function () {
                    angular.forEach(self.data.options, function (option, key) {
                        if (key.split('_').length > 1 && key.split('_')[1] === LAYERS_PARAM_DEFS.SEARCH_PREFIX) {
                            self.data.data[key] = [];
                            self.data.layers[key] = [];
                            self.data.options[key].config.state = 'refresh';
                        }
                    });
                }
            };

            this.settings = {
                isLoading: function () {
                    var options;

                    for (var i = 0; i <= UtilsService.views.viewConfigFlat.length - 1; i++) {
                        var config = UtilsService.views.viewConfigFlat[i];
                        options = self.data.options[config.LayerId];

                        if ((angular.isDefined(options) && angular.isDefined(options.config) &&
                            options.config.loading)) {
                            return true;
                        }
                    }

                    return false;
                },
                isLayerLoading: function (layerId) {
                    return angular.isDefined(self.data.options[layerId]) &&
                        angular.isDefined(self.data.options[layerId].config) && self.data.options[layerId].config.loading ?
                        true : checkChildLoading(layerId);
                },
                tileAdd: function (tile) {
                    if (MapProviderService.mapIsInit()) {
                        //tile.layer.addTo(map);
                        MapProviderService.map.addLayer(tile.layer);
                    }
                },
                tileRemove: function (tile) {
                    if (MapProviderService.mapIsInit()) {
                        MapProviderService.map.removeLayer(tile.layer);
                    }
                },
                popupOpen: function (layerId, dataItem, latlng, pointer) {
                    UtilsService.layers.createPopup(layerId, dataItem, latlng, pointer);

                    if (layerId && dataItem) {
                        var layers = self.data.layers[layerId], exists = false, layer, i;

                        if (angular.isDefined(layers) && layers.length) {
                            for (i = layers.length - 1; i >= 0; i--) {
                                layer = layers[i];
                                if (layer.options.id === dataItem.ID && !layer.options.fake) {
                                    exists = true;

                                    if (UtilsService.layers.isCluster(layer.__parent)) {
                                        //UtilsService.layers.openCluster(layer.__parent);
                                    }

                                    UtilsService.layers.openPopup(layer._map || MapProviderService.map);
                                    UtilsService.layers.selectLayer(layer);
                                    break;
                                }
                            }
                        }

                        if (!exists) {
                            //fix for global search
                            layers = self.data.layers[layerId + '_' + LAYERS_PARAM_DEFS.SEARCH_PREFIX];

                            if (angular.isDefined(layers) && layers.length) {
                                for (i = layers.length - 1; i >= 0; i--) {
                                    layer = layers[i];
                                    if (layer.options.id === dataItem.ID && !layer.options.fake) {
                                        exists = true;

                                        if (layer.__parent && UtilsService.layers.isCluster(layer.__parent)) {
                                            //UtilsService.layers.openCluster(layer.__parent);
                                        }

                                        //debugger;
                                        UtilsService.layers.openPopup(layer._map || MapProviderService.map);
                                        UtilsService.layers.selectLayer(layer);
                                        break;
                                    }
                                }
                            }

                        }

                    }
                },
                popupCheck: function () {
                    //debugger;
                    var popup = UtilsService.layers.getPopup(), exists = false, layers = [], i, layer;

                    if (popup.layerId && popup.id && popup.popup) {
                        //$log.info('WTF Open:', popup.popup);

                        layers = self.data.layers[popup.layerId];

                        if (layers) {
                            for (i = layers.length - 1; i >= 0; i--) {
                                layer = layers[i];
                                if (layer.options.id === popup.id && !layer.options.fake) {
                                    exists = true;

                                    if (layer.__parent && UtilsService.layers.isCluster(layer.__parent)) {
                                        //UtilsService.layers.openCluster(layer.__parent);
                                    }

                                    UtilsService.layers.openPopup(layer._map || MapProviderService.map);
                                    UtilsService.layers.selectLayer(layer);
                                    break;
                                }
                            }
                        }

                        if (!exists) {
                            //fix for global search
                            layers = self.data.layers[popup.layerId + '_' + LAYERS_PARAM_DEFS.SEARCH_PREFIX];

                            if (layers) {
                                for (i = layers.length - 1; i >= 0; i--) {
                                    layer = layers[i];
                                    if (layer.options.id === popup.id && !layer.options.fake) {
                                        exists = true;

                                        if (layer.__parent && UtilsService.layers.isCluster(layer.__parent)) {
                                            //UtilsService.layers.openCluster(layer.__parent);
                                        }

                                        UtilsService.layers.openPopup(layer._map || MapProviderService.map);
                                        UtilsService.layers.selectLayer(layer);
                                        break;
                                    }
                                }
                            }
                        }

                        if (!exists) {
                            //destroy
                            UtilsService.layers.closePopup();
                        }
                    }
                },
                popupClose: function () {
                    var popup = UtilsService.layers.getPopup(), exists = false, layers = [], i, layer;

                    if (popup.layerId && popup.id && popup.popup) {
                        layers = self.data.layers[popup.layerId];

                        if (layers) {
                            for (i = layers.length - 1; i >= 0; i--) {
                                layer = layers[i];
                                if (layer.options.id === popup.id && !layer.options.fake) {
                                    exists = true;
                                    UtilsService.layers.unselectLayer(layer);
                                    break;
                                }
                            }
                        }

                        if (!exists) {
                            //fix for global search
                            layers = self.data.layers[popup.layerId + '_' + LAYERS_PARAM_DEFS.SEARCH_PREFIX];

                            if (layers) {
                                for (i = layers.length - 1; i >= 0; i--) {
                                    layer = layers[i];
                                    if (layer.options.id === popup.id && !layer.options.fake) {
                                        exists = true;
                                        UtilsService.layers.unselectLayer(layer);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    UtilsService.layers.closePopup();
                },

                setDisplacementPoint: function (point) {
                    lastPoint.x = point.x;
                    lastPoint.y = point.y;
                },
                checkDisplacement: function (point) {
                    $log.info('Displacement:', lastPoint, point);

                    if (Math.max(Math.abs(lastPoint.x - point.x), Math.abs(lastPoint.y - point.y)) >= GLOBAL_PARAM_DEFS.LAYER_RELOAD_DISPLACEMENT) {

                        self.settings.setDisplacementPoint(point);

                        $log.info('Detect displacement on ' + GLOBAL_PARAM_DEFS.LAYER_RELOAD_DISPLACEMENT + 'px');

                        return true;
                    }

                    return false;
                }
            };

            SignalService.on('changeLocation', function (data) {
                updateTracking(data);
            });
        };

        return new service();
    };

    mapService.$inject = ['$log', '$location', '$timeout', 'DataService', 'UtilsService', 'URLFactory', 'MapProviderService', 'LAYERS_PARAM_DEFS', 'GLOBAL_PARAM_DEFS', 'SignalService', 'GeoItemFactory', 'GeoItem'];

    angular.module('MapApp').
        factory('MapService', mapService);

})(window.angular);