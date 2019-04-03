pbaAPI.registerDisplay('LinkBaseObject', pbaAPI.Editor.extend({
    objId: null,
    objMnemonic: null,
    init: function ($wrap, propertyName) {
        'use strict';

        var editor = this;

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

        var $a = editor.wrap.find('a');

        $a.on('click', function () {
            if (editor.objMnemonic && editor.objId) {
                pbaAPI.openDetailView(editor.objMnemonic,
                {
                    wid: editor.wrapId,
                    id: editor.objId
                });
            }
        });
    },
    onAfterBind: function () {
        var editor = this;

        var $a = editor.wrap.find('a');

        $a.html('');

        editor.objMnemonic = null;
        editor.objId = null;

        var obj = editor.readProperty();

        if (obj && obj.ID) {
            editor.objMnemonic = obj.Mnemonic || obj.TypeName;
            editor.objId = obj.ID;

            application.viewModelConfigs.get(editor.objMnemonic).done(function (config) {
                pbaAPI.proxyclient.crud.get({ mnemonic: config.Mnemonic, id: obj.ID }).done(function (res) {
                    if (res.error) {
                        $a.html('erorr: ' + res.error);
                    } else {
                        $a.html(config.DetailView.Title + ': ' + res.model[config.LookupProperty.Text]);
                    }
                });
            });
        }
    }
}));