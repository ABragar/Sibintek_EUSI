/* globals $, kendo, pbaAPI, application */
(function () {
    'use strict';

    pbaAPI.gridClearFileldFilter = function (filter, colName) {
        for (var i = 0; i < filter.filters.length; i++) {
            if (filter.filters[i].field === colName) {
                filter.filters.splice(i, 1);
                i--;
            }
        }
    };

    pbaAPI.gridBaseObjectColumnFilterUi = function (params) {
        var insertToMultiselectProcess = function (myFilter, searchingValue, multiSel, op, isEmptyElem) {
            if (!myFilter.filters) {
                return;
            }

            var findFlag = false;

            pbaAPI.each(myFilter.filters, function (filter) {
                if (filter.filters) {
                    insertToMultiselectProcess(filter, searchingValue, multiSel, op, isEmptyElem);
                } else if (filter.field === searchingValue) {
                    findFlag = true;
                    return false; // loop exit;
                }
            });

            if (!findFlag) {
                return;
            }

            var addArray = pbaAPI.filter(myFilter.filters, function (filter) {
                return filter.value !== '\'\'';
            }).map(function (filter) {
                return filter.value;
            });

            if (!addArray.length) {
                return;
            }

            multiSel.value(addArray);

            if (myFilter.filters[0].operator === 'neq') {
                myFilter.filters.logic = 'and';
                op.value('neq');
            } else {
                myFilter.filters.logic = 'or';
                op.value('eq');
            }

            //проверка для флага "пустые"
            isEmptyElem.prop('checked', (myFilter.filters[0].value === 'null'));
        };

        var wrapGrid = params.grid;
        var colName = params.colName;
        var lookuppropery = params.lookuppropery;
        var $element = params.element;
        var tmpFilter = wrapGrid.getFilter();

        var placeholder = 'Введите значение...';

        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + colName + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');
        var $btnReset = $form.find('button[type="reset"]');

        $select.removeAttr('data-bind').removeAttr('data-role');
        $element.removeAttr('data-bind');

        $element
            .wrap($('<div>'))
            .wrap($('<div class="input-group">'))
            .wrap($('<div class="form-control">'))
            .parent().parent().parent()
            .append($('<input id="isNull" class="k-checkbox" type="checkbox"/><label class="k-checkbox-label" for="isNull">Пустые</label>').click(function () {
                var multiSelect = $element.data('kendoMultiSelect');

                if ($(this).is(':checked')) {
                    multiSelect.enable(false);
                    multiSelect.value('');
                } else {
                    multiSelect.enable(true);
                }
            }));

        var dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: application.url.GetStandart('FilterBaseObject_Read'),
                    data: function () {
                        var val = $element.data('kendoMultiSelect').input.val();

                        if (val === placeholder) val = '';

                        return {
                            startswith: val,
                            mnemonicCollection: wrapGrid.mnemonic,
                            property: colName
                        };
                    }
                }
            },
            serverSorting: true,
            serverFiltering: true
        });


        var itemTemplate = "<span title='#:data." + lookuppropery + " #'>#: data." + lookuppropery + " #</span>";

        $element.kendoMultiSelect({
            autoBind: false,
            placeholder: 'Введите значение...',
            dataTextField: lookuppropery,
            dataValueField: 'ID',
            dataSource: dataSource,
            filter: 'startswith',
            tagTemplate: itemTemplate,
            itemTemplate: itemTemplate
        });

        $select.kendoDropDownList({
            dataTextField: 'text',
            dataValueField: 'value',
            dataSource: [
                {
                    text: 'Равно',
                    value: 'eq'
                },
                {
                    text: 'Не равно',
                    value: 'neq'
                }
            ],
            index: 0
        });

        var multiSelect = $element.data('kendoMultiSelect');
        var operatorDropDown = $select.data('kendoDropDownList');

        if (tmpFilter) {
            insertToMultiselectProcess(tmpFilter, colName, multiSelect, operatorDropDown, $form.find('input#isNull'));
        }

        $btnSubmit.click(function () {
            var i;
            var filter = wrapGrid.getFilter();

            pbaAPI.gridClearFileldFilter(filter, colName);

            var filters = [];
            var values = $element.data('kendoMultiSelect').value();

            var valuesTmp = [];

            $.each(values, function (i, el) {
                if (($.inArray(el, valuesTmp) === -1) && (el !== '')) valuesTmp.push(el);
            });

            var operator = $select.data('kendoDropDownList').value();

            if ($form.find('input#isNull').is(':checked')) {
                if (operator === 'eq')
                    filters.push({
                        field: colName,
                        operator: 'isnull'
                    });
                else {
                    filters.push({
                        field: colName,
                        operator: 'isnotnull'
                    });
                }
            } else {
                for (i = 0; i < valuesTmp.length; i++) {
                    filters.push({
                        field: colName,
                        value: valuesTmp[i],
                        operator: operator
                    });
                }
            }

            if (filters.length > 0) {
                filter.filters.push({
                    filters: filters,
                    logic: operator === 'eq' ? 'or' : 'and',
                    field: colName
                });
            }

            wrapGrid.setFilter(filter);
            $btnFilter.click();

            return false;
        });


        $btnReset.click(function () {
            var filter = wrapGrid.getFilter();

            pbaAPI.gridClearFileldFilter(filter, colName);

            multiSelect.value('');
            multiSelect.enable(true);

            $form.find('input#isNull').prop('checked', false);
            $form.find('.k-button.openDialog').removeClass('k-state-disabled');

            wrapGrid.setFilter(filter);
            $btnFilter.click();

            return false;
        });
    };

    pbaAPI.gridStringColumnFilterUi = function (wrapGrid, property, $element) {
        $element.css({ width: '100%' });

        var dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: application.url.GetStandart('GetStrPropertyForFilter'),
                    data: function () {
                        return {
                            startswith: $element.val(),
                            mnemonic: wrapGrid.mnemonic,
                            property: property,
                            propertyIsBaseObject: false,
                        };
                    }
                }
            },
            serverSorting: true,
            serverFiltering: true
        });

        $element.kendoAutoComplete({
            dataSource: dataSource,
            filter: 'startswith'
        });
        if ($element.hasClass("k-input")) {
            $element.removeClass("k-input");
        }
        if (!$element.hasClass("k-textbox")) {
            $element.addClass("k-textbox");
        }
    };

    pbaAPI.gridBooleanFilterUi = function ($element) {
        debugger;
    }

    pbaAPI.gridDateTimeFilterUi = function (wrapGrid, fieldCol, $element) {
        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + fieldCol + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');
        $select.remove();
        $element.remove();
        var $content = $form.find('div[data-id="content"]');
        if ($content.length > 0) {
            return;
        }

        $content = $('<div data-id="content">').insertAfter($form.find('div.k-filter-help-text'));

        $content
            .append($('<input>', { id: 'dtm1', placeholder: 'С', css: { width: '90%' } }))
            .append($('<input>', { id: 'dtm2', placeholder: 'По', css: { width: '90%' } }));

        var $dtm1 = $content.find('#dtm1');
        var $dtm2 = $content.find('#dtm2');

        $dtm1.kendoDateTimePicker({
            format: 'dd.MM.yyyy HH:mm:ss',
            animation: false,
            start: 'month',
            depth: 'month',
            change: function () {
                var $picker = this;
                $dtm2.data("kendoDateTimePicker").setOptions({
                    min: $picker.value()
                });
            }
        });

        $dtm2.kendoDateTimePicker({
            format: 'dd.MM.yyyy HH:mm:ss',
            animation: false,
            start: 'month',
            depth: 'month',
            change: function () {
                var $picker = this;
                $dtm1.data("kendoDateTimePicker").setOptions({
                    min: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var filter = wrapGrid.getFilter();

            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            var filters = [];

            var val1 = $dtm1.data('kendoDateTimePicker').value();

            if (val1) {
                filters.push({
                    field: fieldCol,
                    value: val1,
                    operator: 'gte'
                });
            }

            var val2 = $dtm2.data('kendoDateTimePicker').value();

            if (val2) {
                filters.push({
                    field: fieldCol,
                    value: val2,
                    operator: 'lte'
                });
            }

            if (filters.length > 0) {
                filter.filters.push({
                    filters: filters,
                    logic: 'and',
                    field: fieldCol
                });
            }

            wrapGrid.setFilter(filter);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            var filter = wrapGrid.getFilter();
            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            $dtm1.data('kendoDateTimePicker').value(null);
            $dtm2.data('kendoDateTimePicker').value(null);

            for (var i = 0; i < filter.filters.length; i++) {
                if (filter.filters[i]._field_ === fieldCol) {
                    filter.filters.splice(i, 1);
                    i--;
                }
            }

            wrapGrid.widget().dataSource.filter(filter);
            wrapGrid.bind();

            $btnFilter.click();

            return false;
        });
    };

    pbaAPI.gridDateFilterUi = function (wrapGrid, fieldCol, $element) {
        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + fieldCol + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');
        $select.remove();
        $element.remove();
        var $content = $form.find('div[data-id="content"]');
        if ($content.length > 0) {
            return;
        }

        $content = $('<div data-id="content">').insertAfter($form.find('div.k-filter-help-text'));

        $content
            .append($('<input>', { id: 'dtm1', placeholder: 'С', css: { width: '90%' } }))
            .append($('<input>', { id: 'dtm2', placeholder: 'По', css: { width: '90%' } }));

        var $dtm1 = $content.find('#dtm1');
        var $dtm2 = $content.find('#dtm2');

        $dtm1.kendoDatePicker({
            format: 'dd.MM.yyyy',
            animation: false,
            start: 'month',
            depth: 'month',

            change: function () {
                var $picker = this;
                $dtm2.data("kendoDatePicker").setOptions({
                    min: $picker.value()
                });
            }
        });

        $dtm2.kendoDatePicker({
            format: 'dd.MM.yyyy',
            animation: false,
            start: 'month',
            depth: 'month',

            change: function () {
                var $picker = this;
                $dtm1.data("kendoDatePicker").setOptions({
                    max: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var filter = wrapGrid.getFilter();

            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            var filters = [];

            var val1 = $dtm1.data('kendoDatePicker').value();

            if (val1) {
                filters.push({
                    field: fieldCol,
                    value: val1,
                    operator: 'gte'
                });
            }

            var val2 = $dtm2.data('kendoDatePicker').value();

            if (val2) {
                filters.push({
                    field: fieldCol,
                    value: val2,
                    operator: 'lte'
                });
            }

            if (filters.length > 0) {
                filter.filters.push({
                    filters: filters,
                    logic: 'and',
                    field: fieldCol
                });
            }

            wrapGrid.setFilter(filter);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            var filter = wrapGrid.getFilter();
            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            $dtm1.data('kendoDatePicker').value(null);
            $dtm2.data('kendoDatePicker').value(null);

            for (var i = 0; i < filter.filters.length; i++) {
                if (filter.filters[i]._field_ === fieldCol) {
                    filter.filters.splice(i, 1);
                    i--;
                }
            }

            wrapGrid.widget().dataSource.filter(filter);
            wrapGrid.bind();

            $btnFilter.click();

            return false;
        });
    };

    pbaAPI.gridMonthFilterUi = function (wrapGrid, fieldCol, $element) {
        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + fieldCol + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');
        $select.remove();
        $element.remove();
        var $content = $form.find('div[data-id="content"]');
        if ($content.length > 0) {
            return;
        }

        $content = $('<div data-id="content">').insertAfter($form.find('div.k-filter-help-text'));

        $content
            .append($('<input>', { id: 'dtm1', placeholder: 'С', css: { width: '90%' } }))
            .append($('<input>', { id: 'dtm2', placeholder: 'По', css: { width: '90%' } }));

        var $dtm1 = $content.find('#dtm1');
        var $dtm2 = $content.find('#dtm2');

        $dtm1.kendoDatePicker({
            format: 'MM yyyy',
            animation: false,
            start: 'year',
            depth: 'year',
            change: function () {
                var $picker = this;
                $dtm2.data("kendoDatePicker").setOptions({
                    min: $picker.value()
                });
            }
        });

        $dtm2.kendoDatePicker({
            format: 'MM yyyy',
            animation: false,
            start: 'year',
            depth: 'year',
            change: function () {
                var $picker = this;
                $dtm1.data("kendoDatePicker").setOptions({
                    max: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var filter = wrapGrid.getFilter();

            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            var filters = [];

            var val1 = $dtm1.data('kendoDatePicker').value();

            if (val1) {
                filters.push({
                    field: fieldCol,
                    value: new Date(val1.getFullYear(), val1.getMonth(), 1),
                    operator: 'gte'
                });
            }

            var val2 = $dtm2.data('kendoDatePicker').value();

            if (val2) {
                filters.push({
                    field: fieldCol,
                    value: new Date(val2.getFullYear(), val2.getMonth() + 1, 0),
                    operator: 'lte'
                });
            }

            if (filters.length > 0) {
                filter.filters.push({
                    filters: filters,
                    logic: 'and',
                    field: fieldCol
                });
            }

            wrapGrid.setFilter(filter);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            var filter = wrapGrid.getFilter();
            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            $dtm1.data('kendoDatePicker').value(null);
            $dtm2.data('kendoDatePicker').value(null);

            for (var i = 0; i < filter.filters.length; i++) {
                if (filter.filters[i]._field_ === fieldCol) {
                    filter.filters.splice(i, 1);
                    i--;
                }
            }

            wrapGrid.widget().dataSource.filter(filter);
            wrapGrid.bind();

            $btnFilter.click();

            return false;
        });
    };

    pbaAPI.gridYearFilterUi = function (wrapGrid, fieldCol, $element) {
        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + fieldCol + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');
        $select.remove();
        $element.remove();
        var $content = $form.find('div[data-id="content"]');
        if ($content.length > 0) {
            return;
        }

        $content = $('<div data-id="content">').insertAfter($form.find('div.k-filter-help-text'));

        $content
            .append($('<input>', { id: 'dtm1', placeholder: 'С', css: { width: '90%' } }))
            .append($('<input>', { id: 'dtm2', placeholder: 'По', css: { width: '90%' } }));

        var $dtm1 = $content.find('#dtm1');
        var $dtm2 = $content.find('#dtm2');

        $dtm1.kendoDatePicker({
            format: 'yyyy',
            animation: false,
            start: 'decade',
            depth: 'decade',
            change: function () {
                var $picker = this;
                $dtm2.data("kendoDatePicker").setOptions({
                    min: $picker.value()
                });
            }
        });

        $dtm2.kendoDatePicker({
            format: 'yyyy',
            animation: false,
            start: 'decade',
            depth: 'decade',
            change: function () {
                var $picker = this;
                $dtm1.data("kendoDatePicker").setOptions({
                    max: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var filter = wrapGrid.getFilter();

            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            var filters = [];

            var val1 = $dtm1.data('kendoDatePicker').value();

            if (val1) {
                filters.push({
                    field: fieldCol,
                    value: new Date(val1.getFullYear(), 0, 1),
                    operator: 'gte'
                });
            }

            var val2 = $dtm2.data('kendoDatePicker').value();

            if (val2) {
                filters.push({
                    field: fieldCol,
                    value: new Date(val2.getFullYear(), 11, 31),
                    operator: 'lte'
                });
            }

            if (filters.length > 0) {
                filter.filters.push({
                    filters: filters,
                    logic: 'and',
                    field: fieldCol
                });
            }

            wrapGrid.setFilter(filter);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            var filter = wrapGrid.getFilter();
            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            $dtm1.data('kendoDatePicker').value(null);
            $dtm2.data('kendoDatePicker').value(null);

            for (var i = 0; i < filter.filters.length; i++) {
                if (filter.filters[i]._field_ === fieldCol) {
                    filter.filters.splice(i, 1);
                    i--;
                }
            }

            wrapGrid.widget().dataSource.filter(filter);
            wrapGrid.bind();

            $btnFilter.click();

            return false;
        });
    };

    pbaAPI.gridColumnFilterUi = function (values, $element, dataTextField, dataValueField) {
        $element.css({ width: '100%' });

        $element.kendoDropDownList({
            dataTextField: dataTextField || 'Text',
            dataValueField: dataValueField || 'Value',
            optionLabel: '-значение-',
            dataSource: values
        });
    };

    pbaAPI.gridEnumColumnFilterUi = function (wrapGrid, fieldCol, typeEnum, $element) {
        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + fieldCol + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');

        $select.remove();
        $element.remove();

        var $buttons = $('<ul>', {
            attr: {
                'class': 'kwidget kwidget--list',
            },
            css: {
                padding: '12px',
            }
        }).insertAfter($form.find('div.k-filter-help-text'));

        var uiEnum = application.UiEnums.get(typeEnum);
        var values = uiEnum.Values;

        var template = kendo.template('<div><i class="#=data.Icon#" style="width: 20px; color: #=data.Color#;"></i><span>#= pbaAPI.truncate(data.Title, 25)#<span></div>');
        var onClick = function () {
            $(this)
                .toggleClass('t-selected')
                .toggleClass('active');
        };

        for (var val in values) {
            if (values.hasOwnProperty(val)) {
                $buttons.append($('<li>', {
                    'title': values[val].Title,
                    'data-toggle': 'buttons',
                    'data-val': val,
                    css: {
                        'text-align': 'left'
                    },
                    html: $(template(values[val])).css('padding-left', '14px')
                }).click(onClick));
            }
        }
        var tmpfilter = wrapGrid.getFilter();
        $.each(tmpfilter.filters, function (i, gf) {
            if (gf.filters) {
                $.each(gf.filters, function (j, filter) {
                    if (filter.field === fieldCol) {
                        $buttons.find('li[data-val="' + filter.value + '"]').click();
                        gf._field_ = fieldCol;
                    }
                });
            }
        });

        $btnSubmit.click(function () {
            var filter = wrapGrid.widget().dataSource.filter();
            if (!filter || !filter.filters) {
                filter = { filters: [], logic: 'and' };
            }

            for (var i = 0; i < filter.filters.length; i++) {
                if (filter.filters[i]._field_ === fieldCol) {
                    filter.filters.splice(i, 1);
                    i--;
                }
            }

            var filters = [];

            $buttons.find('li.active').each(function (i, btn) {
                var $btn = $(btn);
                var val = $btn.attr('data-val');

                filters.push({
                    field: fieldCol,
                    value: val,
                    operator: 'eq'
                });
            });

            if (filters.length > 0) {
                filter.filters.push({
                    filters: filters,
                    logic: 'or',
                    _field_: fieldCol
                });
            }

            wrapGrid.widget().dataSource.filter(filter);
            wrapGrid.bind();

            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            var filter = wrapGrid.widget().dataSource.filter();
            if (!filter || !filter.filters) {
                filter = { filters: [], logic: 'and' };
            }

            $buttons.find('li.active').each(function (i, btn) {
                $(btn).click();
            });

            for (var i = 0; i < filter.filters.length; i++) {
                if (filter.filters[i]._field_ === fieldCol) {
                    filter.filters.splice(i, 1);
                    i--;
                }
            }

            wrapGrid.widget().dataSource.filter(filter);
            wrapGrid.bind();

            $btnFilter.click();

            return false;
        });
    };

    pbaAPI.gridPeriodColumnFilterUi = function (wrapGrid, fieldCol, format, $element) {
        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + fieldCol + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');

        $select.remove();
        $element.remove();

        var $content = $('<div>').insertAfter($form.find('div.k-filter-help-text'));

        $content
            .append($('<input>', { id: 'dtm1', placeholder: 'C', css: { width: '90%' } }))
            .append($('<input>', { id: 'dtm2', placeholder: 'По', css: { width: '90%' } }));

        var $dtm1 = $content.find('#dtm1');
        var $dtm2 = $content.find('#dtm2');

        $dtm1.kendoDateTimePicker({
            format: format,
            change:function() {
                var $picker = this;
                $dtm2.data("kendoDateTimePicker").setOptions({
                    min: $picker.value()
                });
            }
        });

        $dtm2.kendoDateTimePicker({
            format: format,
            change: function () {
                var $picker = this;
                $dtm1.data("kendoDateTimePicker").setOptions({
                    max: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var filter = wrapGrid.getFilter();

            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            var filters = [];

            var val1 = $dtm1.data('kendoDateTimePicker').value();

            if (val1) {
                filters.push({
                    field: fieldCol + '.Start',
                    value: val1,
                    operator: 'gte'
                });
            }
            var val2 = $dtm2.data('kendoDateTimePicker').value();

            if (val2) {
                filters.push({
                    field: fieldCol + '.End',
                    value: val2,
                    operator: 'lte'
                });
            }

            if (filters.length > 0) {
                filter.filters.push({
                    filters: filters,
                    logic: 'and',
                    field: fieldCol
                });
            }

            wrapGrid.setFilter(filter);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            var filter = wrapGrid.getFilter();
            pbaAPI.gridClearFileldFilter(filter, fieldCol);

            $dtm1.data('kendoDateTimePicker').value(null);
            $dtm2.data('kendoDateTimePicker').value(null);

            for (var i = 0; i < filter.filters.length; i++) {
                if (filter.filters[i]._field_ === fieldCol) {
                    filter.filters.splice(i, 1);
                    i--;
                }
            }

            wrapGrid.widget().dataSource.filter(filter);
            wrapGrid.bind();

            $btnFilter.click();

            return false;
        });
    };

    pbaAPI.itemTemplateAll = function () {
        return '<li><input id="all" type="checkbox" class="k-checkbox"/><label class="k-checkbox-label" for="all">Все</label></li>';
    };

    pbaAPI.itemTemplateExtraId = function (e) {
        if (e.field === 'all') {
            return pbaAPI.itemTemplateAll();
        } else {
            //handle the other checkboxes
            return '<li><input id="#=' + e.field + '#" type="checkbox" name="' + e.field + '" value="#=' + e.field + '#" class="k-checkbox"/><label class="k-checkbox-label" for="#=' + e.field + '#">#= application.viewModelConfigs.getConfig(' + e.field + ').Title #</label></li>';
        }
    };

    pbaAPI.itemTemplateBaseObject = function (e, mnemonic) {
        if (e.field === 'all') {
            return pbaAPI.itemTemplateAll();
        } else {
            var config = application.viewModelConfigs.getConfig(mnemonic);
            var lookup = config.LookupProperty.Text || "ID";
            var icon = '';

            if (config.LookupProperty.Image) {
                //icon = '<img class="img-circle" src="#= pbaAPI.imageHelpers.getImageSrc(' + config.LookupProperty.Image.FileID + ', 32, 32) #" alt="" />';
            } else if (config.LookupProperty.Icon) {
                //icon = '<span class="#= ' + config.LookupProperty.Icon + '.Value #" style="color: #= ' + config.LookupProperty.Icon + '.Color #"></span>';
            }

            return '<li><input id="checkbox__#=ID#" type="checkbox" name="' + e.field + '" value="#=ID#" class="k-checkbox"/><label class="k-checkbox-label" for="checkbox__#=ID#">' + icon + ' #=' + lookup + '#</label></li>';
        }
    };
}());
