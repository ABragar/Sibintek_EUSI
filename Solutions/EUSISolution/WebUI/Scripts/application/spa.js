/* global $, pbaAPI, application */
/* jshint strict: false */
pbaAPI.ensurePath('application.spa');
(function() {
    'use strict';

    // ############
    // DEPENDENCIES
    // ############

    var w = window;
    var spa = application.spa;

    // ###############
    // PRIVATE HELPERS
    // ###############

    // IE, Edge
    var isIE = function() {
        var ua = window.navigator.userAgent;

        var lte10 = ua.indexOf('MSIE ') > 0;
        var ie11 = !lte10 && !!ua.match(/Trident.*rv\:11\./);
        var isEdge = !lte10 && !ie11 && ua.match(/Edge\/\d+/);

        return lte10 || ie11 || isEdge;
    };

    $.extend(spa, pbaAPI.emitterMixin());

    $.extend(spa, {
        _loading: false,
        _allowed: [],
        _excepted: [],
        _enableBackground: [],
        _container: null,
        _supportCache: {},

        _containerSelector: '#content',
        _currentSelector: '#content-html',
        
        host: w.location.protocol + '//' + w.location.host,
        allow: [
            '^/$',
            '^/Dashboard',
            '^/Entities',
            '^/Map/View',
            '^/EntityType/[a-z]+-Frame-\\d+'
        ],
        except: [],
        enableBackground: [
            '^/$',
            '^/Dashboard'
        ],
        ignoreCase: true,
        isLoading: function() { return spa._loading; },
        getContainer: function() {
            if (spa._$container && spa._$container.length) {
                return spa._$container;
            }
            return (spa._$container = $(spa._containerSelector));
        },
        getCurrentWrapper: function() {
            if (spa._$currentWrapper && spa._$currentWrapper.length) {
                return spa._$currentWrapper;
            }
            return (spa._$currentWrapper = $(spa._currentSelector));
        },
        init: function() {

            // CHECK JQUERY
            if (!w.$) {
                throw Error('jQuery is required by application.spa');
            }

            // CHECK ADAPTER
            if (!spa.adapter) {
                throw Error('application.spa.adapter is required by application.spa');
            }

            // SPA DISABLED IN ALL IE (even Edge)
            if (isIE()) return;

            // INITIALIZE REGULAR EXPRESSIONS
            spa._allowed = spa.allow.map(function(pattern) {
                return new RegExp(pattern, this.ignoreCase ? 'i' : '');
            }, spa);
            spa._excepted = spa.except.map(function(pattern) {
                return new RegExp(pattern, this.ignoreCase ? 'i' : '');
            }, spa);
            spa._enableBackground = spa.enableBackground.map(function(pattern) {
                return new RegExp(pattern, this.ignoreCase ? 'i' : '');
            }, spa);

            if (!spa.getContainer().length) {
                throw Error('application container not found');
            }

            $(document.body).on('click', 'a[href]', function(evt) {
                if (spa.isLoading()) return;

                //var $link = $(evt.target);

                //if (!$link.is('a')) {
                //    $link = $link.closest('a[href]');
                //}
                var $link = $(this);

                var href = $link.attr('href');
                if (!href || href[0] === '#' || href.indexOf('javascript:') >= 0) {
                    evt.preventDefault(); // ?
                    return;
                }

                if ($link.attr('target') === '_blank') {
                    return;
                }

                var relUrl = spa.adapter.getRelativeUrl(href);
                var fromRoute = w.location.pathname;

                if (relUrl === fromRoute) {
                    evt.preventDefault();
                    return;
                }

                if (spa.hasSpaSupport(relUrl) && spa.adapter.hasSupport()) {
                    evt.preventDefault();
                    spa.adapter.pushState(relUrl);
                }
            });

            spa.adapter.onChange(spa.onRoute.bind(spa));
        },
        onRoute: function(data) {
            spa.emit('route:changing');

            var fromUrl = data[0];
            var toUrl = data[1];

            var relUrl = w.location.pathname;

            if (!spa.hasSpaSupport(fromUrl)) {
                w.location.reload();
                return;
            }

            spa.showLoader();

            $.get(relUrl, function(html) {
                var $contentCurrent = spa.getCurrentWrapper();
                
                $contentCurrent.html(html);
                spa.emit('route:changed');

                var pageTitle = $contentCurrent.find('#page-title')[0];
                if (pageTitle) {
                    document.title = pageTitle.textContent;
                }
                layout.setLayoutClass();
                layout.element.trigger('resize');
                spa.hideLoader();
            });
        },
        showLoader: function() {
            spa.getContainer().addClass('loading');
            spa._loading = true;
        },
        hideLoader: function() {
            spa.getContainer().removeClass('loading');
            spa._loading = false;
        },
        hasSpaSupport: function(relUrl) {
            if (spa._supportCache.hasOwnProperty(relUrl)) {
                return spa._supportCache[relUrl];
            }

            return (spa._supportCache[relUrl] = spa._excepted.every(function(reg) {
                return !reg.test(relUrl);
            }) && spa._allowed.some(function(reg) {
                return reg.test(relUrl);
            }));
        },
        currentRouteHasBackground: function() {
            return spa._enableBackground.some(function(reg) { return reg.test(w.location.pathname) });
        }
    });

}());
