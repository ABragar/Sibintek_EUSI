(function (marker) {
    marker.prototype._setPos = function (pos) {
        if (this._icon) {
            L.DomUtil.setPosition(this._icon, pos);
        }

        if (this._shadow) {
            L.DomUtil.setPosition(this._shadow, pos);
        }

        this._zIndex = pos.y + this.options.zIndexOffset;

        this._resetZIndex();
    }


    marker.prototype._updateZIndex = function (offset) {
        if (this._icon) {
            this._icon.style.zIndex = this._zIndex + offset;
        }
    }

})(L.Marker);

(function (leafletpath) {
    //TODO: Sometime click event doesn't work after bringToFront
    leafletpath.include({
        bringToFront: function() {
            var root = this._map._pathRoot,
                path = this._container;

            if (path && root.lastChild !== path) {
                root.appendChild(path);
            }
            return this;
        }
    });
})(L.Path);