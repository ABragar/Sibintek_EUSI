(function(angular) {
	'use strict';

	var parseDialogType = function(dialogType) {
		switch (dialogType) {
			case 'conference': return 'PublicMessage';
			case 'private': return 'PrivateMessage';
		}

		return null;
	};

	var urlServiceFactory = function() {
		return {
			createConference: function() {
				return '/communication/createconference';
			},
			inviteToConference: function() {
				return '/communication/invitetoconference';
			},
			getDialog: function(dialogType, dialogId) {
				dialogType = parseDialogType(dialogType);

				if (!dialogType || !dialogId) return 'error';

				return '/communication/getdialog?dialogType=' + dialogType + '&dialogId=' + dialogId;
			},
			getDialogs: function() {
			    return '/communication/getdialogs';
			},
			getMessages: function(dialogType, dialogId) {
				dialogType = parseDialogType(dialogType);

				if (!dialogType || !dialogId) return 'error';

				return '/communication/getmessages?dialogType=' + dialogType + '&dialogId=' + dialogId;
			},
			readMessages: function(dialogType, dialogId) {
				dialogType = parseDialogType(dialogType);

				if (!dialogType || !dialogId) return 'error';

				return '/communication/readmessages?dialogType=' + dialogType + '&dialogId=' + dialogId;
			},
			getMembers: function(conferenceId) {
				if (!conferenceId) return 'error';

				return '/communication/getmembers?conferenceId=' + conferenceId;
			},
		};
	};

	angular.module('ChatApp.Services')
		.factory('UrlService', urlServiceFactory);

	urlServiceFactory.$inject = [];

}(window.angular));