/*
 * L.TileLayer.Rosreestr
 * Vadim Smirnov version
 */

(function (L) {

var addTileUrlMixin = function(baseClass) {
    return baseClass.extend({
        options: {
            tileSize: 1024
        },

        getTileUrl: function(tilePoint) {
            var map = this._map,
                crs = map.options.crs,
                tileSize = this.options.tileSize,
                nwPoint = tilePoint.multiplyBy(tileSize),
                sePoint = nwPoint.add([tileSize, tileSize]);

            var nw = crs.project(map.unproject(nwPoint, tilePoint.z)),
                se = crs.project(map.unproject(sePoint, tilePoint.z)),
                bbox = [nw.x, se.y, se.x, nw.y].join(',');

            return L.Util.template(this._url, L.extend({
                s: this._getSubdomain(tilePoint),
                bbox: bbox
            }, this.options));
        },

        onAdd: function (map) {
            L.TileLayer.prototype.onAdd.call(this, map);

            if (this.options.onAdd)
                this.options.onAdd(map);
        },

        onRemove: function (map) {
            L.TileLayer.prototype.onRemove.call(this, map);

            if (this.options.onRemove)
                this.options.onRemove(map);
        }
    });
};

var addInteractionMixin = function(baseClass) {
    return baseClass.extend({
        onAdd: function(map) {
            L.TileLayer.prototype.onAdd.call(this, map);

            if (this.options.clickable) {

                L.DomUtil.addClass(this._container, 'leaflet-clickable-raster-layer');

                this._initInteraction();
            }

            if (this.options.onAdd)
                this.options.onAdd(map);
        },

        onRemove: function (map) {
            L.TileLayer.prototype.onRemove.call(this, map);

            if (this.options.onRemove)
                this.options.onRemove(map);
        },

        _initInteraction: function() {
            var div = this._container,
                events = ['dblclick', 'click', 'mousedown', 'mouseover', 'mouseout', 'contextmenu'];

            for (var i = 0; i < events.length; i++) {
                L.DomEvent.on(div, events[i], this._fireMouseEvent, this);
            }
        },

        _fireMouseEvent: function(e) {
            var map = this._map;
            if (map.dragging && map.dragging.moved()) {
                return;
            }

            var containerPoint = map.mouseEventToContainerPoint(e),
                layerPoint = map.containerPointToLayerPoint(containerPoint),
                latlng = map.layerPointToLatLng(layerPoint);

            this.fire(e.type, {
                latlng: latlng,
                layerPoint: layerPoint,
                containerPoint: containerPoint,
                originalEvent: e
            });
        }
    });
};

L.TileLayer.Rosreestr = addTileUrlMixin(L.TileLayer);

L.tileLayer.Rosreestr = function (url, options) {
    if (options.clickable) {
        L.TileLayer.Rosreestr = addInteractionMixin(L.TileLayer.Rosreestr);
    }

    return new L.TileLayer.Rosreestr(url, options);
};

})(window.L);
