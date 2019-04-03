pbaAPI.registerEditor('Time', pbaAPI.Editor.extend({
    picker: null,

    init: function($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

        this.picker = this.wrap.getKendoTimePicker();
    },
    onScroll: function() {
        this.picker.close();
    }
}));
