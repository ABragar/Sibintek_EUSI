pbaAPI.registerEditor('Date', pbaAPI.Editor.extend({
    picker: null,

    init: function ($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);
    },
    onAfterBind: function () {
        if (!this.picker)
            this.picker = this.wrap.getKendoDatePicker();
    },
    onSave: function () {
        this.pbaForm.setPr(this.propertyName, kendo.toString(this.picker.value(), application.DATE_TIME_FORMATE));
    },
    onScroll: function () {
        if (this.picker)
            this.picker.close();
    }
}));
