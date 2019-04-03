pbaAPI.registerEditor('OneToManyAssociation_InLine',
    pbaAPI.Editor.extend({
        _mnemonic: null,
        _parentMnemonic: null,
        _sysname: null,
        _controller: null,
        _id: null,
        _isDialogLoaded: false,
        _lock: false,
        _singleEditor: false,
        _oid: null,
        _date: null,
        getOneToManyAssociation: function () {
            return {
                Mnemonic: this._parentMnemonic,
                Id: this._id,
                SysName: this._sysname,
                Oid: this._oid,
                Date: this._date
            }
        },
        init: function ($wrap, propertyName) {
            'use strict';
            var editor = this;

            pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

            this._mnemonic = this.wrap.data('mnemonic');
            this._parentMnemonic = this.wrap.data('parent_mnemonic');
            this._sysname = this.wrap.data('sysname');
            this._controller = this.wrap.data('controller');
            this._link = this.wrap.data('link');
            this._freeze = false;

            this.wrapDialog = this.wrap.find('#wrap-list-view-dialog');

            var $singleEditor = this.wrap.closest('.single-editor');

            if ($singleEditor.length !== 0) {
                this._singleEditor = true;
                $singleEditor.resize(function () {
                    editor.resize();
                });
                editor.form.on('onResize',
                    function () {
                        editor.resize();
                    });
            }
        },
        onAfterBind: function () {
            var editor = this;

            this._id = this.pbaForm.getPr('ID');
            this._oid = this.pbaForm.getPr('Oid');
            this._date = this.pbaForm.model.byDate;
            var $msg = this.wrap.find('#msg-no-save');

            var isNew = this.isNew();
            $msg.toggle(isNew);
            if (isNew) {
                editor.wrapDialog.toggle(!isNew);
            } else {
                editor.initDialog();
            }
        },
        __dialog: null,
        __getDialog: function () {
            if (!this.__dialog)
                this.__dialog = this.wrapDialog.find('.dialog-listview').data('dialogListView');

            return this.__dialog;
        },
        __initDialog: function () {
            var editor = this;

            var dialog = editor.__getDialog();

            var params = editor.getOneToManyAssociation();

            var controller = this._controller || 'oneToManyAssociation';
			if (this._link) {
	            var customCmds = [
	                {
	                    id: 'add_link',
	                    text: 'Добавить связь',
	                    execute: function () {
	                        pbaAPI.openModalDialog(editor._mnemonic,
	                            function (items) {
	                                if (items.length > 0) {
	                                    if (editor._mnemonic === "association_PropertyComplexIO_InventoryObject") {
	                                        $.ajax({
	                                            url: '/api/corpProp/addInComplexIO/' + editor._id + '/' + pbaAPI.extract(items, 'ID').join(';'),
	                                            method: 'POST',
	                                            contentType: 'application/json',
	                                            dataType: 'json',
	                                            cache: false
	                                        }).done(function (res) {	
	                                            if (res.error) {
	                                                pbaAPI.errorMsg(res.error);
	                                            } else {
	                                                dialog.refresh();
	                                            }
	                                        }).fail(function () {
	                                            pbaAPI.errorMsg('error');
	                                        });
	                                        return;
	                                    }
	
	                                    $.ajax('/api/oneToManyAssociation/' +
	                                        editor._mnemonic +
	                                        '/addAssociation?' +
	                                        $.param({
	                                            associationParams: editor.getOneToManyAssociation(),
	                                            ids: pbaAPI.extract(items, 'ID')
	                                        }),
	                                        {
	                                            method: 'POST',
	                                            contentType: 'application/json',
	                                            dataType: 'json'
	                                        }).done(function (res) {
	                                            if (res.error) {
	                                                pbaAPI.errorMsg(res.error);
	                                            } else {
	                                                dialog.refresh();
	                                            }
	                                        }).fail(function () {
	                                            pbaAPI.errorMsg('error');
	                                        });
	                                }
	                            },
	                            {
	                                title: 'ВЫБОР...',
	                                multiselect: true,
									transport: {
                                        read: {
                                            controller: 'oneToManyAssociation',
                                            data: {
                                                associationParams: $.extend({ SelectionDialog: true }, params)
                                            }
                                        }
                                    }
	                            });
	                    }
	                },
	                {
	                    id: 'remove_link',
	                    text: 'Удалить связь',
	                    canExecute: function (e) {
	                        return e.selectItems.length > 0;
	                    },
	                    execute: function () {
	                        var command = this;
	                        pbaAPI.confirm('Ассоциации',
	                            'Удалить связь?',
	                            function () {
	                                var items = command.selectItems == 0 ? command.listView.getSelectItems() : command.selectItems;
	                                if (items && items.length > 0) {
	                                    $.ajax('/api/oneToManyAssociation/' +
	                                        editor._mnemonic +
	                                        '/deleteAssociation?' +
	                                        $.param({
	                                            associationParams: editor.getOneToManyAssociation(),
	                                            ids: pbaAPI.extract(items, 'ID')
	                                        }),
	                                        {
	                                            method: 'POST',
	                                            contentType: 'application/json',
	                                            dataType: 'json'
	                                        }).done(function (res) {	
	                                            if (res.error) {
	                                                pbaAPI.errorMsg(res.error);
	                                            } else {
	                                                dialog.refresh();
	                                            }
	                                        }).fail(function () {
	                                            pbaAPI.errorMsg('error');
	                                        });
	                                }
	                            });
	                    }
	                }
	            ];
			}

            if (editor._mnemonic === "association_ScheduleStateRegistration_ScheduleStateRegistrationRecord")
                customCmds.push({
                    id: "addAccountingObject",
                    text: "",
                    execute: function () {
                        var command = this;
                        var parentWidget = dialog.element().closest('form').closest('.form-widget');

                        if (parentWidget.length > 0 && parentWidget[0].id !== dialog.element()[0].id) {
                            var win = window[parentWidget[0].id];
                            var parentMnemonic = win.composite.mnemonic;
                            var widget = win.widget();
                            var model = widget.getModel();
                            pbaAPI.openModalDialog("AccountingObject", function (items) {
                                if (items.length === 0)
                                    return pbaAPI.errorMsg("Выберите элемент.");

                                var listIds = Array();

                                for (var i = 0; i < items.length; i++) {
                                    listIds.push(items[i].id);
                                }

                                if (parentMnemonic === "ScheduleStateRegistration") {
                                    pbaAPI.proxyclient.corpProp.createScheduleStateRegistrationRecords({
                                        itemsIds: listIds.join(),
                                        ssrId: model.ID
                                    }).done(function (res) {
                                        if (res.error && res.error === 1)
                                            return pbaAPI.alertError(res.message);
                                        else
                                            return pbaAPI.alertSuccess(res.message);
                                    })
                                }
                                else if (parentMnemonic === "ScheduleStateTerminate") {
                                    pbaAPI.proxyclient.corpProp.createScheduleStateTerminateRecords({
                                        itemsIds: listIds.join(),
                                        sstId: model.ID
                                    }).done(function (res) {
                                        if (res.error && res.error === 1)
                                            return pbaAPI.alertError(res.message);
                                        else
                                            return pbaAPI.alertSuccess(res.message);
                                    });
                                }
                            },
                                {
                                    dialogData: {
                                        buttonName: "Добавить"
                                    }
                                })
                        }
                    }
                });

            dialog.init({
                transport: {
                    read: {
                        controller: controller,
                        data: {
                            associationParams: params
                        }
                    },
                    createDefault: {
                        controller: controller,
                        data: {
                            associationParams: params
                        }
                    }
                },
                customCommands: customCmds
            });

            editor.wrapDialog.show();
            editor.resize();
            editor._lock = false;

        },
        initDialog: function () {
            var editor = this;

            if (editor._isDialogLoaded && !editor._lock) {
                editor.__initDialog();
            }
            editor.wrapDialog.toggle(editor._isDialogLoaded);
        },
        onShown: function () {
            var editor = this;

            if (editor.isNew()) return;

            if (!editor._isDialogLoaded) {
                editor._lock = true;

                var $msg = this.wrap.find('#msg-loading-dialog');

                $msg.removeClass('hidden');

                $.get(application.url.GetView('GetDialog', { mnemonic: editor._mnemonic, typeDialog: 'Modal' }),
                    function (html) {
                        editor.wrapDialog.html(html);
                        editor._isDialogLoaded = true;
                        $msg.addClass('hidden');
                        editor.__initDialog();
                    });

            } else {
                var dialog = editor.__getDialog();
                dialog.refresh();
                editor.resize();
            }
        },
        resize: function () {
            var editor = this;

            if (editor._freeze) return;
            editor._freeze = true;

            var $dialog = editor.wrap.find('#wrap-list-view-dialog');
            var dialog = $dialog.find('.dialog-listview').data('dialogListView');

            if (dialog) {
                if (this._singleEditor) {
                    var $singleEditor = editor.wrap.closest('.single-editor');

                    var h = $singleEditor.height();

                    dialog.resize(h > 500 ? h : 500);
                } else {
                    dialog.resize(500);
                }
            }

            editor._freeze = false;
        },
        isNew: function () {
            return !this._id;
        }
    }));