pbaAPI.registerEditor('Year', pbaAPI.Editor.extend({
    picker: null,

    init: function($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);
    },
    onAfterBind: function () {
        if (!this.picker)
            this.picker = this.wrap.getKendoDatePicker();
    },
    onScroll: function () {
        if (this.picker)
            this.picker.close();
    },
    onSave: function () {
        this.pbaForm.setPr(this.propertyName, kendo.toString(this.picker.value(), application.DATE_TIME_FORMATE));
    }
}));

