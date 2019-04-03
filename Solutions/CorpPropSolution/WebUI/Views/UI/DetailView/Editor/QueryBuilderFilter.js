pbaAPI.registerEditor('QueryBuilderFilter', pbaAPI.Editor.extend({
    init: function ($wrap, propertyName) {
        'use strict';
        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);
    },
    onAfterBind: function () {

        var that = this;
        var $wrap = this.wrap.find(".querytreebuilder");

        $wrap.empty();

        var $builder = $("<div class='root-querytree-builder'/>");

        $wrap.append($builder);

        var mnemonic = this.pbaForm.getViewData('Mnemonic') || this.pbaForm.getViewData('ObjectType') || this.pbaForm.getPr('Mnemonic') || this.pbaForm.getPr('ObjectType');

        if (!mnemonic && this.pbaForm.parentForm) {
            mnemonic = this.pbaForm.parentForm.getPr('Mnemonic') || this.pbaForm.parentForm.getPr('ObjectType');
        }

        $builder.queryTree({ Mnemonic: mnemonic, IsRoot: true });
        $builder.queryTree("setFilterOid", this.pbaForm.getPr("Oid"));
        var rule = this.pbaForm.getPr(that.propertyName);

        try {
            if (rule) {
                rule = JSON.parse(rule);
                $builder.queryTree("setValue", rule);
            }
        } catch (e) {
            rule = null;
        }

        $builder.on("change",
            function () {

                var rule = $builder.queryTree("getValue");

                if (rule) {
                    rule = JSON.stringify(rule);
                    that.pbaForm.setPr(that.propertyName, rule);
                    var preview = $builder.queryTree("getPreview");
                    that.pbaForm.setPr('Description', preview);
                }
            });
    }
}));