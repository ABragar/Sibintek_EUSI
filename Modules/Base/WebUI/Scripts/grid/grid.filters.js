(function () {
    'use strict';

    var gridFilters = window.gridFilters = {};

    gridFilters.baseObjectColumn = function (params) {
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
                    url: "/api/listview/" + wrapGrid.mnemonic + "/filter/baseObject",
                    data: function () {
                        var val = $element.data('kendoMultiSelect').input.val();

                        if (val === placeholder) val = '';

                        return {
                            startswith: val,
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
                        field: colName+".ID",
                        operator: 'isnull'
                    });
                else {
                    filters.push({
                        field: colName + ".ID",
                        operator: 'isnotnull'
                    });
                }
            } else {
                for (i = 0; i < valuesTmp.length; i++) {
                    filters.push({
                        field: colName + ".ID",
                        value: valuesTmp[i],
                        operator: operator
                    });
                }
            }

            var newFilter = { filters: [], logic: operator === 'eq' ? 'or' : 'and' };
            if (filters.length > 0) {
                newFilter.filters.push({
                    filters: filters,
                    field: colName,
                    logic: operator === 'eq' ? 'or' : 'and'
                });
            }
            wrapGrid.addFilterAndUpdate(newFilter, colName);
            $btnFilter.click();

            return false;
        });


        $btnReset.click(function () {
            multiSelect.value('');
            multiSelect.enable(true);

            $form.find('input#isNull').prop('checked', false);
            $form.find('.k-button.openDialog').removeClass('k-state-disabled');

            wrapGrid.removeFilterForColumnAndUpdate(colName);
            $btnFilter.click();

            return false;
        });
    };

   gridFilters.stringColumn = function (wrapGrid, property, $element) {
        $element.css({ width: '100%' });

        var dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "/api/listview/" + wrapGrid.mnemonic + "/filter/string",
                    data: function () {
                        return {
                            startswith: $element.val(),
                            property: property,
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

    gridFilters.boolColumn = function ($element) {
    }

    gridFilters.dateTime = function (wrapGrid, fieldCol, $element) {
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

        var foundedFilters = wrapGrid.getFiltersByColName(wrapGrid.getFilter(), fieldCol);
        var defaultStartDate = null;
        var defaultEndDate = null;

        if (foundedFilters && foundedFilters.length) {
            for (var i = 0; i < foundedFilters.length; i++) {
                if (foundedFilters[i].operator === 'gte') {
                    defaultStartDate = foundedFilters[i].value;
                }
                if (foundedFilters[i].operator === 'lte') {
                    defaultEndDate = foundedFilters[i].value;
                }
            }
        }

        $dtm1.kendoDateTimePicker({
            animation: false,
            start: 'month',
            depth: 'month',
            value: defaultStartDate,
            change: function () {
                var $picker = this;
                $dtm2.data("kendoDateTimePicker").setOptions({
                    min: $picker.value()
                });
            }
        });

        $dtm2.kendoDateTimePicker({
            animation: false,
            start: 'month',
            depth: 'month',
            value: defaultEndDate,
            change: function () {
                var $picker = this;
                $dtm1.data("kendoDateTimePicker").setOptions({
                    min: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var newFilter = wrapGrid.getEmptyAndFilter();
            var val1 = $dtm1.data('kendoDateTimePicker').value();

            if (val1) {
                newFilter.filters.push({
                    field: fieldCol,
                    value: val1,
                    operator: 'gte'
                });
            }

            var val2 = $dtm2.data('kendoDateTimePicker').value();

            if (val2) {
                newFilter.filters.push({
                    field: fieldCol,
                    value: val2,
                    operator: 'lte'
                });
            }

            wrapGrid.addFilterAndUpdate(newFilter, fieldCol);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            $dtm1.data('kendoDateTimePicker').value(null);
            $dtm2.data('kendoDateTimePicker').value(null);

            wrapGrid.removeFilterForColumnAndUpdate(fieldCol);

            $btnFilter.click();

            return false;
        });
    };

    gridFilters.dateColumn = function (wrapGrid, fieldCol, $element) {
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

        var foundedFilters = wrapGrid.getFiltersByColName(wrapGrid.getFilter(), fieldCol);
        var defaultStartDate = null;
        var defaultEndDate = null;



        if (foundedFilters && foundedFilters.length) {
            for (var i = 0; i < foundedFilters.length; i++) {
                if (foundedFilters[i].operator === 'gte') {
                    defaultStartDate = foundedFilters[i].value;                    
                }
                if (foundedFilters[i].operator === 'lte') {
                    defaultEndDate = foundedFilters[i].value;                    
                }
            }
        }        
        $dtm1.kendoDatePicker({
            parseFormats: ['yyyy-MM-ddThh-mm-ss'],
            format: 'dd.MM.yyyy',
            animation: false,
            start: 'month',
            depth: 'month',
            value: defaultStartDate,
            change: function () {                
                var picker = this;
                $dtm2.data("kendoDatePicker").setOptions({
                    min: picker.value()
                });
            }
        });

        $dtm2.kendoDatePicker({
            parseFormats: ['yyyy-MM-ddThh-mm-ss'],
            format: 'dd.MM.yyyy',
            animation: false,
            start: 'month',
            depth: 'month',
            value: defaultEndDate,
            change: function () {
                var $picker = this;
                $dtm1.data("kendoDatePicker").setOptions({
                    max: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var newFilter = wrapGrid.getEmptyAndFilter();
            var val1 = $dtm1.data('kendoDatePicker').value();
            if (val1) {
                newFilter.filters.push({
                    field: fieldCol,
                    value: val1,
                    operator: 'gte'
                });
            }

            var val2 = $dtm2.data('kendoDatePicker').value();

            if (val2) {
                newFilter.filters.push({
                    field: fieldCol,
                    value: val2,
                    operator: 'lte'
                });
            }

            wrapGrid.addFilterAndUpdate(newFilter, fieldCol);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            $dtm1.data('kendoDatePicker').value(null);
            $dtm2.data('kendoDatePicker').value(null);

            wrapGrid.removeFilterForColumnAndUpdate(fieldCol);

            $btnFilter.click();

            return false;
        });
    };

    gridFilters.monthColumn = function (wrapGrid, fieldCol, $element) {
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

        var foundedFilters = wrapGrid.getFiltersByColName(wrapGrid.getFilter(), fieldCol);
        var defaultStartDate = null;
        var defaultEndDate = null;

        if (foundedFilters && foundedFilters.length) {
            for (var i = 0; i < foundedFilters.length; i++) {
                if (foundedFilters[i].operator === 'gte') {
                    defaultStartDate = foundedFilters[i].value;
                }
                if (foundedFilters[i].operator === 'lte') {
                    defaultEndDate = foundedFilters[i].value;
                }
            }
        }

        $dtm1.kendoDatePicker({
            format: 'MM yyyy',
            animation: false,
            start: 'year',
            depth: 'year',
            value: defaultStartDate,
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
            value: defaultEndDate,
            change: function () {
                var $picker = this;
                $dtm1.data("kendoDatePicker").setOptions({
                    max: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var newFilter = wrapGrid.getEmptyAndFilter();

            var val1 = $dtm1.data('kendoDatePicker').value();

            if (val1) {
                newFilter.filters.push({
                    field: fieldCol,
                    value: new Date(val1.getFullYear(), val1.getMonth(), 1),
                    operator: 'gte'
                });
            }

            var val2 = $dtm2.data('kendoDatePicker').value();

            if (val2) {
                newFilter.filters.push({
                    field: fieldCol,
                    value: new Date(val2.getFullYear(), val2.getMonth() + 1, 0),
                    operator: 'lte'
                });
            }

            wrapGrid.addFilterAndUpdate(newFilter, fieldCol);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            $dtm1.data('kendoDatePicker').value(null);
            $dtm2.data('kendoDatePicker').value(null);

            wrapGrid.removeFilterForColumnAndUpdate(fieldCol);

            $btnFilter.click();

            return false;
        });
    };

    gridFilters.yearColumn = function (wrapGrid, fieldCol, $element) {
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

        var foundedFilters = wrapGrid.getFiltersByColName(wrapGrid.getFilter(), fieldCol);
        var defaultStartDate = null;
        var defaultEndDate = null;

        if (foundedFilters && foundedFilters.length) {
            for (var i = 0; i < foundedFilters.length; i++) {
                if (foundedFilters[i].operator === 'gte') {
                    defaultStartDate = foundedFilters[i].value;
                }
                if (foundedFilters[i].operator === 'lte') {
                    defaultEndDate = foundedFilters[i].value;
                }
            }
        }


        $dtm1.kendoDatePicker({
            format: 'yyyy',
            animation: false,
            start: 'decade',
            depth: 'decade',
            value: defaultStartDate,
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
            value: defaultEndDate,
            change: function () {
                var $picker = this;
                $dtm1.data("kendoDatePicker").setOptions({
                    max: $picker.value()
                });
            }
        });

        $btnSubmit.click(function () {
            var newFilter = wrapGrid.getEmptyAndFilter();
            var val1 = $dtm1.data('kendoDatePicker').value();

            if (val1) {
                newFilter.filters.push({
                    field: fieldCol,
                    value: new Date(val1.getFullYear(), 0, 1),
                    operator: 'gte'
                });
            }

            var val2 = $dtm2.data('kendoDatePicker').value();

            if (val2) {
                newFilter.filters.push({
                    field: fieldCol,
                    value: new Date(val2.getFullYear(), 11, 31),
                    operator: 'lte'
                });
            }

            wrapGrid.addFilterAndUpdate(newFilter, fieldCol);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            $dtm1.data('kendoDatePicker').value(null);
            $dtm2.data('kendoDatePicker').value(null);

            wrapGrid.removeFilterForColumnAndUpdate(fieldCol);
            $btnFilter.click();

            return false;
        });
    };

    gridFilters.column = function (values, $element, dataTextField, dataValueField) {
        $element.css({ width: '100%' });

        $element.kendoDropDownList({
            dataTextField: dataTextField || 'Text',
            dataValueField: dataValueField || 'Value',
            optionLabel: '-значение-',
            dataSource: values
        });
    };

    gridFilters.enumColumn = function (wrapGrid, fieldCol, typeEnum, $element) {
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

        application.UiEnums.get(typeEnum,
	        function (uiEnum) {
	            var values = uiEnum.Values;

	            var template = kendo
	                .template('<div><i class="#=data.Icon#" style="width: 20px; color: #=data.Color#;"></i><span>#= pbaAPI.truncate(data.Title, 25)#<span></div>');

	            var onClick = function () {
	                $(this)
	                    .toggleClass('t-selected')
	                    .toggleClass('active');
	            };

	            for (var val in values) {
	                if (values.hasOwnProperty(val)) {
	                    $buttons.append($('<li>',
	                    {
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
	        });

        var tmpfilter = wrapGrid.getFilter();
        $.each(tmpfilter.filters, function (i, gf) {
            if (gf.filters) {
                $.each(gf.filters, function (j, filter) {
                    if (filter.field === fieldCol) {
                        $buttons.find('li[data-val="' + filter.value + '"]').click();
                        gf.field = fieldCol;
                    }
                });
            }
        });

        $btnSubmit.click(function () {
            var newFilter = { filters: [], logic: 'or' };

            $buttons.find('li.active').each(function (i, btn) {
                var $btn = $(btn);
                var val = $btn.attr('data-val');

                newFilter.filters.push({
                    field: fieldCol,
                    value: val,
                    operator: 'eq'
                });
            });

            wrapGrid.addFilterAndUpdate(newFilter, fieldCol);

            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            $buttons.find('li.active').each(function (i, btn) {
                $(btn).click();
            });

            wrapGrid.removeFilterForColumnAndUpdate(fieldCol);
            $btnFilter.click();

            return false;
        });
    };

    gridFilters.periodColumn = function (wrapGrid, fieldCol, format, $element) {
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
            change: function () {
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
            var newFilter = wrapGrid.getEmptyAndFilter();
            var val1 = $dtm1.data('kendoDateTimePicker').value();

            if (val1) {
                newFilter.filters.push({
                    field: fieldCol + '.Start',
                    value: val1,
                    operator: 'gte'
                });
            }
            var val2 = $dtm2.data('kendoDateTimePicker').value();

            if (val2) {
                newFilter.filters.push({
                    field: fieldCol + '.End',
                    value: val2,
                    operator: 'lte'
                });
            }
            newFilter.field = fieldCol;
            wrapGrid.addFilterAndUpdate(newFilter, fieldCol);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {                        
            $dtm1.data('kendoDateTimePicker').value(null);
            $dtm2.data('kendoDateTimePicker').value(null);

            wrapGrid.removeFilterForColumnAndUpdate(fieldCol);
            $btnFilter.click();

            return false;
        });
    };

    gridFilters._itemTemplateAll = function () {
        return '<li><input id="all" type="checkbox" class="k-checkbox"/><label class="k-checkbox-label" for="all">Все</label></li>';
    };


    gridFilters.objectTypeColumn = function (e) {
        if (e.field === 'all') {
            return gridFilters._itemTemplateAll();
        } else {
            //handle the other checkboxes
            return '<li><input id="#=' + e.field + '#" type="checkbox" name="' + e.field + '" value="#=' + e.field + '#" class="k-checkbox"/><label class="k-checkbox-label" for="#=' + e.field + '#">#= gridTemplates.vmConfig(' + e.field + ') #</label></li>';
        }
    };


    gridFilters.extraIdColumn = function (wrapGrid, colName, $element) {
        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + colName + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');
        var $btnReset = $form.find('button[type="reset"]');
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


        application.viewModelConfigs.get(wrapGrid.mnemonic).then(function (res) {
            if (res.Ext && res.Ext.Relations && res.Ext.Relations) {                
                var template = kendo
	                .template('<div><i class="#=data.Icon !=null && data.Icon.Value!=null  ? data.Icon.Value : \'mdi mdi-multiplication\' #" style="width: 20px; color: #=data.Icon !=null && data.Icon.Color!=null ? data.Icon.Color : \'rgb(66, 139, 202)\' #;"></i><span>#= pbaAPI.truncate(data.Title, 25)#<span></div>');

                var onClick = function () {
                    $(this)
	                    .toggleClass('t-selected')
	                    .toggleClass('active');
                };

                var tmpfilter = wrapGrid.getFilter();
                var foundedFilters = wrapGrid.getFiltersByColName(tmpfilter, colName);                
                for (var val in res.Ext.Relations) {
                    if (res.Ext.Relations.hasOwnProperty(val)) {
                        $buttons.append($('<li>',
	                    {
	                        'title': res.Ext.Relations[val].Title,
	                        'data-toggle': 'buttons',
	                        'data-val': res.Ext.Relations[val].Mnemonic,
	                        css: {
	                            'text-align': 'left'
	                        },
	                        html: $(template(res.Ext.Relations[val])).css('padding-left', '14px')
	                    }).click(onClick));
                        
                        if (foundedFilters && foundedFilters.length > 0) {
                            for (var i = 0; i < foundedFilters.length; i++) {
                                if (res.Ext.Relations[val].Mnemonic === foundedFilters[i].value) {
                                    $buttons.find('li[data-val="' + res.Ext.Relations[val].Mnemonic + '"]').click();
                                    break;
                                }
                            }
                        }                        
                    }                    
                }
            }            
        });        
        $btnSubmit.click(function () {           
            var newFilter = { filters: [], logic: 'or' };

            $buttons.find('li.active').each(function (i, btn) {
                var $btn = $(btn);
                var val = $btn.attr('data-val');

                newFilter.filters.push({
                    field: colName,
                    value: val,
                    operator: 'eq'
                });
            });           
            wrapGrid.addFilterAndUpdate(newFilter, colName);
            $btnFilter.click();
            return false;
        });

        $btnReset.click(function () {            
            $buttons.find('li.active').each(function (i, btn) {
                $(btn).click();
            });           
            wrapGrid.removeFilterForColumnAndUpdate(colName);
            $btnFilter.click();                        
            return false;
        });        
    };

    gridFilters.baseObjectColumnMulti = function (e, lookup) {
        if (e.field === 'all') {
            return gridFilters._itemTemplateAll();
        } else {
            return '<li><input id="checkbox__#=ID#" type="checkbox" name="' + e.field + '" value="#=ID#" class="k-checkbox"/><label class="k-checkbox-label" for="checkbox__#=ID#">#= ' + lookup + '#</label></li>';
        }
    };

    gridFilters.locationColumn = function (wrapGrid, property, $element) {
        var $form = $element.closest('form');
        var $select = $form.find('select');
        var $btnFilter = wrapGrid.element().find('th[data-field="' + property + '"]').find('.k-grid-filter');
        var $btnSubmit = $form.find('button[type="submit"]');
        var autoComplete = null;

        var $address = $form.find('#address');

        $select.remove();
        $element.remove();

        if ($address.length > 0) {
            return;
        }

        $address = $('<input>', { id: 'address', css: { width: '100%' } }).insertAfter($form.find('div.k-filter-help-text'));

        var dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "/api/listview/" + wrapGrid.mnemonic + "/filter/string",
                    data: function () {
                        return {
                            startswith: autoComplete.value(),
                            property: property + '.Address',
                        };
                    }
                }
            },
            serverSorting: true,
            serverFiltering: true
        });

        $address.kendoAutoComplete({
            dataSource: dataSource,
            filter: 'startswith',
            placeholder: 'Введите адрес...'
        });

        autoComplete = $address.data('kendoAutoComplete');

        var foundedFilters = wrapGrid.getFiltersByColName(wrapGrid.getFilter(), property);
        if (foundedFilters && foundedFilters.length === 1) {
            if (foundedFilters[0].filters && foundedFilters[0].filters.length === 1) {
                if (foundedFilters[0].filters[0].value) {
                    autoComplete.value(foundedFilters[0].filters[0].value);
                }                
            }
        }

        $btnSubmit.click(function () {           
            var newFilter = wrapGrid.getEmptyAndFilter();
            newFilter.field = property;
            var val = autoComplete.value();
            if (val) {
                newFilter.filters.push({
                    field: property + '.Address',
                    value: val,
                    operator: 'contains'
                });
            }

            wrapGrid.addFilterAndUpdate(newFilter, property);
            $btnFilter.click();

            return false;
        });

        $form.find('button[type="reset"]').click(function () {
            autoComplete.value('');
            wrapGrid.removeFilterForColumnAndUpdate(property);
            $btnFilter.click();
            return false;
        });
    };
}());