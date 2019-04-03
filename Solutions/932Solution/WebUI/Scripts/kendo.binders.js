(function (kendo) {
    'use strict';

    kendo.data.binders.imagedata = kendo.data.Binder.extend({
        refresh: function () {
            //debugger;
            var $img = $(this.element);

            var data = this.bindings["imagedata"].get();

            if (data) {
                var url = $img.data('url') ? $img.data('url') + '/' + data.FileID : data.FileID;
                var width = $img.data('width');
                var height = $img.data('height');

                this.element.src = url + '?width=' + width + '&height=' + height + '&defImage=NoImage&type=Crop';
            } else {
                this.element.src = '/Files/GetImage/null';
            }
        }
    });

})(window.kendo);