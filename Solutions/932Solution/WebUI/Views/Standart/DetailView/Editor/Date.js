pbaAPI.registerEditor('Date', pbaAPI.Editor.extend({
    picker: null,

    init: function ($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

        this.picker = this.wrap.getKendoDatePicker();
    },
    onSave: function () {
        var self = this;
        var propValue = self.picker.value();
        var strValue = kendo.toString(propValue, "dd.MM.yyyy HH:mm:ss");
        self.pbaForm.setPr(self.propertyName, strValue);
    },
    onScroll: function () {
        if (this.picker)
            this.picker.close();
    }
}));
