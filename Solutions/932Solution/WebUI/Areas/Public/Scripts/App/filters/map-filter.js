(function(angular) {

    angular.module('MapFilters', [])
        .filter('exceptSerchable', function () {
            return function (configs) {
                return configs.filter(function(config) {
                    return !config.SearchOnClick;
                });
            };
        });

})(window.angular);