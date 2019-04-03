/* globals $, kendo, pbaAPI, application */
(function () {
    "use strict";

    $.extend(pbaAPI,
    {
        log: function (msg) {
            if (application.isDebug)
                console.log(msg);
        },
        imageHelpers: {
            src: function (img, id, defImage) {
                img = $(img);
                defImage = defImage || '';
                if (img.length) {
                    var width = img.attr('width');
                    var height = img.attr('height');

                    img.attr('src', this.getsrc(id, width, height, defImage));
                }
            },
            getsrc: function (id, width, height, defImage) {
                return application.url.GetFiles('GetImage',
                    { id: id, width: width, height: height, defImage: defImage });
            },
            //TODO: устарела, не использовать
            getImageSrc: function (img, w, h, defImage, type) {
                if (img) {
                    return application.url.GetFiles('GetImage',
                        { id: img.FileID, width: w, height: h, defImage: defImage, type: type });
                }
                return application.url.GetFiles('GetImage',
                    { id: null, width: w, height: h, defImage: defImage, type: type });
            },
            getImageThumbnailSrc: function (img, size, defImage, type, scale, anchor) {
                if (img) {
                    return application.url.GetFiles('GetImageThumbnail', { id: img.FileID, size: size, defImage: defImage, type: type, scale: scale, anchor: anchor });
                }
                return application.url.GetFiles('GetImageThumbnail', { id: null, size: size, defImage: defImage, type: type, scale: scale, anchor: anchor });
            },
        },
        getHrefFile: function (fileid) {
            return application.url.GetFiles('GetFile', { fileid: fileid });
        },
        getFileWidget: function (id, callback) {
            $.get(application.url.GetFileData('GetWidget'), { id: id }, function (res) { callback(res.html); });
        },
        getPrVal: function (obj, prs, def) {
            var arrprs = prs.split('.');

            for (var i = 0; i < arrprs.length; i++) {
                var pr = arrprs[i];
                try {
                    if (pr in obj) {
                        obj = obj[pr];

                        if (!obj) return def;

                    } else {
                        return def;
                    }
                }
                catch (err) {
                    if (obj)
                        return pbaAPI.htmlEncode(obj);
                    else
                        return obj;
                }
            }

            if (obj)
                return pbaAPI.htmlEncode(obj);
            else
                return obj;
        },
        getCollectionPrVal: function (obj, prs, def) {
            var res = def;

            if (obj) {

                res = '<ul>';

                for (var i = 0; i < obj.length; i++) {
                    res += '<li>' + pbaAPI.getPrVal(obj[i], prs, '') + '</li>';
                }

                res += '</ul>';
            }

            return res;
        },

        replaceObjectPlaceholders: function (obj, params) {
            var initializedParams = {};
            $.each(params, function (i) {
                initializedParams[i] = params[i].replace(/\[(.*?)\]/g, function (g1, g2) {
                    return obj[g2];
                });
            });

            return initializedParams;
        },

        replaceListPlaceholders: function (currentObject, params) {
            var initializedParams = {};
            $.each(params, function (i) {
                initializedParams[i] = params[i].replace(/\[(.*?)\]/g, function (g1, g2) {
                    return currentObject[g2];
                });
            });

            return initializedParams;
        },
        extension: function (fileName) {
            if (!fileName || typeof fileName !== 'string')
                return null;

            var arr = fileName.toLowerCase().split('.');

            return arr[arr.length - 1];
        },
        _fileTypes: {
            text: 'txt,doc,rtf,log,tex,msg,text,wpd,wps,docx,page',
            table: 'csv,dat,tar,xml,vcf,pps,key,ppt,pptx,sdf,gbr,ged',
            sound: 'mp3,m4a,waw,wma,mpa,iff,aif,ra,mid,m3v',
            video: 'e-3gp,shf,avi,asx,mp4,e-3g2,mpg,asf,vob,wmv,mov,srt,m4v,flv,rm',
            image: 'png,psd,psp,jpg,jpeg,tif,tiff,gif,bmp,tga,thm,yuv,dds',
            vector: 'ai,eps,ps,svg',
            exdoc: 'pdf,pct,indd',
            spreadsheet: 'xlr,xls,xlsx',
            database: 'db,dbf,mdb,pdb,sql,aacd',
            executable: 'app,exe,com,bat,apk,jar,hsf,pif,vb,cgi',
            code: 'css,js,php,xhtml,htm,html,asp,cer,jsp,cfm,aspx,rss,csr,less',
            font: 'otf,ttf,font,fnt,eot,woff',
            archive: 'zip,zipx,rar,targ,sitx,deb,e-7z,pkg,rpm,cbr,gz',
            mount: 'dmg,cue,bin,iso,hdf,vcd',
            system: 'bak,tmp,ics,msi,cfg,ini,prf'
        },
        fileType: function (fileName) {
            var ext = pbaAPI.extension(fileName);

            if (!ext) return null;

            for (var key in pbaAPI._fileTypes) {
                if (pbaAPI._fileTypes.hasOwnProperty(key)) {
                    if (pbaAPI._fileTypes[key].split(',').indexOf(ext) !== -1) {
                        return key;
                    }
                }
            }

            return null;
        },
        extensionClass: function (fileName) {
            var ext = pbaAPI.extension(fileName);

            if (!ext) return null;

            var knownType = !!pbaAPI.fileType(ext);

            return knownType ? ('filetype filetype-' + ext) : 'default-file';
        },
        replaceUrlParametr: function (uri, key, value) {
            var pattern = '([?|&])' + key + '=.*?(&|$)';
            var re = new RegExp(pattern, 'i');
            var separator = uri.indexOf('?') !== -1 ? '&' : '?';

            if (uri.match(re)) {
                return uri.replace(re, '$1' + key + '=' + value + '$2');
            }

            return uri + separator + key + '=' + value;
        },
        getUrlParametr: function (uri, key) {
            var pattern = '([?|&])' + key + '=.*?(&|$)';
            var re = new RegExp(pattern, 'i');
            var separator = uri.indexOf('?') !== -1 ? '&' : '?';
            var match = uri.match(re);
            if (match) {
                return match;
            }
            return null;
        },
        addUrlParametrs: function (uri, params) {
            if (params) {
                for (var key in params) {
                    uri = pbaAPI.replaceUrlParametr(uri, key, params[key]);
                }
            }
            return uri;
        },
        openModalDialog: function (mnemonic, callbackSelect, params) {           
            application.viewModelConfigs.get(mnemonic).done(
                function (config) {
                    var _params = $.extend({
                        title: null,
                        width: null,
                        height: null,
                        searchStr: null,
                        filter: null,
                        maximize: false,
                        initProps: null,
                        dialogData: {}
                    },
                        params);

                    var wid = pbaAPI.uid("wid");

                    $("body").append("<div id=\"" + wid + "\" class=\"view-model-window wnd-loading-content\"></div>");

                    var $w = $("#" + wid);

                    var getparams = { mnemonic: mnemonic, typeDialog: "Modal" };

                    if (_params.searchStr != null)
                        getparams.searchStr = _params.searchStr;

                    if (_params.filter != null)
                        getparams.filter = _params.filter;

                    var title = _params.title ||
                        (config ? (config.ListView ? config.ListView.Title : config.Title) : mnemonic);

                    getparams.multiSelect = _params.multiSelect;

                    $w.kendoWindow({
                        width: _params.width || application.getDefaultModalWidth("listview"),
                        height: _params.height || application.getDefaultModalHeight("listview"),
                        title: title,
                        actions: ["Maximize", "Close"],
                        content: application.url.GetView("GetDialog", getparams),
                        modal: true,
                        visible: false,
                        refresh: function (e) {
                            var wnd = e.sender.element;
                            var dialog = wnd.find(".dialog-listview").data("dialogListView");

                            dialog.init({
                                multiSelect: _params.multiSelect,
                                callbackSelect: callbackSelect,
                                dialogData: _params.dialogData,
                                initProps: _params.initProps,
                                close: function () {
                                    e.sender.close();
                                }
                            });

                            dialog.resize(wnd.height());
                            wnd.removeClass("wnd-loading-content");
                        },
                        close: function () {
                            if (_params.callbackCancel) {
                                _params.callbackCancel();
                            }
                            $w.empty();
                        },
                        open: function (e) {
                            if (_params.zIndex) {
                                $(e.sender.wrapper).css("z-index", _params.zIndex);
                            }
                        }
                    });

                    var wnd = $w.data("kendoWindow");
                    wnd.center();
                    wnd.toFront();
                    wnd.open();
                });
        },

        // TODO: Make private method and dublicate method in api-migrate.js
        // Usings:
        //
        // initViewModel
        // openDetailView
        // initWizardViewModel
        // openWizardViewModelEx
        wndID: function (id, mnemonic) {
            if (typeof id === 'string' && id.indexOf('wnd_') === 0) {
                return id;
            }

            if (!id) {
                return pbaAPI.guid('wnd').replace(/\-/g, '');
            }

            var wid = mnemonic.split(',')[0];

            return 'wnd_' + wid.replace(/\./g, '_') + '_' + id;
        },

        initViewModel: function (config, params) {
            var mnemonic = config.Mnemonic;

            var _params = $.extend({
                wid: null,
                title: null,
                width: null,
                height: null
            },
                params);

            _params.wid = pbaAPI.wndID(_params.wid, mnemonic);

            var $w = $('#' + _params.wid);

            if (!$w.length) {
                $('body').append('<div id="' +
                    _params.wid +
                    '" class="view-model-window wnd-loading-content"></div>');

                $w = $('#' + _params.wid);
            }

            if (!$w.data('kendoWindow')) {
                $w.kendoWindow({
                    width: _params.width || config.DetailView.Width || application.getDefaultModalWidth(),
                    height: _params.height || config.DetailView.Height || application.getDefaultModalHeight(),
                    title: _params.title || config.DetailView.Title || config.Title,
                    actions: _params.actions || ['Maximize', 'Close'],
                    content: application.url.GetView('GetPartialViewModel',
                    {
                        mnemonic: mnemonic,
                        typeDialog: 'Modal',
                        isReadOnly: _params.isReadOnly
                    }),
                    modal: true,
                    visible: false
                });
            }

            return $w;
        },
        selectSimple: function (items, params) {
            var _params = $.extend({
                wid: null,
                title: "Выбор...",
                width: 400,
                height: 500,
                template: "<li  class='list-group-item' href='\\\\#'><span class='#: Icon.Value #' style='color: #: Icon.Color #'></span>&nbsp#: Title || Name #</li>",
                callback: function () { },
                cancel: function () { },
                multiSelect: false
            }, params);

            var selectedItems = [];

            if (_params.wid)
                _params.wid = "selectSimple_" + _params.wid;
            else
                _params.wid = pbaAPI.guid("selectSimple").replace(/\-/g, "");

            var $w = $("#" + _params.wid);

            if (!$w.length) {
                var body = $("body");
                if (_params.multiSelect) {
                    var toolbarId = _params.wid + "_toolbar";
                    var contentId = _params.wid + "_content";
                    var html =
                        "<div id='" + contentId + "'>" +
                            "<div class='dialog'> " +
                                "<div class='dialog__toolbar'> " +
                                    "<div class='kwidget kwidget--tolbar' id='" + toolbarId + "'/>" +
                                "</div>" +
                                "<ui id=\"" + _params.wid + "\" class=\"view-model-window kwidget kwidget--list\"></ui>" +
                            "</div>" +
                        "</div>";
                    body.append(html);
                    $("#" + toolbarId).kendoToolBar({
                        items: [
                            {
                                attributes: {
                                    "title": "Выбрать и закрыть",
                                    "class": "primary",
                                    "style": "float: right"
                                },
                                type: "button",
                                text: "Выбрать и закрыть",
                                spriteCssClass: "fa fa-check",
                                showText: "overflow",
                                click: function () {
                                    $w.data('kendoWindow').destroy();
                                    _params.callback(selectedItems);
                                }
                            }
                        ]
                    });
                    $w = $("#" + contentId);
                } else {
                    body.append("<ui id=\"" + _params.wid + "\" class=\"view-model-window kwidget kwidget--list\"></ui>");
                    $w = $("#" + _params.wid);
                }
            }

            if (!$w.data("kendoWindow")) {
                $w.kendoWindow({
                    width: _params.width,
                    height: _params.height,
                    title: _params.title,
                    modal: true,
                    visible: false,
                    close: function () {
                        _params.cancel();
                    }
                });
            }

            var wnd = $w.data('kendoWindow');

            var template = kendo.template(_params.template);

            var $list = $("#" + _params.wid);

            $list.empty();

            Array.prototype.forEach.call(items, function (item) {
                var templateItem;

                if (params.template) {
                    templateItem = item;
                } else {
                    templateItem = $.extend({ Title: "", Icon: { Value: "", Color: "" } }, item);
                }

                $list.append($(template(templateItem)).click(function () {
                    if (_params.multiSelect) {
                        if ($(this).hasClass("active")) {
                            selectedItems.splice(selectedItems.indexOf(item), 1);
                            $(this).removeClass("active");
                        } else {
                            selectedItems.push(item);
                            $(this).addClass("active");
                        }
                    } else {
                        wnd.destroy();
                        _params.callback(item);
                    }
                }));
            });

            wnd.center();
            wnd.open();
        },
        selectSimpleTreeView: function (items, params) {
            var _params = $.extend({
                wid: null,
                title: "Выбор",
                width: 400,
                height: 500,
                template: "<span data-id='#: item.ID #' class='#: item.Icon.Value #' style='color: #: item.Icon.Color #'></span>&nbsp#: item.Name #",
                children: "Items",
                callback: function () { },
                cancel: function () { }
            }, params);

            if (_params.wid)
                _params.wid = "selectSimpleTreeView_" + _params.wid;
            else
                _params.wid = pbaAPI.guid("selectSimpleTreeView").replace(/\-/g, "");

            var $w = $("#" + _params.wid);

            if (!$w.length) {
                $("body").append("<div id=\"" + _params.wid + "\" class=\"view-model-window list-group\"></div>");

                $w = $("#" + _params.wid);
            }
            var wnd = $w.data('kendoWindow');

            if (!wnd) {
                $w.kendoWindow({
                    width: _params.width,
                    height: _params.height,
                    title: _params.title,
                    modal: true,
                    visible: false,
                    close: function () {
                        _params.cancel();
                    }
                });

                wnd = $w.data('kendoWindow');

                // FOOTER BUTTONS
                $('<div class="footer-buttons right" style="margin-top: 7px;">')
                    .append($('<button class="k-button"><span class="k-icon k-update"></span></span class="button-text">Выбрать</span></button>').click(function () {
                        var treeview = wnd.element.find("#treeview").data("kendoTreeView");
                        var $select = treeview.select();

                        if ($select.length > 0) {
                            wnd.destroy();
                            _params.callback(treeview.dataItem($select));
                        } else {
                            pbaAPI.errorMsg("Необходимо выбрать запись");
                        }
                    }))
                    .append($('<button class="k-button"><span class="k-icon k-cancel"></span></span class="button-text">Отмена</span></button>').click(function () {
                        wnd.close();
                    }))
                    .appendTo(wnd.element);

                wnd.element.append("<div id='treeview' style='height:400px;'></div>");

                wnd.element.find("#treeview").kendoTreeView({
                    template: kendo.template(_params.template)
                });
            }

            var $treeview = wnd.element.find("#treeview");
            var treeview = $treeview.data("kendoTreeView");

            treeview.setDataSource(new kendo.data.HierarchicalDataSource({
                data: items,
                schema: {
                    model: {
                        id: "ID",
                        children: "Items",
                        hasChildren: "HasChildren"
                    }
                }
            }));

            var disable = function (item) {
                if (item.IsAbstract) {
                    treeview.enable($treeview.find("[data-id='" + item.ID + "']"), false);
                }

                for (var i = 0; i < item.Items.length; i++) {
                    disable(item.Items[i]);
                }
            };

            for (var i = 0; i < items.length; i++) {
                disable(items[i]);
            }

            treeview.expand(".k-item");

            wnd.center();
            wnd.open();
        },
        openDetailView: function (mnemonic, params) {

            var config;
            if (mnemonic == 'DuplicateRightView' || mnemonic == 'RightCostView')
                mnemonic = 'Right';
            if (mnemonic == 'Subject' && params.id) {
                
                pbaAPI.proxyclient.corpProp.getActiveSociety({
                    id: params.id
                }).done(
                    function (config) {
                        if (config) {
                           var mn = config.Mnemonic;
                            if (config.Ext && config.Ext.ObjectID && params.id != config.ObjectID)
                                params.id = config.Ext.ObjectID;
                        }
                        pbaAPI.openDv(mn, params, config);
                    });
            }
            else
             application.viewModelConfigs.get(mnemonic).done(
                function (config) { pbaAPI.openDv(mnemonic, params, config); }
                );
        },
        openDv: function (mnemonic, params, config) {
            var _params = $.extend({
                wid: null,
                title: null,
                width: null,
                height: null,
                IsMaximaze: config.DetailView.IsMaximaze || Math.min($(window).width(), $(window).height()) < 768,
                id: 0,
                ids: null,
                entity: null,
                entities: null,
                toSave: null,
                createDefault: null,
                initProps: {},
                initNewEntity: null,
                beforeSave: null,
                callback: function (e) { },
                isReadOnly: config.IsReadOnly,
                parentForm: null,
                hideToolbar: false,
                customQueryGetParams: null,
                customQuerySaveParams: null,
                link: null,
                onOpen: function (e) { },
                onError: function (e) { },
                isCorrectMnemonic: false,
                changedProperties: {},
                byDate: null
            },
                params);

            var id = _params.id || (_params.entity ? _params.entity.ID || 0 : 0);

            var isNew = id === 0;

            _params.initProps = _params.initProps || {};

            if (isNew && ("MnemonicCategory" in config.Ext)) {
                var categoryMnemonic = config.Ext.MnemonicCategory;

                if (categoryMnemonic && !_params.initProps.CategoryID) {
                    if (_params.parentForm && _params.parentForm.getPr("CategoryID") !== 0) {
                        _params.initProps.CategoryID = _params.parentForm.getPr("CategoryID");
                    } else {
                        pbaAPI.openModalDialog(categoryMnemonic,
                            function (res) {
                                if (res && res.length > 0) {
                                    _params.initProps.CategoryID = res[0].ID;
                                    pbaAPI.openDetailView(mnemonic, _params);
                                    return;
                                } else {
                                    _params.callback({
                                        type: "cancel"
                                    });
                                }
                            },
                            {
                                showMaximize: false,
                                title: "Выбор категории",
                                width: 400,
                                height: 500
                            });

                        return false;
                    }
                }
            }

            if (config.Ext && config.Ext.Relations && Object.keys(config.Ext.Relations).length > 0) {
                //TODO: Удаление косм.корабля
                delete config.Ext.Relations.SpaceShip;
                if (isNew) {
                    var relations = [];

                    for (var key in config.Ext.Relations) {
                        if (config.Ext.Relations.hasOwnProperty(key)) {
                            var rconfig = config.Ext.Relations[key];
                            if (!rconfig.IsReadOnly)
                                relations.push(rconfig);
                        }
                    }

                    if (relations.length === 1) {
                        pbaAPI.openDetailView(relations[0].Mnemonic, _params);
                        return false;
                    } else if (relations.length > 1) {
                        pbaAPI.selectSimple(relations,
                        {
                            wid: "_superclass_" + _params.wid,
                            callback: function (res) {
                                pbaAPI.openDetailView(res.Mnemonic, _params);
                            },
                            cancel: function () {
                                _params.callback({
                                    type: "cancel"
                                });
                            }
                        });

                        return false;
                    } else {
                        pbaAPI.errorMsg("Relations is empty");
                        return false;
                    }
                } else {
                    if (!_params.isCorrectMnemonic) {
                        pbaAPI.proxyclient.viewConfig.getExtraId({
                            mnemonic: mnemonic,
                            id: _params.id || _params.entity.ID
                        }).done(function (result) {
                            if (result.error) {
                                pbaAPI.errorMsg(result.error);
                                _params.onError(result.error);
                            } else {
                                _params.isCorrectMnemonic = true;
                                pbaAPI.openDetailView(result.Mnemonic, _params);
                            }
                        });

                        return false;
                    }
                }
            }

            if (isNew && !pbaAPI.isEmpty(_params.initProps)) {
                var initNewEntity = _params.initNewEntity;

                _params.initNewEntity = function (model) {
                    for (var pr in _params.initProps) {
                        if (_params.initProps.hasOwnProperty(pr)) {
                            model[pr] = _params.initProps[pr];
                        }
                    }

                    if (initNewEntity)
                        initNewEntity(model);
                };
            }

            var wizardName = config.DetailView.WizardName;

            if (isNew && wizardName) {
                pbaAPI.openWizardViewModelEx(wizardName, _params);
            } else {
                _params.wid = pbaAPI.wndID(_params.wid, mnemonic);

                // eqeqeq!!!!
                if (_params.toSave == null)
                    _params.toSave = _params.entity == null;

                var wnd = pbaAPI.initViewModel(config, _params).data("kendoWindow");

                wnd.unbind("close");
                wnd.bind("close",
                    function (e) {
                        var $dialog = wnd.element.find(".dialog-vm");
                        var dialog = $dialog.data("dialogVM");

                        if (wnd.__close) {
                            dialog.destroy();
                            dialog.element().hide();
                            return;
                        } else {
                            e.preventDefault();
                        }

                        var callback = function () {
                            if (dialog.changeObjects.length > 0) {
                                _params.callback({
                                    type: "save",
                                    model: dialog.getCurrentModel(),
                                    changeObjects: dialog.changeObjects
                                });
                            } else {
                                _params.callback({
                                    type: "cancel",
                                    model: dialog.getCurrentModel(),
                                    changeObjects: dialog.changeObjects
                                });
                            }
                        };

                        //if (dialog.getViewModel().dirty) {
                        //    pbaAPI.confirmEx({
                        //        title: "Редактирование",
                        //        text: "Вы не сохранили внесенные изменения. Сохранить и закрыть или выйти без сохранения?",
                        //        titleYes: "Сохранить",
                        //        titleNo: "Выйти без сохранения",
                        //        callbackYes: function () {
                        //            $.when(dialog.save()).done(function () {
                        //                callback();
                        //                wnd.__close = true;
                        //                wnd.close();
                        //            });
                        //        },
                        //        callbackNo: function () {
                        //            callback();
                        //            wnd.__close = true;
                        //            wnd.close();
                        //        }
                        //    });
                        //    return;
                        //} else {
                        wnd.__close = true;
                        wnd.close();
                        //}

                        callback();
                    });

                wnd.unbind('resize');
                wnd.bind('resize',
                    function (e) {
                        e.sender.element.find('form').each(function (indx, elform) {
                            var $form = $(elform);

                            if ($form.is(':visible')) {
                                $form.data('pbaForm').onResize(e.sender);
                            }
                        });
                    });

                var dialogID = wnd.element.find('#DialogID').val();

                var entities = {};

                if (_params.id) {
                    entities[_params.id] = { model: null, order: 0 };
                } else if (_params.entity) {
                    if (!('ID' in _params.entity))
                        _params.entity.ID = _params.entity.uid || pbaAPI.guid();

                    entities[_params.entity.ID] = { model: _params.entity, order: 0 };
                }

                if ($.isArray(_params.ids)) {
                    for (i = 0; i < _params.ids.length; i++) {
                        var id = _params.ids[i];

                        if (id && id !== 0) {
                            entities[id] = { model: null, order: i };
                        }
                    }
                } else if (_params.entities) {
                    for (var i = 0; i < _params.entities.length; i++) {

                        var entity = _params.entities[i];

                        if (!('ID' in entity))
                            entity.ID = entity.uid || pbaAPI.guid();

                        entities[entity.ID] = { model: entity, order: i };
                    }
                }

                var dialogParams = {
                    wnd: wnd,
                    currentID: _params.entity ? _params.entity.ID : _params.id,
                    entities: entities,
                    parentForm: _params.parentForm,
                    isReadOnly: _params.isReadOnly,
                    toSave: _params.toSave,
                    hideToolbar: _params.hideToolbar,
                    buttons: _params.buttons,
                    link: _params.link,
                    customQueryParams: {
                        get: _params.customQueryGetParams,
                        save: _params.customQuerySaveParams
                    },
                    changeProperties: _params.changedProperties,
                    createDefault: _params.createDefault,
                    events: {
                        initNewEntity: _params.initNewEntity,
                        beforeSave: _params.beforeSave,
                        save: function (e) {
                            var model = e.sender.changeObjects[e.sender.changeObjects.length - 1];

                            if (_params.callback)
                                _params.callback({
                                    type: "save",
                                    model: model,
                                    changeObjects: e.sender.changeObjects
                                });

                            e.sender.destroy();
                            wnd.unbind("close");
                            wnd.close();
                        }
                    },
                    byDate: _params.byDate
                };
                if (dialogID) {
                    window[dialogID].initDialog(dialogParams);
                } else {
                    wnd.bind('refresh',
                        function (e) {
                            var wnd = e.sender;

                            var $dialogID = wnd.element.find('#DialogID');

                            dialogID = $dialogID.val();

                            window[dialogID].initDialog(dialogParams);
                        });
                }

                wnd.unbind('activate');
                wnd.bind('activate',
                    function (e) {
                        _params.onOpen();
                    });

                wnd.center();
                wnd.pin();
                wnd.open();
                wnd.__close = false;

                if (_params.IsMaximaze)
                    wnd.maximize();

                return wnd.element;
            }

            return false;

        },

        initWizardViewModel: function (config, params) {
            var _params = $.extend({
                wid: null,
                title: null,
                width: null,
                height: null,
                initProps: {}
            }, params);

            var mnemonic = config.Mnemonic;

            _params.wid = pbaAPI.wndID(_params.wid, mnemonic);

            var $w = $('#' + _params.wid);

            if (!$w.length) {
                $('body').append('<div id="' + _params.wid + '" class="view-model-window wnd-loading-content"></div>');

                $w = $('#' + _params.wid);

                $w.kendoWindow({
                    width: _params.width || config.DetailView.Width || application.getDefaultModalWidth(),
                    height: _params.height || config.DetailView.Height || application.getDefaultModalHeight(),
                    title: _params.title || config.DetailView.Title || config.Title,
                    actions: ['Maximize', 'Close'],
                    content: application.url.GetWizard('GetViewModel',
                        {
                            mnemonic: mnemonic,
                            typeDialog: 'Modal'
                            // : ""
                        }),
                    modal: true,
                    visible: false
                });
            }

            return $w;
        },

        openWizardViewModelEx: function (mnemonic, params) {
            mnemonic = mnemonic.split(',')[0].trim();

            application.viewModelConfigs.get(mnemonic).done(
                function (config) {

                    var _params = $.extend({
                        wid: null,
                        title: null,
                        width: null,
                        height: null,
                        isMaximaze: config.DetailView.IsMaximaze,
                        id: 0,
                        ids: null,
                        entity: null,
                        entities: null,
                        toSave: null,
                        initNewEntity: null,
                        nextStep: null,
                        onNextStep: null,
                        beforeSave: null,
                        callback: function (e) { },
                        isReadOnly: false,
                        parentForm: null,
                        hideToolbar: false,
                        customQueryGetParams: null,
                        customQuerySaveParams: null,
                        initProps: {}
                    },
                        params);

                    _params.wid = pbaAPI.wndID(_params.wid, mnemonic);

                    if (_params.toSave == null)
                        _params.toSave = _params.entity == null;

                    var wnd = pbaAPI.initWizardViewModel(config, _params).data('kendoWindow');

                    wnd.bind('close',
                        function (e) {
                            var wnd = e.sender;

                            var $dialogID = wnd.element.find('#DialogID');

                            var dialogID = $dialogID.val();

                            var dialog = window[dialogID];

                            dialog.element().hide();

                            if (dialog.changeObjects.length > 0) {
                                _params.callback({
                                    type: 'save',
                                    model: dialog.changeObjects[dialog.changeObjects.length - 1],
                                    changeObjects: dialog.changeObjects
                                });

                                dialog.destroy();
                            }
                        });

                    wnd.bind('resize',
                        function (e) {
                            e.sender.element.find('form').each(function (indx, elform) {
                                var $form = $(elform);

                                if ($form.is(':visible')) {
                                    $form.data('pbaForm').onResize(e.sender);
                                }
                            });
                        });

                    var dialogID = wnd.element.find('#DialogID').val();

                    var entities = {};


                    if (_params.id) {
                        entities[_params.id] = { model: null, order: 0 };
                    } else if (_params.entity) {
                        if (!('ID' in _params.entity))
                            _params.entity.ID = _params.entity.uid || pbaAPI.guid();

                        entities[_params.entity.ID] = { model: _params.entity, order: 0 };
                    }

                    if ($.isArray(_params.ids)) {
                        for (var i = 0; i < _params.ids.length; i++) {
                            var id = _params.ids[i];

                            if (id && id !== 0) {
                                entities[id] = { model: null, order: i };
                            }
                        }
                    } else if (_params.entities) {
                        for (var i = 0; i < _params.entities.length; i++) {
                            var entity = _params.entities[i];

                            if (!('ID' in entity))
                                entity.ID = entity.uid || pbaAPI.guid();

                            entities[entity.ID] = { model: entity, order: i };
                        }
                    }

                    var dialogParams = {
                        wnd: wnd,
                        currentID: _params.entity ? _params.entity.ID : _params.id,
                        entities: entities,
                        parentForm: _params.parentForm,
                        isReadOnly: _params.isReadOnly,
                        toSave: _params.toSave,
                        hideToolbar: _params.hideToolbar,
                        customQueryParams: {
                            get: _params.customQueryGetParams,
                            save: _params.customQuerySaveParams
                        },
                        events: {
                            initNewEntity: _params.initNewEntity,
                            nextStep: function (e) {
                                if (_params.nextStep) {
                                    _params.nextStep(e);
                                } else {

                                }
                            },
                            onNextStep: function (e) {
                                if (_params.onNextStep) {
                                    _params.onNextStep(e);
                                } else {

                                }
                            },
                            beforeSave: _params.beforeSave,
                            save: function (e) {
                                if (_params.callback)
                                    _params.callback({
                                        type: 'save',
                                        model: e.sender.changeObjects[e.sender.changeObjects.length - 1],
                                        changeObjects: e.sender.changeObjects
                                    });

                                e.sender.destroy();
                                wnd.close();
                            }
                        }
                    };

                    if (dialogID) {
                        window[dialogID].initDialog(dialogParams); //init dialog
                    } else {
                        wnd.bind('refresh',
                            function (e) {
                                var wnd = e.sender;

                                var $dialogID = wnd.element.find('#DialogID');

                                dialogID = $dialogID.val();

                                window[dialogID].initDialog(dialogParams); //init dialog
                            });
                    }

                    wnd.center();
                    wnd.open();

                    if (_params.isMaximaze)
                        wnd.maximize();

                    return wnd.element;
                });
        },

        getFilePreviewHtml: function (file, w, h, defImage, visibleName) {
            if (!file) {
                return '<a href="javascript:void(0);" title="Скачать" onclick="pbaAPI.errorMsg("Файл недоступен")" ontouchstart="pbaAPI.errorMsg("Файл недоступен")">' +
                            '<span class="file-icon default-file"></span>' +
                        '</a>';
            }

            var href = pbaAPI.getHrefFile(file.FileID);
            var ext = pbaAPI.extensionClass(file.FileName);
            var fileType = pbaAPI.fileType(file.FileName);
            var filename = (visibleName ? file.FileName : "");
            w = w || 32;
            h = h || 32;

            if (!fileType) {
                // unknown filetype file icon
                return '<a title="Скачать" href="' + href + '" target="_blank">' +
                            '<span class="file-icon default-file"></span>' + filename +
                        '</a>';
            } else if (fileType === 'image') {
                // image icon
                return '<a class="imageModal" title="Открыть изображение" data-title="' + (file.Title || file.FileName) + '" href="javascript: void(0);" data-id="' + file.FileID + '" data-key="' + file.Key + '">' +
                            '<img class="file-icon" src="' + pbaAPI.imageHelpers.getsrc(file.FileID, w, h, defImage) + '" width="' + w + '" height="' + h + '" alt=""/>' +
                        '</a>';
            } else {
                // other KNOWN filetype file icon
                return '<a title="' + (ext.indexOf('docx') === -1 ? 'Скачать' : 'Просмотр') + '" href="' + href + '" target="_blank">' +
                            '<span class="file-icon ' + ext + '"></span>' + filename +
                        '</a>';
            }
        },
        showImage: function (id, title) {
            if (!id) return;

            var $modal = $('<div class="text-center">' +
                                '<img src="" alt="" />' +
                                '<div>' +
                                    '<a class="btn k-button btn-default close_button" ' +
                                       'href="javascript: void(0);"' +
                                    '>' +
                                        '<i class="fa fa-close"></i>&nbsp;Закрыть' +
                                    '</a>' +
                                    '<a class="btn k-button btn-default download_button" ' +
                                       'target="_blank" ' +
                                       'href="javascript: void(0);"' +
                                    '>' +
                                       '<i class="fa fa-download"></i>&nbsp;Скачать' +
                                    '</a>' +
                                    '<div style="clear:both"></div>' +
                                '</div>' +
                            '</div>').appendTo('body');
            var maxWidth = window.innerWidth * 0.8;
            var maxHeight = window.innerHeight * 0.8;
            var fileHref = application.url.GetFiles('GetImage', {
                id: id
            });
            var fullSizeHref = application.url.GetFiles('GetImageThumbnail', {
                id: id,
                size: "XXL"
            });
            var btnDownload = $modal.find('.download_button');
            var btnClose = $modal.find('.close_button');

            // setup widget
            $modal.kendoWindow({
                title: title || '',
                modal: true,
                resizable: false,
                visible: false,
                deactivate: function () {
                    this.destroy();
                }
            });

            // preload image
            var image = new Image();
            image.onload = function () {
                var wnd = $modal.getKendoWindow(),
                    img = $modal.find('img')[0],
                    width,
                    height;

                // to keep image proportions
                if (image.width <= maxWidth && image.height <= maxHeight) {
                    width = image.width;
                    height = image.height;
                } else {
                    width = maxWidth;
                    height = width / image.width * image.height;
                    if (height > maxHeight) {
                        height = maxHeight;
                        width = height / image.height * image.width;
                    }
                }

                // setup image attributes
                img.width = width;
                img.height = height;
                img.src = image.src;

                // buttons styling and binding
                btnDownload.add(btnClose).css({
                    display: 'block',
                    width: 100,
                    margin: '0 5px',
                    float: 'right'
                }).parent().css('margin-top', 8);

                btnDownload.attr('href', fullSizeHref);

                btnClose.click(function () {
                    wnd.close();
                });

                // show modal
                wnd.center();
                wnd.open();
            };

            image.src = fileHref;
        },
        showDoc: function (fileid, title) {
            if (fileid) {

                var $modal = $('<div >').appendTo('body');

                $modal.kendoWindow({
                    title: title || '',
                    modal: true,
                    resizable: false,
                    visible: false,
                    activate: function () {
                    },
                    deactivate: function () {
                        this.destroy();
                    }
                });

                $.get('/FileData/ShowDoc/' + fileid, function (data) {
                    $modal.html(data);

                    $modal.getKendoWindow()
                        .maximize()
                        .center()
                        .open();
                });

            }
        },
        openWorkflowTimelineModal: function (objectType, objectID, workflowID, showCurrentStages) {
            var kendoWindow = $('<div />').kendoWindow({
                width: $(window).width(),
                height: $(window).height(),
                title: 'История движения объекта',
                content: "/BusinessProcesses/TimeLine?objectType=" + objectType + "&objectid=" + objectID + "&implID=" + workflowID + "&showcurrentstages=" + showCurrentStages,
                resizable: false,
                maximize: true,
                actions: ['Close'],
                modal: true,
                deactivate: function () {
                    this.destroy();
                },
            });

            var wnd = kendoWindow.data('kendoWindow');
            kendoWindow.addClass('overflowscroll');
            wnd.center().open().maximize();
        },

        showPresentation: function (fileid) {
            pbaAPI.infoMsg('Не реализовано');
        },

        getGridBooleanColumn: function (val) {
            if (val == null) {
                return "<span></span>";
            }
            if (val === true) {
                return "<span class='k-icon icon-yes'></span>";
            }
            if (val === false) {
				return "<span class='k-icon fa fa-minus'></span>";
            }
        },

        base64ToBlob: function (base64, mimetype, slicesize) {
            if (!window.atob || !window.Uint8Array) {
                // The current browser doesn't have the atob function. Cannot continue
                return null;
            }
            mimetype = mimetype || '';
            slicesize = slicesize || 512;
            var bytechars = atob(base64);
            var bytearrays = [];
            for (var offset = 0; offset < bytechars.length; offset += slicesize) {
                var slice = bytechars.slice(offset, offset + slicesize);
                var bytenums = new Array(slice.length);
                for (var i = 0; i < slice.length; i++) {
                    bytenums[i] = slice.charCodeAt(i);
                }
                var bytearray = new Uint8Array(bytenums);
                bytearrays[bytearrays.length] = bytearray;
            }
            return new Blob(bytearrays, { type: mimetype });
        },

        download: function (data, strFileName, strMimeType) {

            var self = window, // this script is only for browsers anyway...
                u = "application/octet-stream", // this default mime also triggers iframe downloads
                m = strMimeType || u,
                x = data,
                D = document,
                a = D.createElement("a"),
                z = function (a) { return String(a); },


                B = self.Blob || self.MozBlob || self.WebKitBlob || z,
                BB = self.MSBlobBuilder || self.WebKitBlobBuilder || self.BlobBuilder,
                fn = strFileName || "download",
                blob,
                b,
                ua,
                fr;

            //if(typeof B.bind === 'function' ){ B=B.bind(self); }

            if (String(this) === "true") { //reverse arguments, allowing download.bind(true, "text/xml", "export.xml") to act as a callback
                x = [x, m];
                m = x[0];
                x = x[1];
            }



            //go ahead and download dataURLs right away
            if (String(x).match(/^data\:[\w+\-]+\/[\w+\-]+[,;]/)) {
                return navigator.msSaveBlob ?  // IE10 can't do a[download], only Blobs:
                    navigator.msSaveBlob(d2b(x), fn) :
                    saver(x); // everyone else can save dataURLs un-processed
            }//end if dataURL passed?

            try {

                blob = x instanceof B ?
                    x :
                    new B([x], { type: m });
            } catch (y) {
                if (BB) {
                    b = new BB();
                    b.append([x]);
                    blob = b.getBlob(m); // the blob
                }

            }



            function d2b(u) {
                var p = u.split(/[:;,]/),
                t = p[1],
                dec = p[2] == "base64" ? atob : decodeURIComponent,
                bin = dec(p.pop()),
                mx = bin.length,
                i = 0,
                uia = new Uint8Array(mx);

                for (i; i < mx; ++i) uia[i] = bin.charCodeAt(i);

                return new B([uia], { type: t });
            }

            function saver(url, winMode) {


                if ('download' in a) { //html5 A[download]
                    a.href = url;
                    a.setAttribute("download", fn);
                    a.innerHTML = "downloading...";
                    D.body.appendChild(a);
                    setTimeout(function () {
                        a.click();
                        D.body.removeChild(a);
                        if (winMode === true) { setTimeout(function () { self.URL.revokeObjectURL(a.href); }, 250); }
                    }, 66);
                    return true;
                }

                //do iframe dataURL download (old ch+FF):
                var f = D.createElement("iframe");
                D.body.appendChild(f);
                if (!winMode) { // force a mime that will download:
                    url = "data:" + url.replace(/^data:([\w\/\-\+]+)/, u);
                }


                f.src = url;
                setTimeout(function () { D.body.removeChild(f); }, 333);

            }//end saver


            if (navigator.msSaveBlob) { // IE10+ : (has Blob, but not a[download] or URL)
                return navigator.msSaveBlob(blob, fn);
            }

            if (self.URL) { // simple fast and modern way using Blob and URL:
                saver(self.URL.createObjectURL(blob), true);
            } else {
                // handle non-Blob()+non-URL browsers:
                if (typeof blob === "string" || blob.constructor === z) {
                    try {
                        return saver("data:" + m + ";base64," + self.btoa(blob));
                    } catch (y) {
                        return saver("data:" + m + "," + encodeURIComponent(blob));
                    }
                }

                // Blob but not URL:
                fr = new FileReader();
                fr.onload = function (e) {
                    saver(this.result);
                };
                fr.readAsDataURL(blob);
            }
            return true;
        }, /* end download() */

        // PRESETS
        getPreset: function (presetType, presetOwnerName, callback) {
            callback = callback || function () { };


            pbaAPI.proxyclient.preset.get({
                ownerName: presetOwnerName,
                preset: presetType
            }).done(function (res) {
                if (!res || res.error) {
                    pbaAPI.errorMsg("Ошибка загрузки пресета" + (res && res.error ? ": " + res.error : ""));
                    return;
                }

                callback(res);
            }).fail(function (err) {
                pbaAPI.errorMsg("Ошибка загрузки пресета" + (err && err.message ? ": " + err.message : ""));
                callback(null);
            });
        },
        savePreset: function (presetType, preset, callback) {
            callback = callback || function () { };

            pbaAPI.proxyclient.preset.save({
                preset: presetType
            },
                preset
            ).done(function (res) {
                if (!res || res.error) {
                    pbaAPI.errorMsg("Ошибка сохранения пресета" + (res && res.error ? ": " + res.error : ""));
                    return;
                }
                callback();
            });
        },
        editPreset: function (presetType, presetOwnerName, callback) {
            pbaAPI.getPreset(presetType, presetOwnerName, function (preset) {
                pbaAPI.openDetailView(presetType, {
                    isMaximaze: false,
                    entity: preset,
                    toSave: false,
                    callback: function (e) {
                        if (e && e.type === "save" && e.model) {
                            pbaAPI.savePreset(presetType, e.model, callback);
                        }
                    }
                });
            });
        },
        toObj: function (config, src) {
            var dest = {};

            if (src) {
                dest.ID = src.ID;
                dest[config.LookupProperty.Text] = src[config.LookupProperty.Text];

                if (config.LookupProperty.Image) {
                    dest[config.LookupProperty.Image.ID] = src[config.LookupProperty.Image.ID];
                    dest[config.LookupProperty.Image.FileID] = src[config.LookupProperty.Image.FileID];
                }

                if (config.LookupProperty.Icon) {
                    dest[config.LookupProperty.Icon.Value] = src[config.LookupProperty.Icon.Value];
                    dest[config.LookupProperty.Icon.Color] = src[config.LookupProperty.Icon.Color];
                }
            }
            return dest;
        },
        getNewUrl: function (url) {
            var AllUrl = window.location.href,
                SplitUrl = AllUrl.split("/"),
                BaseUrl = SplitUrl[0] + "//" + SplitUrl[2];
            return BaseUrl + (url ? url : '');
        },
        getNormalKendoDate: function (inputDate) {
            var da = inputDate.getDate() < 10 ? "0" + inputDate.getDate() : inputDate.getDate();
            var mo = inputDate.getMonth() < 9 ? "0" + (inputDate.getMonth() + 1) : (inputDate.getMonth() + 1);
            var ye = inputDate.getFullYear();
            var ho = inputDate.getHours() < 10 ? "0" + inputDate.getHours() : inputDate.getHours();
            var mi = inputDate.getMinutes() < 10 ? "0" + inputDate.getMinutes() : inputDate.getMinutes();
            return da + "." + mo + "." + ye + " " + ho + ":" + mi;
        },
        _$createModal: function (template,
            model,
            windowSaveTag,
            windowCloseTag,
            kinput,
            fieldNameTag,
            dataItem) {
            var mnemonicTag = "mnemonic";
            var selectDialogTag = "selectDialog";
            var clearTag = "clearInput";
            var customTemplateRows = "";
            var columnById = {}

            for (var i = 0; i < template.Columns.length; i++) {
                var $dialogEditor = $("");
                var $editor = $("");
                var column = template.Columns[i];
                switch (column.Mnemonic) {
                    case "String":
                        $dialogEditor = $("<div> <input class =\"" + kinput + " width:100%\"/> </div>");
                        $editor = $dialogEditor.find("." + kinput);
                        if (column.ColumnConstrains.HasColumnItems) {
                            $editor.addClass("dropDownList");
                            $editor.attr({ columnId: column.ID });
                        } else {
                            $editor.addClass("k-textbox");
                            if (column.ColumnConstrains
                                .MinLength) $editor.attr({ minlength: column.ColumnConstrains.MinLength });
                            if (column.ColumnConstrains
                                .MaxLength) $editor.attr({ maxlength: column.ColumnConstrains.MaxLength });
                        }
                        break;
                    case "Integer":
                        $dialogEditor = $("<div> <input class =\"" + kinput + " width:100%\" /> </div>");
                        $editor = $dialogEditor.find("." + kinput);
                        $editor.addClass("integerTextBox");
                        if (column.ColumnConstrains.MaxValue) $editor.attr({ max: column.ColumnConstrains.MaxValue });
                        if (column.ColumnConstrains.MinValue) $editor.attr({ min: column.ColumnConstrains.MinValue });
                        break;
                    case "Decimal":
                        $dialogEditor = $("<div> <input class =\"" +
                            kinput +
                            " width:100%\" style=\"float:right\"/> </div>");
                        $editor = $dialogEditor.find("." + kinput);
                        $editor.addClass("decimalTextBox");
                        if (column.ColumnConstrains.MaxValue) $editor.attr({ max: column.ColumnConstrains.MaxValue });
                        if (column.ColumnConstrains.MinValue) $editor.attr({ min: column.ColumnConstrains.MinValue });
                        break;
                    case "Date":
                        $dialogEditor = $("<div> <input class =\"" + kinput + " width:100%\"/> </div>");
                        $editor = $dialogEditor.find("." + kinput);
                        $editor.addClass("dateTextBox");
                        if (dataItem)
                            $editor.attr({
                                value: new Date(dataItem[column.PropertyName]).toLocaleDateString(kendo.culture().name)
                            });
                        if (column.ColumnConstrains.MinDate) $editor.attr({ min: column.ColumnConstrains.MinDate });
                        if (column.ColumnConstrains.MaxDate) $editor.attr({ max: column.ColumnConstrains.MaxDate });
                        break;
                    case "Boolean":
                        var value;
                        if (dataItem)
                            value = dataItem[column.PropertyName];
                        var trueSelector = (dataItem && value) ? 'selected="selected"' : "";
                        var falseSelector = (dataItem && !value) ? 'selected="selected"' : "";
                        $dialogEditor = $(
                            '<div><select class ="' +
                            kinput +
                            ' k-input">' +
                            '<option ' +
                            trueSelector +
                            ' value="true">Да</option>' +
                            '<option ' +
                            falseSelector +
                            ' value="false">Нет</option>' +
                            '</select></div>');
                        $editor = $dialogEditor.find("." + kinput);
                        $editor.addClass("booleanTextBox");
                        break;
                    default:
                        var $selectDialog = $("<a href=\"#\" class =\"tooltipstered " +
                            selectDialogTag +
                            "\" " +
                            mnemonicTag +
                            " = \"" +
                            column.Mnemonic +
                            "\" " +
                            fieldNameTag +
                            " = '" +
                            column.PropertyName +
                            "' ><i class =\"fa fa-navicon\"></i></a>");
                        var $inputBtns = $("<span class=\"input-group-btn\">" +
                            "<a href=\"#\" title=\"Очистить\"><i class =\"fa fa-close " +
                            clearTag +
                            "\"></i></a>" +
                            "</span>");
                        $inputBtns.prepend($selectDialog);
                        $dialogEditor = $("<div class=\"input-group\">" +
                            "     <div class=\"form-control\">" +
                            "        <div class =\"k-widget k-multiselect k-header\" de=\"input-groupselectable=\"on\" title=\"\" style=\"\">" +
                            "           <div class=\"k-multiselect-wrap k-floatwrap\" deselectable=\"on\">" +
                            "              <ul role=\"listbox\" deselectable=\"on\" class=\"k-reset\"></ul>" +
                            "              <input class =\"" +
                            kinput +
                            " k-input width:100%\"/>" +
                            "              <span deselectable=\"on\" class=\"k-icon k-i-close tooltipstered\" role=\"button\" tabindex=\"-1\"></span><span class=\"k-icon k-loading-hidden\"></span>" +
                            "           </div>" +
                            "           <select multiple=\"multiple\" data-role=\"multiselect\" aria-disabled=\"false\" style=\"display: none;\" class=\"k-valid\">" +
                            "              <option value=\"1\">1</option>" +
                            "           </select>" +
                            "           <span style=\"font-family: Roboto, Helvetica, sans-serif; font-size: 14px; font-stretch: normal; font-style: normal; font-weight: normal; letter-spacing: normal; text-transform: none; line-height: 18.34px; position: absolute; visibility: hidden; top: -3333px; left: -3333px;\">Выберите значение...</span>" +
                            "        </div>" +
                            "     </div>" +
                            "</div>");
                        $dialogEditor.closest("div").append($inputBtns);
                        $editor = $dialogEditor.find("." + kinput);
                        $editor.attr("readonly", true);
                        if (dataItem) {
                            $editor.attr({
                                id: dataItem[column.PropertyName]
                            });
                            $editor.attr({
                                "LinkedRowID": dataItem.LinkedRowID
                            });
                            columnById[i] = { index: i, column: column, row: dataItem }

                            $.ajax({
                                type: "GET",
                                url: pbaAPI.getNewUrl("/Response/GetCellObjectLookupTitleById"),
                                data: {
                                    id: dataItem[column.PropertyName],
                                    mnemonic: column.Mnemonic,
                                    cellIndex: i
                                },
                                contentType: "application/json",
                                success: function (data) {
                                    if (data.error) {
                                        console.log("Не удалось получить имя объекта");
                                        return;
                                    }
                                    var title = data.Data[0].Title;
                                    var cellIndex = undefined;
                                    var cellId = undefined;
                                    this.url.split("?")[1].split("&").forEach(function (s) {
                                        var kv = s.split("=");
                                        if (kv[0] === "cellIndex")
                                            cellIndex = kv[1];
                                        if (kv[0] === "id")
                                            cellId = kv[1];
                                    });
                                    var rowObj = columnById[cellIndex];
                                    var columns = $("div.row input.k-input");
                                    var column = columns[rowObj.index];
                                    column.value = title;
                                    $(column).attr({
                                        "id": cellId
                                    });
                                }
                            });
                        }
                }

                var columnField = {};
                columnField[fieldNameTag] = column.PropertyName;
                $editor.attr(columnField);
                if (dataItem && !$editor.attr("value"))
                    $editor.attr({
                        value: dataItem[column.PropertyName]
                    });

                var required = "";
                if (column.ColumnConstrains && column.ColumnConstrains.Required) {
                    required = "<span class=\"required-mark\">•</span>";
                    $editor.addClass("required");
                }

                var $editorHtml = $dialogEditor.prop('outerHTML');
                customTemplateRows += "<div class='row e-row'>" +
                    "  <div class='col-md-3 d-label'>" +
                    "      <label> " +
                    column.Title +
                    " </label>" +
                    required +
                    "  </div>" +
                    "  <div class='col-md-9 d-editor'>" +
                    $editorHtml +
                    "  </div>" +
                    "</div>";
            };


            var $modal = $("<div style='padding: 0;' class='dialog dialog--modal dialog-vm'>" +
                "    <div class='dialog__toolbar toolbar-vm'>" +
                "        <div class=\"k-toolbar k-widget k-toolbar-resizable\">" +
                "            <div style=\"float: right; visibility: visible;\" class=\"k-button-group\">" +
                "            <a href=\"\" class =\"k-button success k-group-start\" id=\"" +
                windowSaveTag +
                "\">Сохранить</a>" +
                "            <a href=\"\" class =\"k-button primary k-group-end\" id=\"" +
                windowCloseTag +
                "\">Сохранить и закрыть</a>" +
                "            </div>" +
                "        </div>" +
                "    </div>" +
                "    <div class='view-model' style='padding:10px'>" +
                customTemplateRows +
                "</div>" +
                "</div>");

            var $dictTypeInput = $modal.find("." + selectDialogTag);
            $dictTypeInput.click(function () {
                var $input = $(this);
                var mnemonic = $input.attr(mnemonicTag);
                pbaAPI.openModalDialog(mnemonic,
                    function (res) {
                        application.viewModelConfigs.get(mnemonic).done(function (config) {
                            var dest = pbaAPI.toObj(config, res[0]);
                            $input = $input.closest(".input-group").find("." + kinput);
                            $input.attr({
                                value: dest[Object.keys(dest)[1]],
                                id: dest[Object.keys(dest)[0]]
                            });
                        });

                    },
                    {
                        title: application.viewModelConfigs.get(mnemonic).Title,
                        multiSelect: false
                    });

            });
            var $clearInput = $modal.find("." + clearTag);
            $clearInput.click(function () {
                var $input = $(this).closest(".input-group").find("." + kinput);
                $input.val(undefined);
                $input.attr({ value: "" });
            });
            return $modal;
        },
        _setStyle: function () {
            var commonStyle = {
                style: "display: block; width:100%"
            };
            var $stringInput = $(".k-textbox");
            $stringInput.attr(commonStyle);

            var kendoNumericValidatorRule = {
                rules: {
                    myRule: function (input) {
                        if (input.is("[data-role='numerictextbox']")) {
                            var min = parseInt($(input).attr("min"));
                            var max = parseInt($(input).attr("max"));
                            var val = parseInt(input.val());
                            if (val < min || val > max) {
                                return false;
                            }
                            return true;
                        }
                        return true;
                    }
                },
                messages: {
                    myRule: function (input) {
                        var min = $(input).attr("min");
                        var max = $(input).attr("max");
                        var msg = "Запросом ограничен ввод значений в диапазоне от " + min + " до " + max;
                        var customMsg = $(input).attr("data-val-msg");
                        if (customMsg) {
                            return customMsg;
                        } else {
                            return msg;
                        }
                    }
                }
            };

            var $integerInput = $(".integerTextBox");
            $integerInput.removeClass("integerTextBox");
            $integerInput.kendoNumericTextBox({ format: "0: n0" });
            $integerInput.kendoValidator(kendoNumericValidatorRule);
            $integerInput.closest(".k-widget").attr(commonStyle);

            var $decimalInput = $(".decimalTextBox");
            $decimalInput.removeClass("decimalTextBox");
            $decimalInput.kendoNumericTextBox({
                culture: kendo.culture().name,
                decimals: 2,
                step: .01
            });
            $decimalInput.kendoValidator(kendoNumericValidatorRule);
            $decimalInput.closest(".k-widget").attr(commonStyle);

            var $booleanInput = $(".booleanTextBox");
            $booleanInput.removeClass("booleanTextBox");
            $booleanInput.attr(commonStyle);


            var kendoDateTimeValidatorRule = {
                rules: {
                    datepicker: function (input) {
                        if (input.is("[data-role=datetimepicker]")) {
                            return input.data("kendoDateTimePicker").value();
                        } else {
                            return true;
                        }
                    }
                },
                messages: {
                    datepicker: "Введите правильную дату!"
                }
            };
            var $dateInput = $(".dateTextBox");
            $dateInput.removeClass("dateTextBox");
            $dateInput.kendoDatePicker({
                culture: kendo.culture().name
            });

            var $datepicker2 = $dateInput;
            $datepicker2.kendoMaskedTextBox({
                mask: "00/00/0000"
            });
            $datepicker2.closest(".k-datepicker")
                .add($datepicker2)
                .removeClass("k-textbox");
            $datepicker2.kendoValidator(kendoDateTimeValidatorRule);
            $dateInput.closest(".k-widget").parent().closest(".k-widget").attr(commonStyle);

            var $dropdownInput = $(".dropDownList");
            $dropdownInput.attr(commonStyle);
            $dropdownInput.each(function (index) {
                var $obj = $($dropdownInput[index]);
                var columnId = $obj.attr("columnId");
                $obj.kendoDropDownList({
                    serverFiltering: true,
                    dataTextField: "Item",
                    dataValueField: "Item",
                    dataSource: {
                        transport: {
                            read: {
                                type: "post",
                                url: pbaAPI.getNewUrl("/Response/GetRequestColumnItems?columnId=" + columnId)
                            }
                        },
                        schema: {
                            data: "Data"
                        }
                    }
                });
            });
            $dropdownInput.removeClass("dropDownList");
        },
        _getSendData: function (kinput, fieldNameTag) {
            var sentData = {};
            $("." + kinput).each(function () {
                //var $input = $(this).closest(".input-group").find(".k-input");
                var $input = $(this);
                var $requiredItems = [];
                if ($input.attr(fieldNameTag)) {
                    var value = $input.attr("id") || $input.attr("value");
                    var required = $input.hasClass("required");
                    if (required && !value) {
                        var msg = "Не заполнены обязательные поля.";
                        pbaAPI.errorMsg(msg);
                        sentData = undefined;
                        return false;
                    }
                    sentData[$input.attr(fieldNameTag)] = $input.attr("id") || $input.attr("value");
                    if (!sentData["LinkedRowID"] && $input.attr("LinkedRowID"))
                        sentData["LinkedRowID"] = $input.attr("LinkedRowID");
                }
                return true;
            });
            return sentData;
        },
        showCustomCreateWindow: function (template, model, id, callback) {
            var customWindowSaveTag = "CustomWindowSave";
            var customWindowSaveCloseTag = "CustomWindowSaveClose";
            var kinput = "request-row-input";
            var fieldNameTag = "fieldname";

            var $modal = pbaAPI._$createModal(template,
                                              model,
                                              customWindowSaveTag,
                                              customWindowSaveCloseTag,
                                              kinput,
                                              fieldNameTag);

            var customWindowSave = $modal.find("#" + customWindowSaveTag),
                customWindowSaveClose = $modal.find("#" + customWindowSaveCloseTag);

            var kendoWindowName = "kendoWindow";
            var kendoWindow = $modal[kendoWindowName]({
                title: "Создание строки ответа",
                modal: true,
                resizable: true,
                visible: false,
                width: window.innerWidth - 100,
                height: window.innerHeight - 100,
                deactivate: function () {
                    this.destroy();
                },
                open: function () {
                    $(document).ready(function () {
                        pbaAPI._setStyle();
                    });
                }
            });

            kendoWindow.data(kendoWindowName)
                .center().open();

            customWindowSave.click(function () {
                customWindowSaveData();
            });
            customWindowSaveClose.click(function () {
                customWindowSaveData();
                kendoWindow.data(kendoWindowName).close();
            });

            function customWindowSaveData() {
                $(':focus').blur();
                $('input').blur();
                var sentData = pbaAPI._getSendData(kinput, fieldNameTag);
                if (sentData)
                    $.ajax({
                        type: "POST",
                        url: pbaAPI.getNewUrl("/Response/SaveDynamicObject/" + id),
                        data: JSON.stringify(sentData),
                        contentType: "application/json",
                        success: function (data) {
                            //callback();
                            if (data.error) {
                                pbaAPI.errorMsg("Ошибка при сохранении объекта.");
                            } else {
                                var rowid = data.rowId;
                                $("." + kinput).attr("LinkedRowID", rowid);
                                pbaAPI.uploadMsg("Новый объект успешно сохранен.");
                                callback();
                            }
                        }
                    });
            }
        },
        showCustomEditWindow: function (template, model, dataItem, id, callback) {
            var customWindowSaveTag = "CustomWindowSave";
            var customWindowSaveCloseTag = "CustomWindowSaveClose";
            var kinput = "request-row-input";
            var fieldNameTag = "fieldname";

            var $modal = pbaAPI._$createModal(template,
                                              model,
                                              customWindowSaveTag,
                                              customWindowSaveCloseTag,
                                              kinput,
                                              fieldNameTag,
                                              dataItem);

            var customWindowSave = $modal.find("#" + customWindowSaveTag),
                customWindowSaveClose = $modal.find("#" + customWindowSaveCloseTag);

            var kendoWindowName = "kendoWindow";
            var kendoWindow = $modal[kendoWindowName]({
                title: "Изменение строки ответа",
                modal: true,
                resizable: true,
                visible: false,
                width: window.innerWidth - 100,
                height: window.innerHeight - 100,
                deactivate: function () {
                    this.destroy();
                },
                open: function () {
                    $(document).ready(function () {
                        pbaAPI._setStyle();
                    });
                }
            });

            kendoWindow.data(kendoWindowName)
                .center().open();

            customWindowSave.click(function () {
                customWindowSaveData();
            });
            customWindowSaveClose.click(function () {
                customWindowSaveData();
                kendoWindow.data(kendoWindowName).close();
            });

            function customWindowSaveData() {
                $(':focus').blur();
                $('input').blur();
                var sentData = pbaAPI._getSendData(kinput, fieldNameTag);
                if (sentData)
                    $.ajax({
                        type: "POST",
                        url: pbaAPI.getNewUrl("/Response/SaveDynamicObject/" + id),
                        data: JSON.stringify(sentData),
                        contentType: "application/json",
                        success: function (data) {
                            //callback();
                            if (data.error) {
                                pbaAPI.errorMsg("Ошибка при сохранении объекта.");
                            } else {
                                pbaAPI.uploadMsg("Новый объект успешно сохранен.");
                                callback();
                            }
                        }
                    });
            }
        },
        DeleteRow: function (responseId, rowId) {
            return $.ajax({
                type: "DELETE",
                url: pbaAPI.getNewUrl("/Response/DeleteRow") + "?" + $.param({ responseId: responseId, rowId: rowId }),
                contentType: "application/json",
                success: function (data) {
                    if (data.error) {
                        pbaAPI.errorMsg("Ошибка при сохранении объекта.");
                    } else {
                        pbaAPI.uploadMsg("Новый объект успешно сохранен.");
                    }
                }
            });
        }
    }
    );
}());
