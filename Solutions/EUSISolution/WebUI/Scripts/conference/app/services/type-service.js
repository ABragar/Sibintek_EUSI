(function(angular) {
	'use strict';

	var typeServiceFactory = function() {
		return {
			dialog: function(type) {
				if (type && type.toLowerCase) {
					type = type.toLowerCase();
				}

				switch (type) {
					case 10:
					case '10':
					case 'private':
					case 'privatemessage':
						return 'private';
					case 20:
					case '20':
					case 'public':
					case 'publicmessage':
					case 'conference':
						return 'conference';
				}

				console.error('Cannot parse dialog type', type);
				return null;
			},
			message: function(type) {
				if (type && type.toLowerCase) {
					type = type.toLowerCase();
				}

				switch (type) {
					case 0:
					case '0':
					case 'text':
						return 'text';
					case 1:
					case '1':
					case 'file':
						return 'file';
				    case 2:
				    case '2':
				    case 'multimedia':
				        return 'multimedia';
				    case 3:
				    case '3':
				    case 'presentation':
				        return 'presentation';
				    case 99:
					case '99':
					case 'system':
						return 'system';
				}

				console.error('Cannot parse message type', type);
				return null;
			},
			multimedia: function(type) {
				if (type && type.toLowerCase) {
					type = type.toLowerCase();
				}

				switch (type) {
					case 0:
					case '0':
					case 'audio':
						return 'audio';
					case 1:
					case '1':
					case 'video':
						return 'video';
					case 2:
					case '2':
					case 'unknown':
						return 'unknown';
				}

				console.error('Cannot parse multimedia type', type);
				return null;
			},
		};
	};

	angular.module('ChatApp.Services')
		.factory('TypeService', typeServiceFactory);

	typeServiceFactory.$inject = [];

}(window.angular));