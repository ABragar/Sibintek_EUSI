window.querytreeFilters = window.querytreeFilters || {};

(function ($, pbaApi, filters) {
    'use strict';

    filters.UserId = function (item) {
        var selector = '.rule-value-container div.input-group';

        return {
            input: function (rule, name) {
                return '<div class="input-group">' +
                    '<div>' +
                    '<input id="base-object-value" name="' + name + '" class="k-textbox" type="text" style="display:none;"/>' +
                    '<input class="base-object-lookup k-textbox" type="text" title placeholder="Выберите значение..." readonly="readonly"/>' +
                    '</div>' +
                    '<span class="input-group-btn">' +
                    '<a id="open-dialog" href="#" title="Выбрать..."><i class="fa fa-navicon"></i></a>' +
                    '<a id="clear" href="#" title="Очистить"><i class="fa fa-close"></i></a>' +
                    '<a id="current_user" href="#" title="Текущий пользователь"><i class="fa fa-user-circle-o"></i></a>' +
                    '</span>' +
                    '</div>';
            },
            plugin: 'queryTreeUserId',
            plugin_config: {
                SystemData: item.SystemData
            },
            valueSetter: function (rule, value) {
                rule.$el.find(selector).queryTreeUserId('setValue', value);
            },
            valueGetter: function (rule) {
                return rule.$el.find(selector).queryTreeUserId('getValue');
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
    };

    $.widget('pba.queryTreeUserId',
        {
            options: {
                SystemData: null
            },
            _create: function () {
                var that = this;

                that._id = pbaApi.guid();

                that.element.find('#open-dialog').click(function () {
                    pbaAPI.openModalDialog(that.options.SystemData, function (res) {
                        that.element.find('#base-object-value').val(res[0].ID);
                        that._setLookupProperty(res[0]);
                        that.element.trigger('change');
                    }, {
                            title: 'Выбор...',
                            multiselect: false
                        });
                });

                that.element.find('#clear').click(function () {
                    that.element.find('.form-control input').val("");
                    that.element.find('.base-object-lookup').prop("title", "");
                    that.element.trigger('change');
                });

                that.element.find('#current_user').click(function () {
                    that.element.find('#base-object-value').val("current_user");

                    that._setLookupCurrentUser();

                    that.element.trigger('change');
                });


            },


            _setLookupCurrentUser: function () {
                this.element.find('.base-object-lookup').prop("title", "Текущий пользователь");
                this.element.find('.base-object-lookup').val("Текущий пользователь");
            },

            _setLookupProperty: function (obj) {
                var that = this;
                application.viewModelConfigs.get(this.options.SystemData).done(
                    function (config) {
                        var text = obj.ID + ": " + obj[config.LookupProperty.Text];
                        that.element.find('.base-object-lookup').prop("title", obj[config.LookupProperty.Text]);
                        that.element.find('.base-object-lookup').val(text);
                    });

            },

            getValue: function () {
                return this.element.find('#base-object-value').val();
            },

            setValue: function (value) {
                var that = this;

                if (value) {

                    if (value == "current_user") {
                        this._setLookupCurrentUser();

                    } else {

                        pbaAPI.proxyclient.crud.get({
                            mnemonic: this.options.SystemData,
                            id: value
                        }).done(function (result) {
                            if (result.error) {
                                pbaAPI.errorMsg(result.error);
                            } else {
                                that._setLookupProperty(result.model);
                            }
                        });
                    }
                }

                that.element.find('#base-object-value').val(value);

            },
            _destroy: function () {

            }
        });

})(window.jQuery, window.pbaAPI, window.querytreeFilters);