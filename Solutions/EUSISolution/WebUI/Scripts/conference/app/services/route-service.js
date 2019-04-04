(function(angular) {
	'use strict';
    
	var routeServiceFactory = function(AppService, TypeService) {
        var listeners = [];
        
        var currentPage = 'dialogs';
        var currentState = {};
        
		var route = function(page, newState) {
            switch (page) {
                case 'dialogs':
                case 'loading':
                    currentPage = page;
                    currentState = {};
                    break;
                case 'dialog-action':
                    if (!newState || !newState.action || !newState.dialogType || !newState.dialogId)
                        return route('error', {msg: 'Invalid state in route "dialog-action"', args: newState});
                    currentPage = page;
                    currentState = {
                        action: newState.action,
                        dialogType: newState.dialogType,
                        dialogId: newState.dialogId,
                    };
                    break;
                case 'messages':
                    if (!newState || !newState.dialogType || !newState.dialogId)
                        return route('dialogs');
                    currentPage = page;
                    currentState = {
                        dialogType: newState.dialogType,
                        dialogId: newState.dialogId
                    };
                    break;
                case 'error':
                    currentPage = page;
                    currentState = {
                        msg: newState && newState.msg || 'no message',
                        args: newState && newState.args || {}
                    };
                default:
                    route('dialogs');
                    break;
            }
            
            listeners.forEach(function(listener) {
               listener(currentPage, currentState); 
            });
		};
        
        setTimeout(function() {
            route('dialogs');
        }, 0);

		return {
			goTo: {
				error: function(msg) {
					AppService.errors.push({
						msg: msg,
						args: arguments,
					});

					route('error');
				},
				dialogs: function() {
					route('dialogs');
				},
				newDialog: function(dialogType, dialogId) {
					dialogType = TypeService.dialog(dialogType);

					if (dialogType) {
						route('dialog-action', {action: 'new', dialogType: dialogType, dialogId: dialogId});
					}
				},
				messages: function(dialogType, dialogId) {
					dialogType = TypeService.dialog(dialogType);

					if (dialogType) {
						route('messages', {dialogType: dialogType, dialogId: dialogId});
					}
				},
			},
            
            onRouteChange: function(callback) {
                if (typeof callback === 'function') {
                    listeners.push(callback);
                }
            },
            
            getPage: function() {
                return currentPage;
            },
            
            getState: function() {
                return $.extend({}, currentState);
            },
            
            getTitle: function(page, activeDialog) {
                switch (page) {
                    case 'dialogs':
                        return 'Диалоги';
                    case 'messages':
                        if (activeDialog.type === 'private')
                            return 'Диалог';
                        return 'Конференция';
                    case 'dialog-action':
                    case 'loading':
                        return 'Загрузка...';
                    case 'error':
                        return 'Ошибка!';
                }
            }
		};
	};

	angular.module('ChatApp.Services')
		.factory('RouteService', routeServiceFactory);

	routeServiceFactory.$inject = ['AppService', 'TypeService'];

}(window.angular));