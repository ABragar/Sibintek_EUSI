/* global application, kendo, $ */

(function() {
	'use strict';

    // ############
    // DEPENDENCIES
    // ############

	var application = window.application;
    var layout = window.layout;

    var windowId = 0;
    var windows = [];

    var kendoWindow = $.fn.kendoWindow;
    $.fn.kendoWindow = function(options) {
        var wnd = kendoWindow.call(this, options).getKendoWindow();

        windows[windowId++] = wnd;

        return this;
    };

    // CLOSE SIDEBAR ON OPEN MODAL WINDOW
    //var open = kendo.ui.Window.prototype.open;
	//kendo.ui.Window.prototype.open = function windowOpenExtension() {
	//	if (this.options.modal) {
	//		layout.sidebar.close();
	//	}

	//	return open.call(this);
	//};

    // ON ROUTE CHANGE - CLOSE ALL OPENED MODAL WINDOWS
	application.spa.adapter.onChange(function() {
	    windows.forEach(function(kendoWindow) {
            if (kendoWindow.options.visible && kendoWindow.options.modal) {
                kendoWindow.close();
            }
        });
    });

}());
