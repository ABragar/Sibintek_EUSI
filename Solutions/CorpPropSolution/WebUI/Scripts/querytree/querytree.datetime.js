window.querytreeFilters = window.querytreeFilters || {};

function getTodayWithTimezone(value) {
    var currentTimezone = new Date().getTimezoneOffset() / 60 * (-1);

    return !value
        ? new Date(new Date().setHours(currentTimezone, 0, 0, 0))
        : new Date(new Date(value).setHours(currentTimezone, 0, 0, 0));
}   

var attachTemplateDays = function (e) {
    var t = kendo.template("#=text#");
    var dp = this;

    window.CalcDate = function (delta) {
        var dt = getTodayWithTimezone();
        dt.setDate(dt.getDate() + delta);
        dp.value(dt);
        dp.trigger("change");
        dp.close();
    };

    window.getYesterday = function () {
        CalcDate(-1);
    }
    window.getTomorrow = function () {
        CalcDate(1);
    }
    window.getToday = function () {
        CalcDate(0);
    }
    setTimeout(function () {
        dp.dateView.popup.wrapper.find(".k-footer")
            .html(t({
                text:
                    "<div style=\"\"> <div style=\"margin: 0 15PX;display: flex; justify-content: space-between;\"> <a href=\"#\" class=\"k-link k-nav-today tooltipstered\" onclick=\"getYesterday()\" >Вчера</a><a class=\"k-link k-nav-today tooltipstered\" onclick=\"getToday()\">Сегодня</a><a class=\"k-link k-nav-today tooltipstered\" onclick=\"getTomorrow()\" >Завтра</a></div></div>"
            }));
    });
};

(function ($, pbaApi, filters) {
    'use strict';
    $.extend(filters,
        {
            DateTime: function (item) {

                var selector = '.rule-value-container input';
                return {
                    input: function (rule, name) {
                        return '<input id="datetimepicker" name="' + name + '"/>';
                    },
                    plugin: 'queryTreeDateTime',
                    //selectedOperator: rule? rule.operator.type: '',
                    plugin_config: {
                        SystemData: item.SystemData
                    },
                    valueSetter: function (rule, value) {
                        //rule.$el.find(selector).queryTreeDateTime('setValue', value);
                        if (rule.operator.nb_inputs == 1 &&
                            rule.operator.type !== 'in' && rule.operator.type !== 'not_in') {
                            value = [value]
                        };
                        rule.$el.find('.rule-value-container input').each(function (i) {
                            var val = rule.operator.type !== 'in' && rule.operator.type !== 'not_in' ? value[i] : value;
                            $(this).queryTreeDateTime('setValue', val);
                        });
                    },
                    valueGetter: function (rule) {
                        var value = [];
                        rule.$el.find(selector).each(function () {
                            value.push($(this).queryTreeDateTime('getValue'));
                        });
                        return rule.operator.nb_inputs == 1 && rule.operator.type !== 'in' && rule.operator.type !== 'not_in' ? value[0] : value;
                        //return rule.$el.find(selector).queryTreeDateTime('getValue');
                    },
                    validation: {
                        //allow_empty_value: false,
                        callback: function (value, rule) {
                            var emptyError = 'Не заполненно';
                            if (!value) {
                                //pbaAPI.errorMsg('Невозможно выполнить запрос. Проверьте правильность');
                                return emptyError;
                            }
                            var valToValidate = rule.operator.nb_inputs == 1 ? value[0] : value;
                            $(valToValidate).each(function (i, val) {
                                if (!val || val === '') {
                                    //pbaAPI.errorMsg('Невозможно выполнить запрос. Проверьте правильность');
                                    return emptyError;
                                }
                            });
                            return true;
                        }
                    }
                }
            }
        });

    $.widget('pba.queryTreeDateTime',
        {
            options: {
                SystemData: null
            },
            _create: function () {
                var that = this;

                that._picker = null;

                that.options.SystemData = that.options.SystemData || 'Date';

                switch (that.options.SystemData) {
                    case 'DateTime':
                        that.element.kendoDateTimePicker({
                            value: new Date(),
                            dateInput: true,
                            change: function (e) {
                                that.element.trigger('change');
                            }
                        });

                        that._picker = that.element.getKendoDateTimePicker();

                        break;
                    case 'Date':
                        that.element.kendoDatePicker({
                            value: getTodayWithTimezone(),
                            dateInput: true,
                            format: 'dd.MM.yyyy',
                            change: function (e) {
                                that.element.trigger('change');
                            },
                            selectable: "multiple",
                            open: attachTemplateDays
                        });

                        that._picker = that.element.getKendoDatePicker();

                        //that.element.kendoMultiCalendar({
                        //    values: [new Date()],
                        //    change: function (e) {
                        //        that.element.trigger('change');
                        //    }
                        //});

                        //that._picker = that.element.data('kendoMultiCalendar');

                        break;
                    case 'Month':
                        that.element.kendoDatePicker({
                            value: new Date(),
                            dateInput: true,
                            start: 'year',
                            depth: 'year',
                            format: 'MMMM yyyy',
                            change: function (e) {
                                that.element.trigger('change');
                            }
                        });

                        that._picker = that.element.getKendoDatePicker();

                        break;
                    case 'Year':
                        that.element.kendoDatePicker({
                            value: new Date(),
                            dateInput: true,
                            start: 'decade',
                            depth: 'decade',
                            format: 'yyyy',
                            change: function (e) {
                                that.element.trigger('change');
                            }
                        });

                        that._picker = that.element.getKendoDatePicker();

                        break;

                    default:
                        that.element.val('Не поддерживается');

                        break;
                }
            },
            getValue: function () {
                if (!this._picker) return null;
                return this._picker.value();
                //return this._picker.values();
            },
            setValue: function (value) {
                if (!this._picker) return;
                if (value) {
                    value = getTodayWithTimezone(value);
                }
                this._picker.value(value);
                //this._picker.values(value);
            },
            _destroy: function () {
                if (!this._picker) return;
                this._picker.destroy();
            }
        });

})(window.jQuery, window.pbaAPI, window.querytreeFilters);