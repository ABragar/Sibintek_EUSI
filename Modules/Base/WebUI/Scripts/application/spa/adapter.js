/* global $, pbaAPI */
/* jshint strict: false */
pbaAPI.ensurePath('application.spa.adapter');
(function() {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var w = window;
    var adapter = w.application.spa.adapter;

    // ###############
    // PRIVATE HELPERS
    // ###############

    // IE lte 10
    var isOldIE = function() {
        return w.navigator.userAgent.indexOf('MSIE ') > 0;
    };

    // #################
    // PRIVATE VARIABLES
    // #################

    var history = w.history;
    var hasSupport = !isOldIE() && history && history.pushState && 'onpopstate' in w;

    // location.pathname = /Dashboard
    // location.hash = #hash
    // location.host = localhost:1234
    // location.hostname = localhost
    // location.href = http://localhost:1234/Dashboard#hash
    var currentPath = w.location.pathname;
    var rRelativeUrl = /(?:https?:\/\/.*?(?=\/))?(.*)#?/; // removes protocol, host and hash in 1st group

    $.extend(adapter, pbaAPI.emitterMixin());

    $.extend(adapter, {
        hasSupport: function() {
            return hasSupport;
        },
        getRelativeUrl: function(url) {
            var reRelativeUrl = rRelativeUrl.exec(url);
            return reRelativeUrl && reRelativeUrl[1] || null;
        },
        pushState: function(url) {
            var relativePath = adapter.getRelativeUrl(url);

            if (currentPath !== relativePath) {
                history.pushState(null, null, url);
                adapter.emit("change", [currentPath, relativePath]);
                currentPath = relativePath;
            }
        },
        onChange: function(callback) {
            adapter.on("change", callback);
        }
    });

    if (hasSupport) {
        w.addEventListener('popstate', function() {
            if (w.location.pathname === currentPath) {
                return;
            }

            var fromPath = currentPath;
            currentPath = w.location.pathname;

            adapter.emit("change", [fromPath, currentPath]);
        }, false);
    }

}());
