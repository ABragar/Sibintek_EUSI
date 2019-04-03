pbaAPI.registerEditor('Period', pbaAPI.Editor.extend({
    start: null,
    end: null,
    days: null,
    _setDays: function () {
        if (!this.end.value()) {
            this.days.value(0);
            return;
        }
        var timeDiff = Math.abs(this.end.value() - this.start.value());
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        this.days.value(diffDays);
    },
    onBeforeBind: function() {
        var editor = this;
        var minDate = new Date(1800, 0, 1);
        var maxDate = new Date(2099, 11, 31);
        var start = editor.start;
        var end = editor.end;
        if (start != null) {
            start.min(minDate);
            start.max(maxDate);
        }
        if (end != null) {
            end.min(minDate);
            end.max(maxDate);
        }
    },
    onAfterBind: function() {
        var editor = this;
        var start = editor.start;
        var end = editor.end;

        if (start === null) {
            start = editor.start = editor.wrap.find("#start").getKendoDatePicker();
            start.bind("change",
                function() {
                    editor.startChange();
                });
        }

        if (end === null) {
            end = editor.end = editor.wrap.find("#end").getKendoDatePicker();
            end.bind("change",
                function() {
                    editor.endChange();
                });
        }

        if (editor.days === null) {
            editor.days = editor.wrap.find("#days").getKendoNumericTextBox();
            editor.days.bind("change",
                function() {
                    editor.daysChange();
                });
        }

        this._setDays();

        start.max(end.value());
        end.min(start.value());
    },
    startChange: function() {
        var start = this.start;
        var end = this.end;

        var startDate = start.value(),
            endDate = end.value();

        if (startDate) {
            startDate = new Date(startDate);
            startDate.setDate(startDate.getDate());
            end.min(startDate);
        } else if (endDate) {
            start.max(new Date(endDate));
        } else {
            endDate = new Date();
            start.max(endDate);
            end.min(endDate);
        }

        this._setDays();
    },
    endChange: function() {
        var start = this.start;
        var end = this.end;

        var endDate = end.value(),
            startDate = start.value();

        if (endDate) {
            endDate = new Date(endDate);
            endDate.setDate(endDate.getDate());
            start.max(endDate);
        } else if (startDate) {
            end.min(new Date(startDate));
        } else {
            endDate = new Date();
            start.max(endDate);
            end.min(endDate);
        }

        this._setDays();
    },
    daysChange: function() {
        var start = this.start;
        var startDate = start.value();
        var days = this.days.value();
        var end = this.end;

        function addDays(date, val) {
            var result = new Date(date);
            result.setDate(result.getDate() + val);
            return result;
        }

        if (startDate) {
            end.value(addDays(startDate, days));
        } else {
            startDate = new Date();
            start.value(startDate);
            end.value(addDays(startDate, days));
        }

        end.trigger("change");
    },
    onSave: function () {
        var self = this;
        var propStartValue = self.start.value();
        var strStartValue = kendo.toString(propStartValue, "dd.MM.yyyy HH:mm:ss");
        var propEndValue = self.end.value();
        var strEndValue = kendo.toString(propEndValue, "dd.MM.yyyy HH:mm:ss");
        self.pbaForm.setPr("Start", strStartValue);
        self.pbaForm.setPr("End", strEndValue);
        self.pbaForm.setPr(self.propertyName + ".Start", strStartValue);
        self.pbaForm.setPr(self.propertyName + ".End", strEndValue);
    }
}));