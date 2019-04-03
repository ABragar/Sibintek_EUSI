(function ($, pbaApi, window) {
    "use strict";

    var filters = {
        QueryTree: function(item) {

            var dd = pbaApi.guid();
            var selector = ".rule-value-container>div.query-tree";

            return {
                input: function(rule, name) {
                    return '<div class="query-tree" name="' + name + '"/>';
                },
                plugin: "queryTree",
                plugin_config: {
                    Mnemonic: item.SystemData
                },
                valueSetter: function(rule, value) {
                    rule.$el.find(selector).queryTree("setValue", value);
                },
                valueGetter: function(rule) {
                    console.log(dd);
                    return rule.$el.find(selector).queryTree("getValue");
                },
                validation: {
                    callback: function(value, rule) {
                        return true;
                    }
                }
            };
        }
    };

    $.extend(window,
        {
            querytreeFilters: filters
        });

    var getFilter = function(item) {

        if (!item)
            return null;

        var result = {
            id: item.Id,
            field: item.Id,
            operators: item.Operators,
            label: item.Label,
            type: item.PrimitiveType
        };

        if (item.Plugins && item.Plugins.length) {
            result.filters = {};
            if (item.Plugins.indexOf("InAndNotIn") !== -1) {
                result.filters.Default = Object.assign({}, result);
                result.filters.Default.input = item.PrimitiveType === "string" ? "text" : "number";
                delete result.filters.Default.filters;
            }                   
            item.Plugins.forEach(function(type) {
                var custom = filters[type];
                result.filters[type] = $.extend({}, result, custom(item), { name: type });             
                delete result.filters[type].filters;
            });         
        }
        if (result.filters.Default) {
            $.extend(result, result.filters.Default);
        } else {
            $.extend(result, filters[item.Plugins[0]](item));
        }
        return result;
    };

    var getConfig = function (mnemonic) {

        return pbaApi.proxyclient.querytree.get({ mnemonic: mnemonic })
            .then(function (e) {

                return {
                    title: e.Title,
                    filters: e.Items.map(getFilter)
                };

            });
    };

    var getRule = function(original) {
        if (!original) {
            return null;
        }

        if (original.condition) {
            return {
                condition: original.condition,
                rules: original.rules.map(getRule)
            };
        }

        return {
            id: original.id,
            operator: original.operator,
            value: original.value,
            label: original.label
        };

    };

    var getQueryTreeWindow = function (title, id) {

        var $wnd = $("<div id='window" + id + "' class='query-tree-window view-model-window' >\
            <div class='dialog dialog--modal'>\
               <div class='dialog__toolbar' style:'overflow-y:auto'>\
                  <div class='kwidget kwidget--toolbar k-toolbar'>\
                        <div style='float: right; visibility: visible;'  class='k-button-group k-toolbar-last-visible'>\
                            <a id='success"+ id + "' class='k-button success'>Применить</a>\
                        </div >\
                  </div>\
               </div>\
                <div class='dialog__content' style='overflow: scroll;'>\
                    <form>\
                      <div id='builder"+ id + "'></div>\
                    </form >\
                </div>\
            </div>\
          </div>");

        $("body").append($wnd);

        return $wnd.kendoWindow({
            width: 900,
            height: 600,
            title: title,
            actions: ["Maximize", "Close"],
            modal: true
        }).data("kendoWindow");

    };

    $.widget("pba.queryTree",
        {
            options: {
                Mnemonic: null,
                IsRoot: false
            },
            _create: function () {

                this._value = null;

                this._id = pbaApi.guid();

                this.element.append('<a id="querytree-button' + this._id + '" class="k-button" data-popup="right">Создать...</a>');

                var that = this;

                getConfig(this.options.Mnemonic).done(function (config) {
                    that._config = config;

                    $(that.element).find("#querytree-button" + that._id).click(function () {
                        that.edit();
                    });

                    that.refresh();
                });
            },

            refresh: function () {
                var $a = $(this.element).find("#querytree-button" + this._id);

                if (this._value) {
                    $a.text("Редактировать...");
                    $a.attr("title", JSON.stringify(this._value));
                    $a.addClass("success");
                } else {
                    $a.text("Создать...");
                    $a.attr("title", "");
                    $a.removeClass("success");
                }
            },

            edit: function () {

                var that = this;

                if (!this._wnd) {

                    this._wnd = getQueryTreeWindow(this._config.title, this._id);
                }

                var wnd = this._wnd;

                if (this.options.IsRoot) {
                    wnd.maximize();
                }

                wnd.center().open();

                var $builder = $("<div/>");

                $("#builder" + this._id).append($builder);

                var $success = $("#success" + this._id);

                try {

                    $builder.queryBuilder(
                        {
                            filters: that._config.filters,
                            rules: that._value
                        });

                } catch (e) {
                    console.log(e);
                }

                var success = function() {

                    var rule = $builder.queryBuilder("getRules");
                    var preview = $builder.queryBuilder("getFriendly");

                    that._value = getRule(rule);
                    that._preview = preview;

                    that.element.trigger("change");
                    // do not call success if not valid
                    if (that._value) {
                        that.element.trigger("success",
                            {
                                rules: that._value,
                                preview: that._preview
                            });
                        that.element.trigger("close");

                        that.refresh();
                        wnd.close();
                    }
                };

                $success.click(success);


                var close =
                    function () {
                        $builder.remove();
                        $success.unbind("click", success);
                        that.element.trigger("close");
                        wnd.unbind("close", close);
                    }

                wnd.bind("close", close);

                this.refresh();
            },
            getValue: function () {
                return this._value;
            },
            getPreview: function () {
                return this._preview;
            },
            setValue: function (value) {
                this._value = value;
                this.refresh();
            },
            getFilterOid: function () {
                return this._filterOid;
            },
            setFilterOid: function (value) {
                this._filterOid = value;
            },

            _destroy: function () {

                if (!this._wnd) {
                    _wnd.destroy();
                }
            }
        });


    //$.extend(pbaApi, {});

}(window.jQuery, window.pbaAPI, window));