(function (angular) {
    function panoramController($scope, $log, PanoService, $location, $uibModalInstance, panoram) {
        var self = this;

        this.title = 'Панорама';
        this.scenes = {};

        PanoService.getPanoram(panoram.id)
            .then(function(data) {
                console.log("Scope panoram", data);

                if (data) {
                    self.scenes = {};
                    self.scenes[data.pano.ID] = {
                        type: 'sphere',
                        image: data.url
                    };

                    $scope.$applyAsync();
                }                
            }, function() {
                $log.error('Load panoram error...');
            });

        this.Close = function () {
            $uibModalInstance.close();
        };
    };

    angular.module('MapApp')
        .controller('PanoramController', panoramController);

    panoramController.$inject = ['$scope', '$log', 'PanoService', '$location', '$uibModalInstance', 'panoram'];

})(window.angular);
