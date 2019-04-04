/* globals pbaAPI, globalapp */
(function() {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var socketService = globalapp.socketService;
    var events = globalapp.events;

    // ###############
    // SERVICE METHODS
    // ###############

    var sendTextMessage = function(dialogType, dialogId, text) {
        socketService.invoke(events.chat.sendTextMessage, dialogType, dialogId, text);
    };

    var sendFileMessage = function(dialogType, dialogId, file) {
        socketService.invoke(events.chat.sendFileMessage, dialogType, dialogId, file);
    };

    var sendMultimediaMessage = function (dialogType, dialogId, files, multimediaType) {
        socketService.invoke(events.chat.sendMultimediaMessage, dialogType, dialogId, files, multimediaType);
    };

    var onNewMessage = function (callback) {
        if (typeof callback !== 'function') {
            return;
        }

        socketService.on(events.chat.onNewMessage, function(json, dialogType) {
            var msg = pbaAPI.json.parse(json);

            if (!msg) return;

            callback(msg, dialogType);
        });
    };

    var updateMissed = function() {
        socketService.invoke(events.chat.updateMissed);
    };

    var onUpdateMissed = function(callback) {
        socketService.on(events.chat.onUpdateMissed, callback);
    };

    // ######
    // EXPORT
    // ######

    globalapp.messageService = {
        sendTextMessage: sendTextMessage,
        sendFileMessage: sendFileMessage,
        sendMultimediaMessage: sendMultimediaMessage,
        onNewMessage: onNewMessage,
        updateMissed: updateMissed,
        onUpdateMissed: onUpdateMissed,
    };
})();
