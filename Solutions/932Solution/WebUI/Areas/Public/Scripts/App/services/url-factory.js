(function (angular) {
    var objectToParam = function (data) {

        var serialize = function(obj, prefix) {
            var str = [];

            angular.forEach(obj,
                function(item, key) {
                    var prefixKey = prefix ? prefix + (angular.isArray(obj) ? '' : '[' + key + ']') : key;
                    str.push(angular.isObject(item)
                        ? serialize(item, prefixKey)
                        : encodeURIComponent(prefixKey) + '=' + encodeURIComponent(item));
                });

            return str.join('&');
        };

        return serialize(data);
    };

    angular.module('MapApp')
        .factory('URLFactory', ['$log', '$location', 'format', function ($log, $location, format) {
            var URLFactory = function () {

                this.createBorderURL = function () {
                    return '/Areas/Public/Scripts/App/data/mo.json';
                };

                this.createBboxUrl = function (layerIds, lat, lng, bbox, zoom, single) {
                    return format('/Public/Map/GetAroundGeoObjects?{}',
                        objectToParam({
                            layerIds,
                            lat,
                            lng,
                            bbox,
                            zoom,
                            single
                        }));
                };

                this.createPanoramBboxUrl = function() {
                    return '/Public/Map/GetPanoramObjects';
                };

                //this.createStandartObjectUrl = function() {
                //    return '/Standart/Get';
                //};

                this.createPanoramObjectUrl = function(id) {
                    return format('/Public/Map/GetPanoramObject/{}', id);
                };

                this.createPanoramImageUrl = function(fileName) {
                    return format('{protocol}://{host}:{port}/Areas/Public/Content/panorams/{fileName}.jpg?{random}',
                        {
                            protocol: $location.$$protocol,
                            host: $location.$$host,
                            port: $location.$$port,
                            fileName: fileName,
                            random: (Math.random() * (99999999 - 1111) + 1111)
                        });
                };

                this.createLayerUrl = function(data) {
                    return format('/Public/Map/GetGeoObjects?{}', objectToParam(data));
                };

                this.createMnemonicCountUrl = function (layerId, filters) {
                    return format('/Public/Map/GetGeoObjectCount?{}', objectToParam({
                        layerId,
                        filters
                    }));
                };

                this.createDetailView = function(mnemonic, id) {
                    return format('/api/crud/{mnemonic}/preview/{id}', { id, mnemonic });
                };

                this.createClusterListViewUrl = function(layerId, id, zoom, page, search) {
                    page = angular.isUndefined(page) ? 1 : page;
                    return format('/Public/Map/GetGeoObjectsByCluster?layerId={layerId}&clusterId={id}&zoom={zoom}&page={page}&pageSize=15&searchString={search}', { layerId, id, zoom, page, search });
                };

                this.createLayerListViewUrl = function(layerId, page, search, filters) {
                    page = angular.isUndefined(page) ? 1 : page;

                    return format('/Public/Map/GetPagingGeoObjects?{}', objectToParam({
                        layerId: layerId,
                        page: page,
                        searchString: search,
                        filters: filters,
                        pageSize: 15
                    }));
                };

                this.createFiltersUrl = function(layerId) {
                    return format('/Public/Map/GetFilters?layerId={}', layerId);
                };

                this.createEnumUrl = function(uitype) {
                    return format('/api/standard/getUiEnum/{}', uitype);
                };

                this.createCadastreDetailUrl = function(kadlevel, id) {
                    return format('https://pkk5.rosreestr.ru/api/features/{kadlevel}/{id}?callback=JSON_CALLBACK', { kadlevel, id });
                };

                this.createCadastreListUrl = function(kadlevel, query, tolerance, limit) {
                    tolerance = angular.isDefined(tolerance) ? tolerance : 300;
                    limit = angular.isDefined(limit) ? limit : 20;
                    return format('https://pkk5.rosreestr.ru/api/features/{kadlevel}?text={query}&tolerance={tolerance}&limit={limit}&callback=JSON_CALLBACK', { kadlevel, query, tolerance, limit });
                };

                this.createCadastreDashboardDetailUrl = function(kadNumber, type) {
                    return format('#/KadDashboard/{kadNumber}', { kadNumber, type });
                };

                this.createObjectDashboardDetailUrl = function(id, mnemonic) {
                    return format('/EntityType/{mnemonic}-Frame-{id}', { mnemonic, id });
                };

                this.createGlobalSearchUrl = function (layerIds, query, page, pageSize) {
                    return format('/Public/Map/FullTextSearchInLayers?{}',
                        objectToParam({
                            searchStr: query,
                            layerIds: layerIds,
                            page: page ? page : 1,
                            pageSize: pageSize ? pageSize : 15
                        }));
                };

                this.createLazyPropertiesUrl = function(id, layerId) {
                    return format('/Public/Map/GetLazyProperties?layerId={layerId}&id={id}', { id, layerId });
                };

            };

            return new URLFactory();
        }]);

})(window.angular);