(function (angular) {
    function cadastreController($scope, $log, CadastreService) {
        var self = this;

        this.dataTypes = CadastreService.getTypes();
        this.search = '';
        this.selectedResult = -1;

        this.customSearch = function() {
            CadastreService.customSearch(self.search);
        };

        this.getSelected = function () {
            return CadastreService.getSelectedType();
        };

        this.select = function(item) {
            angular.forEach(this.dataTypes, function(t) {
                if (t.id !== item.id) {
                    t.selected = false;
                } else {
                    t.selected = true;
                }
            });
        };

        this.selectResult = function (index) {
            if (self.selectedResult === index) {
                self.selectedResult = -1;
            } else {
                self.selectedResult = index;
            }
        };

        this.getSearchResults = function() {
            return CadastreService.getSearchResult();
        };

        $scope.$watch(function() {
            return CadastreService.getSearchString();
        }, function(value) {
            self.search = value;
        }, true);

        $scope.$watch(function () {
            return self.getSearchResults();
        }, function (value) {
            //self.results = value;
            self.selectedResult = -1;
            //$scope.$applyAsync();
            $log.info('kadastr seach:', value, value.length);
        }, true);

        $scope.$watch(function () {
            return self.getSelected();
        }, function (value, oldValue) {
            if (angular.isDefined(value) && value !== oldValue) {
                CadastreService.reSearch();
            }
        }, true);
    }

    angular.module('MapApp')
        .controller('CadastreController', cadastreController);

    cadastreController.$inject = ['$scope', '$log', 'CadastreService'];
})(window.angular);


(function(angular) {
    function cadastreDetailController($scope, $sce, $log, CadastreService, URLFactory, deviceDetector, KAD_PARCEL_STATES, KAD_CATEGORY_TYPES, KAD_PARCEL_OWNERSHIP, KAD_AREA_TYPES, KAD_MEASURMENT_UNITS, KAD_UTILIZATIONS, $routeParams) {
        var self = this;

        this.isMobile = deviceDetector.isMobile();
        this.id = $routeParams.id;
        this.code = $routeParams.code;
        this.opened = true;
        this.feature = null;
        this.loading = true;
        this.dashboardUrl = '#';

        this.parcel_states = KAD_PARCEL_STATES;
        this.category_types = KAD_CATEGORY_TYPES;
        this.parcel_ownership = KAD_PARCEL_OWNERSHIP;
        this.area_types = KAD_AREA_TYPES;
        this.measurement_units = KAD_MEASURMENT_UNITS;
        this.utilizations = KAD_UTILIZATIONS;

        this.sanitize = function(html) {
            return $sce.trustAsHtml(html);
        };

        CadastreService.loadDetailInfo(this.code, this.id).then(function(data) {
            if (data.status === 200 && data.feature) {
                self.feature = data.feature;
                self.dashboardUrl = URLFactory.createCadastreDashboardDetailUrl(self.feature.attrs.cn, self.feature.type);
            }

            self.loading = false;
            },
            function(err) {
                
            });
    }

    angular.module('MapApp')
        .controller('CadastreDetailController', cadastreDetailController);

    cadastreDetailController.$inject = ['$scope', '$sce', '$log', 'CadastreService', 'URLFactory', 'deviceDetector', 'KAD_PARCEL_STATES', 'KAD_CATEGORY_TYPES', 'KAD_PARCEL_OWNERSHIP', 'KAD_AREA_TYPES', 'KAD_MEASURMENT_UNITS', 'KAD_UTILIZATIONS', '$routeParams'];
})(window.angular);