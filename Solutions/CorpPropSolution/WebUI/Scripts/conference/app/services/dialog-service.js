(function(angular, globalApp, pbaAPI) {
	'use strict';

	var dialogServiceFactory = function($http, TypeService, RouteService, UrlService, UserService) {

		// ############
		// DEPENDENCIES
		// ############

		var socketEvents = globalApp.events;		
		var socketService = globalApp.socketService;
		var globalMessageService = globalApp.messageService;

		// #####
		// STATE
		// #####

		var onUpdateListeners = [];

		var dialogs = [];
		var tempDialogs = [];
		var conferenceMembers = {};
		var membersIsLoading = {};

		var isLoaded = false;
		var isLoading = false;
        var dialogIsCreating = {};

		// #######
		// METHODS
		// #######

		var getDialog = function(type, id) {
			if (!type || !id) {
				return null;
			}

			var i;

			for (i = 0; i < dialogs.length; i++) {
				if (dialogs[i].id === id && dialogs[i].type === type) {
					return dialogs[i];
				}
			}

			for (i = 0; i < tempDialogs.length; i++) {
				if (tempDialogs[i].id === id && tempDialogs[i].type === type) {
					return tempDialogs[i];
				}
			}

			return null;
		};

		var getMembers = function(dialog) {
			if (!dialog || dialog.type !== 'conference') return [];

			if (conferenceMembers[dialog.id]) {
				return conferenceMembers[dialog.id];
			}

			if (membersIsLoading[dialog.id]) {
				return conferenceMembers[dialog.id] || [];
			}

			membersIsLoading[dialog.id] = true;

			$http.get(UrlService.getMembers(dialog.id)).then(function(res) {
				membersIsLoading[dialog.id] = false;

				if (!res || !res.data) {
					RouteService.goTo.error('get members invalid response data');
					return;
				}

				conferenceMembers[dialog.id] = (res.data || [])
					.map(_parseDialog)
					.filter(function(dialog) {
						return dialog.id !== UserService.current.id;
					});

				_execUpdate();

			}, function() {
				RouteService.goTo.error('get members server error');
			});

			return [];
		};

		var _makeUid = function(type, id) {
			if (!type || !id) return null;

			return 'dialog_' + type + '_' + id;
		};

		var _parseMember = function(data) {
			return _parseDialog(data);
		};

		var _parseDialog = function(data, isNew) {
			if (!data) return null;

			var type = TypeService.dialog(data.DialogType);
            var title = data.Title && data.Title.trim() || '<Без имени>';

			return {
				type: type,
				id: data.ID,
				uid: _makeUid(type, data.ID),
				unreaded: data.UnreadedCount || 0,
				imageId: data.ImageID,
				title: title,
				isOnline: false,
				isNew: isNew === true,
			};
		};

		var releaseControllers = function() {
			onUpdateListeners = [];
		};

		var onUpdate = function(callback) {
			if (typeof callback !== 'function') {
				return;
			}

			if (isLoaded && !isLoading) {
				setTimeout(callback, 0);
			}
				
			onUpdateListeners.push(callback);
		};

		var _execUpdate = function() {
			onUpdateListeners.forEach(function(listener) {
				listener();
			});
		};

		var cleanTemp = function() {
			tempDialogs = [];
		};

		var newDialog = function(type, id) {

			// ENSURE TYPE
			type = TypeService.dialog(type);

			// VALIDATE PARAMETERS
			if (!type || !(id > 0)) {
				RouteService.goTo.error();
			}

			// SEARCH DIALOG
			var dialog = getDialog(type, id);

			// THAT DIALOG ALREADY EXISTS
			if (dialog) {
				RouteService.goTo.messages(type, id);
			}

            if (dialogIsCreating[type + ':' + id]) {
                return;
            }
            
            dialogIsCreating[type + ':' + id] = true;

			// LOAD DIALOG (CONFERENCE/USER) FROM SERVER
			$http.get(UrlService.getDialog(type, id)).then(function(res) {

				// PARSE DIALOG DATA
				var dialog = _parseDialog(res.data, true);

				// CANNOT PARSE THIS DIALOG DATA
				if (!dialog) {
					RouteService.goTo.error();
					return;
				}

				tempDialogs.push(dialog);

				_execUpdate();

				RouteService.goTo.messages(type, id);

			}, function() {

				RouteService.goTo.error('add dialoG error', type, id);

			});
		};

		var load = function(callback) {
			if (isLoading) return;

			isLoading = true;

			// GET SERVER DATA			
			$http.get(UrlService.getDialogs()).then(function(res) {

				dialogs = (res.data || []).map(_parseDialog);

				globalMessageService.updateMissed();

				isLoaded = true;
				isLoading = false;

				_execUpdate();

				if (callback)
					callback();

			}, function() {

				console.error('[dialog-service] load: error');

				isLoaded = true;
				isLoading = false;

				console.error('[dialog-service] _execUpdate: load dialogs error');
				_execUpdate();

			});
		};

		var _initConference = function(userIds, createNew, conferenceId) {
			if (!userIds || !userIds.length) return;

			var url = UrlService[createNew ? 'createConference' : 'inviteToConference']();
			var params = { userIds: userIds };

			if (!createNew) {
				params.conferenceId = conferenceId;
			}

			$http.post(url, params).then(function(res) {
				if (!res || !res.data) {
					if (!createNew) {
						pbaAPI.errorMsg('Вы не являетесь создателем конференции');
						return;
					}

					RouteService.goTo.error('Error creating conference', userIds, res);
					return;
				}

				var dialog = _parseDialog(res.data);

				if (!dialog) {
					return;
				}

				// UPDATE DIALOG MEMBERS
				if (!createNew) {
					conferenceMembers[dialog.id] = res.data.Members
						.map(_parseMember)
						.filter(function(dialog) {
							return dialog.id !== UserService.current.id;
						});
				}

				if (!dialogs.some(function(d) {
					return d.uid === dialog.uid;
				})) {
					dialogs.push(dialog);
				}

				_execUpdate();

				RouteService.goTo.messages(dialog.type, dialog.id);

			}, function() {
				RouteService.goTo.error('Error creating conference', userIds);
			});
		};

		var inviteToConference = function(conferenceId, userIds) {
			_initConference(userIds, false, conferenceId);
		};

		var createConference = function(userIds) {
			_initConference(userIds, true);
		};

		var excludeFromConference = function(conferenceId, userId) {
			socketService.invoke(socketEvents.chat.excludeConferenceMember, conferenceId, userId);
		};

		// #################
		// LISTEN TO SOCKETS
		// #################
		
		socketService.on(socketEvents.chat.onConferenceMemberExclude, function(conferenceId, userId) {

			if (!conferenceMembers[conferenceId]) {
				return;
			}

			// REMOVE THAT USER FROM APPROPRIATE CONFERENCE MEMBERS LIST
			conferenceMembers[conferenceId] = conferenceMembers[conferenceId].filter(function(member) {
				return member.id !== userId;
			});

			// IF THAT USER IS CURRENT USER ITSELF
			if (UserService.current.id === userId) {

				// REMOVE THAT DIALOG FROM DIALOGS LIST
				dialogs = dialogs.filter(function(dialog) {
					return dialog.id !== conferenceId;
				});

				// RELOAD DIALOGS LIST
				load();
			} else {
				_execUpdate();
			}

		});

		globalMessageService.onUpdateMissed(function(data) {
			if (isLoading || !isLoaded) return;

			var requireUpdate = false;
			var requireReload = false;

			data = data.reduce(function(result, item) {
				var type = TypeService.dialog(item.DialogType);
				var id = item.Id;
				var uid = _makeUid(type, id);

				// has such dialog
				if (dialogs.some(function(dialog) { return dialog.uid === uid; })) {
					result[uid] = item;

				// no such dialog
				} else {
					requireReload = true;
				}

				return result;
			}, {});

			dialogs.forEach(function(dialog) {
				var updated = data[dialog.uid];

				if (!updated) {
					dialog.unreaded = 0;
					requireUpdate = true;
				} else if (updated.MissedCount !== dialog.unreaded) {
					dialog.unreaded = updated.MissedCount;
					requireUpdate = true;
				}
			});

			if (requireUpdate) {
				_execUpdate();
			}

			if (requireReload) {
				load();
			}
		});

		// ###
		// API
		// ###

		return {
			// GETTERS
			get isLoaded() {
				return isLoaded;
			},
			get dialogs() {
				return dialogs;
			},
			// METHODS
			cleanTemp: cleanTemp,
			//addDialog: addDialog,
			createConference: createConference,
			inviteToConference: inviteToConference,
			excludeFromConference: excludeFromConference,
			getDialog: getDialog,
			newDialog: newDialog,
			getMembers: getMembers,
			releaseControllers: releaseControllers,
			onUpdate: onUpdate,
			load: load,
		};
	};

	angular.module('ChatApp.Services')
		.factory('DialogService', dialogServiceFactory);

	dialogServiceFactory.$inject = ['$http', 'TypeService', 'RouteService', 'UrlService', 'UserService'];

}(window.angular, window.globalapp, window.pbaAPI));