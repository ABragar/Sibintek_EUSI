(function () {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var layout = window.layout || {};
    var toolbar = layout.sidebar;

    var sidebar = layout.sidebar = {
        _blockUiUntilTransitionEnds: function (callback) {
            var max = 1000;
            var total = 0;
            var lastWidth = sidebar.element.width();
            var intervalId = setInterval(function () {
                total += 150;

                if (total >= max) {
                    clearInterval(intervalId);
                    layout.blockUi(false);
                    callback && callback();
                    return;
                }

                var w = sidebar.element.width();

                if (w === lastWidth) {
                    clearInterval(intervalId);
                    layout.blockUi(false);
                    callback && callback();
                    return;
                }

                lastWidth = w;
            }, 100);
        },
        _readCookie: function () {
            return Cookies.get('layout.sidebar.opened') === 'true';
        },
        _writeCookie: function (opened) {
            Cookies.set('layout.sidebar.opened', opened === true);
        },
        _expandActive: function () {
            //var selector = sidebar.element.find('[data-toggle=collapse].active').attr('href');
            //$(selector).collapse('show');
        },
        _element: $(),
        _menuElement: $(),
        _routeInfoCache: {},
        _initialized: false,
        // ----------------------------
        get element() {
            return sidebar._element.length ? sidebar._element : (sidebar._element = $('#layout-sidebar'));
        },
        get menuElement() {
            return sidebar._menuElement.length ? sidebar._menuElement : (sidebar._menuElement = $('.layout__sidebar-menu'));
        },
        get initialized() {
            return sidebar._initialized;
        },
        init: function () {
            if (sidebar._initialized) {
                return;
			}

			//TODO: Костыль. На этапе инициализации меню не заполнено и поля не выбираются. 
			//Через setInterval проверку наличия неправильно начинает работать
			var SelectNav = setInterval(function () {
				if ($('#layout-sidebar-menu a').length) {
					clearInterval(SelectNav);
					var DatElement = $('#layout-sidebar-menu a[href="' + location.pathname + '"]');
					if (DatElement.length) {
						DatElement.addClass('main_active');
						DatElement.parents("li").children("a").addClass('active');
						DatElement.parents("li").children("a").removeClass('collapsed');
						DatElement.parents("li").children("ul").addClass('in');
					}
				}
			}, 500)

			//setTimeout(function () {
			//	var DatElement = $('#layout-sidebar-menu a[href="' + location.pathname + '"]');
			//	if (DatElement.length) {
			//		DatElement.addClass('main_active');
			//		DatElement.parents("li").children("a").addClass('active');
			//		DatElement.parents("li").children("a").removeClass('collapsed');
			//		DatElement.parents("li").children("ul").addClass('in');
			//	}
			//}, 1000);

            sidebar.update();

            application.spa.on('route:changing', sidebar.update.bind(toolbar));

            //if (sidebar._readCookie()) {
            //    sidebar.open();
            //    sidebar._expandActive();
            //} else {
            //    sidebar.close();
            //}

            sidebar.element.find('[data-toggle=collapse]').click(function (evt) {
                if (sidebar.closed()) {
                    evt.preventDefault();           // prevents adding hash to the browser location
                    evt.stopImmediatePropagation(); // prevents bootstrap to expand menu list
                }
            });

            sidebar._initialized = true;
        },
        getCurrentRouteInfo: function () {
            if (sidebar._routeInfoCache[location.pathname]) {
                return sidebar._routeInfoCache[location.pathname];
            }

            if (location.pathname === '/') {
                return (sidebar._routeInfoCache[location.pathname] = {
                    element: $(),
                    icon: 'mdi mdi-home',
                    caption: 'Главная'
                });
            }

            var currentRouteItem = sidebar.menuElement.find('a[href="' + location.pathname + '"]');
            var currentRouteInfo = {
                element: currentRouteItem,
                icon: (currentRouteItem.children('.sidebar-menu__item-icon').attr('class') || '').replace('sidebar-menu__item-icon', '').trim() || 'mdi mdi-file-hidden',
                caption: (currentRouteItem.children('.sidebar-menu__item-caption').text() || '').trim() || document.title || ''
            };

            return (sidebar._routeInfoCache[location.pathname] = currentRouteInfo);
        },
		update: function () {
            sidebar.element.find('.active').removeClass('active');
           /* sidebar.getCurrentRouteInfo().element
                .addClass('active')
                .closest('ul')
                .siblings('[data-toggle=collapse]')
				.addClass('active');*/
			sidebar.element.find('.main_active').removeClass('main_active');
			sidebar.getCurrentRouteInfo().element.addClass('main_active');
			sidebar.getCurrentRouteInfo().element.parents("li").children("a").addClass('active');
        },
        opened: function () {
            return layout.element.hasClass('layout--sidebar-opened');
        },
        closed: function () {
            return layout.element.hasClass('layout--sidebar-closed');
        },
        _open: function () {
            if (sidebar.opened()) {
                return;
            }

            sidebar._blockUiUntilTransitionEnds(function () {
                layout.element.removeClass('layout--sidebar-opening');
                sidebar.emit('changed');
                sidebar.emit('opened');
                layout.emit('resize');
            });

            layout.element
                .removeClass('layout--sidebar-closing')
                .removeClass('layout--sidebar-closed')
                .addClass('layout--sidebar-opened')
                .addClass('layout--sidebar-opening');

            sidebar.emit('changing');
            sidebar.emit('opening');
        },
        open: function () {
            sidebar._open();
            layout.blockUi(true);
            layout.element.addClass('opened-using-hamburger');
            sidebar._writeCookie(true);
        },
        _close: function () {
            if (sidebar.closed()) {
                return;
            }

            sidebar._blockUiUntilTransitionEnds(function () {
                layout.element.removeClass('layout--sidebar-closing');
                sidebar.emit('changed');
                sidebar.emit('closed');
                layout.emit('resize');
            });

            layout.element
                .removeClass('layout--sidebar-opening')
                .removeClass('layout--sidebar-opened')
                .addClass('layout--sidebar-closed')
                .addClass('layout--sidebar-closing');

            sidebar.emit('changing');
            sidebar.emit('closing');
        },
        close: function () {
            layout.blockUi(true);
            sidebar._close();
            
            layout.element.removeClass('opened-using-hamburger');

            sidebar.element.find('.collapse').each(function () {
                var collapse = $(this).data('bs.collapse');
                if (collapse) {
                    collapse.hide();
                }
            });
            
            sidebar._writeCookie(false);
        },
        toggle: function () {
            if (sidebar.opened()) {
                sidebar.close();
            } else {
                sidebar.open();
            }
        }
    };

    $.extend(sidebar, pbaAPI.emitterMixin());
}());