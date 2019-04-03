//import { debug } from "util";

$.fn.pbaButton = function (clickHandler) {
    var btn = {
        click: function () {
            clickHandler();
        }
    };

    var isEnabled = true;
    var self = this;

    btn.enable = function (boolEnable) {
        isEnabled = boolEnable;

        if (boolEnable) {
            $(self).removeClass('k-state-disabled');
        } else {
            $(self).addClass('k-state-disabled');
        }
    }

    $(this).click(function () {
        if (isEnabled) {
            btn.click();
        }
    });

    $(this).data('pbaButton', btn);

    return btn;
};

$.fn.pbaToolbar = function (eventHandler) {
    $(this).on('click', function () {
        eventHandler($(this).attr("id"));
    });
}

$.fn.pbaHistorySearch = function (eventHandler) {
    var self = this;
    var historySearch = {
        searchAll: function (state) {
            eventHandler({ sender: self, type: "checkbox", value: state });
        },
        searchDate: function (date) {
            eventHandler({ sender: self, type: "date", value: date });
        }
    }

    self.change(function (e) {
        if (e.target.type === "checkbox") {
            elState = e.srcElement.checked;
            historySearch.searchAll(elState);
        }
        else {
            elDate = e.target.value || "";
            historySearch.searchDate(elDate);
        }
    });
}
//sib
$.fn.pbaAllGridData = function (eventHandler) {
    var self = this;
    var showAllData = function (state) {
        eventHandler({ sender: self, checked: state });
    }
    self.change(function (event) {
        var state = event.target.checked
        showAllData(state);
    })
}



$.fn.pbaHideEmptyEditors = function (eventHandler) {
    var self = this;
    var hideEmptyEditors = function (state) {
        eventHandler({ sender: self, checked: state });
    }
    self.change(function (e) {
        var state = e.srcElement.checked
        hideEmptyEditors(state);
    })
}

//end sib

$.fn.pbaSearchBox = function (eventHandler) {
    var self = this;
    var input = $(self).find('input');
    var span = $(self).find('.cancel-search');
    var searchButton = $(self).find('span.icon-search');

    var searchBox = {
        search: function (str) {
            eventHandler({ sender: self, str: str });
        },
        clear: function () {
            input.val('');
            input.trigger('blur');
            eventHandler({ sender: self, str: '' });
        }
    };

    searchButton.bind('click', function () {
        input.focus();
    });

    span.bind('click', function () {
        searchBox.clear();
    });

    input.bind("keyup", function (e) {
        if (e.keyCode === 13 || e.which === 13) {
            searchBox.search(input.val());
        }
    });

    return searchBox;
};

$.fn.pbaExport = function (clickHandler) {
    var exporter = {
        click: function (type) {
            clickHandler(type);
        }
    };

    var self = this;

    var types = $(self).find('[data-type]');

    $.each(types, function (i, v) {
        $(v).bind('click', function () {
            exporter.click($(v).data('type'));
        })
    });

    return exporter;
};


