(function(angular) {
	'use strict';

	var dialogActionsController = function(DialogService, RouteService, TypeService) {
        RouteService.onRouteChange(function(page, state) {
            if (page !== 'dialog-action') return;
            
            var action = state.action;
            var type = TypeService.dialog(state.dialogType);
            var id = state.dialogId;

            if (!(id > 0) || !action || !type) return;

            switch (action) {
                case 'new':
                    DialogService.newDialog(type, id);
                    break;
                default:
                    RouteService.goTo.error();
                    break;
            }
        });
	};

	angular.module('ChatApp.Controllers')
		.controller('DialogActionsController', dialogActionsController);

	dialogActionsController.$inject = ['DialogService', 'RouteService', 'TypeService'];

}(window.angular));