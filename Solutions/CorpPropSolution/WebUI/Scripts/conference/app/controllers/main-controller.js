window.application.chat = window.application.chat || {};

(function(angular, kendo, pbaAPI, exports) {
	'use strict';

	var mainController = function($scope, $element, DialogService, RouteService) {

        var mainCtrl = this;
        var isMaximized = false;
        var isVisible = false;

        this.page = null;
        this.state = null;
		this.goTo = function(route) {
			var slice = Array.prototype.slice;

			if (RouteService.goTo[route]) {
				RouteService.goTo[route].apply(RouteService, slice.call(arguments, 1));
			}
		};
        
        this.isMaximized = function() { return isMaximized };
        this.isMinimized = function() { return !isMaximized }
        this.isVisible = function() { return isVisible };
        this.isHidden = function() { return !isVisible };
        this.setTitle = function(str) { setOptions({title:str}) };
        
		DialogService.load();
        
        RouteService.onRouteChange(function(page, state) {
            if (page === 'dialog-action') {
                return;
            }
            
            mainCtrl.page = page;
            mainCtrl.state = state;
            
            var activeDialog = page === 'messages'
                ? DialogService.getDialog(state.dialogType, state.dialogId)
                : null;
                
            var title = RouteService.getTitle(page, activeDialog);

            mainCtrl.setTitle(pbaAPI.truncate(title, 47));

            $scope.$applyAsync();
        });
        
        // #################
        // KENDO WINDOW WRAP
        // #################
        
        var CONTAINER = 'body';
	    var ANCHOR = '#frames-anchor';
        var MIN_WIDTH = 480;
        var MIN_HEIGHT = 480;
        var MAX_WIDTH = 1200;
        var MAX_HEIGHT = 700;
        var TITLEBAR = 96;

        var kendoWindow = $element.parent().kendoWindow({
            actions: ['maximize', 'close'],
            appendTo: CONTAINER,
            minHeight: MIN_HEIGHT,
            minWidth: MIN_WIDTH,
            maxHeight: MAX_HEIGHT,
            maxWidth: MAX_WIDTH,
            resizable: false,
            title: 'Диалоги',
            visible: false,
            
            close: onClose,
            open: onOpen,
            resize: onResize,
            dragend: onDragend
        }).data('kendoWindow');
        
        // ######################
        // KENDO WINDOW OVERRIDES
        // ######################
        
        kendoWindow.maximize = function() {
            kendoWindow.trigger('resize');
            setOptions({
                height: MAX_HEIGHT,
                width: MAX_WIDTH
            });
            kendoWindow.center();
        };
        
        kendoWindow.restore = function() {
            if (ignoreRestore) {
                return;
            }
            
            kendoWindow.trigger('resize');
            setOptions({
                height: MIN_HEIGHT,
                width: MIN_WIDTH
            });
            kendoWindow.center();
        };
        
        kendoWindow.center = function() {
            kendoWindow.constructor.prototype.center.call(kendoWindow);
            correctPosition();
        };

	    $(window).resize(pbaAPI.debounce(function() {
	        if (isVisible) {
	            correctPosition();
	        }
	    }));
        
        // ##############
        // EVENT HANDLERS
        // ##############
        
        function onClose() {
            isVisible = false;
            RouteService.goTo.dialogs();
        }
        
        function onOpen() {
            isVisible = true;
        }
        
        function onResize() {
            isMaximized = !isMaximized;
            $scope.$applyAsync();
        }
        
        function onDragend() {
            correctPosition();
        }
        
        // ###############
        // PRIVATE HELPERS
        // ###############
        
        var ignoreRestore = false;
        function setOptions(opts) {
            
            ignoreRestore = true;
            kendoWindow.setOptions(opts);
            ignoreRestore = false;
            
            correctIcon();
        }
        
        function correctIcon() {
            var current = isMaximized ? 'k-i-restore' : 'k-i-maximize';
            var actions = kendoWindow.element.parent().find('.k-window-action > span');
            var icon = actions.filter(function(i, el) {
                return el.className.indexOf('k-i-maximize') !== -1 || el.className.indexOf('k-i-restore') !== -1;
            });
            
            icon
                .removeClass(isMaximized ? 'k-i-maximize' : 'k-i-restore')
                .addClass(current);
        }
        
        function correctPosition() {
            var wrapper = $(ANCHOR);
            var _wrapperOffset = wrapper.offset();

            var wrapperX = _wrapperOffset.left;
            var wrapperY = _wrapperOffset.top;
            var wrapperW = wrapper.width();
            var wrapperH = wrapper.height();

            var _size = kendoWindow.getSize();

            var windowX = kendoWindow.options.position.left;
            var windowY = kendoWindow.options.position.top;
            var windowW = _size.width;
            var windowH = _size.height + TITLEBAR;

            if (windowX < wrapperX) {
                windowX = wrapperX;
            } else if (windowX + windowW > wrapperX + wrapperW) {
                windowX = wrapperX + wrapperW - windowW;
            }

            if (windowY < wrapperY) {
                windowY = wrapperY;
            } else if (windowY + windowH > wrapperY + wrapperH) {
                windowY = wrapperY + wrapperH - windowH;
            }
            
            setOptions({ position: { top:windowY, left:windowX } });
        }
        
        // ###############
        // GLOBAL CONTROLS
        // ###############
        
        exports.show = function() {
            if (!isVisible) {
                kendoWindow
                    .open()
                    .center();
            } 
        };
        
        exports.hide = function() {
            if (isVisible) {
                kendoWindow.close();
            }
        };
        
        exports.toggleVisibility = function() {
            exports[isVisible ? 'hide' : 'show']();
        };
        
        exports.isVisible = this.isVisible;
        exports.isHidden = this.isHidden;
        exports.isMinimized = this.isMinimized;
        exports.isMaximized = this.isMaximized;
        
        exports.openDialogs = function() {
            RouteService.goTo.dialogs();
            exports.show();
        };
        
        exports.openDialog = function(dialogType, dialogId) {
            RouteService.goTo.newDialog(dialogType, dialogId);
            exports.show();
        };
	};

	angular.module('ChatApp.Controllers', ['ChatApp.Services'])
		.controller('MainController', mainController);

	mainController.$inject = ['$scope', '$element', 'DialogService', 'RouteService'];

}(window.angular, window.kendo, window.pbaAPI, window.application.chat));
