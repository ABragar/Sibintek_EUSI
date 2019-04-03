window.querytreeFilters = window.querytreeFilters || {};

(function ($, pbaApi, filters) {
    'use strict';

    filters.AggregateFuncs = function(item) {

        var dd = pbaApi.guid();
        var selector = '.rule-value-container>div';

        return {
            input: function(rule, name) {
                return '<div>\
                            <select class="form-control" name="property">\
                            </select>\
                            <select class="form-control" name="operator">\
                                <option value="greater">больше</option>\
                                <option value="less">меньше</option>\
                            </select>\
                            <input class="form-control" type="text" name="value">\
                        </div>';
            },
            plugin: 'queryTreeAggregateFuncs',
            plugin_config: {
                Mnemonic: item.SystemData
            },
            valueSetter: function(rule, value) {
                rule.$el.find(selector).queryTreeAggregateFuncs('setValue', value);
            },
            valueGetter: function(rule) {
                console.log(dd);
                return rule.$el.find(selector).queryTreeAggregateFuncs('getValue');
            },
            validation: {
                callback: function(value, rule) {
                    return true;
                }
            }
        };
    };

    $.extend(window,
        {
            querytreeFilters: filters
        });

    var getAggregatableProperties = function (mnemonic) {

        return pbaApi.proxyclient.querytree.getAggregatableProperties({ mnemonic: mnemonic })
            .then(function (e) {
                return e;
            });
    };

    var getValue = function(element) {
        return {
            label: element.find("[name=property]").text(),
            id: element.find("[name=property]").val(),
            operator: element.find("[name=operator]").val(),
            value: element.find("[name=value]").val()
        };
    };

    $.widget('pba.queryTreeAggregateFuncs',
        {
            options: {
                Mnemonic: null,
                IsRoot: false
            },
            _create: function () {

                this._value = null;

                this._id = pbaApi.guid();

                var dropdown = this.element.find('[name="property"]');

                getAggregatableProperties(this.options.Mnemonic).done(function(props) {
                    props.forEach(function(prop) {
                        dropdown.append("<option value='" + prop.Id + "'>" + prop.Label + "</option>");
                    });
                });
            },

            refresh: function () {
               
            },
            edit: function () {
     
            },
            getValue: function () {
                return getValue(this.element);
            },
            setValue: function (value) {
                this.element.find("[name=property]").val(value.id);
                this.element.find("[name=operator]").val(value.operator);
                this.element.find("[name=value]").val(value.value);
                this.refresh();
            },
            _destroy: function () {
            }
        });


    $.extend(pbaApi,
        {});

}(window.jQuery, window.pbaAPI, window.querytreeFilters));