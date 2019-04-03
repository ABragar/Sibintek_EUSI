pbaAPI.registerDisplay('OneToManyAssociation_InLine',
    pbaAPI.Editor.extend({
        _mnemonic: null,
        _parentMnemonic: null,
        _sysname: null,
        _controller: null,
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
            this._controller = this.wrap.data('controller');
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

            var controller = this._controller || 'oneToManyAssociation';

            dialog.init({
                transport: {
                    read: {
                        controller: controller,
                        data: {
                            associationParams: params
                        }
                    }
                }
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

                $.get(application.url.GetView('GetDialog', { mnemonic: editor._mnemonic, typeDialog: 'Modal', isReadOnly: true }),
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