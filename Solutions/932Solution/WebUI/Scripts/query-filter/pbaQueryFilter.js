$(function () {
    $.extend(pbaAPI,
    {
        queryFilter: {
            wnd: null,
            query: null,
            mnemonic: null,
            callbackFunc: function (e) { return e; },
            openQueryBuilder: function (mnemonic, query, callback) {
                if (callback === undefined)
                    throw new Error("Не задана функция обратного вызова");

                var self = this;
                self.callbackFunc = callback;
                self.query = query;
                self.mnemonic = mnemonic;
                $.when(self._getBuilderForm()).done(function () {
                    self._getFilters();
                }).done(function () {
                    self.wnd.element.removeClass("wnd-loading-content");
                });
            },
            //PRIVATE
            _getBuilderForm: function () {
                var self = this;
                if (self.wnd === null) {
                    var wndId = 'q_wnd_' + pbaAPI.guid();
                    $('body').append('<div id="' + wndId + '" class="view-model-window wnd-loading-content"/>');
                    var $wnd = $("#" + wndId);

                    self.wnd = $wnd.kendoWindow({
                        width: 900,
                        height: 600,
                        title: "Фильтр",
                        actions: ["Maximize", "Close"],
                        modal: true
                    }).data("kendoWindow");
                    self.wnd.center().open();
                    return $.ajax({
                        type: 'GET',
                        url: '/QueryBuilderFilter/BuilderForm',
                        success: function (res) {
                            $wnd.append(res);
                            var $btn = $wnd.find('#accept_btn_id');
                            $wnd.find('form').kendoValidator();
                            $btn.click(function () {
                                if (self.wnd != null) {
                                    var $wnd = $(this).closest('#query_builder_window_id');
                                    var $builder = $wnd.find('#qbuilder_id');
                                    var validator = $wnd.find('form').data('kendoValidator');
                                    if (validator.validate()) {
                                        var result = $builder.queryBuilder('getDynamiclinq');
                                        var query = result;
                                        if (result.query) {
                                            query = result.query;
                                        }
                                        $.ajax({
                                            type: "GET",
                                            url: '/QueryBuilderFilter/VerifyQuery',
                                            data: { mnemonic: self.mnemonic, query: query },
                                            success: function (res) {
                                                if (res.wasVerified) {
                                                    self.wnd.close();
                                                    self.callbackFunc(result);
                                                } else {
                                                    pbaAPI.errorMsg('Невозможно выполнить запрос. Проверьте правильность');
                                                }
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    });
                } else {
                    self.wnd.center().open();
                }
                var dfd = $.Deferred();
                dfd.resolve();
                return dfd.promise();
            },
            _getFilters: function () {
                var self = this;                
                return $.ajax({
                    type: 'GET',
                    url: '/QueryBuilderFilter/GetFilters',
                    data: { mnemonic: self.mnemonic },
                    success: function (res) {
                        var filters = res.Data;
                        if (filters.length > 0) {
                            for (var i = 0; i < filters.length; i++) {
                                filters[i].valueSetter = function (rule, value) {
                                    if (value) {
                                        var filter = rule.filter;
                                        var sysType = filter.data.system_type;
                                        if (sysType === "Date") {
                                            if (value instanceof Array) {
                                                var $editor = rule.$el.find('input');
                                                // for two inputs
                                                if (rule.operator.nb_inputs === 2) {
                                                    $.each(value, function (i, val) {
                                                        if (val) {
                                                            var dateParts = val.replace('DateTime(', '').replace(')', '').split(',');
                                                            //NOTE: из-за различия форматов в C# и JS
                                                            var month = dateParts[1] - 1;
                                                            var date = new Date(dateParts[0], month, dateParts[2]);
                                                            var datePicker = $('#' + $editor[i].id).data('kendoDatePicker');
                                                            datePicker.value(date);
                                                        }
                                                    });
                                                } else {
                                                    $editor.val(value);
                                                }
                                            } else {
                                                var dateParts = value.replace('DateTime(', '').replace(')', '').split(',');
                                                //NOTE: из-за различия форматов в C# и JS
                                                var month = dateParts[1] - 1;
                                                var date = new Date(dateParts[0], month, dateParts[2]);
                                                var datePicker = $('#' + filter.data.editor_id).data('kendoDatePicker');
                                                if (datePicker) {
                                                    datePicker.value(date);
                                                }
                                            }
                                        }
                                        else if (sysType === "Boolean") {
                                            var $editor = rule.$el.find('input');
                                            $editor.prop('checked', value);
                                        }
                                        else if (sysType === "Enum") {
                                            var dropDownList = $('#' + filter.data.editor_id).data('kendoDropDownList');
                                            var enumVal = value.toString();
                                            dropDownList.value(enumVal);
                                        }
                                        else if (sysType === "BaseObjectOne") {
                                            var $span = rule.$el.find('span.base-object-one-value');
                                            var mnemonic = rule.filter.data.additional_info;

                                            pbaAPI.proxyclient.viewConfig.getLookupPropertyValue({
                                                mnemonic: mnemonic,
                                                id: value
                                            }).done(function (result) {
                                                if (result.error) {
                                                    pbaAPI.errorMsg(result.error);
                                                } else {
                                                    $span.html(result.value);
                                                }
                                            });
                                        }
                                        else if (sysType === "EasyCollection") {
                                            var $span = rule.$el.find('span.base-object-one-value');
                                            var mnemonic = rule.filter.data.additional_info;

                                            pbaAPI.proxyclient.viewConfig.getLookupPropertyValue({
                                                mnemonic: mnemonic,
                                                ids: value
                                            }).done(function (result) {
                                                if (result.error) {
                                                    pbaAPI.errorMsg(result.error);
                                                } else {
                                                    $span.html(res.value.join(", "));
                                                }
                                            });
                                        }
                                        else {
                                            var $editor = rule.$el.find('input');
                                            $editor.val(value);
                                        }
                                    }
                                };
                            }

                            var options = {
                                plugins: [
                                    'custom-editors',
                                ],
                                allow_empty: true,
                                filters: filters
                            };
                            var $builder = self.wnd.element.find('#qbuilder_id');
                            $builder.queryBuilder(options);
                            if (self.query) {
                                var rules = $builder.queryBuilder('getRulesFromDynamiclinq', self.query);
                                $builder.queryBuilder('setRules', rules);
                            } else {
                                $builder.queryBuilder('setRules', []);
                            }
                            $builder.find('.btn').addClass('k-button');
                        }
                    }
                });
            }
        }
    });
});