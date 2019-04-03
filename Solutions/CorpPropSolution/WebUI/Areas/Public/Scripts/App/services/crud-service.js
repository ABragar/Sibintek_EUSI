(function (angular) {
    function crudService($log, $q, DataService, CRUD_PARAM_DEFS) {

        var service = function () {

            var self = this;

            var enumsLoaded = {};
            var enums = {};

            this.isEnumLoaded = function (uitype) {
                return angular.isDefined(enumsLoaded[uitype]);
            };

            this.loadEnumValues = function (uitype) {
                return $q(function (resolve, reject) {
                    if (self.isEnumLoaded(uitype)) {
                        resolve(enums[uitype]);
                    } else {
                        enumsLoaded[uitype] = true;

                        enums[uitype] = {
                            Values: {}
                        };

                        DataService.loadEnumValues(uitype).then(function(response) {
                            $log.info('Loaded enum:', response);

                            angular.extend(enums[uitype], response);

                            resolve(enums[uitype]);
                        }, function(err) {
                            $log.error('Load enum (' + uitype + ') config error...');
                            reject(err);
                        });
                    }
                });
            };

            this.getEnumValues = function (uitype) {
                if (!self.isEnumLoaded(uitype)) {
                    enums[uitype] = {
                        Values: {}
                    };
                }
                return enums[uitype];
            };


            this.hasValidTypes = function (properties) {
                var exists = false;

                angular.forEach(properties,
                    function (prop) {
                        if (CRUD_PARAM_DEFS.IGNORE_VIEW_PROP_TYPES.indexOf(prop.Type) === -1) {
                            exists = true;
                        }
                    });

                return exists;
            };

            this.isFullColumn = function(propertyType) {
                return [
                    'OneToMany',
                    'ManyToMany',
                    'EasyCollection',
                    'Gallery',
                    'Image'
                ].indexOf(propertyType) >= 0;
            };
        };

        return new service();
    };

    crudService.$inject = ['$log', '$q', 'DataService', 'CRUD_PARAM_DEFS'];

    angular.module('MapApp').
        factory('CRUDService', crudService);

})(window.angular);