(function() {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var layout = window.layout;
    var sidebar = layout.sidebar;

    // ##############
    // MODULE EXPORTS
    // ##############

    var toolbar = layout.toolbar = {
        _element: $(),
        _routeElement: $(),
        _routeTemplate: kendo.template("<span style='margin-right: 10px;'>/</span><i class='#: icon #'></i>&nbsp;<span>#: caption #</span>"),
        // ----------------
        get element() {
            return toolbar._element.length ? toolbar._element : (toolbar._element = $('#layout-toolbar'));
        },
        get routeElement() {
            return toolbar._routeElement.length ? toolbar._routeElement : (toolbar._routeElement = toolbar.element.find('.layout__toolbar-route'));
        },
        init: function() {
            sidebar.on('changing', function() {
                toolbar.element
                    .find('[data-action=toggle-sidebar] .hamburger')
                    .toggleClass('hamburger--active', sidebar.opened());
            });
            toolbar.element.find('[data-action]').click(function(evt) {
                var $element = $(this);
                var action = $element.attr('data-action');

                switch (action) {
                    case 'toggle-sidebar':
                        sidebar.toggle();
                        //$element.find('.hamburger').toggleClass('hamburger--active', sidebar.opened());
                        break;
                    case 'toggle-fullscreen':
                        layout.toggleFullscreen();
                        $element.attr('title', layout.isFullscreen ? 'Режим веб-страницы' : 'Полноэкранный режим');
                        break;
                    case 'create-support-ticket':
                        pbaAPI.openDetailView('ApplySupportRequest', { id: 0, buttons: { save: 'Отправить' } });
                        break;
                    case 'open-chats':
                        application.chat.toggleVisibility();
                        break;
                    default:
                        return; // without preventing default behavior
                }

                evt.preventDefault();
            });

            //if (sidebar.initialized && sidebar.opened()) {
            //    toolbar.element.find('[data-action="toggle-sidebar"] .hamburger').addClass('hamburger--active');
            //    return;
            //}

            toolbar.update();

            // if sidebar menu has no such route, and route is not in private route-cache yet,
            // then on 'route:changing' event, sidebar doesn't know about new page title,
            // until page is fully loaded, and 'route:changed' is emited, so:
            // 'route:changing' - optimistic speed
            // 'route:changed' - checking and correction
            application.spa.on('route:changing', toolbar.update.bind(toolbar));
            application.spa.on('route:changed', toolbar.update.bind(toolbar));

            var intervalId = setInterval(function() {
                if (!sidebar.initialized) {
                    return;
                }

                clearInterval(intervalId);

                toolbar.element.find('[data-action="toggle-sidebar"] .hamburger').toggleClass('hamburger--active', sidebar.opened());
            }, 250);
        },
        update: function() {
            var currentRoute = sidebar.getCurrentRouteInfo();
            var routeHtml = toolbar._routeTemplate(currentRoute);
            //Check of the number and replace template
            toolbar.routeElement.html(currentRoute.element.length? routeHtml : "");
        }
    };
}());
