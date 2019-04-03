(function (angular) {
    function signalService($log, $interval, eventEmitter) {

        var service = function () {

            var self = this;

            var ConnectionState = {
                0: 'connecting',
                1: 'connected',
                2: 'reconnecting',
                4: 'disconnected'
            };

            var connection = $.hubConnection();

            var hub = connection.createHubProxy('updateVehiclePositionHub');

            var deferredInvokes = [];

            var invokeOnConnected = function () {
                deferredInvokes.forEach(function (deferredInvoke) {
                    hub.invoke.apply(hub, deferredInvoke);
                });

                deferredInvokes = [];
            };

            this.connectionStatus = function () {
                return ConnectionState[connection.state] || 'error';
            };

            var invoke = function (eventName) {
                if (!eventName) return;

                if (self.connectionStatus() === ConnectionState[1]) {
                    hub.invoke.apply(hub, arguments);
                    return;
                }

                deferredInvokes.push(arguments);
            };

            var ping = function() {
                invoke('ping');
            };

            hub.on('pong', function (data) {
                self.emit('pong', data);
            });

            hub.on('changeLocation', function (data) {
                self.emit('changeLocation', data);
            });

            connection.start().done(function () {
                $interval(ping, 20000, false);

                invokeOnConnected();

                $log.info('SignalR connected...');
            });

            connection.reconnected(invokeOnConnected);
        };

        eventEmitter.inject(service);

        return new service();
    };

    signalService.$inject = ['$log', '$interval', 'eventEmitter'];

    angular.module('MapApp').
        factory('SignalService', signalService);

})(window.angular);