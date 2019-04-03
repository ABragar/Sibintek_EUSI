/* globals pbaAPI, globalapp */
/* jshint strict: false */
pbaAPI.ensurePath('globalapp');
(function() {
	'use strict';

	globalapp.events = {
		ping: 'ConfirmConnection',
		updateCounters: 'UpdateCounters',
		chat: {
			sendTextMessage: 'SendTextMessage',
			sendFileMessage: 'SendFileMessage',
			sendMultimediaMessage: 'SendMultimediaMessage',
			onNewMessage: 'OnTextMessageSend',
			updateMissed: 'UpdateMissedMessages',
			onUpdateMissed: 'OnUpdateMissedMessages',
			excludeConferenceMember: 'ExcludeConferenceMember',
			onConferenceMemberExclude: 'OnConferenceMemberExclude',
		},
		user: {
			changeStatus: 'ChangeCustomStatus',
			onChangeStatus: 'OnChangeCustomStatus',
			connect: 'SignIn',
			onConnect: 'OnSignIn',
			onDisconnect: 'OnSignOut',
		},
		system: {
			onNotification: 'OnServerNotification',
		}
	};
})();
