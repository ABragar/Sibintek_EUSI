/* globals pbaAPI, globalapp */
(function() {
	'use strict';

	// ############
	// DEPENDENCIES
	// ############

	var socketEvents = globalapp.events;
	var socketService = globalapp.socketService;

	// #######
	// PRIVATE
	// #######

	var systemNotificationCallbacks = {
		'1': pbaAPI.errorMsg.bind(pbaAPI),
		'2': pbaAPI.infoMsg.bind(pbaAPI),
		'3': pbaAPI.uploadMsg.bind(pbaAPI),
		'4': pbaAPI.chatMsg.bind(pbaAPI),
	};

    // #########################
    // LISTEN TO SYSTEM MESSAGES
    // #########################

	socketService.on(socketEvents.system.onNotification, function(type, message) {
		if (systemNotificationCallbacks[type]) {
			systemNotificationCallbacks[type](message);
		}
	});

}());
