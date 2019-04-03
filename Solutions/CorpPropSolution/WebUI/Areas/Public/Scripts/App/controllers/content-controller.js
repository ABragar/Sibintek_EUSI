(function (angular) {
    function contentController($scope, $log, $sce, $uibModal, $location, $routeParams, WidgetService) {
        var self = this;

        //TODO: sidebar not worket correctly

        this.widget = WidgetService.getWidget($routeParams.layerId);
        this.url = $sce.trustAsResourceUrl(this.widget.resourceUrl($routeParams.id));
        this.frame = this.widget.options.frame;

        this.isModal = this.widget.options.type !== 'sidebar';
        this.active = !this.isModal;
        this.opened = !this.isModal;

        this.loading = true;

        this.onLoad = function () {
            self.loading = false;
            $scope.$applyAsync();
        };

        var modalPreview = function() {
            var modal = $uibModal.open({
                animation: true,
                windowClass: 'modal-fixed-size',
                templateUrl: 'contentmodal.container.html',
                controller: 'ContentModalController',
                controllerAs: 'contentmodal',
                size: self.widget.options.size,
                resolve: {
                    data: function() {
                        return {
                            id: $routeParams.id,
                            layerId: self.widget.options.layerId,
                            mnemonic: self.widget.options.mnemonic,
                            widget: self.widget
                        };
                    }
                }
            });

            modal.result.then(function () {
                $location.url('/');
            }, function () {
                $location.url('/');
            });
        };

        if (this.isModal) {
            modalPreview();
        }

        $log.info('Widget:', this.widget, this.resourceUrl);
    };

    angular.module('MapApp')
       .controller('ContentController', contentController);

    contentController.$inject = ['$scope', '$log', '$sce', '$uibModal', '$location', '$routeParams', 'WidgetService'];
})(window.angular);

(function (angular) {
    function contentModalController($scope, $log, $sce, $uibModalInstance, WidgetService, data) {
        var self = this;

        this.id = data.id;
        this.layerId = data.layerId;
        this.mnemonic = data.mnemonic;
        this.url = data.widget.resourceUrl(data.id);
        this.title = data.widget.options.title;
        this.frame = data.widget.options.frame;
        this.template = $sce.trustAsHtml(data.widget.options.template);

        this.loading = true;

        if (!data.widget.templateIsLoaded && !data.widget.options.frame) {
            WidgetService.loadTemplate(data.widget).then(function (template) {
                self.loading = true;
                self.template = $sce.trustAsHtml(template);
            }, function (error) {
                self.onLoad();
            });
        }

        this.Close = function () {
            $uibModalInstance.close();
        };

        this.onLoad = function () {
            self.loading = false;
            $scope.$applyAsync();
        };
    };

    angular.module('MapApp')
        .controller('ContentModalController', contentModalController);

    contentModalController.$inject = ['$scope', '$log', '$sce', '$uibModalInstance', 'WidgetService', 'data'];

})(window.angular);
