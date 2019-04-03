(function ($, angular) {
    function widgetService($log, $q, DataService, format, ContentWidget) {
        var service = function () {            
            var widgets = [
                //new ContentWidget({
                //    mnemonic: 'RoadSign',
                //    title: 'RoadSign Test',
                //    frame: true,
                //    global: true,
                //    urltemplate: '/Public/Map/TestFrameTemplate?mnemonic={mnemonic}&id=142850'
                //}),
                //new ContentWidget({
                //    mnemonic: 'RoadSign',
                //    title: 'RoadSign Test',
                //    frame: true,
                //    urltemplate: '/Public/Map/TestFrameTemplate?mnemonic={mnemonic}&id={id}'
                //}),
                //new ContentWidget({
                //    mnemonic: 'TestMarkerMapObject',
                //    title: 'Тестовый виджет',
                //    frame: true,
                //    urltemplate: '/MapWidget/TestMarkerMapObjectWidget?id={id}'
                //}),


                //new ContentWidget({
                //    mnemonic: 'GarbageCamPlace',
                //    title: 'Коллекция фото',
                //    frame: true,
                //    urltemplate: '/Garbage/ListPhotoMap?mnemonic={mnemonic}&objectID={id}'
                //}),

                //new ContentWidget({
                //    mnemonic: 'RegionalRoadKind',
                //    title: 'Автомобильная дорога',
                //    size: 'fullscreen',
                //    urltemplate: '/MapWidget/RoadWidgetTemplate?isPublicMap=true'
                //}),
                //new ContentWidget({
                //    mnemonic: 'NoRoadKind',
                //    title: 'Вид дороги не определен',
                //    size: 'fullscreen',
                //    urltemplate: '/MapWidget/RoadWidgetTemplate?isPublicMap=true'
                //}),
                //new ContentWidget({
                //    mnemonic: 'FederalRoadKind',
                //    title: 'Автомобильная дорога федерального значения',
                //    size: 'fullscreen',
                //    urltemplate: '/MapWidget/RoadWidgetTemplate?isPublicMap=true'
                //}),
                new ContentWidget({
                    mnemonic: 'TestMarkerMapObject',
                    layerId: 'TestMarkerMapObject3',
                    title: 'Дорога местного значения',
                    size: 'fullscreen',
                    urltemplate: '/MapWidget/RoadWidgetTemplate?isPublicMap=true'
                })
            ];

            this.hasWidget = function (config, global) {
                return this.getWidget(config.LayerId, global) !== null;
            };

            this.getWidgets = function(global) {
                return widgets.filter(function(widget) {
                    return angular.isDefined(global) ? widget.options.global === global : true;
                });
            };

            this.getWidget = function (layerId, global) {
                var _widgets = this.getWidgets(global).filter(function(widget) {
                    return widget.options.layerId.toLowerCase() === layerId.toLowerCase();
                });

                return _widgets.length > 0 ? _widgets[0] : null;
            };

            this.loadTemplate = function (widget) {
                return $q(function (resolve, reject) {
                    DataService.loadWidgetTemplate(widget.options.urltemplate).then(function(data) {
                            widget.options.template = data;
                            widget.templateIsLoaded = true;
                            resolve(widget.options.template);
                        },
                        function(err) {
                            reject(err);
                        });
                });
            };

            $log.info('Content widgets:', widgets);
        };

        return new service();
    };

    widgetService.$inject = ['$log', '$q', 'DataService', 'format', 'ContentWidget'];

    angular.module('MapApp').
        factory('WidgetService', widgetService);

})(jQuery, window.angular);