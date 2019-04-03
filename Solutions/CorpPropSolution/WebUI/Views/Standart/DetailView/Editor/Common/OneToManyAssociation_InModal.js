pbaAPI.registerEditor("OneToManyAssociation_InModal", pbaAPI.Editor.extend({
    _mnemonic: null,
    _parentMnemonic: null,
    _sysname: null,
    _id: null,
    _oid: null,
    _date: null,
    getParam: function () {
        return "parent_mnemonic(" + this._parentMnemonic + ")parent_id(" + this._id + ")property_sysname(" + this._sysname + ")";
    },
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
        "use strict";

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

        var editor = this;

        this._mnemonic = this.wrap.data("mnemonic");
        this._parentMnemonic = this.wrap.data("parent_mnemonic");
        this._sysname = this.wrap.data("sysname");

        var $openDialog = this.wrap.find(".open-dialog");

        var createDefault = function (callback) {
            pbaAPI.proxyclient.standard.create_default(
                {
                    mnemonic: editor._mnemonic,
                    oneToManyAssociation: editor.getOneToManyAssociation()
                })
                .done(function (res) {
                    if (res.error) {
                        pbaAPI.errorMsg(res.message);
                    } else {
                        callback(res);
                    }
                });
        };

        var check = function (callback) {
            if (editor._id === 0) {
                pbaAPI.infoMsg("Необходимо сохранить объект");
                return;
            }

            callback();
        };

        $openDialog.on("click", function () {
            check(function () {
                createDefault(function (res) {
                    pbaAPI.openModalDialog(editor._mnemonic, null, {
                        initProps: res.model,
                        callbackCancel: function () {
                            editor.countRefresh();
                        },
                        urlParameters: [
                            {
                                key: "associationParams",
                                value: editor.getParam()
                            }
                        ]
                    });
                });
            });
        });

        var $add = this.wrap.find(".add");

        $add.on("click", function () {
            check(function () {
                createDefault(function (res) {
                    pbaAPI.openDetailView(editor._mnemonic, {
                        initProps: res.model,
                        callback: function () {
                            editor.countRefresh();
                        }
                    });
                });
            });
        });
    },
    onAfterBind: function () {
        
        this._id = this.pbaForm.getPr('ID');
        this._oid = this.pbaForm.getPr('Oid');
        this._date = this.pbaForm.model.byDate;
        this.countRefresh();
    },
    countRefresh: function () {
        var editor = this;
        var $count = editor.wrap.find(".count");

        $count.html("0");
    }
}));
