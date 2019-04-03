(function (angular) {
    function aLeaflet($timeout, $log, $uibModal, LAYERS_PARAM_DEFS, clipboard, format) {
        var map;

        var mapLayers = {};
        var positionInitialized = false;
        //var pattentInitialized = false;

        var getOptionsDiff = function(newOptions, oldOptions) {
            var options = {};

            angular.forEach(newOptions, function(option, layerId) {
                angular.forEach(option, function(value, key) {
                    if (oldOptions[layerId]) {
                        if (!angular.equals(oldOptions[layerId][key], value)) {
                            if (!options[layerId]) options[layerId] = {};

                            options[layerId][key] = value;
                        }
                    } else {
                        options[layerId] = newOptions[layerId];
                    }
                });
            });


            return options;
        };

        var createClusterIcon = function (cluster) {
            var count = cluster.getChildCount();

            var options;

            if (!angular.isDefined(cluster._group.options.markerOptions)) {
                options = {
                    background: '#366284',
                    color: '#366284',
                    icon: 'glyphicon glyphicon-google-maps',
                    opacity: 1,
                    size: 40
                };
            } else {
                options = cluster._group.options.markerOptions;
            }

            return new L.DivIcon({
                html: format('<p class="cluster-total" style="background-color: {background}; border: 1px solid {color}; color: {color};">{count}</a><div class="marker-icon-placeholder" style="background-color: {background};"><i style="color: {color}; margin-left: -{size4}px; margin-top: -{size4}px; font-size: {fontsize}px; line-height: {fontsize}px;" class="{icon}"></i></div><div class="marker-decoration" style="width: {placeholderwidth}px; height: {placeholderheight}px;"></div>',
                    {
                        background: options.color,
                        color: options.fontcolor,
                        size4: options.size / 4,
                        size2: options.size / 2,
                        fontsize: (options.size / 2).toFixed(0),
                        icon: options.icon,
                        placeholderwidth: options.size + 4,
                        placeholderheight: (options.size + 4) * 1.5,
                        count: count
                    }),
                //html1: '<p class="cluster-total" style="background-color: ' + options.color + '; border: 1px solid ' + options.fontcolor + '; color: ' + options.fontcolor + ';">' + count + '</p><div style="background-color: ' + options.color + '; border-radius: ' + options.size / 2 + 'px;"><i style="color: ' + options.fontcolor + '; margin-left: -' + options.size / 4 + 'px; margin-top: -' + options.size / 4 + 'px; font-size: ' + options.size / 2 + 'px" class="' + options.icon + '"></i></div>',
                iconSize: [options.size, options.size],
                className: 'map-icon'
            });
        };

        var createClusterLayer = function (layerId, option) {
            var options = option.config.options;

            $log.info('On create layer:', option);

            var layer = L.markerClusterGroup({
                iconCreateFunction: createClusterIcon,
                polygonOptions: {
                    color: angular.isDefined(options) && angular.isDefined(options.background) ? options.background: '#b7d2f1',
                    weight: 1
                },
                chunkedLoading: true,
                chunkProgress: function (processed, total, elapsed) {
                    $log.info('Chunk (' + layerId + '):', processed, total, elapsed);
                    var zoom = map.getZoom();

                    if (processed >= total) {

                        $timeout(function() {
                            option.config.loading = false;
                        }, 0);

                        if (!option.config.clientCluster || (option.config.serverCluster && angular.isDefined(option.config.clusterLevel) && option.config.clusterLevel > zoom)) {
                            $log.info('Client cluster for (' + layerId + ') is disabled');
                            layer.disableClustering();
                        } else {
                            $log.info('Client cluster for (' + layerId + ') is enabled');
                            //layer.freezeAtZoom(zoom);
                        }
                    }
                },
                markerOptions: options
            });

            $log.info('Zoom (' + map.getZoom() + ') Cluster level (' + option.config.clusterLevel + ') Layer (' + layerId + ')');

            return layer;
        };

        var getLayers = function (id, layers) {
            var findLayers = [];

            for (var i = layers.length - 1; i >= 0; i--) {
                if (layers[i].options.id == id)
                    findLayers.push(layers[i]);
            }

            return findLayers;
        }

        var createMapLayer = function (layerId, layers, option) {
            if (option.config) {
                mapLayers[layerId] = createClusterLayer(layerId, option);
                mapLayers[layerId].addLayers(layers);
            }
        };

        var updateMapLayer = function(layerId, layers, option) {
            if (mapLayers[layerId]) {
                map.removeLayer(mapLayers[layerId]);
            }

            createMapLayer(layerId, layers, option);
        };

        var updateLayers = function (layerId, layers, option) {
            if (!mapLayers[layerId])
                createMapLayer(layerId, layers, option);

            var optionProperties = Object.keys(option);

            var targetLayers, layer;

            for (var i = optionProperties.length - 1; i >= 0; i--) {
                var value = option[optionProperties[i]];
                var key = optionProperties[i];

                if (key === 'config') {
                    switch (value.state) {
                        case 'visible':
                            mapLayers[layerId].addTo(map);
                            break;
                        case 'hidden':
                            map.removeLayer(mapLayers[layerId]);
                            break;
                        case 'refresh':
                            updateMapLayer(layerId, layers, option);
                            return;
                    }

                    continue;
                }

                //single layer logic

                targetLayers = getLayers(key, layers);

                if (targetLayers && targetLayers.length) {
                    for (var target = targetLayers.length - 1; target >= 0; target--) {
                        layer = targetLayers[target];
                        if (value) {
                            mapLayers[layerId].addLayer(layer);
                        } else {
                            mapLayers[layerId].removeLayer(layer);
                        }
                    }
                }


            }
        };

        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                tiles: '=ngTiles',
                //patterns: '=ngPatterns',
                model: '=ngModel',
                controls: '=ngControls',
                events: '=ngEvents',
                position: '=ngPosition'
            },
            link: function ($scope, $element, $attrs) {

                var mapEvent = function(name) {
                    if (angular.isDefined($scope.events[name])) {

                        var bounds = map.getBounds();
                        var center = map.getCenter();
                        var zoom = map.getZoom();
                        var point = map.latLngToLayerPoint(center, zoom);

                        $scope.events[name](bounds, center, zoom, point, map);
                    }
                };

                var pointEvent = function(name, latlng) {
                    if (angular.isDefined($scope.events[name])) {
                        $scope.events[name](latlng, map.getZoom());
                    }
                };

                return $timeout(function() {

                    $scope.$watch('model.options', function (newValue, oldValue) {
                        //debugger;
                        var options;

                        if (angular.equals(newValue, oldValue)) {
                            options = newValue;
                        } else {
                            options = getOptionsDiff(newValue, oldValue);
                        }

                        var layerIds = Object.keys(options);

                        for (var i = layerIds.length - 1; i >= 0; i--) {
                            var layerId = layerIds[i];
                            var option = options[layerId];

                            $log.info('Watch on change (' + layerId + ')', $scope.model.layers[layerId] ? $scope.model.layers[layerId].length : 0, option.config ? option.config.state : '');
                            //debugger;
                            //When 'state' of 'new' option is undefined no layers exists
                            //if (angular.isDefined(newValue[mnemonic].config.state)) {

                            //When layers[mnemonic] is defined
                            if (angular.isDefined($scope.model.layers[layerId])) {

                                updateLayers(layerId, $scope.model.layers[layerId], option);

                                //After Refresh set state to previous
                                if (option.config && option.config.state === 'refresh') {
                                    if (angular.isDefined(oldValue[layerId])) {
                                        option.config.state = oldValue[layerId].config.state;
                                    } else {
                                        option.config.state = 'visible';
                                    }
                                }
                            }
                        }

                    }, true);


                    $scope.$watch('position', function (newValue, oldValue) {
                        //$log.info('Focus', newValue, oldValue);

                        if (newValue && oldValue) {
                            if (!newValue.setview) {
                                if (newValue.zoom !== oldValue.zoom || newValue.position.lat.toFixed(4) !== oldValue.position.lat.toFixed(4) ||
                                    newValue.position.lng.toFixed(4) !== oldValue.position.lng.toFixed(4) || !positionInitialized) {

                                    positionInitialized = true;

                                    setTimeout(function() {
                                        //debugger;
                                        //map.setZoom(newValue.zoom);
                                        //map.panTo(newValue.position);

                                        if (newValue.zoom !== -1) {
                                            map.setView(newValue.position, newValue.zoom);
                                        } else {
                                            var zoom = map.getBoundsZoom(newValue.bounds);
                                            $log.info('Fit:', zoom);
                                            map.setView(newValue.position, zoom);
                                            //map.fitBounds(newValue.bounds);
                                        }

                                        $log.info('bounds:', newValue.bounds, newValue.position.lat, newValue.position.lng, newValue.zoom);

                                        //if (angular.isDefined($scope.patterns) && !pattentInitialized) {
                                        //    $log.info('Map select patterns:', $scope.patterns);
                                        //    angular.forEach($scope.patterns, function (pattern) {
                                        //        $log.info('Map select patterns:', pattern);
                                        //        pattern.addTo(map);
                                        //    });
                                        //    pattentInitialized = true;
                                        //}

                                    }, 0);

                                    $log.info('Position changed:', $scope.position);
                                }
                            } else {
                                //newValue.setview = false;
                            }
                        }
                    }, true);

                    map = L.map($element[0], {
                        zoomAnimation: true,
                        minZoom: LAYERS_PARAM_DEFS.MIN_ZOOM,
                        maxZoom: LAYERS_PARAM_DEFS.MAX_ZOOM
                    });

                    if (angular.isDefined($scope.events)) {

                        map.on('click', function (e) {
                            pointEvent('OnMapClick', e.latlng);
                        });


                        map.on('moveend', function (e) {
                            mapEvent('OnMoveEnd');
                        });


                        map.on('zoomstart', function (e) {
                            
                        });


                        map.on('zoomend', function (e) {
                            mapEvent('OnZoomEnd');
                        });

                        map.on('load', function (e) {
                            mapEvent('OnLoad');
                        });

                        map.on('mousemove', function (e) {
                            pointEvent('OnMouseMove', e.latlng);
                        });
                    };

                    if (angular.isDefined($scope.controls)) {
                        $log.info('Controls:', $scope.controls);

                        angular.forEach($scope.controls, function (control) {
                            control.addTo(map);
                        });
                    }

                    if (angular.isDefined($scope.tiles) && angular.isArray($scope.tiles)) {

                        var baseLayers = {};
                        var overlayLayers = {};

                        var firstAdded = false;

                        angular.forEach($scope.tiles, function(tile, index) {
                            //if (tile.layer.click) {
                            //    tile.layer.on('click', tile.layer.click);
                            //}

                            if (tile.overlay) {
                                overlayLayers[tile.name] = tile.layer;
                                if (tile.checked)
                                    tile.layer.addTo(map);
                            } else {
                                baseLayers[tile.name] = tile.layer;

                                if (!firstAdded) {
                                    tile.layer.addTo(map);
                                    firstAdded = true;
                                }
                            }

                            $log.info('Tiles:', tile, index);
                        });

                        map.addControl(new L.Control.Layers(baseLayers, overlayLayers, { position: 'topright', collapsed: false, ignoreTouch: true }));

                        //iCheck for leflet controls
                        var checkboxAwaiter = setInterval(function() {
                            if ($('.leaflet-control-layers input').length) {
                                clearInterval(checkboxAwaiter);

                                $('.leaflet-control-layers input').iCheck({
                                    checkboxClass: 'icheckbox_square-red',
                                    radioClass: 'iradio_square-red',
                                    increaseArea: '20%'
                                });

                                $('.leaflet-control-layers input[type=radio]').on('ifToggled', function (event) {
                                    if(event.currentTarget) event.currentTarget.click();
                                });

                                $('.leaflet-control-layers input[type=checkbox]').on('ifClicked', function (event) {
                                    if (event.currentTarget) $(event.currentTarget).click();
                                });
                            }
                        }, 100);

                    }

                    $element.on('click', 'a.btn-link-url[link-item]', function (e) {
                        var $target = angular.element(e.currentTarget);

                        var url = document.location.origin + document.location.pathname + $target.attr('link-item');

                        $uibModal.open({
                            animation: true,
                            backdrop: false,
                            templateUrl: 'link.container.html',
                            controller: function ($uibModalInstance, data) {
                                this.title = data.title;
                                this.url = data.url;

                                this.close = function() {
                                    $uibModalInstance.close();
                                }
                            },
                            controllerAs: 'link',
                            size: 'ls',
                            resolve: {
                                data: function () {
                                    return {
                                        title: 'Прямая ссылка на объект',
                                        url: url
                                    }
                                }
                            }
                        });
                    });

                    $element.on('click', 'a.btn-link-url[coords-item]', function (e) {
                        if (map) {
                            var $target = angular.element(e.currentTarget);

                            var textLatLng = $target.attr('coords-item');

                            var lat = textLatLng.split(':')[0];
                            var lng = textLatLng.split(':')[1];

                            var text = format('Latitude: {lat} / Longitude: {lng}', { lat, lng });

                            clipboard.copyText(text);
                        }
                    });

                    $log.info('Leaflet map:', map);
                });
            }
        };
    }

    angular.module('ngLeaflet', [])
        .directive('ngLeaflet', aLeaflet);

    aLeaflet.$inject = ['$timeout', '$log', '$uibModal', 'LAYERS_PARAM_DEFS', 'clipboard', 'format'];

})(window.angular);