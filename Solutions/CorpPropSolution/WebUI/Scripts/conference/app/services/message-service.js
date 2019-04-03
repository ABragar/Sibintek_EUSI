(function(angular, globalMessageService, pbaAPI) {
	'use strict';

	var messageServiceFactory = function($http, DialogService, TypeService, UserService, UrlService) {

		// #####
		// STATE
		// #####
		
		var currentUser = UserService.current;

		var onNewMessageListeners = [];
		var onUpdateListeners = [];

		var messages = {};
		var loading = {};
		var loaded = {};

		// #######
		// METHODS
		// #######
		
		var _resolveDialog = function(message) {
			if (!message || !message.$$$parsed) return null;

			var dialogType = message.isPrivate ? 'private' : 'conference';
			var dialogId = message.isConference || message.isOutcoming ? message.toId : message.fromId;

			return DialogService.getDialog(dialogType, dialogId);
		};

		var _addMessage = function(dialog, message) {
			if (!messages[dialog.uid]) {
				messages[dialog.uid] = [];
			}

			if (!message.$$$parsed) {
				message = _parseMessage(message, dialog.type);
			}

			messages[dialog.uid].push(message);
		};

		var getMessages = function(dialog) {
			return dialog && messages[dialog.uid] || [];
		};

		var _parseDate = function(serverDateString) {

			// NOTHING TO PARSE
			if (!serverDateString) {
				return null;
			}

			// APPLY REGEXP ON THAT STRING
			var reg = /^(\d\d)\.(\d\d)\.(\d{4}) (\d\d):(\d\d):(\d\d)$/;
			var res = reg.exec(serverDateString) || [];

			// CONVERT RESULT STRINGS TO NUMBERS AND IGNORE BAD-CONVERTED-ONES
			// [ DAY, MONTH, YEAR, HOURS, MINUTES, SECONDS ]
			var d = Array.prototype.slice.call(res, 1).map(function(str) {
				return parseInt(str);
			}).filter(function(num) {
				return !isNaN(num);
			});

			// IF ANY OF THEM CANNOT BE CONVERTED PROPERLY
			if (d.length < 6) {
				return null;
			}

			return new Date(d[2], d[1], d[0], d[3], d[4], d[5]) || null;
		};

		var _parseMessage = function(msg, dialType) {
			dialType = TypeService.dialog(dialType);

			return {
				id: msg.ID,
				date: _parseDate(msg.Date),
				text: msg.TextMessage,

				file: msg.File ? {
					id: msg.File.FileID,
					title: msg.File.FileName,
					size: msg.File.Size,
					created: msg.File.CreationDate,
                    orig: msg.File,
				} : null,

				fromId: msg.FromId,
				fromImageId: msg.FromImageId,

				toId: msg.ToId,
				toImageId: msg.ToImageId,

				type: TypeService.message(msg.MessageType),

				multimediaType: TypeService.multimedia(msg.MultimediaType),
				multimediaId: msg.MultimediaId,

				isPrivate: dialType === 'private',
				isConference: dialType === 'conference',
				isIncoming: msg.FromId !== currentUser.id,
				isOutcoming: msg.FromId === currentUser.id,
				isNew: msg.IsNew,

				get $$$parsed() { return true; }
			};
		};

		var releaseControllers = function() {
			onUpdateListeners = [];
			onNewMessageListeners = [];
		};

		var onUpdate = function(callback) {
			if (typeof callback === 'function') {
				onUpdateListeners.push(callback);
			}
		};

		var _execUpdate = function(dialog) {
			onUpdateListeners.forEach(function(listener) {
				listener(dialog);
			});
		};

		var readMessages = function(dialog) {
			if (!dialog) return;

			var hasUnreaded = messages[dialog.uid].some(function(message) { return message.isNew; });
			if (!hasUnreaded) return;

			var url = UrlService.readMessages(dialog.type, dialog.id);

			$http.post(url).then(function() {

				globalMessageService.updateMissed();

				_execUpdate(dialog);

			}, function() { console.error('[message-service] readMessages: error'); });

			messages[dialog.uid] = (messages[dialog.uid] || []).map(function(message) {
				message.isNew = false;
				return message;
			});
		};

		var load = function(dialog) {
			if (!dialog || loading[dialog.uid]) return;

			loading[dialog.uid] = true;

			// GET SERVER DATA
			$http.get(UrlService.getMessages(dialog.type, dialog.id)).then(function(res) {

				messages[dialog.uid] = (res.data || []).map(function(message) {
					return _parseMessage(message, dialog.type);
				});

				loading[dialog.uid] = false;
				loaded[dialog.uid] = true;

				_execUpdate(dialog);

			}, function() {

				console.error('[message-service] load: error');

				loading[dialog.uid] = false;
				loaded[dialog.uid] = true;

				console.error('[message-service] _execUpdate: load messages error');
				_execUpdate(dialog);

			});
		};

		var isLoaded = function(dialog) {
			return dialog && !!loaded[dialog.uid] || false;
		};

		var onNewMessage = function(callback) {
			if (typeof callback === 'function') {
				onNewMessageListeners.push(callback);
			}
		};

		var sendTextMessage = function(dialog, text) {
			var type = null;

			switch (dialog.type) {
				case 'private':
					type = 'PrivateMessage';
					break;
				case 'conference':
					type = 'PublicMessage';
					break;
				default: return;
			}

			globalMessageService.sendTextMessage(type, dialog.id, text);
		};

		var sendFileMessage = function(dialog, file) {
			var type = null;

			switch (dialog.type) {
				case 'private':
					type = 'PrivateMessage';
					break;
				case 'conference':
					type = 'PublicMessage';
					break;
				default: return;
			}

			globalMessageService.sendFileMessage(type, dialog.id, pbaAPI.json.stringify(file));
		};

		var sendMultimediaMessage = function (dialog, files, multimediaType) {
		    var type = null;

		    switch (dialog.type) {
		        case 'private':
		            type = 'PrivateMessage';
		            break;
		        case 'conference':
		            type = 'PublicMessage';
		            break;
		        default: return;
		    }

		    globalMessageService.sendMultimediaMessage(type, dialog.id, pbaAPI.json.stringify(files), multimediaType);
	    };

		// #################
		// LISTEN TO SOCKETS
		// #################
		
		globalMessageService.onNewMessage(function (message, dialogType) {
			var msg = _parseMessage(message, dialogType);
			var dialog = _resolveDialog(msg);

			if (!msg) {
				console.error('Cannot handle new message', message, msg);
				return;
			}

			if (!dialog || dialog.isNew) {
				DialogService.load(function() {
					var msg = _parseMessage(message, dialogType);
					var dialog = _resolveDialog(msg);
					if (dialog) {
						load(dialog);
					}
				});
				return;
			}

			_addMessage(dialog, msg);

			globalMessageService.updateMissed();

			_execUpdate(dialog);

			onNewMessageListeners.forEach(function(listener) {
				listener(dialog, msg);
			});
		});

		// ###
		// API
		// ###

		return {
			// METHODS
			releaseControllers: releaseControllers,
			sendTextMessage: sendTextMessage,
			sendFileMessage: sendFileMessage,
			sendMultimediaMessage: sendMultimediaMessage,
			onNewMessage: onNewMessage,
			readMessages: readMessages,
			getMessages: getMessages,
			isLoaded: isLoaded,
			onUpdate: onUpdate,
			load: load,
		};
	};

	angular.module('ChatApp.Services')
		.factory('MessageService', messageServiceFactory);

	messageServiceFactory.$inject = ['$http', 'DialogService', 'TypeService', 'UserService', 'UrlService'];

}(window.angular, window.globalapp.messageService, window.pbaAPI));