window.querytreeFilters = window.querytreeFilters || {};

(function ($, pbaApi, filters) {
    'use strict';

    filters.InAndNotIn = function (item) {
        var selector = '.rule-value-container div.input-group';
        var oid;
        item.OperatorOid = item.OperatorOid || pbaApi.guid();
        return {
            input: function (rule, name, id) {
                return '<div class="input-group">\
                            <div>\
                                <input type="hidden" class="id-for-values" value="' + item.OperatorOid + '">\
                                <input class=" k-textbox" type="text" title placeholder="Значения..." readonly="readonly"/>\
                            </div>\
                            <span class="input-group-btn">\
                                <a href="#" title="Открыть..."><i class="fa fa-navicon open-dialog"></i></a>\
                            </span>\
                        </div>';
            },
            plugin: 'queryTreeInAndNotIn',
            valueSetter: function (rule, value) {
                rule.$el.find(selector).queryTreeInAndNotIn('setValue', value);
            },
            valueGetter: function (rule) {
                return rule.$el.find(selector).queryTreeInAndNotIn('getValue');
            },
            validation: {
                callback: function (value, rule) {
                    var emptyError = 'Не заполненно';
                    if (!value) {
                        return emptyError;
                    }
                    return true;
                }
            }
        }
    };

    $.widget('pba.queryTreeInAndNotIn',
        {
            options: {
                SystemData: null
            },
            _create: function () {
                var that = this;

                that._id = pbaApi.guid();

                that.element.find('.open-dialog').click(function () {
                    var filterOid = $('.root-querytree-builder').queryTree('getFilterOid');
                    pbaAPI.openModalDialog("OperatorInValues", null, {
                        title: 'Значения для фильтрации',
                        multiselect: false,
                        filter: 'it.MnemonicFilterOid=="' +
                            filterOid +
                            '" %26%26 it.IdForValue=="' +
                            that.getValue()+'"',
                        initProps: {
                            MnemonicFilterOid: filterOid,
                            IdForValue: that.getValue()
                        }
                    });
                });
            },

            getValue: function () {
                return this.element.find('.id-for-values').val();
            },

            setValue: function (value) {
                var that = this;
                that.element.find('.id-for-values').val(value);

            },
            _destroy: function () {

            }
        });

})(window.jQuery, window.pbaAPI, window.querytreeFilters);