(function (angular) {
    function utilsService($log, $q, DataService, MapProviderService, LAYERS_VIEW_DEFS, LAYERS_PARAM_DEFS, GLOBAL_PARAM_DEFS, format, ViewConfig, VIEW_TYPE) {

        var service = function () {
            var self = this;

            var layerOptions = {};
            var counterUpdatedState = {};

            var filtersLoadOptions = {};
            var filters = {};
            var filterProperties = {};
            var selectedLayers = [];

            var lazyPropertyLoadLock = {};

            var popupObject = {
                layerId: null,
                id: -1,
                data: null,
                popup: null,
                latlng: null,
                pointer: null,
                fixed: false
            };

            var kladrPopup;

            var removeKladrPopup = function () {
                if (kladrPopup) {
                    kladrPopup._close();
                    kladrPopup = null;
                }
            };

            var createKladrPopup = function (map, latlng, address) {
                removeKladrPopup();

                kladrPopup = L.popup({
                    autoPan: false,
                    offset: L.point(0, -10)
                }).setLatLng(latlng)
                    .setContent('<p>' + address + '</p>');

                if (map) {
                    kladrPopup.openOn(map);
                }
            };

            var patterns = {
                selectPolygon: new L.StripePattern({
                    opacity: 1,
                    weight: 3,
                    spaceWeight: 2,
                    color: LAYERS_VIEW_DEFS.DEF_SELECT_FILL_COLOR
                })
            };

            var checkPatterns = function () {
                if (MapProviderService.mapIsInit()) {
                    angular.forEach(patterns, function (pattern) {
                        if (!pattern._map) {
                            pattern.addTo(MapProviderService.map);
                        }
                    });
                }
            };

            var loadFilter = function (config) {
                return $q(function (resolve, reject) {
                    if (!config) {
                        reject('Config is NULL');
                    }

                    if ((config.Filters && config.Filters.length) || filtersLoadOptions[config.LayerId]) {
                        resolve(config.Filters);
                    }

                    DataService.loadFilter(config).then(function (response) {
                        $log.info('Filter response:', response);

                        if (response && response.length) {
                            angular.forEach(response,
                                function (item) {
                                    config.AddFilter(item);
                                });
                        }

                        filtersLoadOptions[config.LayerId] = true;

                        resolve(config.Filters);
                    },
                        function (err) {
                            $log.error('Load layer filter config error...');
                            reject(err);
                        });
                });
            };

            var createViewConfigs = function (configs, mnemonic) {
                var viewconfigs = [];
                for (var i = 0; i < configs.length; i++) {
                    var config = new ViewConfig(configs[i], mnemonic);

                    viewconfigs.push(config);

                    self.views.viewConfigDict[configs[i].LayerId] = config;
                    self.views.viewConfigFlat.push(config);

                    if (config.Filterable) {
                        filtersLoadOptions[config.LayerId] = false;
                        filters[config.LayerId] = {};
                        filterProperties[config.LayerId] = {};
                    }

                    if (config.HasChildren) {
                        config.Children = createViewConfigs(configs[i].Children, config.Mnemonic);
                    }
                }
                return viewconfigs;
            };

            var updateCounter = function (layerId, count) {
                var config = self.views.getViewConfig(layerId);

                if (config) {
                    config.Count = count;
                    counterUpdatedState[layerId] = true;
                }
            };

            var hexToRgb = function (hex) {
                var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
                return result ? {
                    r: parseInt(result[1], 16),
                    g: parseInt(result[2], 16),
                    b: parseInt(result[3], 16)
                } : null;
            }

            var getContrast = function (color) {
                var rgb = hexToRgb(color);

                if (rgb) {
                    var coef = Math.round(((parseInt(rgb.r) * 299) + (parseInt(rgb.g) * 587) + (parseInt(rgb.b) * 114)) / 1000);
                    return coef > 190 ? '#424242' : '#fff';
                }

                return color;
            };

            var createOptions = function (config) {
                if (angular.isUndefined(layerOptions[config.LayerId])) {
                    layerOptions[config.LayerId] = {
                        layerId: config.LayerId,
                        mnemonic: config.Mnemonic,

                        icon: config.Style.Icon ? config.Style.Icon : LAYERS_VIEW_DEFS.DEF_ICON,
                        color: config.Style.Color ? config.Style.Color : LAYERS_VIEW_DEFS.DEF_ICON_COLOR,
                        fontcolor: getContrast(config.Style.Color ? config.Style.Color : LAYERS_VIEW_DEFS.DEF_ICON_COLOR),

                        background: config.Style.Background ? config.Style.Background : LAYERS_VIEW_DEFS.DEF_BACKGROUND_COLOR,
                        opacity: config.Style.Opacity ? config.Style.Opacity : LAYERS_VIEW_DEFS.DEF_OPACITY,

                        bordercolor: config.Style.BorderColor ? config.Style.BorderColor : LAYERS_VIEW_DEFS.DEF_BORDER_COLOR,
                        borderopacity: config.Style.BorderOpacity ? config.Style.BorderOpacity : LAYERS_VIEW_DEFS.DEF_BORDER_OPACITY,
                        borderwidth: config.Style.BorderWidth ? config.Style.BorderWidth : LAYERS_VIEW_DEFS.DEF_BORDER_WIDTH,

                        size: LAYERS_VIEW_DEFS.DEF_ICON_SIZE
                    };
                }

                return layerOptions[config.LayerId];
            };

            var createDefaultIcon = function (options) {
                

                return new L.DivIcon({
                    html: format('<div class="marker-icon-placeholder" style="background-color: {background};"><i style="color: {color}; margin-left: -{size4}px; margin-top: -{size4}px; font-size: {fontsize}px; line-height: {fontsize}px;" class="{icon}"></i></div><div class="marker-decoration" style="width: {placeholderwidth}px; height: {placeholderheight}px;"></div>',
                    {
                        background: options.color,
                        color: options.fontcolor,
                        size4: options.size / 4,
                        size2: options.size / 2,
                        fontsize: (options.size / 2).toFixed(0),
                        icon: options.icon,
                        placeholderwidth: options.size + 4,
                        placeholderheight: (options.size + 4) * 1.5
                    }),
                    //html: '<div style="background-color: ' + options.color + '; border-radius: ' + options.size / 2 + 'px;"><i style="color: ' + options.fontcolor + '; margin-left: -' + options.size / 4 + 'px; margin-top: -' + options.size / 4 + 'px; font-size: ' + options.size / 2 + 'px; line-height: ' + (options.size / 2).toFixed(0) + 'px;" class="' + options.icon + '"></i></div>',
                    iconSize: [options.size, options.size],
                    className: 'map-icon'
                });
            };

            var createClusterIcon = function (options) {
                var boundsAndZoom = MapProviderService.getUpdatedBoundsAndZoom();
                return new L.DivIcon({
                    html: format('<a href="#/clusterlist/{layerId}/{id}/{zoom}" onclick="event.stopPropagation()" class="cluster-total" style="background-color: {background}; border: 1px solid {color}; color: {color};">{count}</a><div class="marker-icon-placeholder" style="background-color: {background};"><i style="color: {color}; margin-left: -{size4}px; margin-top: -{size4}px; font-size: {fontsize}px; line-height: {fontsize}px;" class="{icon}"></i></div><div class="marker-decoration" style="width: {placeholderwidth}px; height: {placeholderheight}px;"></div>',
                        {
                            id: options.id,
                            layerId: options.layerId,
                            background: options.color,
                            color: options.fontcolor,
                            size4: options.size / 4,
                            size2: options.size / 2,
                            fontsize: (options.size / 2).toFixed(0),
                            icon: options.icon,
                            placeholderwidth: options.size + 4,
                            placeholderheight: (options.size + 4) * 1.5,
                            zoom: boundsAndZoom.zoom,
                            count: options.count
                        }),
                    //html1: '<a href="#/clusterlist/' + options.layerId + '/' + options.id + '/' + boundsAndZoom.zoom + '" onclick="event.stopPropagation()" class="cluster-total" style="background-color: ' + options.color + '; border: 1px solid ' + options.fontcolor + '; color: ' + options.fontcolor + ';">' + options.count + '</a><div cluster-list="#/clusterlist/' + options.layerId + '/' + options.id + '/' + boundsAndZoom.zoom + '" style="background-color: ' + options.color + '; border-radius: ' + options.size / 2 + 'px;border: 1px solid ' + options.fontcolor + ';"><i style="color: ' + options.fontcolor + '; margin-left: -' + options.size / 4 + 'px; margin-top: -' + options.size / 4 + 'px; font-size: ' + options.size / 2 + 'px; line-height: ' + (options.size / 2).toFixed(0) + 'px;" class="' + options.icon + '"></i></div>',
                    iconSize: [options.size, options.size],
                    className: 'map-icon server-cluster'
                });
            };

            var createIcon = function (options) {
                //TODO: Must check only "type"
                if (options.type === 3 && options.count > 1) {
                    return createClusterIcon(options);
                }

                return createDefaultIcon(options);
            };

            var createTooltipContent = function (config, dataItem, options, latlng) {
                var html = '';

                html += '<div class="popup-icon" style="background-color: {color};"><i class="{icon}"></i></div>';
                html += '<p class="popup-title">{configtitle}</p>';
                html += '<div class="popup-body">';

                //Lazy
                if (config.Lazy === 0 || dataItem.LazyLoaded()) {
                    html += dataItem.Title && dataItem.Title.length ? '<p>{itemtitle}</p>' : '<p>{configtitle}</p>';
                    html += dataItem.Description && dataItem.Description.length ? '<p>{itemdescription}</p>' : '';
                    html += dataItem.IsLinear() ? '<p class="map-tooltip-distance">Расстояние от начала: {distance} км. <span>(+{disposition})</span></p>' : '';
                } else {
                    html += '<img src="/areas/public/content/images/gears.svg" width="30" height="30">';
                }

                //End Lazy

                html += '</div>'; //body
                html += '<div class="popup-buttons btn-group btn-group-xs">';
                html += '<a class="btn btn-default" href="#/detail/{layerId}/{id}/show">Просмотр</a>';
                html += '<a class="btn btn-primary btn-link-url" link-item="#/detail/{layerId}/{id}/move" href="javascript:void(0)"><span class="halfling halfling-link"></span></a>';
                html += '<a class="btn btn-default btn-link-url" title="Скопировать в буфер обмена текущую координату" coords-item="{lat}:{lng}"><span class="halfling halfling-duplicate"></span></a>';
                html += '</div>'; //btn group

                return format(html, {
                    color: options.color,
                    size: options.size,
                    halfsize: options.size / 2,
                    fourthsize: options.size / 4,
                    icon: options.icon,
                    configtitle: config.Title,
                    itemtitle: dataItem.Title,
                    itemdescription: dataItem.Description,
                    distance: (dataItem.DistanceToPoint / 1000 + dataItem.GetStartDisposition()).toFixed(3),
                    disposition: dataItem.GetStartDisposition(),
                    id: dataItem.ID,
                    layerId: config.LayerId,
                    mnemonic: config.Mnemonic,
                    lat: latlng ? latlng.lat : 0,
                    lng: latlng ? latlng.lng : 0
                });
            };

            var createBounds = function (latlng, meters) {
                var d = meters / 2;

                //Coordinate offsets in radians
                var dLat1 = d / LAYERS_PARAM_DEFS.R;
                var dLon1 = d / (LAYERS_PARAM_DEFS.R * Math.cos(Math.PI * latlng.lat / 180));

                var dLat2 = -d / LAYERS_PARAM_DEFS.R;
                var dLon2 = -d / (LAYERS_PARAM_DEFS.R * Math.cos(Math.PI * latlng.lat / 180));

                //OffsetPosition, decimal degrees
                var latO1 = latlng.lat + dLat1 * 180 / Math.PI;
                var lonO1 = latlng.lng + dLon1 * 180 / Math.PI;

                var latO2 = latlng.lat + dLat2 * 180 / Math.PI;
                var lonO2 = latlng.lng + dLon2 * 180 / Math.PI;

                return new L.LatLngBounds([[latO1, lonO1], [latO2, lonO2]]);
            };

            var getDistance = function (lat1, lon1, lat2, lon2) {

                var phi1 = lat1 * Math.PI / 180;
                var phi2 = lat2 * Math.PI / 180;

                var deltaPhi = (lat2 - lat1) * Math.PI / 180;
                var deltaLambda = (lon2 - lon1) * Math.PI / 180;

                var a = Math.sin(deltaPhi / 2) * Math.sin(deltaPhi / 2)
                    + Math.cos(phi1) * Math.cos(phi2) * Math.sin(deltaLambda / 2)
                    * Math.sin(deltaLambda / 2);
                var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
                var d = LAYERS_PARAM_DEFS.R * c;

                return d;
            };

            var getSegmentDistance = function (map, latlng, layer) {
                var length = 0;

                if (!map || !layer || !layer.getLatLngs) return 0;

                var end = L.GeometryUtil.locateOnLine(map, layer, latlng);

                var segment = L.GeometryUtil.extract(map, layer, 0, end);

                length = L.GeometryUtil.length(segment);

                return length;
            };

            var getLineCenter = function (layer) {
                if (!MapProviderService.mapIsInit() || !layer) return null;

                var segment = L.GeometryUtil.extract(MapProviderService.map, layer, 0, .5);

                return segment.length > 0 ? segment[segment.length - 1] : null;
            };

            var getGeometryType = function (geometry) {
                return angular.isDefined(geometry) && angular.isDefined(geometry.type) ?
                    geometry.type.toLowerCase() : 'undefined';
            };

            var removePointer = function () {
                if (popupObject.pointer && popupObject.pointer._map) {
                    MapProviderService.removeLayer(popupObject.pointer);
                }
                popupObject.pointer = null;
            };

            var deletePopup = function () {
                if (popupObject.popup) {
                    popupObject.popup._close();
                }

                popupObject.layerId = null;
                popupObject.id = -1;
                popupObject.popup = null;
                popupObject.fixed = false;

                //TODO
                //popupObject.data.off('OnLazyLoaded');

                popupObject.data = null;

                removePointer();

                self.layers.unselectLayer();
            };

            var openPopup = function (map) {
                if (popupObject.popup && map && !popupObject.popup._isOpen) {
                    popupObject.popup.openOn(map);
                }

                if (popupObject.pointer && map) {
                    if (!popupObject.pointer._map) {
                        popupObject.pointer.addTo(map);
                    }
                }
            };

            var createPopup = function (layerId, dataItem, latlng, pointer, map) {
                var positionChange = !popupObject.latlng || !latlng || popupObject.latlng.lat.toFixed(4) !== latlng.lat.toFixed(4) ||
                    popupObject.latlng.lng.toFixed(4) !== latlng.lng.toFixed(4);


                var config = self.views.getViewConfig(layerId);
                var options = createOptions(config);
                var content = createTooltipContent(config, dataItem, options, (latlng || popupObject.latlng));

                if (popupObject.layerId !== layerId || popupObject.id !== dataItem.ID || !popupObject.popup) {

                    if (popupObject.popup) {
                        //self.layers.unselectLayer();
                        //popupObject.popup._close();
                        deletePopup();
                    }

                    //if (popupObject.pointer) {
                    //    removePointer();
                    //}

                    popupObject.layerId = layerId;
                    popupObject.id = dataItem.ID;
                    popupObject.latlng = (latlng || popupObject.latlng);
                    popupObject.fixed = !pointer;
                    popupObject.data = dataItem;

                    dataItem.on('OnLazyLoaded', function (lazyProperties) {
                        if (popupObject.layerId === layerId && popupObject.id === dataItem.ID) {
                            popupObject.popup.setContent(createTooltipContent(config, dataItem, options, popupObject.latlng));
                        }
                    });

                    if (pointer) {
                        popupObject.pointer = self.layers.createCircle(latlng, dataItem.ID, config, LAYERS_PARAM_DEFS.POINTER_RADIUS, true, true);

                        popupObject.pointer.on('click', function (e) {
                            popupObject.fixed = !popupObject.fixed;
                        });
                    }

                    popupObject.popup = L.popup({
                        autoPan: false,
                        offset: L.point(0, -10)
                    })
                        .setLatLng(latlng || popupObject.latlng)
                        .setContent(content)
                        .on('open', function () {

                        })
                        .on('close', function () {
                            deletePopup();
                        });
                }

                if (popupObject.layerId === layerId && popupObject.id === dataItem.ID && positionChange) {
                    popupObject.latlng = (latlng || popupObject.latlng);
                    popupObject.popup.setLatLng(latlng || popupObject.latlng).setContent(content);

                    if (popupObject.pointer) {
                        popupObject.pointer.setLatLng(latlng || popupObject.latlng);
                    }
                }

                if (map) openPopup(map);
            };

            this.views = {
                isStandart: function () {
                    return VIEW_TYPE === 0;
                },
                isSimple: function () {
                    return VIEW_TYPE === 1;
                },
                viewConfig: [],
                viewConfigFlat: [],
                viewConfigDict: {},
                createConfig: function (configs) {
                    self.views.viewConfig = createViewConfigs(configs);
                },
                getViewConfig: function (layerId) {
                    return self.views.viewConfigDict[layerId];
                },
                getVisibleConfigs: function () {
                    return self.views.viewConfigFlat.filter(function (config) {
                        return config.IsVisible();
                    });
                },
                getTopLayerIds: function () {
                    return self.views.viewConfig.map(function (item) {
                        return item.LayerId;
                    });
                },
                getFTSConfigs: function () {
                    return self.views.viewConfigFlat.filter(function (config) {
                        return config.FullTextSearch;
                    });
                },
                getFTSLayerIds: function () {
                    return self.views.getFTSConfigs().map(function (config) {
                        return config.LayerId;
                    });
                },
                getLayerIds: function (searchable) {
                    return self.views.viewConfigFlat.filter(function (item) {
                        return angular.isUndefined(searchable) || item.SearchOnClick === searchable;
                    }).map(function (item) {
                        return item.LayerId;
                    });
                },
                getChildLayerIds: function (layerId) {
                    var layerIds = [];
                    var config = self.views.getViewConfig(layerId);

                    if (config.Children.length) {
                        angular.forEach(config.Children, function (item) {
                            layerIds.push(item.LayerId);
                            layerIds = layerIds.concat(self.views.getChildLayerIds(item.LayerId));
                        });
                    }
                    return layerIds;
                },
                counterUpdated: function (layerId) {
                    return counterUpdatedState[layerId];
                },
                counterUpdatedChangeState: function (layerId, value) {
                    counterUpdatedState[layerId] = value;
                },
                loadConfigCounter: function (layerId) {
                    DataService.loadConfigCounter(layerId, self.views.getFilterProperties(layerId))
                        .then(function(data) {
                                updateCounter(layerId, data.data.Count);
                            },
                            function(err) {
                                $log.error('Load layer counter ' + layerId + ' error...');
                            });
                },
                loadConfigCounters: function (disabled /*Force update for not loadable/visible configs*/) {

                    var layerIds = self.views.getLayerIds();

                    $log.info('Layers from config:', layerIds);

                    angular.forEach(layerIds, function (layerId) {
                        var config = self.views.getViewConfig(layerId);
                        if (!disabled || (!config.IsSearchable && (!config.IsLoadable || !config.IsVisible()))) {
                            self.views.loadConfigCounter(layerId);
                        }
                    });
                },
                loadFilterConfig: function (config) {
                    return loadFilter(config);
                },
                updateFilter: function (layerId, newFilter) {
                    if (!filters[layerId]) {
                        filters[layerId] = newFilter;
                    } else {
                        angular.extend(filters[layerId], newFilter);
                    }
                },
                getFilter: function (layerId) {
                    return filters[layerId];
                },
                clearFilter: function (layerId) {
                    filters[layerId] = {};
                    filterProperties[layerId] = {};
                },
                isFiltered: function (layerId) {
                    return !angular.equals(filters[layerId], {}) && !angular.equals(filterProperties[layerId], {});
                },
                updateFilterProperties: function (layerId, filterConfig, filterValues) {
                    filterProperties[layerId] = {};

                    if (angular.isUndefined(layerId)) return;

                    var config = self.views.getViewConfig(layerId);
                    filterConfig = config.Filters && config.Filters.length ? config.Filters : filterConfig;

                    if (!filterConfig || !filterConfig.length) return;

                    if (filters[layerId]) {
                        filterValues = angular.extend(filters[layerId], filterValues || {});
                    } else {
                        filters[layerId] = filterValues;
                    }

                    if (!filterValues) return;

                    var prop, filterProperty = filterProperties[layerId];

                    angular.forEach(filterConfig, function (fConfig) {
                        //debugger;
                        prop = filterValues[fConfig.Field];

                        switch (fConfig.Type + '_' + fConfig.UIType) {
                            case 'Enum_MultiSelect':
                                if (angular.isArray(prop) && prop.length) {
                                    filterProperty[fConfig.Field] = prop.map(function (item) { return (item.Value).toString(); });
                                }
                                break;
                            case 'Text_Text':
                                if (angular.isString(prop) && prop.length) {
                                    filterProperty[fConfig.Field] = [prop];
                                }
                                break;
                            case 'Bool_Checkbox':
                                if (angular.isDefined(prop)) {
                                    filterProperty[fConfig.Field] = [prop];
                                }
                                break;
                            case 'Numeric_Range':

                                console.log('Numeric_Range', `prop( ${prop[0]} ${prop[1]} )   fConfig( ${fConfig.MinValue} ${fConfig.MinValue})`);

                                if (angular.isDefined(prop) && angular.isDefined(prop[0]) && angular.isDefined(prop[1]) && prop[0] !== 0 && prop[1] !== 0
                                    && (prop[0] !== fConfig.MinValue || prop[1] !== fConfig.MaxValue)) {
                                    filterProperty[fConfig.Field] = [(prop[0]).toString(), (prop[1]).toString()];
                                }
                                break;
                            case 'DateTime_Range':
                                if (angular.isDefined(prop) && angular.isDefined(prop.StartDate) && angular.isDefined(prop.EndDate) && new Date(prop.StartDate) <= new Date(prop.EndDate)) {
                                    filterProperty[fConfig.Field] = [(prop.StartDate).toString(), (prop.EndDate).toString()];
                                }
                                break;
                            default:
                                $log.error('Wrong filter config:', fConfig);
                                break;
                        }

                    });
                },
                getFilterProperties: function (layerId) {
                    var propertyFilter = filterProperties[layerId];
                    return angular.isUndefined(propertyFilter) || angular.equals(propertyFilter, {}) ? null : propertyFilter;
                },
                loadLazyProperties: function (dataItem, layerId) {
                    return $q(function (resolve, reject) {
                        var key = format('{}_{}', layerId, dataItem.ID);

                        if (lazyPropertyLoadLock[key]) {
                            reject({ message: 'Allready loading...' });
                        } else {
                            lazyPropertyLoadLock[key] = true;

                            $log.info('Load properties for:', key, dataItem.ID, layerId);

                            DataService.loadLazyProperties(dataItem.ID, layerId).then(function(data) {
                                    dataItem.SetLazyProperties(data);
                                    delete lazyPropertyLoadLock[key];
                                    resolve(dataItem.GetLazyProperties());
                                },
                                function(err) {
                                    reject(err);
                                });
                        }
                    });
                }
            };

            this.layers = {
                createLayerOptions: function (config) {
                    return createOptions(config);
                },
                createBorder: function (data, layerId, clickEvent) {
                    return L.geoJson(data.Geometry, {
                        style: {
                            color: '#E74C3C',
                            fillOpacity: 0,
                            fillColor: 'transparent',
                            smoothFactor: 0,
                            weight: 3
                        },
                        smoothFactor: 0,
                        id: -1,
                        layerId: layerId,
                        geotype: data.GeoType,
                        onEachFeature: function (feature, l) {
                            l.on({
                                click: function (e) {
                                    if (clickEvent) {
                                        var zoom = e.target && e.target._map ? e.target._map.getZoom() : 18;
                                        var latlng = e.latlng;
                                        clickEvent(latlng, zoom);
                                    }

                                    deletePopup();
                                }
                            });
                        }
                    });
                },
                createNavigator: function (latLng, layerId, clickEvent) {
                    return L.circle(latLng, GLOBAL_PARAM_DEFS.NAVIGATION_SEARCH_RADIUS, {
                        style: {
                            color: '#E74C3C',
                            fillOpacity: 0,
                            fillColor: 'transparent',
                            smoothFactor: 0,
                            weight: .5
                        },
                        smoothFactor: 0,
                        id: -1,
                        layerId: layerId,
                        geotype: 'circle',
                        onEachFeature: function (feature, l) {
                            l.on({
                                click: function (e) {
                                    if (clickEvent) {
                                        var zoom = e.target && e.target._map ? e.target._map.getZoom() : 18;
                                        var latlng = e.latlng;
                                        clickEvent(latlng, zoom);
                                    }
                                }
                            });
                        }
                    });
                },
                rotateBounds: function (bounds) {
                    return [bounds.getSouthWest().lat, bounds.getSouthWest().lng, bounds.getNorthEast().lat, bounds.getNorthEast().lng];
                },
                createBounds: function (latlng, meters) {
                    return createBounds(latlng, meters);
                },
                createCenter: function (geometry) {
                    var layer = L.geoJson(geometry);
                    var bounds = layer.getBounds();

                    //$log.info('geometry:', geometry, bounds);

                    switch (getGeometryType(geometry)) {
                        case 'point':
                            return {
                                bounds: bounds,
                                latlng: bounds.getCenter(),
                                zoom: LAYERS_PARAM_DEFS.MAX_ZOOM
                            }
                        case 'linestring':
                            var center = getLineCenter(layer.getLayers()[0]);
                            return {
                                bounds: bounds,
                                latlng: L.latLng(center.lat, center.lng),
                                zoom: LAYERS_PARAM_DEFS.MAX_ZOOM
                            }
                        default:
                            return {
                                bounds: bounds,
                                latlng: bounds.getCenter(),
                                zoom: -1
                            }
                    }

                    //return L.geoJson(geometry).getBounds().getCenter();
                },
                createCircle: function (latlng, id, config, radius, clickable, ignorecluster) {
                    var options = createOptions(config), html = '';

                    var markerOptions = {
                        radius1: (radius ? radius : options.borderwidth) * 2,
                        radius2: options.borderwidth,
                        borderradius1: radius ? radius : options.borderwidth,
                        borderradius2: options.borderwidth / 2,

                        offset1: (radius ? radius : options.borderwidth),
                        offset2: options.borderwidth / 2,

                        backgroundcolor: options.bordercolor,
                        opacity: options.borderopacity
                    }

                    html += '<div style="background-color: {backgroundcolor}; opacity: {opacity}; border-radius: {borderradius1}px; width: {radius1}px; height: {radius1}px; margin-top: -{offset1}px; margin-left: -{offset1}px;"></div>';
                    html += '<div style="background-color: {backgroundcolor}; opacity: 1; border-radius: {borderradius2}px; width: {radius2}px; height: {radius2}px; margin-top: -{offset2}px; margin-left: -{offset2}px;"></div>';


                    var icon = new L.DivIcon({
                        html: format(html, markerOptions),
                        iconSize: [markerOptions.radius1, markerOptions.radius1],
                        className: 'map-point-icon'
                    });

                    return new L.marker(latlng, {
                        id: id,
                        layerId: config.LayerId,
                        clickable: clickable,
                        icon: icon,
                        //color: options.color,
                        //type: dataItem.Type,
                        geotype: 'marker',
                        fake: true,
                        ignorecluster: ignorecluster,
                        count: 1
                    });
                },
                createMarker: function (dataItem, config, clickable, fake) {

                    var options = createOptions(config);

                    var latlng = self.layers.createCenter(dataItem.Geometry).latlng;

                    //switch (dataItem.GeoType) {
                    //    case 'point':
                    //        latlng = geometryToPoint(dataItem.Geometry);
                    //        break;
                    //    default:
                    //        latlng = L.geoJson(dataItem.Geometry).getBounds().getCenter();
                    //}

                    return L.marker(latlng, {
                        id: dataItem.ID,
                        layerId: config.LayerId,
                        clickable: clickable,
                        icon: createIcon(angular.extend(options, { id: dataItem.ID, layerId: config.LayerId, type: dataItem.Type, count: angular.isDefined(dataItem.Count) ? dataItem.Count : 1 })),
                        color: options.color,
                        type: dataItem.Type,
                        geotype: 'marker',
                        fake: fake,
                        count: angular.isDefined(dataItem.Count) ? dataItem.Count : 1
                    });
                },
                createPolygon: function (dataItem, config, events) {
                    var options = createOptions(config);

                    var layer = L.geoJson(dataItem.Geometry, {
                        id: dataItem.ID,
                        style: {
                            color: options.bordercolor,
                            opacity: options.borderopacity,
                            fillOpacity: options.opacity,
                            fillColor: options.background,
                            smoothFactor: 0,
                            weight: options.borderwidth
                        },
                        geotype: dataItem.GeoType,
                        onEachFeature: function (feature, l) {
                            l.on(events);
                        },
                        layerId: config.LayerId
                    });

                    return layer;
                },
                selectLayer: function (layer) {
                    //debugger;
                    if (self.layers.isGeometric(layer)) {
                        checkPatterns();
                        //layer.options.defaultStyle = angular.extend({ fillPattern: null }, layer.options.style);
                        layer.setStyle({
                            weight: 8,
                            color: LAYERS_VIEW_DEFS.DEF_SELECT_BORDER_COLOR,
                            opacity: 1,
                            fillOpacity: 1,
                            fillPattern: patterns.selectPolygon
                        });

                        selectedLayers.push(layer);
                        layer.fire('onselect');
                    }
                },
                unselectLayer: function (layer) {
                    var layers = angular.isDefined(layer) ? [layer] : selectedLayers;

                    if (!angular.isArray(layers)) return;

                    var indexes = [];

                    angular.forEach(layers, function (_layer) {
                        //$log.info('On unselect layer:', _layer);

                        if (self.layers.isGeometric(_layer) && _layer.options.style /*layer.options.memoryStyle*/) {
                            //_layer.resetStyle(_layer);
                            _layer.setStyle(angular.extend({ fillPattern: null }, _layer.options.style));
                            _layer.fire('onunselect');
                            indexes.push(selectedLayers.indexOf(_layer));
                        }
                    });

                    angular.forEach(indexes, function (index) {
                        if (index > -1) {
                            selectedLayers.splice(index, 1);
                        }
                    });

                    //$log.info('On unselect layer, selected layers:', selectedLayers);
                },
                createLayer: function (config, dataItem, clickEvent) {
                    var layers = [],
                        marker, polygon, start, end;

                    if (dataItem.IsGeometry) {
                        switch (dataItem.GeoType) {
                            case 'point':
                                if (config.ShowIcon) {
                                    marker = self.layers.createMarker(dataItem, config, true);

                                    switch (dataItem.Type) {
                                        case 1:
                                        case 2:
                                            marker.on('click', function (e) {
                                                createPopup(config.LayerId, dataItem, e.target._latlng, false, e.target._map);
                                            });
                                            break;
                                        case 3:
                                            marker.on('click', function (e) {
                                                var map = e.target._map;
                                                var zoom = map.getZoom();
                                                map.setView(e.latlng, zoom < 18 ? zoom + 1 : 18);
                                            });
                                            break;
                                        default:

                                    }

                                    layers.push(marker);
                                }
                                break;
                            case 'polygon':
                            case 'multipolygon':
                            case 'geometrycollection': //TODO: Need test
                                //if (dataItem.GeoType === 'multipolygon') {
                                //    debugger;
                                //}

                                polygon = self.layers.createPolygon(dataItem, config, {
                                    click: function (e) {

                                        var target = e.layer;

                                        if (target) {
                                            if (clickEvent) {
                                                var zoom = target._map ? target._map.getZoom() : 18;
                                                var latlng = e.latlng;
                                                clickEvent(latlng, zoom);
                                            }

                                            createPopup(config.LayerId, dataItem, e.latlng, false, target._map);

                                            self.layers.selectLayer(target);
                                        }
                                    }
                                });

                                if (config.ShowIcon) {
                                    marker = self.layers.createMarker(dataItem, config, true, true);

                                    marker.on('click', function (e) {
                                        createPopup(config.LayerId, dataItem, e.target._latlng, false, e.target._map);
                                    });

                                    layers.push(marker);
                                }

                                layers.push(polygon);
                                break;
                            case 'linestring':
                            case 'multilinestring':
                                //For debugging
                                if (dataItem.GeoType === 'multilinestring') {
                                    $log.debug('Multi line detected', dataItem, config);
                                }

                                polygon = self.layers.createPolygon(dataItem, config, {
                                    click: function (e) {
                                        dataItem.DistanceToPoint = getSegmentDistance(e.target._map, e.latlng, e.target);
                                        createPopup(config.LayerId, dataItem, e.latlng, true, e.target._map);
                                    },
                                    mouseover: function (e) {
                                        //$log.info("Line onhover event", e);

                                        if (config.Lazy !== 0 && !dataItem.LazyLoaded()) {
                                            self.views.loadLazyProperties(dataItem, config.LayerId)
                                                .then(function (lazyProperties) {
                                                    //createPopup(config.Mnemonic, dataItem, null, true, null);

                                                    if (lazyProperties.StartDisposition) {
                                                        $log.info('Lazy properties on load', dataItem);
                                                    }
                                                });
                                        }
                                    }
                                });

                                layers.push(polygon);

                                if (dataItem.GeoType === 'linestring') {
                                    var layer = polygon.getLayers()[0];
                                    var coords = layer.getLatLngs();

                                    start = self.layers.createCircle(coords[0], dataItem.ID, config, null, false, true);
                                    end = self.layers.createCircle(coords[coords.length - 1], dataItem.ID, config, null, false, true);

                                    //Custom events work only with parent FutureGroup layer
                                    polygon.on('onselect', function (e) {
                                        if (e.target._map) {
                                            polygon.addLayer(start);
                                            polygon.addLayer(end);
                                        }

                                        //$log.info("Line onselect event", e);
                                    });

                                    //Custom events work only with parent FutureGroup layer
                                    polygon.on('onunselect', function (e) {
                                        if (e.target._map) {
                                            polygon.removeLayer(start);
                                            polygon.removeLayer(end);
                                        }
                                    });

                                    if (config.ShowIcon) {
                                        marker = self.layers.createMarker(dataItem, config, true, true);

                                        marker.on('click', function (e) {
                                            createPopup(config.LayerId, dataItem, e.target._latlng, false, e.target._map);
                                        });

                                        //polygon.addLayer(marker);
                                    }
                                }


                                break;
                            default:
                                $log.error('Unknown geometry type:', dataItem.GeoType, dataItem);
                        }

                    }

                    return layers;
                },
                createLayers: function (config, data, clickEvent, customPoint) {
                    var layers = [];

                    if (data && data.length) {
                        for (var i = data.length - 1; i >= 0; i--) {
                            layers = layers.concat(self.layers.createLayer(config, data[i], clickEvent, customPoint));
                        }
                    }

                    return layers;
                },
                getDistance: function (lat1, lng1, lat2, lng2) {
                    return getDistance(lat1, lng1, lat2, lng2);
                },
                getPopup: function () {
                    return popupObject;
                },
                createPopup: function (layerId, dataItem, latlng, pointer, map) {
                    createPopup(layerId, dataItem, latlng, pointer, map);
                },
                openPopup: function (map) {
                    openPopup(map);
                },
                closePopup: function () {
                    deletePopup();
                },
                getPatterns: function () {
                    return patterns;
                },
                createKladrPopup: function (map, latlng, address) {
                    createKladrPopup(map, latlng, address);
                },
                removeKladrPopup: function () {
                    removeKladrPopup();
                },
                isCluster: function (layer) {
                    return layer instanceof L.MarkerCluster;
                },
                isGeometric: function (layer) {
                    return self.layers.isPolygon(layer) || self.layers.isMultiPolygon(layer) || self.layers.isPolyline(layer) || self.layers.isMultiPolyline(layer);
                },
                isPolygon: function (layer) {
                    return layer.options.geotype === 'polygon';
                },
                isMultiPolygon: function (layer) {
                    return layer.options.geotype === 'multipolygon';
                },
                isPolyline: function (layer) {
                    return layer.options.geotype === 'linestring';
                },
                isMultiPolyline: function (layer) {
                    return layer.options.geotype === 'multilinestring';
                },
                openCluster: function (layer) {
                    if (layer._markers.length > 1) {
                        layer.spiderfy();
                    }
                },
                closeCluster: function (layer) {
                    if (layer._markers.length > 1) {
                        layer.unspiderfy();
                    }
                }
            };

            this.common = {
                getDistanceFromStart: function(layer, latlng) {
                    return MapProviderService.mapIsInit()
                        ? getSegmentDistance(MapProviderService.map, latlng, layer)
                        : 0;
                },
                isMoveURL: function(url) {
                    var actionParams = url.split('/').filter(function(action) {
                        return action === 'detail' || action === 'move';
                    });

                    return actionParams.indexOf('detail') !== -1 && actionParams.indexOf('move') !== -1;
                },
                generateUID: function() {
                    var d = new Date().getTime();
                    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g,
                        function(c) {
                            var r = (d + Math.random() * 16) % 16 | 0;
                            d = Math.floor(d / 16);
                            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
                        });
                }
            };
        };

        return new service();
    };

    utilsService.$inject = ['$log', '$q', 'DataService', 'MapProviderService', 'LAYERS_VIEW_DEFS', 'LAYERS_PARAM_DEFS', 'GLOBAL_PARAM_DEFS', 'format', 'ViewConfig', 'VIEW_TYPE'];

    angular.module('MapApp').
        factory('UtilsService', utilsService);

})(window.angular);