(function (angular) {
    angular.module('MapApp')
        .factory('ContentWidget', ['$log', function ($log) {
            var ContentWidget = function (params) {
                var self = this;

                this.options = {
                    mnemonic: null,
                    layerId: null,
                    title: '',
                    type: 'modal', //modal, sidebar
                    size: 'lg', //lg, fullscreen
                    frame: false,
                    global: false,
                    template: '',
                    urltemplate: ''
                };

                angular.extend(this.options, params);

                if (this.options.resourceUrl) {
                    this.resourceUrl = this.options.resourceUrl;
                } else {
                    this.resourceUrl = this.options.resourceUrl = function (id) {
                        return format(self.options.urltemplate, { layerId: self.options.layerId, mnemonic: self.options.mnemonic, id: id });
                    };
                }

                this.templateIsLoaded = false;
            };

            return ContentWidget;
        }]);
})(window.angular);