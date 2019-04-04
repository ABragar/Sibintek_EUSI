pbaAPI.registerEditor('RemindPeriod',
    pbaAPI.Editor.extend({
        init: function($wrap, propertyName) {
            'use strict';

            pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

            var $dropDown = this.wrap.find('#' + this.wrapId + '_drop');

            application.UiEnums.get($dropDown.attr('data-enum-type'),
                function(res) {
                    var values = res.Values;

                    var arrValues = [];

                    for (var key in values) {
                        if (values.hasOwnProperty(key)) {
                            arrValues.push(values[key]);
                        }
                    }

                    $dropDown.kendoDropDownList({
                        dataTextField: 'Title',
                        dataValueField: 'Value',
                        dataSource: arrValues
                    });
                });
        }
    }));