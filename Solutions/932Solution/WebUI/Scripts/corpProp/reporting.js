(function () {
    "use strict";

    var parameters = [];
    var defaultValues = {};
    var reportViewerUid = '';
    var reportName = '';
    var customPrintStarted = false;
    var customPrintMode = false;

    var clearButtonSelector = 'button[data-custom-command="telerik_ReportViewer_clearParameters"]';
    var customPrintSelector = 'a[data-custom-command="telerik_ReportViewer_customPrint"]';
    var defaultPrintSelector = 'a[data-command="telerik_ReportViewer_print"]';
    var swichPrintModeButtonSelector = 'a[data-custom-command="telerik_ReportViewer_togglePrintMode"]';
    var treeViewSelector = 'div.trv-document-map.k-widget div[data-role="treeview"]';

    corpProp.reporting = corpProp.reporting || {};

    corpProp.reporting.getReportByCode = function (code) {
        var reportUrl = application.reportService + "/manager/getByCode/" + code;

        kendoReporting._refreshToken().done(
            function (res) {
                var token = res.Token;
                $.ajax({
                    url: reportUrl,
                    method: "GET",
                    headers: {
                        Authorization: "Bearer " + token
                    },
                    success: function (res) {
                        //                        console.log(res);
                        corpProp.reporting.showReport(res);
                    },
                    error: function (res) {
                        pbaAPI.errorMsg(res.statusText);
                    }
                });
            });
    };

    corpProp.reporting.clearParameters = function () {
        var reportViewer = $('#' + reportViewerUid).data("telerik_ReportViewer");
        debugger;
        var reportParameters = {};

        if (!reportViewer) {
            pbaAPI.errorMsg('report viewer is not initialized.');
            return;
        }

        // parameters.forEach(function(item){
        //     if (item.allowNull === true){
        //         reportParameters[item.name] = null;
        //     }else{
        //         reportParameters[item.name] = defaultValues[item.name];
        //     }
        // })

        parameters.forEach(function (item) {
            if (item.allowNull === false) {
                reportParameters[item.name] = defaultValues[item.name];
            }
        })


        reportViewer.reportSource({
            report: reportName,
            parameters: reportParameters
        });
    }

    corpProp.reporting.togglePrintMode = function () {
        console.log('Action: togglePrintMode');
        customPrintMode = !customPrintMode;
        corpProp.reporting.swichPrintMode(customPrintMode);
    }

    /**
     * Swich print mode default and custom 
     * @param {boolean} value True - to custom mode and false - to default mode
     */
    corpProp.reporting.swichPrintMode = function (value) {
        var report = $('#' + reportViewerUid);
        var treeView = report.find(treeViewSelector);
        if (value) {
            //Custom mode
            treeView.find(".k-checkbox-wrapper").removeClass('hidden');
            report.find(defaultPrintSelector).addClass("hidden");
            report.find(customPrintSelector).removeClass("hidden");
            report.find(swichPrintModeButtonSelector).parent().addClass('k-state-selected');
        } else {
            // Default mode
            treeView.find(".k-checkbox-wrapper").addClass('hidden');
            report.find(defaultPrintSelector).removeClass("hidden");
            report.find(customPrintSelector).addClass("hidden");
            report.find(swichPrintModeButtonSelector).parent().removeClass('k-state-selected');
        }
    }

    corpProp.reporting.printSelected = function (reportParams) {
        var treeView = $("div[data-role='treeview']").data("kendoTreeView");
        var reportParameters = {};

        var selected = treeView.dataSource._data.filter(function (item) {
            return item.checked === true;
        });

        if (selected.length === 0) {
            pbaAPI.infoMsg('Не выбран ни один отчет');
            return;
        }

        var str = reportName + '|' +
            selected.map(function (item) {
                return item.label + '.trdp';
            }).join('|');

        var reportViewer = $('#' + reportViewerUid).data("telerik_ReportViewer");
        reportViewer.reportSource({
            report: str,
            parameters: reportParams
        });
        customPrintStarted = true;
    }

    //    corpProp.reporting.setVisible = function (value) {
    //        var documentMap = $('#' + reportViewerUid).find('div.trv-document-map.k-widget');
    //        var parametersArea = $('#' + reportViewerUid).find('div.trv-parameters-area.k-widget');
    //
    //        if (value) {
    //            documentMap.removeClass('hidden');
    //            parametersArea.removeClass('hidden');
    //        } else {
    //            documentMap.addClass('hidden');
    //            parametersArea.addClass('hidden');
    //        }
    //    }




    corpProp.reporting.showReport = function (report) {
        var reportParams = report.Params;
        var params = null;


        pbaAPI.proxyclient.corpProp.getUserProfile({ id: application.currentUser.id }).done(function (res) {
            if (reportParams) {
                params = reportParams.replace("@currentUserId", application.currentUser.id).replace("@currentSocietyId", res.SocietyIDEUP).replace("@currentYear", (new Date()).getFullYear());
            }
            corpProp.reporting.showWindow(report, params);
        });
    };

    corpProp.reporting.showWindow = function (report, params) {
        var reportUrl = application.reportService + "/api/reports";
        var obj = report;
        var reportParams = (params) ? JSON.parse("{" + params + "}") : {};
        var scaleMode = reportParams !== null ? (reportParams.scaleMode === undefined ? "FIT_PAGE_WIDTH" : reportParams.scaleMode) : "FIT_PAGE_WIDTH";


        var that = this;
        reportName = obj.GuidId + obj.Extension;
        customPrintStarted = false;

        defaultValues = {};
        parameters = [];
        reportParams = reportParams

        var windowOptions = {
            height: "80%",
            width: "80%",
            title: obj.Name || "TestReport",
            isModal: true,
            actions: ["Maximize", "Close"]
        };

        var win = $("<div/>").kendoWindow(windowOptions).data("kendoWindow");




        reportViewerUid = pbaAPI.guid('viewer');

        var reportViewer = $("<div id='" + reportViewerUid + "' style='height: 100%;'/>").telerik_ReportViewer({
            serviceUrl: reportUrl,

            //templateUrl: reportUrl + "/resources/templates/telerikReportViewerTemplate.html",

            templateUrl: reportUrl + "/resources/templates/corpPropReportViewerTemplate.html",
            reportSource: {
                report: reportName,
                parameters: reportParams
            },
            viewMode: "INTERACTIVE",
            refreshReport: true,
            scale: 1,
            // scaleMode: "SPECIFIC",
            // scaleMode: "FIT_PAGE_WIDTH",
            scaleMode: scaleMode,
            parameterEditors: [
                {
                    match: function (parameter) {
                        if (!defaultValues.hasOwnProperty(parameter.name)) {
                            defaultValues[parameter.name] = parameter.value;
                            parameters.push(parameter);
                        }
                        reportParams[parameter.name] = parameter.value;

                        return false;
                    }
                },
                {
                    match: function (parameter) {
                        return parameter.name.indexOf("_percent") > 0;
                    },
                    createEditor: function (placeholder, options) {
                        var numElement = $(placeholder).html('<div><input class="k-input" type="text" style="width: 100%;"/></div>'),
                            parameter,
                            valueChangedCallback = options.parameterChanged;

                        numElement.parent().addClass("trv-half");
                        numElement.parent().find(".trv-parameter-header").addClass("trv-half");

                        function onChange() {
                            var val = this.value();
                            valueChangedCallback(parameter, val);
                        }

                        return {
                            beginEdit: function (param) {
                                parameter = param;

                                $(numElement).find("input").kendoNumericTextBox({
                                    format: "#.## \\%",
                                    min: 0,
                                    max: 100,
                                    step: 0.01,
                                    change: onChange
                                });
                            }
                        }
                    }
                },
                {
                    match: function (parameter) {
                        return Boolean(parameter.availableValues) && parameter.multivalue;
                    },

                    createEditor: function (placeholder, options) {
                        var dropDownElement = $(placeholder).html('<div></div>'),
                            parameter,
                            valueChangedCallback = options.parameterChanged,
                            dropDownList;

                        function onChange() {
                            var val = dropDownList.value();
                            valueChangedCallback(parameter, val);
                        }

                        return {
                            beginEdit: function (param) {

                                parameter = param;
                                var TagTemplateStr = '<span title="#=data.dataItems.map(function(el){return "&\\#9679; " + el.name.replace(/\\|.*$/g, "").replace(/"/g, \'&quot;\');}).join("\\n")#">#:data.dataItems[0].name.substring(0,21).replace(/\\|.*$/g, "")##if(data.dataItems[0].name.length>21){#...#}##if(values.length>1){# (И еще #:values.length - 1#)#}#</span>';
                                //Увеличить стандартный размер, если требуется
                                if (parameter.name.indexOf("_long") > 0) {
                                    dropDownElement.addClass("trv-double");
                                    TagTemplateStr = '<span title="#=data.dataItems.map(function(el){return "&\\#9679; " + el.name.replace(/\\|.*$/g, "").replace(/"/g, \'&quot;\');}).join("\\n")#">#:data.dataItems[0].name.substring(0,80).replace(/\\|.*$/g, "")##if(data.dataItems[0].name.length>80){#...#}##if(values.length>1){# (И еще #:values.length - 1#)#}#</span>';
                                }

                                $(dropDownElement).kendoMultiSelect({
                                    autoClose: false,
                                    tagMode: "single",
                                    //tagTemplate: 'Выбрано #: values.length# из #: maxTotal#',
                                    //tagTemplate: '<span title="#:data.dataItems[0].name.replace(/\\|.*$/g, "")#">#:data.dataItems[0].name.substring(0,21).replace(/\\|.*$/g, "")##if(data.dataItems[0].name.length>21){#...#}##if(values.length>1){# (И еще #:values.length - 1#)#}#</span>',
                                    //tagTemplate: '<span title="#=data.dataItems.map(function(el){return el.name.replace(/\\|.*$/g, "");}).join(" &\\#9679; ")#">#:data.dataItems[0].name.substring(0,21).replace(/\\|.*$/g, "")##if(data.dataItems[0].name.length>21){#...#}##if(values.length>1){# (И еще #:values.length - 1#)#}#</span>',
                                    //tagTemplate: '<span title="#=data.dataItems.map(function(el){return "&\\#9679; " + el.name.replace(/\\|.*$/g, "");}).join("\\n")#">#:data.dataItems[0].name.substring(0,21).replace(/\\|.*$/g, "")##if(data.dataItems[0].name.length>21){#...#}##if(values.length>1){# (И еще #:values.length - 1#)#}#</span>',
                                    //tagTemplate: '<span title="#=data.dataItems.map(function(el){return "&\\#9679; " + el.name.replace(/\\|.*$/g, "").replace(/"/g, \'&quot;\');}).join("\\n")#">#:data.dataItems[0].name.substring(0,21).replace(/\\|.*$/g, "")##if(data.dataItems[0].name.length>21){#...#}##if(values.length>1){# (И еще #:values.length - 1#)#}#</span>',
                                    tagTemplate: TagTemplateStr,
                                    change: onChange,
                                    filter: "contains",
                                    noDataTemplate: 'Нет данных',
                                    dataTextField: "name",
                                    template: "#: data.name.replace(/\\|[0-9]+\\|.*$/g, '') #",
                                    dataValueField: "value",
                                    valueTemplate: "#: data.name.replace(/|[0-9]+|.*$/g, '') #",
                                    placeholder: 'нажмите для выбора',
                                    value: parameter.value,
                                    autoWidth: true,
                                    dataSource: parameter.availableValues,
                                    dataBound: function () {
                                        console.log(this.dataSource);
                                        //Перегрузка удаления не одного элемента, а всех.
                                        var ThisElement = this;
                                        this.element.parent().find(".k-clear-value").click(function () {
                                            ThisElement.value("");
                                            ThisElement.trigger("change");
                                        });
                                    }
                                });


                                dropDownList = $(dropDownElement).data("kendoMultiSelect");
                                $(dropDownElement).find('.k-clear-value').attr("Title", "Очистить");
                            }
                        };
                    }
                },
                {
                    match: function (parameter) {
                        // Here you can use all of the parameter properties in order to
                        // create a more specific editor
                        return parameter.type === 'System.Boolean';
                    },

                    createEditor: function (placeholder, options) {
                        var dropDownElement = $(placeholder);
                        var parameter,
                            valueChangedCallback = options.parameterChanged,
                            dropDownList;

                        function onChange(value) {
                            valueChangedCallback(parameter, value);
                        }

                        return {
                            beginEdit: function (param) {

                                parameter = param;
                                var editor = $('<div></div>');
                                var ratioYes = $('<input type="radio" value="1" name="' + parameter.name + '" id="' + parameter.name + '_yes"/><label for="' + parameter.name + '_yes">Да</label>');
                                var ratioNo = $('<input type="radio" value="2" name="' + parameter.name + '" id="' + parameter.name + '_no"/><label for="' + parameter.name + '_no">Нет</label>');
                                if (parameter.value) {
                                    ratioYes.attr("checked", "checked");
                                } else {
                                    if (!parameter.allowNull) {
                                        ratioNo.attr("checked", "checked");
                                        parameter.value = false;
                                        onChange(false);
                                    }
                                }

                                editor.append(ratioYes);
                                editor.append(ratioNo);
                                /*ratioYes.on('change', function () { return onChange($(this).prop("checked")) });*/
                                $(editor).find('input').change(function () {
                                    onChange($(this).val() === "1");
                                });

                                $(dropDownElement).html(editor);
                            }

                        };
                    }
                },
                {
                    match: function (parameter) {
                        // Here you can use all of the parameter properties in order to
                        // create a more specific editor
                        return Boolean(parameter.availableValues) && !parameter.multivalue;
                    },

                    createEditor: function (placeholder, options) {
                        var dropDownElement = $(placeholder).html('<div></div>');

                        var parameter,
                            valueChangedCallback = options.parameterChanged,
                            dropDownList;

                        function onChange() {
                            var val = dropDownList.value();
                            valueChangedCallback(parameter, val);
                        }

                        return {
                            beginEdit: function (param) {

                                parameter = param;

                                //Увеличить стандартный размер, если требуется
                                if (parameter.name.indexOf("_long") > 0) {
                                    dropDownElement.addClass("trv-double");
                                }

                                $(dropDownElement).kendoDropDownList({
                                    dataTextField: "name",
                                    template: "#: data.name.replace(/\\|[0-9]+\\|.*$/g, '') #",
                                    valueTemplate: "<span title='#:data.name.replace(/\\|[0-9]+\\|.*$/g, '')#'>#:data.name.replace(/\\|[0-9]+\\|.*$/g, '')#</span>",
                                    dataValueField: "value",
                                    value: parameter.value,
                                    dataSource: parameter.availableValues,
                                    filter: "contains",
                                    autoWidth: true,
                                    change: onChange
                                });

                                dropDownList = $(dropDownElement).data("kendoDropDownList");
                            }

                        };
                    }
                },
                {
                    match: function (parameter) {
                        return parameter.type === 'System.DateTime';
                    },
                    createEditor: function (placeholder, options) {
                        var DatePickerElement = $(placeholder).html('<div><input title="datepicker" style="width: 100%" /></div>').find("input"),
                            ContainerDiv = DatePickerElement.parent().parent().parent();
                        //Уменьшить стандартный размер для дейтпикеров
                        ContainerDiv.addClass("trv-half");
                        ContainerDiv.find(".trv-parameter-header").addClass("trv-half");

                        var parameter,
                            valueChangedCallback = options.parameterChanged,
                            dropDownList;

                        function onChange() {
                            var val = DatePickerElement.data("kendoDatePicker").value();
                            //Костыли - кендо не принимает нормальный формат даты
                            var da = val.getDate() < 10 ? "0" + val.getDate() : val.getDate(),
                                mo = val.getMonth() < 9 ? "0" + (val.getMonth() + 1) : (val.getMonth() + 1),
                                ye = val.getFullYear(),
                                newKendoDate = mo + "." + da + "." + ye;

                            valueChangedCallback(parameter, newKendoDate);
                        }

                        return {
                            beginEdit: function (param) {
                                parameter = param;
                                $(DatePickerElement).kendoDatePicker({
                                    change: onChange
                                });
                                $(DatePickerElement).data("kendoDatePicker").value(parameter.label);
                            }

                        };
                    }
                }],

            updateUi: function (e) {
                console.log('updateUi');
                adaptiveFiltersPanel();
                $('.trv-parameter-container .k-clear-value').attr("Title", "Очистить");
            },
            pageReady: function (e) {
                console.log('pageReady');
                console.log({ 'Count': $('.k-treeview .k-state-selected').length });
                if ($('.k-treeview .k-state-selected').length === 0) {
                    $('.k-treeview .k-first').attr("aria-selected", true);
                    $('.k-treeview .k-first .k-in').addClass("k-state-selected");
                }
                if (customPrintStarted) {
                    this.commands.print.exec();
                }
            },
            renderingEnd: function (e) {
                console.log('renderingEnd');
                $(clearButtonSelector).click(function () {
                    that.clearParameters();
                });

                $(customPrintSelector).click(function () {
                    that.printSelected(reportParams);
                });

                $(swichPrintModeButtonSelector).click(function () {
                    that.togglePrintMode();
                });

                var treeView = $("div[data-role='treeview']").data("kendoTreeView");
                treeView.setOptions({
                    checkboxes: !customPrintStarted
                });
                treeView.dataSource.read();

                that.swichPrintMode(customPrintMode);

                //                that.setVisible(!customPrintStarted);
            },
            renderingBegin: function (e) {
                console.log('renderingBegin');
                $(clearButtonSelector).unbind('click');
                $(customPrintSelector).unbind('click');
                $(swichPrintModeButtonSelector).unbind('click');
            },
            printEnd: function () {
                if (customPrintStarted) {
                    customPrintStarted = false;
                    this.reportSource({
                        report: reportName,
                        parameters: reportParams
                    });
                }
            },
        });


        win.bind("resize", function () {
            adaptiveFiltersPanel();
        });
        win.bind("close", function () {
            console.log("close");
            win.destroy();
        })
        function adaptiveFiltersPanel() {
            $('.trv-parameters-area').removeClass('trv-parameters-area-min');
            // console.log({"trv-parameters-area-content.height":$('.trv-parameters-area-content').height()});
            if ($('.trv-parameters-area-content').height() !== 0)
                if ($('.trv-parameters-area-content').height() < 150) {
                    $('.trv-parameters-area').addClass('trv-parameters-area-min');
                }
        }
        var contentElement = $('<div style="height:100%;"/>');
        var rMenu = $('<div style="height:100%;width:20%"/>');
        // contentElement.append(rMenu);
        contentElement.append(reportViewer);
        win.content(contentElement);
        win.center();
        win.maximize();
        win.open();
    };
})();