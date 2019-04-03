pbaAPI.registerEditor('LinkGroupIcon', pbaAPI.Editor.extend({
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

    /**
     * @override
     */
    onAfterBind: function () {
        this.icon = this.readProperty();
    },

    selectIcon: function () {
        'use strict';
        
        var editor = this;

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
                    var char = window.getComputedStyle($this.get(0), ':before').content.replace(/'|"/g, '');
                    var code = char; //.charCodeAt(0).toString(16);

                    if (editor.icon) {
                        editor.icon.set('Value', iconClass);
                        editor.icon.set('Color', iconHexColor);
                        editor.icon.set('Code', code);
                    }

                    wnd.close();
                });
            }
        });

        $wnd.getKendoWindow().center().open();
    },
    clearIcon: function () {
        'use strict';

        if (this.icon) {
            this.icon.set('Value', '');
            this.icon.set('Color', '');
            this.icon.set('Code', '');
        }
    },
    rgb2hex: function (rgb) {
        'use strict';

        rgb = Array.apply(null, arguments).join().match(/\d+/g);
        rgb = ((rgb[0] << 16) + (rgb[1] << 8) + (+rgb[2])).toString(16);

        return rgb;
    }
}));
