(function (angular) {
    angular.module('MapApp')
        .constant('LAYERS_VIEW_DEFS', {
            DEF_ICON: 'glyphicon glyphicon-pushpin',
            DEF_ICON_SIZE: 30,
            DEF_ICON_COLOR: '#376490',
            DEF_OPACITY: 0.1,
            DEF_BORDER_COLOR: '#376490',
            DEF_BORDER_WIDTH: 1,
            DEF_BORDER_OPACITY: 0.1,
            DEF_BACKGROUND_COLOR: '#376490',
            DEF_SELECT_BORDER_COLOR: '#2EB77E',
            DEF_SELECT_FILL_COLOR: '#2EB77E'
        })
        .constant('LAYERS_PARAM_DEFS', {
            R: 6378137,
            BORDER_LAYERID: 'BorderGround_0a4d61a8bf7b442380ce94a8a3f7af42',
            NAVIGATION_LAYERID: 'NavigationLayer_a83cf081709e4971af70de3941451d7d',
            KLADRADDRESS_LAYERID: 'KLADR_868ffa83a0684f5d8b9a625b3b48a8e5',
            SEARCH_PREFIX: 'aee8fdb7c0c64c32acbad5f468b69ca1',
            SNAP_RADIUS: 40,
            POINTER_RADIUS: 10,
            MIN_ZOOM: 6,
            MAX_ZOOM: 25
        })
        .constant('LAYERS_LAZY_ENUM', {
            NONE: 0,
            ONHOVER: 10,
            ONSELECT: 20
        });
})(window.angular);