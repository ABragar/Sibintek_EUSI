(function(angular, pbaAPI, userStateService) {
	'use strict';

	var dialogController = function($scope, $sce, DialogService, RouteService, UserService) {
        var state = RouteService.getState();

		// ##########
		// SCOPE INIT
		// ##########

		$scope.dialogs = DialogService.dialogs;
		$scope.isLoaded = false;
		$scope.isEmpty = true;

		// ############
		// VIEW HELPERS
		// ############

		this.getImageHtml = function(dialog) {
		    var isPrivate = dialog.type === 'private';

			var id = isPrivate ? dialog.id : null;
			var imageId = isPrivate ? dialog.imageId : null;
			var renderStatus = isPrivate;

			var html = UserService.getLogo({
				ID: id,
				Image: { FileID: imageId },
                _noImage: !isPrivate
			}, true, 60, 60, renderStatus, 'med', true);

			return $sce.trustAsHtml(html);
		};

		this.createPrivateChat = function() {

			// COMPOSE SYSTEM FILTER
			var filter = $scope.dialogs
				// take only private dialogs
				.filter(function(dialog) { return dialog.type === 'private'; })
				// get a sysfilter format string from each dialog id
				.map(function(dialog) { return 'it.ID!=' + dialog.id; })
				// add the current user id shorthand to a result array
				.concat(['it.ID!=@CurrentUserID'])
				// and serialize result array to a one sysfilter-formatted string
				.join(' and ');

			pbaAPI.openModalDialog('User', function(user) {
				if (Array.isArray(user)) {
					user = user[0];
				}

				if (!user) return;

				RouteService.goTo.newDialog('private', user.ID);
				$scope.$apply();
			}, {
				title: 'Создание приват-чата (выбор пользователя)',
				multiselect: false,
				filter: filter,
			});
		};

		this.createConference = function() {
			pbaAPI.openModalDialog('User', function(selectedUsers) {
				if (!selectedUsers.length) return;

				var userIds = pbaAPI.extract(selectedUsers, 'ID');

				DialogService.createConference(userIds);
			}, {
				title: 'Создание конференции (выбор пользователей)',
				filter: 'it.ID!=@CurrentUserID',
				multiselect: true,
			});
		};

		// ####################
		// ON CONTROLLER CREATE
		// ####################

        // ???
        // RouteService.onRouteChange(function(page, state) {
        //     if (page === 'dialogs') {
        //         DialogService.cleanTemp();
        //     }
        // });
		

		DialogService.onUpdate(function() {
			$scope.dialogs = DialogService.dialogs;
			$scope.isLoaded = DialogService.isLoaded;
			$scope.isEmpty = DialogService.dialogs.length === 0;
            
			$scope.$applyAsync();
		});

        // #####################
		// ON CONTROLLER DESTROY
		// #####################
		
		$scope.$on('$destroy', function() {
			DialogService.releaseControllers();
		});
	};

	angular.module('ChatApp.Controllers')
		.controller('DialogController', dialogController);

	dialogController.$inject = ['$scope', '$sce', 'DialogService', 'RouteService', 'UserService'];

}(window.angular, window.pbaAPI, window.globalapp.userStateService));