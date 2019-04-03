(function () {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var doc = document;
    var docEl = doc.documentElement;
    var pbaAPI = window.pbaAPI;

    // ##############
    // MODULE EXPORTS
    // ##############

    var layout = window.layout = {
        _element: $(),
        get element() {
            return layout._element.length ? layout._element : (layout._element = $('#layout'));
        },
        get isFullscreen() {
            return !!doc.fullscreenElement || !!doc.mozFullScreenElement || !!doc.webkitFullscreenElement;
        },
        blockUi: function (enable) {
            layout.element.toggleClass('layout--blocked', enable === true);
        },
        toggleFullscreen: function () {
            if (!layout.isFullscreen) {
                if (docEl.requestFullscreen) {
                    docEl.requestFullscreen();
                } else if (docEl.mozRequestFullScreen) {
                    docEl.mozRequestFullScreen();
                } else if (docEl.webkitRequestFullscreen) {
                    docEl.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                }
            } else {
                if (doc.cancelFullScreen) {
                    doc.cancelFullScreen();
                } else if (doc.mozCancelFullScreen) {
                    doc.mozCancelFullScreen();
                } else if (doc.webkitCancelFullScreen) {
                    doc.webkitCancelFullScreen();
                }
            }
        },
        setLayoutClass: function () {         
            var $element = layout.element;
            if ($element.find('.content__html--fit-in').length) {
                $element.addClass('layout--fit-in-screen');
            } else {
                $element.removeClass('layout--fit-in-screen');
            }
        }
    };

    $.extend(layout, pbaAPI.emitterMixin());
}());
