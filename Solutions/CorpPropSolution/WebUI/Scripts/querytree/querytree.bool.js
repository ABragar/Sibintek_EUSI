window.querytreeFilters = window.querytreeFilters || {};

(function ($, pbaApi, filters) {
    'use strict';

    $.extend(filters,
        {
            Boolean: function (item) {
                var selector = '.rule-value-container div.editor';

                return {
                    input: function (rule, name) {
                        return '<div class="editor">' +
                            '<input type= "checkbox" id="' +
                            name +
                            '" class="k-checkbox" name="' +
                            name +
                            '">' +
                            '<label class="k-checkbox-label" for="' +
                            name +
                            '"></label>' +
                            '</div>';
                    },
                    plugin: 'queryTreeBoolean',
                    plugin_config: {
                        SystemData: item.SystemData
                    },
                    valueSetter: function (rule, value) {
                        rule.$el.find(selector).queryTreeBoolean('setValue', value);
                    },
                    valueGetter: function (rule) {
                        return rule.$el.find(selector).queryTreeBoolean('getValue');
                    },
                    validation: {
                        callback: function (value, rule) {
                            return true;
                        }
                    }
                }
            }
        });

    $.widget('pba.queryTreeBoolean',
        {
            options: {
                SystemData: null
            },
            _create: function () {

            },
            getValue: function () {
                return this.element.find('input').prop('checked');
            },
            setValue: function (value) {
                this.element.find('input').prop('checked', value);
            },
            _destroy: function () {

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
        });

})(window.jQuery, window.pbaAPI, window.querytreeFilters);