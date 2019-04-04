(function(angular) {
	'use strict';

	var ENTER_KEY = 13;

	var onEnterPress = function() {
		return function($scope, $element, $attrs) {
			$element.bind("keydown keypress", function(event) {

				// ONLY ENTER KEY PRESSED
				if (event.which !== ENTER_KEY) return;

				var evt = {
					preventDefault: event.preventDefault.bind(event),
					alt: event.altKey,
					ctrl: event.ctrlKey,
					shift: event.shiftKey,
				};

				$scope.$apply(function() {
					$scope.$eval($attrs.onEnterPress, { event: evt });
				});
			});
		};
	};

	angular.module('ChatApp.Directives', [])
		.directive('onEnterPress', onEnterPress);

}(window.angular));