$.fn.pbaForm = function (options) {
    var self = this;

    var settings = $.extend({}, options);

    var form = {
        element: self,
        model: null,
        parentForm: null,
        nameModel: "",
        validator: null,
        viewData: {},
        hotkeysObj: null,
        oldValues: {},
        byDate: null
    };

    if (settings.oldValues) {
        form.oldValues = settings.oldValues;
    }

    if (settings.wrap) {
        if ($(this).find("." + settings.wrap).length === 0) {
            $(this).wrap("<div class='common-form " + settings.wrap + "'></div>");
        }
    }

    if (settings.model) {
        form.model = settings.model;
        form.byDate = settings.model.byDate;
    }

    if (settings.nameModel) {
        form.nameModel = settings.nameModel + ".";
    }

    if (settings.attrBind) {
        $(self).find("[data-bind]").each(function () {
            if (form.nameModel !== "") {
                var str = $(this).attr("data-bind").replace(" ", "");

                str = str.replace("alt:", "alt: " + form.nameModel);
                str = str.replace("src:", "src: " + form.nameModel);
                str = str.replace("checked:", "checked: " + form.nameModel);
                str = str.replace("href:", "href: " + form.nameModel);
                str = str.replace("html:", "html: " + form.nameModel);
                str = str.replace("source:", "source: " + form.nameModel);
                str = str.replace("text:", "text: " + form.nameModel);
                str = str.replace("value:", "value: " + form.nameModel);

                $(this).attr("data-bind", str);
            }
        });
    }

    //-----------Validator----------//
    if (settings.validate) {
        form.validator = $(self).data("kendoValidator");

        if (!form.validator) {
            $(this).kendoValidator({
                    validate: function (e) {
                        $(self).trigger("onValidate", e);
                    }
                }
            );
            form.validator = $(self).data("kendoValidator");
        }
    }

    form.validate = function () {
        if (form.validator !== null)
            return form.validator.validate();
        else
            return false;
    }

    //-----------Bind----------//
    form.bind = function (model) {
        if (this.validator != null) {
            this.validator.hideMessages();
        }

        if (model) {
            this.model = model;
        }

        $(self).trigger("onBeforeBind", this);

        kendo.bind(self, this.model);
        var $tabs = $(self).find('.common-editor-tabs');
        var dialog = $(self).closest(".dialog-vm");

        var arr = [
                    {
                        keyMask: "ctrl+enter",
                        handler: function () {
                            dialog.find('[data-bind="click:apply"]').click();
                        }
                    },
                    {
                        keyMask: "alt+enter",
                        handler: function () {
                            dialog.find('[data-bind="click:save"]').click();
                            var curGrid = $($(".k-grid")[0]);
                            if (curGrid) {
                                var curTable = curGrid.find(".k-selectable");
                                if (curTable) {
                                    curTable.focus();
                                }
                            }
                        }
                    },
                    {
                        keyMask: "ctrl+left",
                        handler: function () {
                            var prevElem = $tabs.find("li.active").prev();
                            if (prevElem && prevElem.length > 0) {
                                prevElem.find("a").click();
                            } else {
                                var lastElem = $tabs.children(":last-child");
                                if (lastElem) {
                                    lastElem.find("a").click();
                                }
                            }
                            
                        }
                    },
                    {
                        keyMask: "ctrl+right",
                        handler: function () {
                            var nextElem = $tabs.find("li.active").next();
                            if (nextElem && nextElem.length > 0) {
                                nextElem.find("a").click();
                            } else {
                                var firstElem = $tabs.children(":first-child");
                                if (firstElem) {
                                    firstElem.find("a").click();
                                }
                            }

                        }
                    },
                    {
                        keyMask: "esc",
                        handler: function () {
                            dialog.data("dialogVM").close();
                            var curGrid = $($(".k-grid")[0]);
                            if (curGrid) {
                                var curTable = curGrid.find(".k-selectable");
                                if (curTable) {
                                    curTable.focus();
                                }
                            }
                        }
                    },

        ];
        var $wnd = $(self).closest('[data-role="window"]');
        if ($wnd) {
            if (this.hotkeysObj != null) {
                this.hotkeysObj.destroy($wnd.attr("id"));
                this.hotkeysObj = null;
            }
            this.hotkeysObj = Object.create(hotkeysObject);
            this.hotkeysObj.init($wnd.attr("id"), arr);
        }
        
        $(self).trigger("onAfterBind", this);

        //event change model
        form.model.bind("change", function (e) {
            $(self).trigger("onChange", {
                sender: form,
                field: e.field.replace(form.nameModel, "")
            });
        });


        form.model.bind("set", function (e) {
            var field = e.field.replace(form.nameModel, "");
            form.oldValues[field] = e.sender.model[field];
        });

        return this;
    }
  

    form.unbind = function () {
        var $wnd = $(self).closest('[data-role="window"]');
        
        if ($wnd && this.hotkeysObj != null) {
            this.hotkeysObj.destroy($wnd.attr("id"));
        }
        this.hotkeysObj = null;
        kendo.unbind(self);
        this.model = null;
        this.oldValues = {};
    }

    //-----------Model----------//
    form.getModel = function () {
        return this.model.get(this.nameModel);
    }

    form.setModel = function (model) {
        this.oldValues = {};
        return this.model.set(this.nameModel, model);
    }

    form.getPr = function (pr) {
        return this.model.get(this.nameModel + pr);
    }

    form.setPr = function (pr, val) {
        this.oldValues[pr] = this.model.get(this.nameModel + pr);
        this.model.set(this.nameModel + pr, val);
    }

    $(this).data('pbaForm', form);

    $(self).keydown(function (e) {
        if (e.which === 13 && e.target.nodeName.toLowerCase() !== "textarea") {

            e.preventDefault();

            var wnd = $(self).closest(".k-window");

            if (wnd.length > 0) {
                if (settings.buttons) {

                } else {

                }
            }
        }
    });

    // Custom data                                  [{"Key" : Value}]
    form.getViewData = function (key) {
        if (key in this.viewData)
            return this.viewData[key];

        var serchParent = function (form) {
            if (form) {
                if (key in form.viewData)
                    return form.viewData[key];

                return serchParent(form.parentForm);
            }

            return null;
        };

        return serchParent(this.parentForm);
    };



    form.addViewData = function (key, value) {
        this.viewData[key] = value;
    };


    //events
    form.onResize = function (wnd) {
        $(self).trigger("onResize", {
            sender: form,
            wnd: wnd,
        });
    };

    form.onTabShown = function (tabID) {
        $(self).trigger("onTabShown", {
            sender: form,
            tabID: tabID,
        });
    };

    //sib   

    form.getEditorInput = function (prName) {
        var editorSelector = "div.label-editor-row[data-field='" + prName + "']"
        var editors = self.find(editorSelector);
        var editorId = editors.find("label").attr('for');
        return editors.find("input");
        
    };

    form.getEditor = function (prName) {
        var editorSelector = "div.label-editor-row[data-field='" + prName + "']"
        var editors = self.find(editorSelector);        
        var editorId = editors.find("label").attr('for');
        var editor = window[editorId];

        if (editor !== undefined) {
            return editor;
        }
        else {
            return editors.find("input");
        }
    };
    
    form.getEditorTextArea = function (prName) {
        var editorSelector = "div.label-editor-row[data-field='" + prName + "']"
        var editors = self.find(editorSelector);        
        var editorId = editors.find("textarea").attr('for');
        var editor = window[editorId];

        if (editor !== undefined) {
            return editor;
        }
        else {
            return editors.find("textarea");
        }
    };

    form.getEditorRow = function (prName) {
        var editorSelector = "div.label-editor-row[data-field='" + prName + "']"
        var editorRow = self.find(editorSelector);        
        return editorRow;
    };
  
    form.requiredEditor = function (prName, val) {
        var editor = form.getEditor(prName)
        if (editor.length === 0)
            editor = form.getEditorTextArea(prName);

        if (editor !== undefined) {
            var label = null;
            if (editor.parent) {
                label = editor.parent().siblings().find('label');
            }
            else
                label = $(editor).parent().siblings().find('label');

            if (!label || label.length == 0) {
                var selector = "div.label-editor-row[data-field='" + prName + "']";
                var fieldRow = $(selector);
                label = fieldRow.find('.label');
            }

            $(editor).attr("required", val);
            if (val) {
                $(editor).attr("validationmessage", "Обязательное поле");
                if (label.length > 0) {
                    if (label.siblings("span.required-mark").length === 0)
                        if (label.find("span.required-mark").length === 0)
                            label.append('<span class="required-mark">•</span>' );
                }
            }
            else {
                $(editor).removeAttr("required");
                label.siblings("span.required-mark").remove();
                if (label.find("span.required-mark").length !== 0)
                    label.find("span.required-mark").remove();
            }
        }
    };


    form.enableEditor = function (prName, val) {
        var editor = form.getEditor(prName);
        if (editor !== undefined) {
            //TODO: сделать универсальный enable
            if (editor.enable !== undefined) {
                editor.enable(val);
            }
            else {
                if (!editor.prop) {
                    if ($(editor).data("kendoNumericTextBox")) {
                        editor = $(editor).data("kendoNumericTextBox");
                        editor.enable(val);
                    }
                        
                }
                if (editor.prop) {
                    editor.prop('disabled', !val);
                    if (val)
                        editor.removeClass('k-state-disabled');
                    else
                        editor.addClass('k-state-disabled');
                }
                else {                    
                    editor.disabled = !val;
                }
                   
            }
        }
    };
    //end sib

    return form;
};

$.fn.pbaActionBar = function () {
    var self = this;
    var $self = $(this);

    var actionBar = {
        toolbarID: null,
        listViewID: null,
    };

    actionBar.toolbarID = $self.closest(".w-custom-toolbar").attr("data-toolbarID");
    actionBar.listViewID = $("#" + actionBar.toolbarID).closest("#list-view").find("[data-role=grid]").attr("id");

    $self.data('pbaActionBar', actionBar);

    return actionBar;
};

$.fn.extend({
    insertAtCaret: function (myValue) {
        var elem = this[0];
        if (document.selection) {
            elem.focus();
            sel = document.selection.createRange();
            sel.text = myValue;
            elem.focus();
        } else if (elem.selectionStart || elem.selectionStart == '0') {
            var startPos = elem.selectionStart;
            var endPos = elem.selectionEnd;
            var scrollTop = elem.scrollTop;
            elem.value = elem.value.substring(0, startPos) + myValue + elem.value.substring(endPos, elem.value.length);
            elem.focus();
            elem.selectionStart = startPos + myValue.length;
            elem.selectionEnd = startPos + myValue.length;
            elem.scrollTop = scrollTop;
        } else {
            elem.value += myValue;
            elem.focus();
        }
    }
});