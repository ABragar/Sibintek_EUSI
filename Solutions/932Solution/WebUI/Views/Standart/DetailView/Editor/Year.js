pbaAPI.registerEditor('Year', pbaAPI.Editor.extend({
    picker: null,

    init: function($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

        this.picker = this.wrap.getKendoDatePicker();
    },

    onAfterBind: function() {
        var date = kendo.parseDate(this.readProperty(), application.DATE_TIME_FORMATE);

        this.picker.value(date);
    },

    onScroll: function() {
        this.picker.close();
    }
}));

