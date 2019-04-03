(function ($, pba_api, application) {




    var createElenemt = function () {
        var uid = pba_api.uid("multiedit_form");

        $("body").append("<div id=\"" + uid + "\" class=\"view-model-window wnd-loading-content\"></div>");

        return $("#" + uid);

    }


    var createWindow = function (options) {

        var $window = createElenemt();

        return $window.kendoWindow($.extend({
            width: application.getDefaultModalWidth(),
            height: application.getDefaultModalHeight(),
            modal: true,
            actions: ["Maximize", "Close"],
            title: 'Мульти-редактирование',
            visible: false,
            deactivate: function () {
                this.destroy();
            }
        }, options)).data("kendoWindow");

    }


    var getNext = function (index, options) {


        if (!options.objects) {
            return options.objects_func.then(function(e) {
                options.objects = e;
                return getNext(index, options);
            });
        };

        var def = $.Deferred();

        if (options.objects[index]) {
            def.resolve({
                object: options.objects[index],
                total: options.objects.length

            });
        } else
            def.reject();

        return def.promise();
    }

    var processMultiedit = function (options, model) {

        var deferred = $.Deferred();

        var canceled = false;

        var index = 0;


        var loop = function () {

            if (canceled === true) {
                deferred.reject();
                return;
            }

            getNext(index++, options).done(function (next) {

                deferred.notify({ index: index, id: next.object.id, start: true, total: next.total });

                var object = $.extend({}, model, next.object);

                pba_api.proxyclient.crud
                    .patch({ mnemonic: options.mnemonic, id: object.id }, { model: object })
                    .fail(function (e) {
                        deferred.notify({ index: index, id: object.id, fail: true });
                    }).done(function (e) {
                        if (e.error) {
                            deferred.notify({ index: index, id: object.id, fail: true, message: e.message });
                        } else {
                            deferred.notify({ index: index, id: object.id, done: true, });
                        }
                    }).always(loop);

            }).fail(function () {
                deferred.resolve();
            });

        }
        setTimeout(loop, 0);

        return deferred.promise({
            cancel: function () {
                canceled = true;
            }
        });

    }


    pba_api.openMultiEdit = function (options) {

        options = $.extend({
            mnemonic: null,
            objects_func: null,
            objects: null,
            dataSource: null
        }, options);

        var total;

        return getNext(0, options).then(function (e) {
            total = e.total;
            return pbaAPI.proxyclient.crud.get({ mnemonic: options.mnemonic, id: e.object.id });
        }).then(function (e) {

            var deferred = $.Deferred();

            var property = pba_api.uid("property");

            var wnd = createWindow({
                content: "/MultiEdit/GetPartialView?" + $.param({ mnemonic: options.mnemonic, property: property }),

                close: function () {
                    deferred.resolve();
                }
            });
            wnd.center();
            wnd.toFront();
            wnd.open();

            var form;
            var notify;

            var prop = kendo.observable({});

            var model = kendo.observable({
                model: e.model,

                apply: function () {

                    var that = this;

                    form.element.trigger("onSave", form);


                    if (!this.apply_enabled())
                    {
                        pba_api.errorMsg("Не выбрано ни одно свойсво");
                        return;
                    }
                    var edit_model = this.model.toJSON();

                    var model = {};

                    for (var x in prop.toJSON()) {
                        if (prop[x] == true)
                            model[x] = edit_model[x];
                    }

                    

                    this._promise = processMultiedit(options, model);

                    that.trigger("change", { field: "apply_run" });

                 
                    
                    this.set("form_visible", false);

                    this._promise.always(function (e) {


                        delete that._promise;
                        that.trigger("change", { field: "apply_run" });

                    }).always(function () {

                        
                        that.set("show_form_visible", true);

                    }).done(function () { that.set("status", "<p> Завершено"); })
                        .fail(function () { that.set("status", "<p> Отмена"); })
                        .progress(function (e) {
                            var template = kendo.template($('#multiedit-row-template').html());
                            var $notify = $('.multiedit__command-notify');
                            if (e.done) {
                                that.set("status", null);
                                var result = template({Title: e.id, Description: 'OK'})
                                $notify.prepend(result);
                            } else if (e.fail) {
                                that.set("status", null);
                                var result = template({ Title: e.id, Description: (e.message || "Произошла ошибка") })
                                $notify.prepend(result);
                            } else if (e.start) {

                                that.set("current_index", e.index);
                                that.set("current_progress", e.index/e.total*100);
                                that.set("total", e.total);

                                that.set("status", "<p> Выполняется" + e.id + "(" + e.index + " из " + e.total + ")");
                            }
                        });


                },
                show_form: function () {
                    this.set("form_visible", true);
                    this.set("show_form_visible", false);
                    $('.multiedit__command-notify').html('');
                    //this.set('current_progress', 0);
                },
                form_visible: true,
                show_form_visible: false,
                current_progress: null,
                current_index: null,
                total: total,

                status: null,

                apply_run: function () {
                    if (this._promise)
                        return true;

                    return false;
                },

                cancel: function () {
                    if (this._promise)
                        this._promise.cancel();
                },

                apply_enabled: function () {
                    for (var x in prop.toJSON()) {
                        if (prop[x] == true)
                            return true;
                    }

                    return false;
                }
            });
            model.bind("change", function (e) {

                if (e.field == "form_visible") {
                    if (model.get("form_visible")) {
                        form.element.show();

                    } else {
                        form.element.hide();
                    }
                }

            })

            $.each(e.model,
            function (name) {
                prop.set("name", false);

            });

            prop.bind("change",
                function (e) {
                    model.trigger("change", { field: "apply_enabled" });
                });

            model.set("model." + property, prop);


            wnd.bind("refresh",
                function (e) {
               


                    kendo.bind(wnd.element.find(".multiedit__command"), model);

                    var $notify = wnd.element.find(".multiedit__command-notify");

                    var notifyTemplate = wnd.element.find('.multiedit-row-template');

                    notify = function (html) {
                        $notify.append(html);
                    }

                    var $form = wnd.element.find("form");

                    $form.pbaForm({
                        model: model,
                        nameModel: "model",
                        attrBind: true,
                        validate: true
                    });


                    form = $form.data("pbaForm");

                   

                    form.bind();

                    if (!e.sender.wrapper.hasClass('multiedit'))
                        e.sender.wrapper.addClass('multiedit');

                    e.sender.element.removeClass("wnd-loading-content");
                });
            return deferred.promise();

        });
    }





}(window.jQuery, window.pbaAPI, window.application));