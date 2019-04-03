(function (angular) {
    function dataService($log, $http, $q, URLFactory, format) {

        var service = function () {

            //CRUD
            this.loadEnumValues = function(uitype) {
                return $q(function(resolve, reject) {
                    $http({
                        method: 'GET',
                        url: URLFactory.createEnumUrl(uitype)
                    }).success(function (response) {
                        resolve(response);
                    }).error(function (err) {
                        reject(err);
                    });
                });
            };


            //CAD
            this.loadCadListInfo = function (kadlevel, query, tolerance, limit) {
                return $q(function(resolve, reject) {
                    $http.jsonp(URLFactory.createCadastreListUrl(kadlevel, query, tolerance, limit))
                        .success(function (data) {
                            resolve(data);
                        }).error(function(err) {
                            reject(err);
                        });
                });
            };

            this.loadCadDetailtInfo = function (kadlevel, id) {
                return $q(function (resolve, reject) {
                    $http.jsonp(URLFactory.createCadastreDetailUrl(kadlevel, id))
                        .success(function (data) {
                            resolve(data);
                        }).error(function (err) {
                            reject(err);
                        });
                });
            };


            //MAP
            var layersPromises = {};

            //TODO: cancelation token logic
            this.loadLayer = function (config, filters, bounds, zoom) {
                if (layersPromises[config.LayerId]) {
                    layersPromises[config.LayerId].reject('Aborted by user');
                }

                layersPromises[config.LayerId] = $q.defer();

                $http.get(URLFactory.createLayerUrl({ layerId: config.LayerId, filters: filters, bbox: bounds, zoom: zoom }), { timeout: layersPromises[config.LayerId].promise })
                    .then(function (response) {
                        layersPromises[config.LayerId].resolve(response.data);
                    }, function (err) {
                        layersPromises[config.LayerId].reject(err.statusText);
                    });

                return layersPromises[config.LayerId].promise;
            };

            this.searchLayers = function (layerIds, lat, lng, bbox, zoom, single) {
                return $q(function(resolve, reject) {
                    $http.get(URLFactory.createBboxUrl(layerIds, lat, lng, bbox, zoom, single))
                        .then(function(response) {
                                resolve(response.data);
                            },
                            function(err) {
                                reject(err);
                            });
                });
            };

            this.globalsearch = function (layerIds, query, page, size) {
                return $q(function (resolve, reject) {
                    $http.get(URLFactory.createGlobalSearchUrl(layerIds, query, page, size)).then(function(response) {
                            resolve(response.data);
                        },
                        function(err) {
                            reject(err);
                        });
                });
            };

            this.loadBorder = function() {
                return $q(function(resolve, reject) {
                    $http.get(URLFactory.createBorderURL()).success(function(response) {
                            resolve(response);
                        },
                        function(err) {
                            reject(err);
                        });
                });
            };

            //Panoram
            this.loadPanoram = function(id) {
                return $q(function (resolve, reject) {
                    $http.get(URLFactory.createPanoramObjectUrl(id))
                        .success(function (data) {
                            resolve(data);
                        })
                        .error(function (err) {
                            reject(err);
                        });
                });
            };


            //Utils
            this.loadFilter = function (config) {
                return $q(function(resolve, reject) {
                    $http.get(URLFactory.createFiltersUrl(config.LayerId)).success(function (response) {
                        resolve(response);
                    }).error(function (err) {
                        reject(err);
                    });
                });
            };

            this.loadLazyProperties = function(id, layerId) {
                return $q(function(resolve, reject) {
                    $http.get(URLFactory.createLazyPropertiesUrl(id, layerId))
                        .success(function (data) {
                            resolve(data);
                        })
                        .error(function (err) {
                            reject(err);
                        });
                });
            };

            this.loadConfigCounter = function(layerId, filters) {
                return $q(function(resolve, reject) {
                    $http.get(URLFactory.createMnemonicCountUrl(layerId, filters))
                        .then(function (response) {
                            resolve(response);
                        }, function (err) {
                            reject(err);
                        });
                });
            };

            this.loadLayerList = function (layerId, page, query, filters) {
                return $q(function(resolve, reject) {
                    $http.get(URLFactory.createLayerListViewUrl(layerId, page, query, filters))
                        .success(function (response) {
                            resolve(response);
                        })
                        .error(function (err) {
                            reject(err);
                        });
                });
            };

            this.loadClusterList = function (layerId, id, zoom, page, query) {
                return $q(function (resolve, reject) {
                    $http.get(URLFactory.createClusterListViewUrl(layerId, id, zoom, page, query))
                        .success(function (response) {
                            resolve(response);
                        })
                        .error(function (err) {
                            reject(err);
                        });
                });
            };

            this.loadDetailData = function (mnemonic, id) {
                return $q(function (resolve, reject) {
                    $http.get(URLFactory.createDetailView(mnemonic, id))
                        .success(function (response) {
                            resolve(response);
                        })
                        .error(function (err) {
                            reject(err);
                        });
                });
            };

            //Widgets
            this.loadWidgetTemplate = function (urltemplate) {
                return $q(function(resolve, reject) {
                    $http.get(urltemplate)
                        .success(function (data) {
                            resolve(data);
                        })
                        .error(function (err) {
                            reject(err);
                        });
                });
            };
        };

        return new service();
    };

    dataService.$inject = ['$log', '$http', '$q', 'URLFactory', 'format'];

    angular.module('MapApp').
        factory('DataService', dataService);

})(window.angular);