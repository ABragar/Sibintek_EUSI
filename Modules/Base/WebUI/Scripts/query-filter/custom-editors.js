$(function () {
    $.fn.queryBuilder.define('custom-editors',
        function (options) {
            this.on('getRuleInput.filter',
                function (h, rule, name) {
                    var filter = rule.filter;
                    var sysType = filter.data.system_type;
                    filter.data.editor_id = name;                    
                    if (sysType === 'Date') {
                        h.value = '<input id="' + name + '"' +
                                    '/>';
                    } if (sysType === 'DateTime') {
                        h.value = '<input id="' + name + '"' +
                                    '/>';
                    } else if (sysType === 'Boolean') {
                        h.value = '<input type="checkbox" id="' + name + '"' +
                                    '/>';
                    } else if (sysType === 'Integer') {
                        h.value = '<input type="number" value="0" id="' + name + '"' +
                                    '/>';
                    }
                    else if (sysType === 'Number') {
                        h.value = '<input type="number" min="0" value="0" id="' + name + '"' +
                                    '/>';
                    }
                    else if (sysType === 'Double') {
                        h.value = '<input type="number" step="0.01" value="0.00" id="' + name + '"' +
                                    '/>';
                    } else if (sysType === 'Enum') {
                        h.value = '<input type="number" id="' + name + '"' +
                                    '/>';
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
                        $('#' + inputName).kendoDatePicker({
                            change: function () {
                                var date = this.value();
                                //NOTE: из-за различия форматов в C# и JS
                                var month = date.getMonth() + 1;
                                rule.value = 'DateTime(' + date.getFullYear() + ',' + month + ',' + date.getDate() + ')';
                            }
                        });
                    } else if (sysType === 'Boolean') {
                        filter.default_value = false;
                        $('#' + inputName).change(function () {
                            rule.value = this.checked;
                        });
                    } else if (sysType === 'Integer') {
                        filter.default_value = 0;
                        $('#' + inputName).kendoNumericTextBox({
                            decimals: 0,
                            format: '#',
                            change: function () {
                                rule.value = this.value();
                            }
                        });
                    } else if (sysType === 'Number') {
                        filter.default_value = 0;
                        $('#' + inputName).kendoNumericTextBox({
                            min: 0,
                            decimals: 0,
                            format: '#',
                            change: function () {
                                rule.value = this.value();
                            }
                        });
                    } else if (sysType === 'Double') {
                        filter.default_value = 0;
                        $('#' + inputName).kendoNumericTextBox({
                            culture: "en-US",
                            format: "#.00",
                            step: 0.01,
                            value: 0,
                            change: function () {
                                rule.value = this.value();
                            }
                        });
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
                                    change: function() {
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
                                   rule.value = res.map(function(item) {
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