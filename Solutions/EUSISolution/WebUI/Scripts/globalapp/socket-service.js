/* globals $, globalapp */
(function() {
    'use strict';

    // #######
    // PRIVATE
    // #######

    $.support.cors = true;

    var events = globalapp.events;

    var ConnectionState = {
        0: 'connecting',
        1: 'connected',
        2: 'reconnecting',
        4: 'disconnected',
    };

    var connection = $.hubConnection();

    var hub = connection.createHubProxy('globalHub');

    var deferredInvokes = [];

    var invokeOnConnected = function () {
        deferredInvokes.forEach(function (deferredInvoke) {
            hub.invoke.apply(hub, deferredInvoke);
        });

        deferredInvokes = [];
    };

    // ###############
    // SERVICE METHODS
    // ###############
    var connectionStatus = function() {
        return ConnectionState[connection.state] || 'error';
    };

    var on = function(eventName, callback) {
        if (eventName && typeof callback === 'function') {
            hub.on(eventName, callback);
        }
    };

    var once = function(eventName, callback) {
        if (!eventName || typeof callback !== 'function') {
            return;
        }

        hub.on(eventName, function onceCallback() {
            hub.off(eventName, onceCallback);
            callback.apply(null, arguments);
        });
    };

    var off = function(eventName, callback) {
        if (eventName && typeof callback === 'function') {
            hub.off(eventName, callback);
        }
    };

    var invoke = function(eventName) {
        if (!eventName) return;

        // IF CONNECTION STATE IS NOT "CONNECTED"
        if (connectionStatus() === ConnectionState[1]) {
            hub.invoke.apply(hub, arguments);
            return;
        }

        deferredInvokes.push(arguments);
    };

    connection.start().done(function () {
        invoke(events.user.connect);

        setInterval(invoke.bind(null, events.ping), 20000);

        invokeOnConnected();
    });

    connection.reconnected(invokeOnConnected);

    globalapp.socketService = {
        on: on,
        off: off,
        once: once,
        invoke: invoke,
        connection: connection,
        connectionStatus: connectionStatus,
    };

}());
