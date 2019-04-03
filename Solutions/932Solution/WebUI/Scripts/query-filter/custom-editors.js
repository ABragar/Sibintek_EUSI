$(function () {
    var attachTemplateDays = function (e) {
        var t = kendo.template("#=text#");
        var dp = this;

        window.CalcDate = function (delta) {
            var dt = new Date();
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
                        "<div style=\"\"> <div style=\"margin: 0 10PX;display: flex; justify-content: space-between;\"> <a href=\"#\" class=\"k-link k-nav-today tooltipstered\" onclick=\"getYesterday()\" >Вчера</a><a class=\"k-link k-nav-today tooltipstered\" onclick=\"getToday()\">Сегодня</a><a class=\"k-link k-nav-today tooltipstered\" onclick=\"getTomorrow()\" >Завтра</a></div></div>"
                }));
        });
    };

    $.fn.queryBuilder.define('custom-editors',
        function (options) {
            this.on('getRuleInput.filter',
                function (h, rule, name) {
                    var filter = rule.filter;
                    var sysType = filter.data.system_type;
                    filter.data.editor_id = name;                    
                    if (sysType === 'Date') {
                        h.value = '<input id="' + name + '"' + '/>';
                    } if (sysType === 'DateTime') {
                        h.value = '<input id="' + name + '"' + '/>';
                    } else if (sysType === 'Boolean') {
                        h.value = '<input type="checkbox" id="' + name + '"' +
                            '/>';
                    } else if (sysType === 'Integer') {
                        h.value = '<input value="0" id="' + name + '"' + '/>';
                    }
                    else if (sysType === 'Number') {
                        h.value = '<input min="0" value="0" id="' + name + '"' + '/>';
                    }
                    else if (sysType === 'Double') {
                        h.value = '<input value="0.00" id="' + name + '"' + '/>';
                    } else if (sysType === 'Enum') {
                        h.value = '<input type="number" id="' + name + '"' + '/>';
                    } else if (sysType === 'BaseObjectOne') {
                        h.value = '<div class="base-object-one-wrapper" id="' + name + '"> ' +
                                        '<span class="base-object-one-value"> </span>' +
                                        '<a href="#" class="k-button">...</a> ' +
                                  '</div>';
                    } else if (sysType === 'MultilineText' || sysType === 'String') {
                        h.value = '<input class="k-textbox"  id="' + name + '"' +
                                    '/>';
                    } else if (sysType === 'EasyCollection') {
                        h.value = '<div class="base-object-one-wrapper" id="' + name + '"> ' +
                                        '<span class="base-object-one-value"> </span>' +
                                        '<a href="#" class="k-button">...</a> ' +
                                  '</div>';
                    }
                });

            this.on('afterUpdateRuleOperator.filter',
                function (rule, operator) {
                    // force to re-create input for all operators
                    rule.builder.createRuleInput(operator);
                });

            this.on('afterCreateRuleInput.queryBuilder',
                function (e, rule) {
                    var filter = rule.filter;
                    var inputName = filter.data.editor_id;
                    var sysType = filter.data.system_type;
                    if (sysType === 'MultilineText' || sysType === 'String') {
                        $('#' + inputName).change(function () {
                            rule.value = this.value;
                        });
                    } else if (sysType === 'Date') {
                        if (rule.operator.type === 'in') {
                            $('#' + inputName).change(function () {
                                rule.value = this.value;
                            });
                        }
                        // for two inputs
                        else if (rule.operator.nb_inputs === 2) {
                            var firstInputName = inputName.substr(0, inputName.length - 1);
                            firstInputName += inputName.endsWith('1') ? '0' : '1';

                            $('#' + firstInputName).kendoDatePicker({
                                change: function () {
                                    var date = this.value();
                                    rule.value[0] = 'DateTime(' + date.getFullYear() + ',' + (date.getMonth() + 1) + ',' + date.getDate() + ')';
                                    var toVal = $('#' + inputName).data('kendoDatePicker').value();
                                    if (toVal) {
                                        rule.value[1] = 'DateTime(' + toVal.getFullYear() + ',' + (toVal.getMonth() + 1) + ',' + toVal.getDate() + ')';
                                        // force calling setter logic
                                        rule.value = rule.value;
                                    }
                                },
                                open: attachTemplateDays
                            });

                            // second input
                            $('#' + inputName).kendoDatePicker({
                                change: function () {
                                    var date = this.value();
                                    rule.value[1] = 'DateTime(' + date.getFullYear() + ',' + (date.getMonth() + 1) + ',' + date.getDate() + ')';
                                    var fromVal = $('#' + firstInputName).data('kendoDatePicker').value();
                                    if (fromVal) {
                                        rule.value[0] = 'DateTime(' + fromVal.getFullYear() + ',' + (fromVal.getMonth() + 1) + ',' + fromVal.getDate() + ')';;
                                        // force calling setter logic
                                        rule.value = rule.value;
                                    }
                                },
                                open: attachTemplateDays
                            });
                        }
                        else {
                            $('#' + inputName).kendoDatePicker({
                                change: function () {
                                    var date = this.value();
                                    //NOTE: из-за различия форматов в C# и JS
                                    var month = date.getMonth() + 1;
                                    rule.value = 'DateTime(' + date.getFullYear() + ',' + month + ',' + date.getDate() + ')';
                                },
                                open: attachTemplateDays
                            });
                        }
                    } else if (sysType === 'Boolean') {
                        filter.default_value = false;
                        $('#' + inputName).change(function () {
                            rule.value = this.checked;
                        });
                    } else if (sysType === 'Integer') {
                        filter.default_value = 0;
                        if (rule.operator.type === 'in') {
                            $('#' + inputName).change(function () {
                                rule.value = this.value.split(',');
                            });
                        } else {
                            $('#' + inputName).kendoNumericTextBox({
                                decimals: 0,
                                format: '#',
                                change: function () {
                                    rule.value = this.value();
                                }
                            });
                        }
                    } else if (sysType === 'Number') {
                        filter.default_value = 0;
                        if (rule.operator.type === 'in') {
                            $('#' + inputName).change(function () {
                                rule.value = this.value.split(',');
                            });
                        } else {
                            $('#' + inputName).kendoNumericTextBox({
                                min: 0,
                                decimals: 0,
                                format: '#',
                                change: function () {
                                    rule.value = this.value();
                                }
                            });
                        }
                    } else if (sysType === 'Double') {
                        filter.default_value = 0;
                        if (rule.operator.type === 'in') {
                            $('#' + inputName).change(function () {
                                rule.value = this.value.split(',');
                            });
                        } else {
                            $('#' + inputName).kendoNumericTextBox({
                                culture: "en-US",
                                format: "#.00",
                                step: 0.01,
                                value: 0,
                                change: function () {
                                    rule.value = this.value();
                                }
                            });
                        }
                    } else if (sysType === 'Enum') {
                        var enumType = filter.data.additional_info;
                        //copy logic from Enum editor

                        application.UiEnums.get(enumType,
                            function (res) {
                                var values = res.Values;
                                var arrValues = [];
                                for (var key in values) {
                                    if (values.hasOwnProperty(key)) {
                                        arrValues.push(values[key]);
                                    }
                                }
                                var dataSource = new kendo.data.DataSource({ data: arrValues });
                                $('#' + inputName).kendoDropDownList({
                                    dataTextField: "Title",
                                    dataValueField: "Value",
                                    dataSource: dataSource,
                                    change: function () {
                                        rule.value = this.value();
                                    },
                                    optionLabel: 'Выберите значение...',
                                    valueTemplate:
                                        "<span style=\"color:#=data.Color#; max-width:500px;\" class='#=data.Icon#'></span><span data-val='#=data.Value#'>&nbsp&nbsp&nbsp#=data.Title#</span>",
                                    template:
                                        "<span style=\"color:#=data.Color#; max-width:500px;\" class='#=data.Icon#'></span><span data-val='#=data.Value#'>&nbsp&nbsp&nbsp#=data.Title#</span>"
                                });
                            });
                    } else if (sysType === 'BaseObjectOne') {
                        var $btn = $('#' + inputName + '> .k-button');
                        $btn.data("mnemonic", filter.data.additional_info);

                        $btn.click(function () {
                            var mnemonic = $(this).data('mnemonic');

                            pbaAPI.openModalDialog(mnemonic,
                               function (res) {
                                   rule.value = res[0].ID;
                               },
                               {
                                   title: "ВЫБОР",
                                   multiselect: false
                               });
                        });
                    } else if (sysType === 'EasyCollection') {
                        var $btn = $('#' + inputName + '> .k-button');
                        $btn.data("mnemonic", filter.data.additional_info);

                        $btn.click(function () {
                            var mnemonic = $(this).data('mnemonic');

                            pbaAPI.openModalDialog(mnemonic,
                               function (res) {
                                   rule.value = res.map(function (item) {
                                       return item.ID;
                                   });
                               },
                               {
                                   title: "ВЫБОР",
                                   multiselect: true
                               });
                        });
                    }

                });
        },
        {

        });
});