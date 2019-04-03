pbaAPI.registerEditor('Duration', pbaAPI.Editor.extend({
    onAfterBind: function() {
        var daysBox = this._getTextBox('days');
        var hoursBox = this._getTextBox('hours');
        var minutesBox = this._getTextBox('minutes');

        var propValue = this.readProperty();

        var daysCount = Math.floor(propValue / 1440);
        var hoursCount = Math.floor(propValue % 1440 / 60);
        var minutesCount = propValue % 1440 - hoursCount * 60;

        daysBox.value(daysCount);
        hoursBox.value(hoursCount);
        minutesBox.value(minutesCount);
    },
    onSave: function() {
        var daysBox = this._getTextBox('days');
        var hoursBox = this._getTextBox('hours');
        var minutesBox = this._getTextBox('minutes');

        var propValue = daysBox.value() * 1440 +
                        hoursBox.value() * 60 +
                        minutesBox.value();

        this.writeProperty(propValue);
    },

    _getTextBox: function(name) {
        return $('#' + this.wrapId + '_' + name).getKendoNumericTextBox();
    }
}));
