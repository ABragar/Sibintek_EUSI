pbaAPI.registerEditor('ManyToManyAssociation_InLine',
    pbaAPI.Editor.extend({
        _mnemonic: null,
        _parentMnemonic: null,
        _sysname: null,
        _id: null,
        _loadDialog: false,
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

            editor.wrapDialog.hide();

            if (this._id === 0) {
                $msg.show();
            } else {
                $msg.hide();
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

            var listview = dialog.listView();

            var widget = listview.widget();

            var urlRead = URI.expand('/api/manyToManyAssociation/{mnemonic}/{type}/{categorized}',
                {
                    type: listview.type,
                    mnemonic: listview.mnemonic,
                    categorized: listview.isCategorizedItem ? 'categorized' : null
                }).path();

            dialog.init({
                transport: {
                    read: {
                        url: urlRead,
                        data: {
                            associationParams: params
                        }
                    }
                },
                add: function(e) {
                    return $.ajax('/api/manyToManyAssociation/' + editor._mnemonic + '/addAssociation?' +
                        $.param({
                            associationParams: editor.getOneToManyAssociation(),
                            ids: [e.model.ID]
                        }),
                        {
                            method: 'POST',
                            contentType: 'application/json',
                            dataType: 'json'
                        });
                },
                customCommands: [
                    {
                        id: 'add_link',
                        text: 'Добавить связь',
                        execute: function () {
                            pbaAPI.openModalDialog(editor._mnemonic,
                                function (items) {
                                    if (items.length > 0) {
                                        $.ajax('/api/manyToManyAssociation/' +
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
                                    multiselect: true
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
                            pbaAPI.confirm('Связи',
                                'Удалить связь?',
                                function () {
                                    var items = command.selectItems == 0 ? command.listView.getSelectItems() : command.selectItems;
                                    if (items && items.length > 0) {
                                        $.ajax('/api/manyToManyAssociation/' +
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
                ]
            });

            editor.wrapDialog.show();
            editor.resize();
            editor._lock = false;

        },
        initDialog: function () {
            var editor = this;

            if (!editor._loadDialog || editor._lock) return;

            editor.__initDialog();
        },
        onShown: function () {
            var editor = this;

            if (editor._id === 0) return;

            if (!editor._loadDialog) {
                editor._lock = true;

                var $msg = this.wrap.find('#msg-loading-dialog');

                $msg.removeClass('hidden');

                $.get(application.url.GetView('GetDialog', { mnemonic: editor._mnemonic, typeDialog: 'Modal' }),
                    function (html) {
                        editor.wrapDialog.html(html);
                        editor._loadDialog = true;
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
        }
    }));