pbaAPI.registerEditor('QueryBuilderFilter', pbaAPI.Editor.extend({
    init: function ($wrap, propertyName) {
        'use strict';
        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);
        var self = this;
        $wrap.find('#open_btn_id').click(function () {
            //var $win = $(this).closest(".view-model-window");
            //$win.addClass("wnd-loading-content");
            var query = self.pbaForm.getPr(propertyName);
            var mnemonic = self.pbaForm.getPr('Mnemonic');
            pbaAPI.queryFilter.openQueryBuilder(mnemonic, query, function (res) {
                //debugger;
                if (res.query) {
                    $wrap.find('#query_text_field_id').html(res.text);
                    $wrap.find('#query_text_field_id').attr('title', res.query);
                    self.pbaForm.setPr(self.propertyName, res.query);
                    self.pbaForm.setPr('Description', res.text);
                }
                else {
                    $wrap.find('#query_text_field_id').html(res);
                    $wrap.find('#query_text_field_id').attr('title', res);
                    self.pbaForm.setPr(self.propertyName, res);
                }
                
            });
        });
    },
    onAfterBind: function () {
        var self = this;
        var value = self.pbaForm.getPr(self.propertyName);   
        var text = self.pbaForm.getPr('Description');
        self.wrap.find('#query_text_field_id').attr('title', value);
        self.wrap.find('#query_text_field_id').html(text);
    }
}));