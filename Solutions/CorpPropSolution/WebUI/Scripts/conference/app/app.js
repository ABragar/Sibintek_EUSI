(function(angular) {
	'use strict';

    angular.module('ChatApp', ['perfect_scrollbar', 'angularFileUpload', 'angular-svg-round-progress', 'ChatApp.Services', 'ChatApp.Controllers', 'ChatApp.Directives'])
		.run();

}(window.angular));