(function(angular, app) {
	'use strict';

	var userServiceFactory = function() {

		// ###
		// API
		// ###

		return {
			// GETTERS
		    get current() { return { id: app.currentUser.id } },

		    // METHODS
		    getLogo: function(user, onlyImage, w, h, showOnlineState, statusSize, showStatusText) {
		        if (!user) {
		            return '';
		        }

		        w = w || 32;
		        h = h || 32;

		        var fileID = user.Image ? user.Image.FileID : null;

		        var imageUrl = pbaAPI.imageHelpers.getsrc(fileID, w, h, user._noImage ? 'NoImage' : 'NoPhoto');

		        var html = '<img data-user-image="' + user.ID + '" class="img-circle" src="' + imageUrl + '">';

		        if (!onlyImage) {
		            html += '<span>&nbsp;&nbsp;' + pbaAPI.htmlEncode(user.FullName || user.Title) + '</span>';
		        }

		        if (showOnlineState !== false) {
		            html = '<div class="user-image">' + html + pbaAPI.getUserState(user.ID, { size: statusSize || 'small', showDesc: showStatusText }) + '</div>';
		        } else {
		            html = '<div class="user-image">' + html + '</div>';
		        }

		        return html;
		    }
		};
	};

	angular.module('ChatApp.Services')
		.factory('UserService', userServiceFactory);

	userServiceFactory.$inject = [];

}(window.angular, window.application));