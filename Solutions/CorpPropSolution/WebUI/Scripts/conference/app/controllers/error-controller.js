(function(angular) {
	'use strict';

	var errorController = function(AppService, RouteService) {
        RouteService.onRouteChange(function(page, state) {
            if (page !== 'error') return;
            
            if (!AppService.hasErrors) {
                RouteService.goTo.dialogs();
                return;
            }

            console.error('errors:', AppService.errors); 
        });
	};

	angular.module('ChatApp.Controllers')
		.controller('ErrorController', errorController);

	errorController.$inject = ['AppService', 'RouteService'];

}(window.angular));