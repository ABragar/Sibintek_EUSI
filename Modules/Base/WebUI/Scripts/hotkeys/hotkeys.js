var hotkeysObject = {
    special: {
        'backspace': 8,
        'tab': 9,
        'enter': 13,
        'pause': 19,
        'capslock': 20,
        'esc': 27,
        'space': 32,
        'pageup': 33,
        'pagedown': 34,
        'end': 35,
        'home': 36,
        'left': 37,
        'up': 38,
        'right': 39,
        'down': 40,
        'insert': 45,
        'delete': 46,
        'f1': 112,
        'f2': 113,
        'f3': 114,
        'f4': 115,
        'f5': 116,
        'f6': 117,
        'f7': 118,
        'f8': 119,
        'f9': 120,
        'f10': 121,
        'f11': 122,
        'f12': 123,
        '?': 191, // Question mark
        'minus': $.browser.opera ? [109, 45] : $.browser.mozilla ? 109 : [189, 109],
        'plus': $.browser.opera ? [61, 43] : $.browser.mozilla ? [61, 107] : [187, 107]
    },
    isExecuteHandler: function (keymask, event) {
        var items = keymask.split('+');
        var res = true;
        if (items && items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                switch (items[i]) {
                    case "ctrl":
                        if (!event.ctrlKey) {
                            res = false;
                        }
                        break;
                    case "alt":
                        if (!event.altKey) {
                            res = false;
                        }
                        break;
                    case "shift":
                        if (!event.shiftKey) {
                            res = false;
                        }
                        break;
                    default:
                        var code = hotkeysObject.special[items[i]] || items[i].toUpperCase().charCodeAt();
                        if (code !== event.keyCode) {
                            res = false;
                        }
                        break;
                }
            }
        }
        return res;
    },
    init: function (guid, array) {
        $("#" + guid).bind('keydown.' + guid, function (e) {
            if (array && array.length > 0) {
                for (var i = 0; i < array.length; i++) {
                    if (hotkeysObject.isExecuteHandler(array[i].keyMask, e)) {
                        array[i].handler();
                    }
                }
            }
        });
    },
    destroy: function (guid) {
        $("#" + guid).unbind('keydown.' + guid);
    }
};



