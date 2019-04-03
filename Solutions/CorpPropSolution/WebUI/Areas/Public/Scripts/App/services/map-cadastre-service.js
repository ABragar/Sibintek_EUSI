(function (angular) {
    function cadastreService($log, $q, DataService) {

        var service = function () {

            var self = this;

            var config = {
                types: [
                    {
                        id: 1,
                        code: 1,
                        title: 'Участки',
                        selected: true
                    },
                    {
                        id: 2,
                        code: 2,
                        title: 'Кварталы',
                        selected: false
                    },
                    {
                        id: 3,
                        code: 3,
                        title: 'Районы',
                        selected: false
                    },
                    {
                        id: 4,
                        code: 4,
                        title: 'Округа',
                        selected: false
                    },
                    {
                        id: 5,
                        code: 5,
                        title: 'ОКС',
                        selected: false
                    },
                    {
                        id: 6,
                        code: 6,
                        title: 'Тер. зоны',
                        selected: false
                    },
                    {
                        id: 7,
                        code: 7,
                        title: 'Границы',
                        selected: false
                    },
                    //{
                    //    id: 8,
                    //    code: 8,
                    //    title: 'Части ЗУ',
                    //    selected: false
                    //},
                    {
                        id: 9,
                        code: 9,
                        title: 'ГОК',
                        selected: false
                    },
                    {
                        id: 10,
                        code: 10,
                        title: 'ЗОУИТ',
                        selected: false
                    },
                    //{
                    //    id: 11,
                    //    code: 11,
                    //    title: 'Картооснова',
                    //    selected: false
                    //},
                    {
                        id: 12,
                        code: 12,
                        title: 'Лес',
                        selected: false
                    },
                    {
                        id: 13,
                        code: 13,
                        title: 'Красные линии',
                        selected: false
                    },
                    //{
                    //    id: 14,
                    //    code: 14,
                    //    title: 'Части ОКС',
                    //    selected: false
                    //},
                    {
                        id: 15,
                        code: 15,
                        title: 'СРЗУ',
                        selected: false
                    },
                    {
                        id: 16,
                        code: 16,
                        title: 'ОЭЗ',
                        selected: false
                    }
                ]
            };

            var options = {
                visible: false,
                search: '',
                searchParams: {
                    zoom: 0,
                    lat: 0,
                    lng: 0,
                    str: ''
                },
                features: []
            };

            var getTolerance = function(zoom) {
                return Math.pow(2, 20 - zoom);
            };

            var search = function() {
                options.search = options.searchParams.str.length ? options.searchParams.str :
                    options.searchParams.lat + ' ' + options.searchParams.lng;
                var tolerance = getTolerance(options.searchParams.zoom);
                var kadlevel = self.getSelectedType().code;

                DataService.loadCadListInfo(kadlevel, options.search, tolerance).then(function(data) {
                        options.features = data.features;
                    },
                    function(err) {
                        $log.err('Load CAD list:', err);
                    });
            };

            this.visibility = function(value) {
                options.visible = value;
            };

            this.isVisible = function() {
                return options.visible;
            };

            this.search = function (lat, lng, zoom) {
                options.searchParams.lat = lat;
                options.searchParams.lng = lng;
                options.searchParams.zoom = zoom;
                options.searchParams.str = '';
                search();
            };

            this.reSearch = function() {
                search();
            };

            this.customSearch = function(str) {
                options.searchParams.str = str;
                search();
            };

            this.loadDetailInfo = function (kadlevel, id) {
                return $q(function(resolve, reject) {
                    DataService.loadCadDetailInfo(kadlevel, id).then(function (data) {
                            resolve(data);
                        },
                        function (err) {
                            $log.error('Load CAD detail:', err);
                            reject(err);
                        });

                });
            };

            this.getTypes = function() {
                return config.types;
            };

            this.getSelectedType = function() {
                return self.getTypes().filter(function (item) {
                    return item.selected;
                })[0];
            };

            this.getSearchString = function() {
                return options.search;
            };

            this.setSearchString = function(str) {
                options.search = str;
            };

            this.getSearchResult = function() {
                return options.features;
            };

            //this.getDictionaries = function() {
            //    return config.dictionaries;
            //};

        };

        return new service();
    };

    cadastreService.$inject = ['$log', '$q', 'DataService'];

    angular.module('MapApp').
        factory('CadastreService', cadastreService);

})(window.angular);