(function (angular) {
    function panoService($log, $q, DataService, URLFactory) {

        var service = function () {

            this.getPanoram = function (id) {
                return $q(function (resolve, reject) {
                    DataService.loadPanoram(id).then(function (data) {
                        resolve({
                            pano: data,
                            url: URLFactory.createPanoramImageUrl(data.FileName)
                        });
                    },
                        function (err) {
                            reject(err);
                        });
                });
            };

        };

        return new service();
    };

    panoService.$inject = ['$log', '$q', 'DataService', 'URLFactory'];

    angular.module('MapApp').
        factory('PanoService', panoService);

})(window.angular);