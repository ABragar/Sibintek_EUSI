(function(angular) {
	'use strict';

	var appServiceFactory = function() {
		var errors = [];

		return {
			get errors() {
				return errors;
			},
			get hasErrors() {
				return errors.length > 0;
			}
		};
	};

	angular.module('ChatApp.Services', [])
		.factory('AppService', appServiceFactory);

	appServiceFactory.$inject = [];

}(window.angular));