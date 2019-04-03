(function (window, angular) {
    angular.module('MapApp')
        .constant('format', window.format)
        .constant('GLOBAL_PARAM_DEFS', {
            ROAD_LAYERS: ["NoRoadKind", "FederalRoadKind", "RegionalRoadKind", "LocalRoadKind"],
            TRACK_LAYERS: ["UrenBaseLayer14", "BologoeBaseLayer13", "AdreanapolBaseLayer12"],
            NAVIGATION_LAYERS: [],
            LAYER_RELOAD_TIMEOUT: 600,
            LAYER_RELOAD_DISPLACEMENT: 20,
            NAVIGATION_TIMEOUT: 3000,
            NAVIGATION_SEARCH_RADIUS: 20,
            CURRENT_REGION_NAME: 'Московская область',
            CURRENT_REGION_POSITION: {
                lat: 55.663644005170006,
                lng: 37.4688720703125
            },
            CURRENT_REGION_ZOOM: 8,
            CURRENT_CENTER_POINT_NAME_TO: 'до Москвы',
            CURRENT_CENTER_POINT_POSITION: { lat: 55.75186146764451, lng: 37.61027812957764 }

        })
        .constant('CRUD_PARAM_DEFS', {
            IGNORE_VIEW_PROP_TYPES: ['Location'],
            FILTER_VIEW_PROP_TYPES: ['Enum', 'Numeric', 'Currency', 'Double', 'Integer', 'Bool', 'Boolean', 'Text', 'MultilineText', 'BaseObjectOne', 'Date'],
            FILTER_VIEW_UI_PROP_TYPES: ['Select', 'MultiSelect', 'Range', 'Checkbox', 'Text']
        })
        .constant('LAZY_PROP_NAMES', {
            StartDisposition: 'START_DISPOSITION'
        });
})(window, window.angular);