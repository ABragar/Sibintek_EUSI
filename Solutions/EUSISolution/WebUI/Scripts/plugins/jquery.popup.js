(function() {
    'use strict';

    var supportedSides = {
        'left': true,
        'right': true,
        'top': true,
        'bottom': true
    };

    var tooltips = new Map();
    var dropdowns = new Map();

    $(function() {
        $(document.body).on('mouseenter', '[title]', function() {
            initTooltip(this);
        });

        $(document.body).on('click', '[data-dropdown]', function() {
            initDropdown(this);
        });
    });

    function initTooltip(element) {
        var $element = $(element);
        var title = $element.attr('title');
        var side = $element.attr('data-popup');
        var tooltipster = tooltips.get(element);

        if (tooltipster) {
            if (!title) {
                tooltipster.destroy();
                tooltips.delete(element);
            } else if (tooltipster.content() !== title) {
                tooltipster.content(title);
                $element.removeAttr('title');
            }
            
            return;
        }

        if (!title) {
            return;
        }

        var opts = {
            arrow: false,
            delay: [100, 0],
            maxWidth: 250,
            multiple: true,
            theme: 'popup-tooltip'
        };

        if (supportedSides[side]) {
            opts.side = side;
        }

        $element.tooltipster(opts);

        var instances = $.tooltipster.instances($element);
        tooltipster = instances[instances.length - 1];

        tooltipster.open();

        tooltips.set(element, tooltipster);
    }

    function initDropdown(element) {
        var $element = $(element);
        var side = $element.attr('data-popup');
        var selector = $element.attr('data-dropdown');
        var $menu = $(selector).eq(0);

        var tooltipster = dropdowns.get(element);
        
        if (tooltipster || !$menu.length) {
            return;
        }

        var opts = {
            arrow: false,
            content: $menu,
            interactive: true,
            multiple: true,
            theme: 'popup-dropdown',
            trigger: 'click'
        };

        if (supportedSides[side]) {
            opts.side = side;
        }

        $element.tooltipster(opts);

        var instances = $.tooltipster.instances($element);
        tooltipster = instances[instances.length - 1];

        tooltipster.show();

        dropdowns.set(element, tooltipster);

        // NOTE: tooltipster.close.bind(tooltipster) working incorrectly!
        $menu.find('a, button').click(function() {
            tooltipster.close();
        });
    }
}());
