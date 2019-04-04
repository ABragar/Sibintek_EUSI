window.querytreeFilters = window.querytreeFilters || {};

(function ($, pbaApi, filters) {
    'use strict';
    $.extend(filters,
        {
            Enum: function (item) {
                var selector = '.rule-value-container input';
                return {
                    input: function (rule, name) {
                        return '<input class="dropdown--enum" style="min-width: 30rem;" name="' + name + '"/>';
                    },
                    plugin: 'queryTreeEnum',
                    plugin_config: {
                        SystemData: item.SystemData
                    },
                    valueSetter: function (rule, value) {
                        rule.$el.find(selector).queryTreeEnum('setValue', value);
                    },
                    valueGetter: function (rule) {
                        return rule.$el.find(selector).queryTreeEnum('getValue');
                    },
                    validation: {
                        callback: function (value, rule) {
                            var emptyError = 'Не заполненно';
                            if (!value) {
                                //pbaAPI.errorMsg('Невозможно выполнить запрос. Проверьте правильность');
                                return emptyError;
                            }
                            return true;
                        }
                    }
                }
            }
        });

    $.widget('pba.queryTreeEnum',
        {
            options: {
                SystemData: null
            },
            _create: function () {
                var that = this;

                that._value = null;
                that._dropDown = null;

                application.UiEnums.get(that.options.SystemData,
                    function (res) {
                        var values = res.Values;

                        var arrValues = [];

                        for (var key in values) {
                            if (values.hasOwnProperty(key)) {
                                arrValues.push(values[key]);
                            }
                        }

                        that.element.kendoDropDownList({
                            dataTextField: 'Title',
                            dataValueField: 'Value',
                            valueTemplate: '<span style="color:#=data.Color#" class="#=data.Icon#"></span><span data-val="#=data.Value#">&nbsp&nbsp&nbsp#=data.Title#</span>',
                            template: '<span style="color:#=data.Color#;" class="#=data.Icon#"></span><span data-val="#=data.Value#">&nbsp&nbsp&nbsp#=data.Title#</span>',
                            dataSource: arrValues,
                            select: function (e) {
                                that._value = e.dataItem.Value;
                                that.element.trigger('change');
                            }
                        });

                        that._dropDown = that.element.getKendoDropDownList();
                    });

            },
            getValue: function () {
                return this._value;
            },
            setValue: function (value) {
                var that = this;
                that._value = value;

                var intervalId = setInterval(function () {
                    if (that._dropDown) {
                        that._dropDown.value(that._value);

                        clearInterval(intervalId);
                    }
                }, 100);
            },
            _destroy: function () {
                this._dropDown.destroy();
            }
        });

})(window.jQuery, window.pbaAPI, window.querytreeFilters);