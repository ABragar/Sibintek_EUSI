pbaAPI.registerEditor('DateTime', pbaAPI.Editor.extend({
    picker: null,

    init: function($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

        this.picker = this.wrap.getKendoDateTimePicker();
    },    
    onScroll: function () {
        if (this.picker)
            this.picker.close();
    }
}));
