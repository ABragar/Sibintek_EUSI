pbaAPI.registerEditor('Icon', pbaAPI.Editor.extend({
    icon: null,

    init: function ($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);

        var editor = this;

        $('#' + this.wrapId + '_toolbar').on('click', '[data-action]', function () {
            var action = $(this).attr('data-action');

            switch (action) {
                case 'select':
                    editor.selectIcon();
                    break;
                case 'clear':
                    editor.clearIcon();
                    break;
            }
        });
    },

    selectIcon: function () {
        'use strict';

        var editor = this;
        var icon = editor.pbaForm.getPr(editor.propertyName);

        var $wnd = $('<div>').addClass('view-model-window wnd-loading-content').kendoWindow({
            width: 1200,
            height: 800,
            content: application.url.Icon('GetIcons'),
            title: 'Выберите иконку',
            actions: ['Maximize', 'Close'],
            modal: true,
            deactivate: function () {
                this.destroy();
            },
            refresh: function () {
                var wnd = this;
                $wnd.removeClass('wnd-loading-content');

                this.element.find('[data-icon]').click(function () {
                    var $this = $(this);
                    var iconClass = $this.attr('class');
                    var iconRgbColor = $this.css('color');
                    var iconHexColor = '#' + editor.rgb2hex(iconRgbColor);

                    if (icon) {
                        icon.set('Value', iconClass);
                        icon.set('Color', iconHexColor);
                    }

                    wnd.close();
                });
            }
        });

        $wnd.getKendoWindow().center().open();
    },
    clearIcon: function () {
        'use strict';

        var editor = this;
        var icon = editor.pbaForm.getPr(editor.propertyName);

        if (icon) {
            icon.set('Value', '');
            icon.set('Color', '');
        }
    },
    rgb2hex: function (rgb) {
        'use strict';

        rgb = Array.apply(null, arguments).join().match(/\d+/g);
        rgb = ((rgb[0] << 16) + (rgb[1] << 8) + (+rgb[2])).toString(16);

        return rgb;
    }
}));
