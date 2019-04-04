pbaAPI.registerEditor('Enum',
    pbaAPI.Editor.extend({
        init: function ($wrap, propertyName) {
            'use strict';

            pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

            var $dropDown = this.wrap;

            application.UiEnums.get($dropDown.attr('data-enum-type'),
                function (res) {
                    var values = res.Values;

                    var arrValues = [];
                    
                    if (pbaAPI.boolParse($dropDown.attr('data-enum-isnullable'))) {
                        arrValues.push({
                            Value: null,
                            Title: 'Выберите значение...'
                        });
                    } 

                    for (var key in values) {
                        if (values.hasOwnProperty(key)) {
                            arrValues.push(values[key]);
                        }
                    }

                    $dropDown.kendoDropDownList({
                        dataTextField: 'Title',
                        dataValueField: 'Value',
                        valueTemplate: '<span style="color:#=data.Color#" class="#=data.Icon#"></span><span data-val="#=data.Value#">&nbsp&nbsp&nbsp#=data.Title#</span>',
                        template: '<span style="color:#=data.Color#;" class="#=data.Icon#"></span><span data-val="#=data.Value#">&nbsp&nbsp&nbsp#=data.Title#</span>',
                        dataSource: arrValues
                    });
                });
        }
    }));