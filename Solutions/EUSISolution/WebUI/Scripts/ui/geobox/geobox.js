var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var GeoBox;
(function (GeoBox) {
    var ClusterIcon = (function () {
        function ClusterIcon() {
        }
        ClusterIcon.createIcon = function (count, icon, color) {
            color = color != null ? color : ClusterIcon._defaultIconColor;
            var totalHtml = "<div style=\"background-color:" + color + ";\" class=\"geoobject-cluster-total\">" + count + "</div>";
            icon = icon != null ? icon : ClusterIcon._defaultIconClass;
            var html = "<div class=\"icon-size-2\" style=\"background-color:" + color + ";\"><span class=\"" + icon + "\"></span>" + totalHtml + "</div>";
            return L.divIcon({
                className: "geoobject-icon",
                html: html,
                iconSize: L.point(ClusterIcon._defaultIconSize, ClusterIcon._defaultIconSize)
            });
        };
        ClusterIcon._defaultIconColor = "#376490";
        ClusterIcon._defaultIconClass = "glyphicon glyphicon-pushpin";
        ClusterIcon._defaultIconSize = 40;
        return ClusterIcon;
    }());
    GeoBox.ClusterIcon = ClusterIcon;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Model = kendo.data.Model;
    var Layer = (function () {
        function Layer(model) {
            this.needReload = false;
            this.currentZoom = null;
            // Events
            this._batchLoadStart = new GeoBox.Common.Event();
            this._batchLoadEnd = new GeoBox.Common.Event();
            if (model == null || !(model instanceof Model)) {
                throw new GeoBox.Common.ArgumentNullException("model");
            }
            this._model = model;
            this._llayer = this.createLlayer();
        }
        //#region Factory Methods
        Layer.create = function () {
            return new Layer(this.createModel());
        };
        Layer.createModel = function () {
            return new GeoBox.LayerModel();
        };
        Layer.prototype.createLlayer = function () {
            return L.featureGroup();
        };
        Object.defineProperty(Layer.prototype, "batchLoadStart", {
            //#endregion Factory Methods
            //#region Events
            get: function () {
                return this._batchLoadStart;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "batchLoadEnd", {
            get: function () {
                return this._batchLoadEnd;
            },
            enumerable: true,
            configurable: true
        });
        Layer.prototype.onBatchLoadStart = function () {
            if (!this._batchLoadStart.empty) {
                this._batchLoadStart.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        Layer.prototype.onBatchLoadEnd = function () {
            if (!this._batchLoadEnd.empty) {
                this._batchLoadEnd.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        Object.defineProperty(Layer.prototype, "model", {
            //#endregion Events
            //#region Generic Properties
            get: function () {
                return this._model;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "llayer", {
            get: function () {
                return this._llayer;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "uid", {
            get: function () {
                return this._model.uid;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "id", {
            get: function () {
                return this._model.get(this._model.idField);
            },
            set: function (value) {
                this._model.set(this._model.idField, value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "title", {
            get: function () {
                return this._model.get(Layer.titleField);
            },
            set: function (value) {
                this._model.set(Layer.titleField, value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "checked", {
            get: function () {
                return this._model.get("Checked");
            },
            set: function (value) {
                this._model.set("Checked", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "loadable", {
            get: function () {
                return this._model.get("Load");
            },
            set: function (value) {
                this._model.set("Load", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "count", {
            get: function () {
                return this._model.get(Layer.countField);
            },
            set: function (value) {
                this._model.set(Layer.countField, value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "children", {
            get: function () {
                return this._model.get("Children");
            },
            set: function (value) {
                this._model.set("Children", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "mode", {
            //#endregion Generic Properties
            //#region Mode Properties
            get: function () {
                return +this._model.get("Mode");
            },
            set: function (value) {
                this._model.set("Mode", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "clientClustering", {
            get: function () {
                return this._model.get("ClientClustering");
            },
            set: function (value) {
                this._model.set("ClientClustering", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "serverClustering", {
            get: function () {
                return this._model.get("ServerClustering");
            },
            set: function (value) {
                this._model.set("ServerClustering", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "serverClusteringMaxZoom", {
            get: function () {
                return this._model.get("ServerClusteringMaxZoom");
            },
            set: function (value) {
                this._model.set("ServerClusteringMaxZoom", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "searchOnClick", {
            get: function () {
                return this._model.get(Layer.searchOnClickField);
            },
            set: function (value) {
                this._model.set(Layer.searchOnClickField, value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "minSearchZoom", {
            get: function () {
                return this._model.get("MinSearchZoom");
            },
            set: function (value) {
                this._model.set("MinSearchZoom", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "maxSearchZoom", {
            get: function () {
                return this._model.get("MaxSearchZoom");
            },
            set: function (value) {
                this._model.set("MaxSearchZoom", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "isVisible", {
            get: function () {
                return this._model.get("checked");
            },
            set: function (value) {
                this._model.set("checked", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "isSelected", {
            get: function () {
                return this._model.get("selected");
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "searchable", {
            get: function () {
                return this.searchOnClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "updatable", {
            get: function () {
                return this.isServerMode && !this.searchable;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "searchByZoom", {
            get: function () {
                return this.minSearchZoom != null || this.maxSearchZoom != null;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "isClientMode", {
            get: function () {
                return this.mode === GeoBox.LayerMode.Client;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "isServerMode", {
            get: function () {
                return this.mode === GeoBox.LayerMode.Server;
            },
            enumerable: true,
            configurable: true
        });
        Layer.ClientClustering = function (model) {
            return model.get("ClientClustering");
        };
        Object.defineProperty(Layer.prototype, "style", {
            //#endregion Mode Properties
            //#region Style Properties
            get: function () {
                return this._model.get("Style");
            },
            set: function (value) {
                this._model.set("Style", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "icon", {
            get: function () {
                return this._model.get(Layer.iconField);
            },
            set: function (value) {
                this._model.set(Layer.iconField, value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "color", {
            get: function () {
                return this._model.get(Layer.colorField);
            },
            set: function (value) {
                this._model.set(Layer.colorField, value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "background", {
            get: function () {
                return this._model.get("Style.Background");
            },
            set: function (value) {
                this._model.set("Style.Background", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "opacity", {
            get: function () {
                return this._model.get("Style.Opacity");
            },
            set: function (value) {
                this._model.set("Style.Opacity", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "borderColor", {
            get: function () {
                return this._model.get("Style.BorderColor");
            },
            set: function (value) {
                this._model.set("Style.BorderColor", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "borderOpacity", {
            get: function () {
                return this._model.get("Style.BorderOpacity");
            },
            set: function (value) {
                this._model.set("Style.BorderOpacity", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "borderWidth", {
            get: function () {
                return this._model.get("Style.BorderWidth");
            },
            set: function (value) {
                this._model.set("Style.BorderWidth", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Layer.prototype, "showIcon", {
            get: function () {
                return this._model.get("Style.ShowIcon");
            },
            set: function (value) {
                this._model.set("Style.ShowIcon", value);
            },
            enumerable: true,
            configurable: true
        });
        //#endregion Style Properties
        //#region Public Methods
        Layer.prototype.enable = function () {
            this._model.set("enabled", true);
        };
        Layer.prototype.disable = function () {
            this._model.set("enabled", false);
        };
        Layer.prototype.addToMap = function (map) {
            if (map == null) {
                throw new GeoBox.Common.ArgumentNullException("map");
            }
            if (map.isRendered && !map.map.hasLayer(this._llayer)) {
                map.map.addLayer(this._llayer);
            }
        };
        Layer.prototype.removeFromMap = function (map) {
            if (map == null) {
                throw new GeoBox.Common.ArgumentNullException("map");
            }
            if (map.isRendered && map.map.hasLayer(this._llayer)) {
                map.map.removeLayer(this._llayer);
            }
        };
        Layer.prototype.addGeoObjects = function (geoObjects) {
            this.onBatchLoadStart();
            for (var _i = 0, geoObjects_1 = geoObjects; _i < geoObjects_1.length; _i++) {
                var geoObject = geoObjects_1[_i];
                geoObject.addToLayer(this);
            }
            this.onBatchLoadEnd();
        };
        Layer.prototype.removeGeoObjects = function (geoObjects) {
            for (var _i = 0, geoObjects_2 = geoObjects; _i < geoObjects_2.length; _i++) {
                var geoObject = geoObjects_2[_i];
                geoObject.removeFromLayer(this);
            }
        };
        Layer.prototype.addGeoObject = function (geoObject) {
            geoObject.addToLayer(this);
        };
        Layer.prototype.removeGeoObject = function (geoObject) {
            geoObject.removeFromLayer(this);
        };
        Layer.prototype.removeAllGeoObjects = function () {
            this._llayer.clearLayers();
        };
        Layer.prototype.destroyEvents = function () {
            this._batchLoadStart.clear();
            this._batchLoadStart = null;
            this._batchLoadEnd.clear();
            this._batchLoadEnd = null;
        };
        Layer.prototype.destroy = function () {
            this._model = null;
            this._llayer = null;
        };
        // Model Field Names
        Layer.titleField = "Title";
        Layer.iconField = "Style.Icon";
        Layer.colorField = "Style.Color";
        Layer.countField = "GeoObjectCount";
        Layer.searchOnClickField = "SearchOnClick";
        return Layer;
    }());
    GeoBox.Layer = Layer;
    GeoBox.LayerModel = kendo.data.Node.define({
        id: "LayerId",
        children: "Children",
        fields: {
            "LayerId": {
                type: "string",
                defaultValue: null
            },
            "Mnemonic": {
                type: "string",
                defaultValue: null
            },
            "Title": {
                type: "string",
                defaultValue: null
            },
            "Checked": {
                type: "boolean",
                defaultValue: false
            },
            "Load": {
                type: "boolean",
                defaultValue: false
            },
            "Filterable": {
                type: "boolean",
                defaultValue: false
            },
            "Mode": {
                type: "number",
                defaultValue: 0
            },
            "ServerClusteringMaxZoom": {
                type: "number",
                defaultValue: 0
            },
            "ServerClustering": {
                type: "boolean",
                defaultValue: false
            },
            "ClientClustering": {
                type: "boolean",
                defaultValue: false
            },
            "SearchOnClick": {
                type: "boolean",
                defaultValue: false
            },
            "MinSearchZoom": {
                type: "number",
                defaultValue: null
            },
            "MaxSearchZoom": {
                type: "number",
                defaultValue: null
            },
            "Count": {
                type: "number",
                defaultValue: 0
            },
            "Style": {
                defaultValue: {}
            },
            "Children": {
                defaultValue: null
            }
        }
    });
})(GeoBox || (GeoBox = {}));
/// <reference path="layer.ts" />
var GeoBox;
(function (GeoBox) {
    var ClusterLayer = (function (_super) {
        __extends(ClusterLayer, _super);
        function ClusterLayer() {
            _super.apply(this, arguments);
        }
        ClusterLayer.prototype.createLlayer = function () {
            return new GeoBox.MarkerClusterGroup({
                chunkedLoading: true,
                iconCreateFunction: this.createIcon.bind(this),
                polygonOptions: this.getPolygonOptions(),
                chunkProgress: this.handleChunkProgress.bind(this)
            });
        };
        ClusterLayer.create = function () {
            return new ClusterLayer(this.createModel());
        };
        ClusterLayer.prototype.addGeoObjects = function (geoObjects) {
            if (geoObjects.length <= 0) {
                this.onBatchLoadStart();
                this.onBatchLoadEnd();
                return;
            }
            this.onBatchLoadStart();
            var lobjects = [];
            for (var _i = 0, geoObjects_3 = geoObjects; _i < geoObjects_3.length; _i++) {
                var geoObject = geoObjects_3[_i];
                geoObject.layer = this;
                if (geoObject.lobject != null) {
                    lobjects.push(geoObject.lobject);
                }
            }
            this.llayer.addLayers(lobjects);
        };
        ClusterLayer.prototype.removeGeoObjects = function (geoObjects) {
            if (geoObjects.length <= 0) {
                return;
            }
            var lobjects = [];
            for (var _i = 0, geoObjects_4 = geoObjects; _i < geoObjects_4.length; _i++) {
                var geoObject = geoObjects_4[_i];
                geoObject.layer = null;
                if (geoObject.lobject != null) {
                    lobjects.push(geoObject.lobject);
                }
            }
            this.llayer.removeLayers(lobjects);
        };
        ClusterLayer.prototype.getPolygonOptions = function () {
            var defaultOptions = ClusterLayer._defaultPoligonOptions;
            if (this.background != null) {
                defaultOptions["color"] = this.background;
            }
            return defaultOptions;
        };
        ClusterLayer.prototype.createIcon = function (cluster) {
            var count = cluster.getChildCount();
            return GeoBox.ClusterIcon.createIcon(count, this.icon, this.color);
        };
        ClusterLayer.prototype.getClusterCount = function (cluster) {
            if (this.serverClustering) {
                var resultCount = 0;
                var childMarkers = cluster.getAllChildMarkers();
                for (var i = 0, c = childMarkers.length; i < c; i++) {
                    var count = childMarkers[i].options.count;
                    if (count != null && count > 0) {
                        resultCount += count;
                    }
                    else {
                        resultCount += 1;
                    }
                }
                return resultCount;
            }
            return cluster.getChildCount();
        };
        ClusterLayer.prototype.handleChunkProgress = function (processed, total, elapsed) {
            if (total > 0 && processed === total) {
                this.onBatchLoadEnd();
            }
        };
        ClusterLayer._defaultPoligonOptions = {
            color: "#b7d2f1",
            weight: 1
        };
        return ClusterLayer;
    }(GeoBox.Layer));
    GeoBox.ClusterLayer = ClusterLayer;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Events;
    (function (Events) {
        var GeoObjectLocationEventArgs = (function () {
            function GeoObjectLocationEventArgs() {
            }
            return GeoObjectLocationEventArgs;
        }());
        Events.GeoObjectLocationEventArgs = GeoObjectLocationEventArgs;
    })(Events = GeoBox.Events || (GeoBox.Events = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="events/geoobjectlocationeventargs.ts" />
var GeoBox;
(function (GeoBox) {
    var Model = kendo.data.Model;
    var Button = kendo.ui.Button;
    var LocationEventArgs = GeoBox.Events.GeoObjectLocationEventArgs;
    var GeoObject = (function () {
        function GeoObject(model) {
            // Events
            this._click = new GeoBox.Common.Event();
            this._doubleClick = new GeoBox.Common.Event();
            this._polygonClick = new GeoBox.Common.Event();
            this._editButtonClick = new GeoBox.Common.Event();
            this._deleteButtonClick = new GeoBox.Common.Event();
            if (model == null || !(model instanceof Model)) {
                throw new GeoBox.Common.ArgumentNullException("model");
            }
            this._model = model;
            this._layer = null;
            this._lobject = null;
            this._popup = null;
            this.handleChangeModel = this.handleChangeModel.bind(this);
            this.handleDoubleClick = this.handleDoubleClick.bind(this);
            this.handleClick = this.handleClick.bind(this);
            this.handlePopupOpen = this.handlePopupOpen.bind(this);
            this.handlePopupClose = this.handlePopupClose.bind(this);
            this.handleEditButtonClick = this.handleEditButtonClick.bind(this);
            this.handleDeleteButtonClick = this.handleDeleteButtonClick.bind(this);
            this.bindModelEvents(this._model);
        }
        //#region Factory Methods
        GeoObject.create = function () {
            return new GeoObject(this.createModel());
        };
        GeoObject.createModel = function () {
            return new GeoObjectModel();
        };
        Object.defineProperty(GeoObject.prototype, "click", {
            //#endregion Factory Methods
            //#region Events
            get: function () {
                return this._click;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "doubleClick", {
            get: function () {
                return this._doubleClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "polygonClick", {
            get: function () {
                return this._polygonClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "editButtonClick", {
            get: function () {
                return this._editButtonClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "deleteButtonClick", {
            get: function () {
                return this._deleteButtonClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "uid", {
            //#endregion Events
            //#region Public Properties
            get: function () {
                return this._model.uid;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "geometry", {
            get: function () {
                return this._model.get("Geometry");
            },
            set: function (value) {
                this._model.set("Geometry", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "model", {
            get: function () {
                return this._model;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "layer", {
            get: function () {
                return this._layer;
            },
            set: function (value) {
                this._layer = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "lobject", {
            get: function () {
                if (this._lobject === null) {
                    this._lobject = this.createLobjectFromGeometry(this.geometry);
                }
                return this._lobject;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "id", {
            get: function () {
                return this._model.get(this._model.idField);
            },
            set: function (value) {
                this._model.set(this._model.idField, value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "title", {
            get: function () {
                return this._model.get("Title");
            },
            set: function (value) {
                this._model.set("Title", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "description", {
            get: function () {
                return this._model.get("Description");
            },
            set: function (value) {
                this._model.set("Description", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "icon", {
            get: function () {
                return this._model.get("Icon");
            },
            set: function (value) {
                this._model.set("Icon", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "color", {
            get: function () {
                return this._model.get("Color");
            },
            set: function (value) {
                this._model.set("Color", value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "type", {
            get: function () {
                return +this._model.get(GeoObject.typeField);
            },
            set: function (value) {
                this._model.set(GeoObject.typeField, value);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoObject.prototype, "count", {
            get: function () {
                return +this._model.get("Count");
            },
            set: function (value) {
                this._model.set("Count", value);
            },
            enumerable: true,
            configurable: true
        });
        //#endregion Public Properties
        //#region Public Methods
        GeoObject.prototype.addToLayer = function (layer) {
            if (layer == null) {
                throw new GeoBox.Common.ArgumentNullException("layer");
            }
            if (this._layer !== null) {
                this.removeFromLayer(this._layer);
            }
            if (this.lobject !== null && !layer.llayer.hasLayer(this.lobject)) {
                layer.llayer.addLayer(this.lobject);
                this._layer = layer;
                this.updateIcon();
                this.bindOpenedPopup(this.lobject);
            }
        };
        GeoObject.prototype.removeFromLayer = function (layer) {
            if (layer == null) {
                throw new GeoBox.Common.ArgumentNullException("layer");
            }
            if (this.lobject !== null && layer.llayer.hasLayer(this.lobject)) {
                layer.llayer.removeLayer(this.lobject);
                this._layer = null;
            }
        };
        GeoObject.prototype.syncModelGeometry = function () {
            if (this.lobject !== null) {
                this.unbindModelEvents(this._model);
                this.geometry = this.lobject.toGeoJSON().geometry;
                this.bindModelEvents(this._model);
            }
        };
        GeoObject.prototype.getEditButton = function () {
            if (this._popup && this._popup._contentNode) {
                return $(this._popup._contentNode).find(".edit-btn").data("kendoButton");
            }
            return null;
        };
        //#endregion Public Methods
        //#region Event Handlers
        GeoObject.prototype.handleChangeModel = function (event) {
            switch (event.field) {
                case "Geometry":
                    this.updateGeometry();
                    break;
                case "Icon":
                    this.updateIcon();
                    break;
                case "Color":
                    this.updateIcon();
                    break;
            }
        };
        GeoObject.prototype.handleDoubleClick = function (event) {
            if (!this._doubleClick.empty) {
                this._doubleClick.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        GeoObject.prototype.handleClick = function (event) {
            this.onClick();
            this.onPoligonClick(event);
        };
        GeoObject.prototype.handleEditButtonClick = function (event) {
            if (!this._editButtonClick.empty) {
                this._editButtonClick.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        GeoObject.prototype.handleDeleteButtonClick = function (event) {
            if (!this._deleteButtonClick.empty) {
                this._deleteButtonClick.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        GeoObject.prototype.handlePopupOpen = function (event) {
            this.bindPopupModel(this._popup, this._model);
            GeoObject.setOpenPopup(this.getPopupId(), this._popup);
        };
        GeoObject.prototype.handlePopupClose = function (event) {
            this.unbindPopupModel(this._popup);
            GeoObject.removeOpenPopup(this.getPopupId());
        };
        GeoObject.prototype.bindPopupModel = function (popup, model) {
            if (popup._contentNode) {
                kendo.bind(popup._contentNode, model);
            }
        };
        GeoObject.prototype.unbindPopupModel = function (popup) {
            if (popup._contentNode) {
                kendo.unbind(popup._contentNode);
            }
        };
        GeoObject.prototype.onClick = function () {
            if (!this._click.empty) {
                this._click.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        GeoObject.prototype.onPoligonClick = function (event) {
            if (!this._polygonClick.empty && event.target instanceof L.Polygon) {
                var args = new LocationEventArgs();
                args.location = event.latlng;
                this._polygonClick.notify(this, args);
            }
        };
        //#endregion Event Handlers
        //#region Update Methods
        GeoObject.prototype.updateGeometry = function () {
            var previosLayer = this.layer;
            if (previosLayer !== null) {
                this.removeFromLayer(previosLayer);
                this.destroyLObject();
                this.addToLayer(previosLayer);
            }
            else {
                this.destroyLObject();
            }
        };
        GeoObject.prototype.updateIcon = function (lobject) {
            lobject = lobject || this.lobject;
            if (this.icon != null) {
                this.setIcon(lobject, this.icon, this.color);
            }
            else if (this._layer != null && this._layer.icon != null) {
                this.setIcon(lobject, this._layer.icon, this._layer.color);
            }
        };
        //#endregion Update Methods
        GeoObject.prototype.createLobjectFromGeometry = function (geometry) {
            var result = null;
            if (geometry != null) {
                try {
                    result = L.GeoJSON.geometryToLayer(geometry);
                    this.doBind(result);
                    this.updateIcon(result);
                }
                catch (e) { }
            }
            return result;
        };
        GeoObject.prototype.doBind = function (lobject) {
            this.bindEvents(lobject);
            this.bindPopup(lobject, this.getPopup(lobject));
        };
        GeoObject.prototype.createIcon = function (icon, color) {
            return icon != null ? GeoBox.GeoObjectUtils.createIcon(icon, color) : null;
        };
        GeoObject.prototype.setIcon = function (lobject, icon, color) {
            if (lobject != null && typeof lobject.setIcon !== "undefined") {
                var licon = this.createIcon(icon, color);
                if (licon !== null) {
                    lobject.setIcon(licon);
                }
            }
        };
        GeoObject.prototype.createButtonId = function (prefix) {
            return "geobox-" + prefix + "-button-" + this.id;
        };
        GeoObject.prototype.createButton = function (buttonId) {
            var node = document.getElementById(buttonId);
            if (node != null) {
                return new Button(node);
            }
            return null;
        };
        //#region Popup Methods
        GeoObject.prototype.getPopup = function (lobject) {
            if (this._popup == null) {
                this._popup = GeoObject.getOpenPopup(this.getPopupId());
                if (this._popup == null) {
                    this._popup = new L.Popup({ autoPan: true }, lobject);
                }
                var popupContent = GeoBox.GeoObjectUtils.getPopupContent();
                if (popupContent != null) {
                    this._popup.setContent(popupContent);
                }
            }
            return this._popup;
        };
        GeoObject.prototype.closePopup = function () {
            var lobject = this.lobject;
            if (lobject != null && lobject.closePopup !== undefined) {
                lobject.closePopup();
            }
        };
        GeoObject.prototype.bindPopup = function (lobject, popup) {
            if (lobject == null || lobject.bindPopup === undefined || popup == null) {
                return;
            }
            lobject.bindPopup(popup);
            popup._source = lobject;
        };
        GeoObject.prototype.unbindPopup = function (lobject, popup) {
            if (lobject == null || lobject.unbindPopup === undefined || popup == null) {
                return;
            }
            lobject.unbindPopup();
            popup._source = null;
        };
        GeoObject.prototype.getPopupId = function () {
            if (this._layer != null) {
                return "" + this._layer.id + this.id;
            }
            return this.id;
        };
        GeoObject.prototype.bindOpenedPopup = function (lobject) {
            var openedPopup = GeoObject.getOpenPopup(this.getPopupId());
            if (openedPopup != null) {
                if (this._popup) {
                    this.unbindPopupModel(this._popup);
                }
                this.destroyPopup();
                this._popup = openedPopup;
                this.bindPopup(lobject, openedPopup);
                this.bindPopupModel(this._popup, this._model);
            }
        };
        GeoObject.setOpenPopup = function (id, popup) {
            if (popup != null) {
                GeoObject._openPopups[id] = popup;
            }
        };
        GeoObject.getOpenPopup = function (id) {
            return GeoObject._openPopups[id] || null;
        };
        GeoObject.removeOpenPopup = function (id) {
            if (GeoObject._openPopups[id] !== undefined) {
                delete GeoObject._openPopups[id];
            }
        };
        GeoObject.closeUnbindedPopups = function () {
            Object.keys(GeoObject._openPopups).map(function (id) {
                var popup = GeoObject._openPopups[id];
                if (popup._source == null) {
                    popup._close();
                    delete GeoObject._openPopups[id];
                }
            });
        };
        GeoObject.spiderfyObjectIfOpenPopup = function () {
            Object.keys(GeoObject._openPopups).map(function (id) {
                var popup = GeoObject._openPopups[id];
                if (popup._source != null &&
                    popup._source.__parent != null &&
                    popup._source.__parent instanceof L.MarkerCluster &&
                    popup._source.__parent._markers != null &&
                    popup._source.__parent._markers.length > 1) {
                    popup._source.__parent.spiderfy();
                }
            });
        };
        //#endregion Popup Methods
        //#region Bind Events Methods
        GeoObject.prototype.bindEvents = function (lobject) {
            lobject.on("click", this.handleClick);
            lobject.on("popupopen", this.handlePopupOpen);
            lobject.on("popupclose", this.handlePopupClose);
        };
        GeoObject.prototype.unbindEvents = function (lobject) {
            lobject.off("click", this.handleClick);
            lobject.off("popupopen", this.handlePopupOpen);
            lobject.off("popupclose", this.handlePopupClose);
        };
        GeoObject.prototype.bindModelEvents = function (model) {
            model.bind("change", this.handleChangeModel);
            model.bind("deleteClick", this.handleDeleteButtonClick);
            model.bind("editClick", this.handleEditButtonClick);
        };
        GeoObject.prototype.unbindModelEvents = function (model) {
            model.unbind("change", this.handleChangeModel);
            model.unbind("deleteClick", this.handleDeleteButtonClick);
            model.unbind("editClick", this.handleEditButtonClick);
        };
        //#endregion Bind Events Methods
        //#region Destroy Members
        GeoObject.prototype.destroyLayer = function () {
            if (this._layer != null) {
                this.removeFromLayer(this._layer);
                this._layer = null;
            }
        };
        GeoObject.prototype.destroyModel = function () {
            if (this._model != null) {
                this.unbindModelEvents(this._model);
                this._model = null;
            }
        };
        GeoObject.prototype.destroyLObject = function () {
            if (this._lobject != null) {
                this.destroyPopup();
                this.unbindEvents(this._lobject);
                this._lobject = null;
            }
        };
        GeoObject.prototype.destroyPopup = function () {
            if (this._popup != null && this._lobject != null) {
                this.unbindPopup(this._lobject, this._popup);
                this._popup = null;
            }
        };
        GeoObject.prototype.destroyEvents = function () {
            if (this._click) {
                this._click.clear();
                this._click = null;
            }
            if (this._doubleClick) {
                this._doubleClick.clear();
                this._doubleClick = null;
            }
            if (this._polygonClick) {
                this._polygonClick.clear();
                this._polygonClick = null;
            }
            if (this._editButtonClick) {
                this._editButtonClick.clear();
                this._editButtonClick = null;
            }
            if (this._deleteButtonClick) {
                this._deleteButtonClick.clear();
                this._deleteButtonClick = null;
            }
        };
        GeoObject.prototype.destroy = function () {
            this.destroyLayer();
            this.destroyModel();
            this.destroyLObject();
            this.destroyEvents();
        };
        GeoObject._openPopups = {};
        // Model Field Names
        GeoObject.typeField = "Type";
        return GeoObject;
    }());
    GeoBox.GeoObject = GeoObject;
    GeoBox.GeoObjectModelDefinition = Model.define({
        id: "ID",
        fields: {
            "ID": {
                type: "number",
                defaultValue: 0
            },
            "Title": {
                type: "string",
                defaultValue: null
            },
            "Description": {
                type: "string",
                defaultValue: null
            },
            "Icon": {
                type: "string",
                defaultValue: null
            },
            "Color": {
                type: "string",
                defaultValue: null
            },
            "Geometry": {
                defaultValue: null
            },
            "Type": {
                type: "number",
                defaultValue: 0
            },
            "Count": {
                type: "number",
                defaultValue: 0
            }
        }
    });
    var GeoObjectModel = (function (_super) {
        __extends(GeoObjectModel, _super);
        function GeoObjectModel() {
            _super.apply(this, arguments);
        }
        GeoObjectModel.prototype.onDeleteClick = function (e) {
            this.trigger("deleteClick");
        };
        GeoObjectModel.prototype.onEditClick = function (e) {
            this.trigger("editClick");
        };
        return GeoObjectModel;
    }(GeoBox.GeoObjectModelDefinition));
    GeoBox.GeoObjectModel = GeoObjectModel;
})(GeoBox || (GeoBox = {}));
/// <reference path="geoobject.ts" />
var GeoBox;
(function (GeoBox) {
    var ClusterObject = (function (_super) {
        __extends(ClusterObject, _super);
        function ClusterObject(model) {
            _super.call(this, model);
        }
        //#region Factory Methods
        ClusterObject.create = function () {
            return new ClusterObject(this.createModel());
        };
        //#endregion Factory Methods
        ClusterObject.prototype.getLatLng = function () {
            return this.lobject.getPosLatLng();
        };
        Object.defineProperty(ClusterObject.prototype, "isMarker", {
            get: function () {
                return this.lobject instanceof L.Marker;
            },
            enumerable: true,
            configurable: true
        });
        //#region Override Methods
        ClusterObject.prototype.createLobjectFromGeometry = function (geometry) {
            var result = _super.prototype.createLobjectFromGeometry.call(this, geometry);
            result.options.count = this.count;
            result.getPosLatLng = result.getLatLng;
            result.getLatLng = undefined;
            return result;
        };
        ClusterObject.prototype.doBind = function (lobject) {
            this.bindEvents(lobject);
        };
        ClusterObject.prototype.bindEvents = function (lobject) {
            lobject.on("click", this.handleClick);
        };
        ClusterObject.prototype.unbindEvents = function (lobject) {
            lobject.off("click", this.handleClick);
        };
        ClusterObject.prototype.createIcon = function (icon, color) {
            return GeoBox.ClusterIcon.createIcon(this.count, icon, color);
        };
        ClusterObject.prototype.updateIcon = function (lobject) {
            lobject = lobject || this.lobject;
            if (this.icon != null) {
                this.setIcon(lobject, this.icon, this.color);
            }
            else if (this.layer != null) {
                this.setIcon(lobject, this.layer.icon, this.layer.color);
            }
        };
        return ClusterObject;
    }(GeoBox.GeoObject));
    GeoBox.ClusterObject = ClusterObject;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var Event = (function () {
            function Event() {
                this._handlers = [];
            }
            Event.prototype.notify = function (sender, event) {
                if (sender == null) {
                    throw new Common.ArgumentNullException("sender");
                }
                if (event == null) {
                    throw new Common.ArgumentNullException("event");
                }
                if (this.handlers.length === 1) {
                    var handler = this.handlers[0];
                    handler(sender, event);
                }
                else if (this.handlers.length > 0) {
                    for (var _i = 0, _a = this.handlers; _i < _a.length; _i++) {
                        var handler = _a[_i];
                        handler(sender, event);
                    }
                }
            };
            Object.defineProperty(Event.prototype, "empty", {
                get: function () {
                    return this.handlers.length === 0;
                },
                enumerable: true,
                configurable: true
            });
            Event.prototype.add = function (handler) {
                if (typeof handler !== "function") {
                    throw new Common.ArgumentNullException("handler");
                }
                this._handlers.push(handler);
                this.doAdd(handler);
            };
            Event.prototype.doAdd = function (handler) {
            };
            Event.prototype.remove = function (handler) {
                if (typeof handler !== "function") {
                    throw new Common.ArgumentNullException("handler");
                }
                if (this._handlers.length > 0) {
                    var removeIndex = this._handlers.indexOf(handler);
                    if (removeIndex !== -1) {
                        var removeHandler = this._handlers[removeIndex];
                        this._handlers.splice(removeIndex, 1);
                        this.doRemove(removeHandler);
                    }
                }
            };
            Event.prototype.doRemove = function (handler) {
            };
            Object.defineProperty(Event.prototype, "handlers", {
                get: function () {
                    return this._handlers;
                },
                enumerable: true,
                configurable: true
            });
            Event.prototype.clear = function () {
                this._handlers = [];
            };
            return Event;
        }());
        Common.Event = Event;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="common/event.ts" />
var GeoBox;
(function (GeoBox) {
    var DrawControl = (function () {
        function DrawControl(map, editableLayer, edit, remove) {
            if (edit === void 0) { edit = true; }
            if (remove === void 0) { remove = true; }
            this._isRendered = false;
            this._editingEnabled = false;
            this._deletingEnabled = false;
            this._drawingEnabled = false;
            // Events
            this._created = new GeoBox.Common.Event();
            this._edited = new GeoBox.Common.Event();
            this._deleted = new GeoBox.Common.Event();
            this._editStart = new GeoBox.Common.Event();
            this._editStop = new GeoBox.Common.Event();
            if (map == null) {
                throw new GeoBox.Common.ArgumentNullException("map");
            }
            if (editableLayer == null || !(editableLayer.llayer instanceof L.FeatureGroup)) {
                throw new GeoBox.Common.ArgumentNullException("editableLayer");
            }
            this._map = map;
            var drawControlOptions = {
                position: "topleft",
                draw: {
                    marker: true,
                    polygon: true,
                    polyline: true,
                    rectangle: false,
                    circle: false
                },
                edit: {
                    featureGroup: editableLayer.llayer,
                    edit: edit,
                    remove: remove
                }
            };
            this._drawControl = new L.Control.Draw(drawControlOptions);
            this._editableLayer = editableLayer;
            this.onCreated = this.onCreated.bind(this);
            this.onEdited = this.onEdited.bind(this);
            this.onDeleted = this.onDeleted.bind(this);
            this.handleEditStart = this.handleEditStart.bind(this);
            this.handleEditStop = this.handleEditStop.bind(this);
            this.handleDeleteStart = this.handleDeleteStart.bind(this);
            this.handleDeleteStop = this.handleDeleteStop.bind(this);
            this.handleDrawStart = this.handleDrawStart.bind(this);
            this.handleDrawStop = this.handleDrawStop.bind(this);
        }
        DrawControl.prototype.render = function () {
            if (!this._isRendered && this._map.isRendered) {
                this._map.map.addControl(this._drawControl);
                this._editableLayer.addToMap(this._map);
                this.subscribeEvents();
                this._isRendered = true;
            }
        };
        Object.defineProperty(DrawControl.prototype, "drawControl", {
            get: function () {
                return this._drawControl;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "editableLayer", {
            get: function () {
                return this._editableLayer;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "editingEnabled", {
            get: function () {
                return this._editingEnabled;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "deletingEnabled", {
            get: function () {
                return this._deletingEnabled;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "drawingEnabled", {
            get: function () {
                return this._drawingEnabled;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "activated", {
            get: function () {
                return this._editingEnabled || this._deletingEnabled || this._drawingEnabled;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "created", {
            get: function () {
                return this._created;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "edited", {
            get: function () {
                return this._edited;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "deleted", {
            get: function () {
                return this._deleted;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "editStart", {
            get: function () {
                return this._editStart;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(DrawControl.prototype, "editStop", {
            get: function () {
                return this._editStop;
            },
            enumerable: true,
            configurable: true
        });
        DrawControl.prototype.onCreated = function (event) {
            if (!this._created.empty) {
                this._created.notify(this, event);
            }
        };
        DrawControl.prototype.onEdited = function (event) {
            if (!this._edited.empty) {
                this._edited.notify(this, event);
            }
        };
        DrawControl.prototype.onDeleted = function (event) {
            if (!this._deleted.empty) {
                this._deleted.notify(this, event);
            }
        };
        DrawControl.prototype.onEditStart = function (event) {
            if (!this._editStart.empty) {
                this._editStart.notify(this, event);
            }
        };
        DrawControl.prototype.onEditStop = function (event) {
            if (!this._editStop.empty) {
                this._editStop.notify(this, event);
            }
        };
        DrawControl.prototype.handleEditStart = function (event) {
            this._editingEnabled = true;
            this.onEditStart(event);
        };
        DrawControl.prototype.handleEditStop = function (event) {
            this._editingEnabled = false;
            this.onEditStop(event);
        };
        DrawControl.prototype.handleDeleteStart = function (event) {
            this._deletingEnabled = true;
        };
        DrawControl.prototype.handleDeleteStop = function (event) {
            this._deletingEnabled = false;
        };
        DrawControl.prototype.handleDrawStart = function (event) {
            this._drawingEnabled = true;
        };
        DrawControl.prototype.handleDrawStop = function (event) {
            this._drawingEnabled = false;
        };
        DrawControl.prototype.subscribeEvents = function () {
            this._map.map.on("draw:created", this.onCreated);
            this._map.map.on("draw:edited", this.onEdited);
            this._map.map.on("draw:deleted", this.onDeleted);
            this._map.map.on("draw:editstart", this.handleEditStart);
            this._map.map.on("draw:editstop", this.handleEditStop);
            this._map.map.on("draw:deletestart", this.handleDeleteStart);
            this._map.map.on("draw:deletestop", this.handleDeleteStop);
            this._map.map.on("draw:drawstart", this.handleDrawStart);
            this._map.map.on("draw:drawstop", this.handleDrawStop);
        };
        DrawControl.prototype.unsubscribeEvents = function () {
            this._map.map.off("draw:created", this.onCreated);
            this._map.map.off("draw:edited", this.onEdited);
            this._map.map.off("draw:deleted", this.onDeleted);
            this._map.map.off("draw:editstart", this.handleEditStart);
            this._map.map.off("draw:editstop", this.handleEditStop);
            this._map.map.off("draw:deletestart", this.handleDeleteStart);
            this._map.map.off("draw:deletestop", this.handleDeleteStop);
            this._map.map.off("draw:drawstart", this.handleDrawStart);
            this._map.map.off("draw:drawstop", this.handleDrawStop);
        };
        DrawControl.prototype.destroyEvents = function () {
            this._created.clear();
            this._edited.clear();
            this._deleted.clear();
            this._editStart.clear();
            this._editStop.clear();
            this._created = null;
            this._edited = null;
            this._deleted = null;
            this._editStart = null;
            this._editStop = null;
        };
        DrawControl.prototype.destroy = function () {
            if (this._map.isRendered) {
                this._map.map.removeControl(this._drawControl);
                this._editableLayer.removeFromMap(this._map);
                this.unsubscribeEvents();
            }
            this.destroyEvents();
            this._map = null;
            this._editableLayer = null;
            this._drawControl = null;
        };
        return DrawControl;
    }());
    GeoBox.DrawControl = DrawControl;
})(GeoBox || (GeoBox = {}));
/// <reference path="layer.ts" />
var GeoBox;
(function (GeoBox) {
    var EditableLayer = (function (_super) {
        __extends(EditableLayer, _super);
        function EditableLayer(model) {
            _super.call(this, model);
        }
        EditableLayer.create = function () {
            return new EditableLayer(this.createModel());
        };
        EditableLayer.prototype.createLlayer = function () {
            return new GeoBox.EditableLayerGroup();
        };
        EditableLayer.prototype.watch = function (layer) {
            if (layer instanceof GeoBox.ClusterLayer) {
                this.llayer.watchLayerGroup(layer.llayer.featureGroup);
                this.llayer.watchLayerGroup(layer.llayer.nonPointGroup);
            }
            else {
                this.llayer.watchLayerGroup(layer.llayer);
            }
        };
        EditableLayer.prototype.unwatch = function (layer) {
            if (layer instanceof GeoBox.ClusterLayer) {
                this.llayer.unwatchLayerGroup(layer.llayer.featureGroup);
                this.llayer.unwatchLayerGroup(layer.llayer.nonPointGroup);
            }
            else {
                this.llayer.unwatchLayerGroup(layer.llayer);
            }
        };
        EditableLayer.prototype.clear = function () {
            this.llayer.clearLayers();
        };
        return EditableLayer;
    }(GeoBox.Layer));
    GeoBox.EditableLayer = EditableLayer;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var EditableLayerGroup = (function (_super) {
        __extends(EditableLayerGroup, _super);
        function EditableLayerGroup(layers) {
            _super.call(this, layers);
            this.handleAddLayer = this.handleAddLayer.bind(this);
            this.handleRemoveLayer = this.handleRemoveLayer.bind(this);
        }
        //public edit: boolean = false;
        EditableLayerGroup.prototype.watchLayerGroup = function (layerGroup) {
            if (!(layerGroup instanceof L.FeatureGroup)) {
                return;
            }
            layerGroup.on("layeradd", this.handleAddLayer);
            layerGroup.on("layerremove", this.handleRemoveLayer);
            ////layerGroup.on("mapadd", this.handleAddLayer);
            ////layerGroup.on("mapremove", this.handleRemoveLayer);
        };
        EditableLayerGroup.prototype.unwatchLayerGroup = function (layerGroup) {
            if (!(layerGroup instanceof L.FeatureGroup)) {
                return;
            }
            layerGroup.off("layeradd", this.handleAddLayer);
            layerGroup.off("layerremove", this.handleRemoveLayer);
            ////layerGroup.off("mapadd", this.handleAddLayer);
            ////layerGroup.off("mapremove", this.handleRemoveLayer);
        };
        EditableLayerGroup.prototype.unwatchRemoveLayer = function (layerGroup) {
            //if (!(layerGroup instanceof L.FeatureGroup)) {
            //    return;
            //}
            ////layerGroup.off("layeradd", this.handleAddLayer);
            //layerGroup.off("layerremove", this.handleRemoveLayer);
            ////layerGroup.off("mapadd", this.handleAddLayer);
            ////layerGroup.off("mapremove", this.handleRemoveLayer);
        };
        EditableLayerGroup.prototype.addLayer = function (layer) {
            //    //debugger;
            //    console.log("addLayer:");
            //    console.log(layer);
            var _this = this;
            if (layer instanceof L.LayerGroup) {
                var layerGroup = layer;
                layerGroup.eachLayer(function (l) {
                    _this.addLayer(l);
                });
            }
            else {
                if (!(layer instanceof L.MarkerCluster)) {
                    _super.prototype.addLayer.call(this, layer);
                }
            }
            //console.log((this as any).getLayers().length);
            return this;
        };
        EditableLayerGroup.prototype.removeLayer = function (layerOrId) {
            //debugger;
            //console.log("removeLayer:");
            //console.log(layerOrId);
            var _this = this;
            if (layerOrId instanceof L.LayerGroup) {
                var layerGroup = layerOrId;
                layerGroup.eachLayer(function (l) {
                    _this.removeLayer(l);
                });
            }
            else {
                if (!(layerOrId instanceof L.MarkerCluster)) {
                    _super.prototype.removeLayer.call(this, layerOrId);
                }
            }
            //console.log((this as any).getLayers().length);
            return this;
        };
        EditableLayerGroup.prototype.handleAddLayer = function (event) {
            //console.log("handleAddLayer: edit:");
            //    if (this.edit && (!(event.layer instanceof (L as any).MarkerCluster) && (event.layer as any).__parent !== undefined && (event.layer as any).__parent._group !== undefined)) {
            //        console.log("Remove from cluster:");
            //        console.log((event.layer as any).__parent._markers);
            //        this.removeLayer(event.layer);
            //        (event.layer as any).__parent._group.removeLayer(event.layer);
            //    }
            this.addLayer(event.layer);
        };
        EditableLayerGroup.prototype.handleRemoveLayer = function (event) {
            //console.log("handleRemoveLayer:");
            this.removeLayer(event.layer);
        };
        return EditableLayerGroup;
    }(L.FeatureGroup));
    GeoBox.EditableLayerGroup = EditableLayerGroup;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Size = (function () {
        function Size(width, height) {
            this.width = width;
            this.height = height;
        }
        Object.defineProperty(Size, "zero", {
            get: function () {
                return new Size(0, 0);
            },
            enumerable: true,
            configurable: true
        });
        Size.prototype.equals = function (object) {
            return (object instanceof Size) &&
                this.width === object.width &&
                this.height === object.height;
        };
        Size.prototype.clone = function () {
            return new Size(this.width, this.height);
        };
        Size.prototype.toString = function () {
            return "{width:" + this.width + " height:" + this.height + "}";
        };
        return Size;
    }());
    GeoBox.Size = Size;
})(GeoBox || (GeoBox = {}));
/// <reference path="size.ts" />
var GeoBox;
(function (GeoBox) {
    var GeoBoxLayout = (function () {
        function GeoBoxLayout() {
            this._containerNode = null;
            this._layerPanelNode = null;
            this._mapPanelNode = null;
            this._splitter = null;
            this._changeSize = null;
            this._size = GeoBox.Size.zero;
            this._isRendered = false;
            this.handleResizeSplitter = this.handleResizeSplitter.bind(this);
        }
        GeoBoxLayout.prototype.renderTo = function (targetNode) {
            if (targetNode == null) {
                throw new GeoBox.Common.ArgumentNullException("targetNode");
            }
            if (this._isRendered) {
                throw new GeoBox.Common.Exception("The GeoBoxLayout was rendered.");
            }
            this._containerNode = document.createElement("div");
            this._containerNode.className = "geobox";
            this._layerPanelNode = document.createElement("div");
            this._layerPanelNode.className = "geobox-layer-panel";
            this._mapPanelNode = document.createElement("div");
            this._mapPanelNode.className = "geobox-map-panel";
            if (!this._size.equals(GeoBox.Size.zero)) {
                this.updateSize(this._size);
            }
            this._containerNode.appendChild(this._layerPanelNode);
            this._containerNode.appendChild(this._mapPanelNode);
            targetNode.appendChild(this._containerNode);
            this._splitter = new kendo.ui.Splitter(this._containerNode, {
                panes: [
                    {
                        collapsible: true,
                        max: GeoBoxLayout._defaultLayerPanelMaxSize,
                        size: GeoBoxLayout._defaultLayerPanelSize
                    },
                    {
                        collapsible: false,
                        scrollable: false
                    }
                ],
                resize: this.handleResizeSplitter
            });
            this._isRendered = true;
        };
        Object.defineProperty(GeoBoxLayout.prototype, "size", {
            get: function () {
                return this._size.clone();
            },
            set: function (value) {
                if (value != null && value.width > 0 && value.height > 0) {
                    this._size = value.clone();
                    if (this._isRendered) {
                        this.updateSize(this._size);
                    }
                    if (this._splitter === null) {
                        this.onChangeSize();
                    }
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxLayout.prototype, "containerNode", {
            get: function () {
                return this._containerNode;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxLayout.prototype, "layerPanelNode", {
            get: function () {
                return this._layerPanelNode;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxLayout.prototype, "mapPanelNode", {
            get: function () {
                return this._mapPanelNode;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxLayout.prototype, "mapPanelSize", {
            get: function () {
                this.assertRendered();
                return new GeoBox.Size(this._mapPanelNode.offsetWidth, this._mapPanelNode.offsetHeight);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxLayout.prototype, "isRendered", {
            get: function () {
                return this._isRendered;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxLayout.prototype, "changeSize", {
            get: function () {
                return this._changeSize;
            },
            set: function (handler) {
                if (typeof handler === "function") {
                    this._changeSize = handler;
                }
            },
            enumerable: true,
            configurable: true
        });
        GeoBoxLayout.prototype.updateSize = function (size) {
            GeoBox.Common.Debug.assert(this._containerNode !== null);
            GeoBox.Common.Debug.assert(this._mapPanelNode !== null);
            GeoBox.Common.Debug.assert(this._layerPanelNode !== null);
            this._containerNode.style.width = size.width + "px";
            this._containerNode.style.height = size.height + "px";
            this._layerPanelNode.style.height = size.height + "px";
            this._mapPanelNode.style.height = size.height + "px";
            if (this._splitter !== null) {
                this._splitter.resize();
            }
        };
        GeoBoxLayout.prototype.handleResizeSplitter = function () {
            this.onChangeSize();
        };
        GeoBoxLayout.prototype.onChangeSize = function () {
            if (this._changeSize != null) {
                this._changeSize(this);
            }
        };
        GeoBoxLayout.prototype.assertRendered = function () {
            if (!this._isRendered) {
                throw new GeoBox.Common.Exception("The GeoBoxLayout object not rendered.");
            }
        };
        GeoBoxLayout.prototype.destroyContainerNode = function () {
            if (this._containerNode != null) {
                if (this._containerNode.parentElement != null) {
                    this._containerNode.parentElement.removeChild(this._containerNode);
                }
                this._containerNode = null;
            }
        };
        GeoBoxLayout.prototype.destroy = function () {
            if (this._splitter != null) {
                this._splitter.destroy();
                this._splitter = null;
            }
            this.destroyContainerNode();
            this._layerPanelNode = null;
            this._mapPanelNode = null;
            this._size = null;
            this._changeSize = null;
        };
        GeoBoxLayout._defaultLayerPanelSize = "325px";
        GeoBoxLayout._defaultLayerPanelMaxSize = "50%";
        return GeoBoxLayout;
    }());
    GeoBox.GeoBoxLayout = GeoBoxLayout;
})(GeoBox || (GeoBox = {}));
/// <reference path="../typings/tsd.d.ts" /> 
/// <reference path="_references.ts" />
var GeoBox;
(function (GeoBox) {
    var OpenStreetMapLayer = (function (_super) {
        __extends(OpenStreetMapLayer, _super);
        function OpenStreetMapLayer(options) {
            _super.call(this, OpenStreetMapLayer._url, options);
        }
        OpenStreetMapLayer._url = "http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png";
        return OpenStreetMapLayer;
    }(L.TileLayer));
    GeoBox.OpenStreetMapLayer = OpenStreetMapLayer;
})(GeoBox || (GeoBox = {}));
/// <reference path="openstreetmaplayer.ts" />
/// <reference path="size.ts" />
/// <reference path="common/event.ts" />
var GeoBox;
(function (GeoBox) {
    var GeoBoxView = (function () {
        function GeoBoxView(centerOrBounds, zoom) {
            this._isRendered = false;
            this._map = new GeoBox.LMap(centerOrBounds, zoom);
            this._layout = new GeoBox.GeoBoxLayout();
            this._stateManager = new GeoBox.StateManager(this._map);
            this._layerControl = new GeoBox.LayerControl(this._map, this._stateManager);
            this._drawControl = new GeoBox.DrawControl(this._map, this._stateManager.editableLayer, false, false);
            this._geolocationControl = new GeoBox.GeolocationControl(this._map);
            this._viewController = new GeoBox.ViewController(this._map, this._layout, this._drawControl, this._stateManager);
            this.bindEvents();
        }
        GeoBoxView.prototype.bindEvents = function () {
            // Layout Events
            this._layout.changeSize = this.handleChangeLayoutSize.bind(this);
        };
        Object.defineProperty(GeoBoxView.prototype, "center", {
            //#endregion Constructor
            //#region Public Properties
            get: function () {
                return this._map.center;
            },
            set: function (value) {
                this._map.center = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "bounds", {
            get: function () {
                return this._map.bounds;
            },
            set: function (value) {
                this._map.bounds = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "zoom", {
            get: function () {
                return this._map.zoom;
            },
            set: function (value) {
                this._map.zoom = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "map", {
            get: function () {
                return this._map;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "clientSize", {
            get: function () {
                return this._clientSize.clone();
            },
            set: function (value) {
                if (value != null &&
                    value instanceof GeoBox.Size &&
                    value.width > 0 &&
                    value.height > 0) {
                    this._clientSize = value.clone();
                    if (this._isRendered) {
                        this._layout.size = this._clientSize;
                    }
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "layerControl", {
            get: function () {
                return this._layerControl;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "drawControl", {
            get: function () {
                return this._drawControl;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "stateManager", {
            get: function () {
                return this._stateManager;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "layout", {
            get: function () {
                return this._layout;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "isRendered", {
            get: function () {
                return this._isRendered;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "layerDataSource", {
            get: function () {
                return this._stateManager.layerDataSource;
            },
            set: function (value) {
                this._stateManager.layerDataSource = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "geoObjectDataSource", {
            get: function () {
                return this._stateManager.geoObjectDataSource;
            },
            set: function (value) {
                this._stateManager.geoObjectDataSource = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "geoObjectCountDataSource", {
            get: function () {
                return this._stateManager.geoObjectCountDataSource;
            },
            set: function (value) {
                this._stateManager.geoObjectCountDataSource = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "geoObjectCreated", {
            //#endregion Public Properties
            //#region Events
            get: function () {
                return this._viewController.geoObjectCreated;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "geoObjectEdited", {
            get: function () {
                return this._viewController.geoObjectEdited;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "geoObjectDeleted", {
            get: function () {
                return this._viewController.geoObjectDeleted;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "geoObjectDoubleClick", {
            get: function () {
                return this._viewController.geoObjectDoubleClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "geoObjectEditButtonClick", {
            get: function () {
                return this._viewController.geoObjectEditButtonClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(GeoBoxView.prototype, "geoObjectDeleteButtonClick", {
            get: function () {
                return this._stateManager.geoObjectDeleteButtonClick;
            },
            enumerable: true,
            configurable: true
        });
        //#endregion Events
        //#region Public Members
        GeoBoxView.prototype.renderTo = function (targetNode) {
            if (targetNode == null) {
                throw new GeoBox.Common.ArgumentNullException("targetNode");
            }
            if (this._isRendered) {
                throw new GeoBox.Common.Exception("The GeoBoxView was rendered.");
            }
            try {
                // Render Layout
                this._layout.size = this._clientSize;
                this._layout.renderTo(targetNode);
                // Render Map
                this._map.clientSize = this._layout.mapPanelSize;
                this._map.renderTo(this._layout.mapPanelNode);
                // Render Layer Control
                this._layerControl.renderTo(this._layout.layerPanelNode);
                // Render Draw Control
                this._drawControl.render();
                // Render Geolocation Control
                this._geolocationControl.render();
                this._isRendered = true;
            }
            catch (e) {
                throw new GeoBox.Common.Exception("Error render GeoBoxView.", e);
            }
        };
        //#endregion Public Members
        //#region Private Members
        GeoBoxView.prototype.handleChangeLayoutSize = function (sender) {
            if (this._layout.isRendered) {
                this._map.clientSize = this._layout.mapPanelSize;
            }
        };
        //#endregion Private Members
        //#region IDestroyable
        GeoBoxView.prototype.destroy = function () {
            this._viewController.destroy();
            this._viewController = null;
            this._stateManager.destroy();
            this._stateManager = null;
            this._layerControl.destroy();
            this._layerControl = null;
            this._drawControl.destroy();
            this._drawControl = null;
            this._geolocationControl.destroy();
            this._geolocationControl = null;
            this._map.destroy();
            this._map = null;
            this._layout.destroy();
            this._layout = null;
            this._clientSize = null;
        };
        return GeoBoxView;
    }());
    GeoBox.GeoBoxView = GeoBoxView;
})(GeoBox || (GeoBox = {}));
/// <reference path="common/event.ts" />
var GeoBox;
(function (GeoBox) {
    var GeolocationControl = (function () {
        function GeolocationControl(map) {
            this._isRendered = false;
            if (map == null) {
                throw new GeoBox.Common.ArgumentNullException("map");
            }
            this._map = map;
            this._control = new LGeolocationControl();
        }
        GeolocationControl.prototype.render = function () {
            if (!this._isRendered) {
                this.addOnMap();
                this._isRendered = true;
            }
        };
        GeolocationControl.prototype.addOnMap = function () {
            if (!this._map.isRendered) {
                return;
            }
            this._map.originMap.addControl(this._control);
        };
        GeolocationControl.prototype.removeFromMap = function () {
            if (!this._map.isRendered) {
                return;
            }
            this._map.originMap.removeControl(this._control);
        };
        GeolocationControl.prototype.destroy = function () {
            this.removeFromMap();
            this._map = null;
            this._control = null;
        };
        return GeolocationControl;
    }());
    GeoBox.GeolocationControl = GeolocationControl;
    var LGeolocationControl = (function (_super) {
        __extends(LGeolocationControl, _super);
        function LGeolocationControl(options) {
            options = L.Util.extend({
                position: "topleft",
                locateOptions: {
                    enableHighAccuracy: true
                }
            }, options);
            _super.call(this, options);
            this.handleClick = this.handleClick.bind(this);
            this.handleLocationFound = this.handleLocationFound.bind(this);
            this.handleLocationError = this.handleLocationError.bind(this);
        }
        LGeolocationControl.prototype.initialize = function (options) {
            this._isActive = false;
            this.options = options;
        };
        LGeolocationControl.prototype.onAdd = function (map) {
            this._layer = new L.LayerGroup();
            this._layer.addTo(map);
            var container = L.DomUtil.create("div", "leaflet-control-geolocation leaflet-bar leaflet-control");
            var link = L.DomUtil.create("a", "leaflet-bar-part leaflet-bar-part-single", container);
            link.href = "#";
            var icon = L.DomUtil.create("span", "fontello2 fontello2-location", link);
            this._link = link;
            this._container = container;
            L.DomEvent
                .on(link, "click", L.DomEvent.stopPropagation)
                .on(link, "click", L.DomEvent.preventDefault)
                .on(link, "click", this.handleClick, this)
                .on(link, "dblclick", L.DomEvent.stopPropagation);
            return container;
        };
        LGeolocationControl.prototype.onRemove = function (map) {
            map.removeLayer(this._layer);
            this._layer.clearLayers();
            this._layer = null;
            this._link = null;
            this._container = null;
            this._marker = null;
            this._isActive = false;
        };
        LGeolocationControl.prototype.createMarker = function (position) {
            return L.marker(position, {
                icon: L.divIcon({
                    className: "geobox-geolocation",
                    html: "<span class=\"halfling halfling-map-marker\"></span>",
                    iconSize: L.point(40, 40)
                })
            });
        };
        LGeolocationControl.prototype.handleClick = function () {
            if (!navigator || !navigator.geolocation || this._isActive) {
                return;
            }
            if (this._container) {
                L.DomUtil.addClass(this._container, "requesting");
            }
            navigator.geolocation.getCurrentPosition(this.handleLocationFound, this.handleLocationError, this.options.locateOptions);
            this._isActive = true;
        };
        LGeolocationControl.prototype.handleLocationFound = function (position) {
            this._isActive = false;
            this.removeClasses();
            if (!this._map) {
                return;
            }
            var posLatLng = new L.LatLng(position.coords.latitude, position.coords.longitude);
            if (this._marker == null) {
                this._marker = this.createMarker(posLatLng);
                this._layer.addLayer(this._marker);
            }
            else {
                this._marker.setLatLng(posLatLng);
            }
            this._map.setView(posLatLng, LGeolocationControl._zoomLevel);
        };
        LGeolocationControl.prototype.handleLocationError = function (error) {
            this._isActive = false;
            this.removeClasses();
        };
        LGeolocationControl.prototype.removeClasses = function () {
            if (this._container) {
                L.DomUtil.removeClass(this._container, "requesting");
            }
        };
        LGeolocationControl._zoomLevel = 17;
        return LGeolocationControl;
    }(L.Control));
    GeoBox.LGeolocationControl = LGeolocationControl;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var GeoObjectFactory = (function () {
        function GeoObjectFactory() {
        }
        GeoObjectFactory.createFromModel = function (model) {
            if (+model[GeoBox.GeoObject.typeField] === GeoBox.GeoObjectModelType.Cluster) {
                return new GeoBox.ClusterObject(model);
            }
            return new GeoBox.GeoObject(model);
        };
        return GeoObjectFactory;
    }());
    GeoBox.GeoObjectFactory = GeoObjectFactory;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    (function (GeoObjectModelType) {
        GeoObjectModelType[GeoObjectModelType["Object"] = 1] = "Object";
        GeoObjectModelType[GeoObjectModelType["ObjectWithIcon"] = 2] = "ObjectWithIcon";
        GeoObjectModelType[GeoObjectModelType["Cluster"] = 3] = "Cluster";
    })(GeoBox.GeoObjectModelType || (GeoBox.GeoObjectModelType = {}));
    var GeoObjectModelType = GeoBox.GeoObjectModelType;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var GeoObjectUtils = (function () {
        function GeoObjectUtils() {
        }
        GeoObjectUtils.createIcon = function (icon, color) {
            var backgroundColor = "";
            if (color != null) {
                backgroundColor = "background-color:" + color + ";";
            }
            var html = "<div class=\"icon-size-3\" style=\"" + backgroundColor + "\"><span class=\"" + icon + "\"></span></div>";
            return L.divIcon({
                className: "geoobject-icon",
                html: html,
                iconSize: L.point(GeoObjectUtils._defaultIconSize, GeoObjectUtils._defaultIconSize)
            });
        };
        GeoObjectUtils.getPopupContent = function () {
            var popupContent = "<div><strong data-bind=\"text: Title\"></strong></div>";
            popupContent += "<div data-bind=\"text: Description\"></div>";
            popupContent += "<div class=\"edit-btn-wrap\">";
            popupContent += "<button data-role=\"button\" data-bind=\"click: onEditClick\" class=\"edit-btn\">\u0420\u0435\u0434\u0430\u043A\u0442\u0438\u0440\u043E\u0432\u0430\u0442\u044C</button>";
            popupContent += "<button data-role=\"button\" data-bind=\"click: onDeleteClick\" class=\"delete-btn\">\u0423\u0434\u0430\u043B\u0438\u0442\u044C</button>";
            popupContent += "</div>";
            return popupContent;
        };
        GeoObjectUtils._defaultIconSize = 34;
        return GeoObjectUtils;
    }());
    GeoBox.GeoObjectUtils = GeoObjectUtils;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var ObservableArray = kendo.data.ObservableArray;
    var LayerControl = (function () {
        function LayerControl(map, stateManager) {
            this._controlNode = null;
            this._treeView = null;
            this._isRendered = false;
            if (map == null) {
                throw new GeoBox.Common.ArgumentNullException("map");
            }
            if (stateManager == null) {
                throw new GeoBox.Common.ArgumentNullException("stateManager");
            }
            this._map = map;
            this._stateManager = stateManager;
            this.handleCheckLayer = this.handleCheckLayer.bind(this);
        }
        Object.defineProperty(LayerControl.prototype, "treeView", {
            //#region Public Properties
            get: function () {
                return this._treeView;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LayerControl.prototype, "isRendered", {
            get: function () {
                return this._isRendered;
            },
            enumerable: true,
            configurable: true
        });
        //#endregion Public Properties
        //#region Public Members
        LayerControl.prototype.renderTo = function (targetNode) {
            var _this = this;
            if (targetNode == null) {
                throw new GeoBox.Common.ArgumentNullException("targetNode");
            }
            if (this._isRendered) {
                throw new GeoBox.Common.Exception("The LayerControl was rendered.");
            }
            this._controlNode = document.createElement("div");
            this._controlNode.className = "geobox-layer-control";
            targetNode.appendChild(this._controlNode);
            if (this._stateManager.layerDataSource !== null) {
                this._treeView = new kendo.ui.TreeView(this._controlNode, {
                    checkboxes: {
                        checkChildren: true
                    },
                    check: this.handleCheckLayer,
                    dataTextField: GeoBox.Layer.titleField,
                    dataSource: this._stateManager.layerDataSource,
                    autoScroll: false,
                    autoBind: false,
                    template: this.getTreeViewTemplate()
                });
                this._stateManager.layerDataSource.fetch().then(function () {
                    _this.loadTreeNodes();
                    _this._treeView.expand(".k-item");
                    _this._stateManager.loadGeoObjectCounts();
                });
            }
            this._isRendered = true;
        };
        //#endregion Public Members
        LayerControl.prototype.findRecursivelyNodes = function (nodes, result, loadChildren) {
            if (loadChildren === void 0) { loadChildren = false; }
            for (var i = 0; i < nodes.length; i++) {
                result.push(nodes[i]);
                if (loadChildren) {
                    nodes[i].load();
                }
                if (nodes[i].hasChildren) {
                    if (this._stateManager.layerDataSource.options.filter != null) {
                        nodes[i].children.filter(this._stateManager.layerDataSource.options.filter);
                    }
                    this.findRecursivelyNodes(nodes[i].children.data(), result, loadChildren);
                }
            }
        };
        LayerControl.prototype.loadTreeNodes = function () {
            var children = [];
            var nodes = this._stateManager.layerDataSource.data();
            this.findRecursivelyNodes(nodes, children, true);
        };
        //#region Event Handlers
        LayerControl.prototype.handleCheckLayer = function (event) {
            var node = this._treeView.dataItem(event.node);
            if (node == null) {
                return;
            }
            var children = [];
            var nodes = new ObservableArray([node]);
            this.findRecursivelyNodes(nodes, children);
            children.forEach(function (item) {
                if (item !== node) {
                    item.trigger("change", { field: "checked" });
                }
            });
        };
        LayerControl.prototype.getTreeViewTemplate = function () {
            return "\n                # if (item." + GeoBox.Layer.iconField + " != null && item." + GeoBox.Layer.colorField + " != null) { #\n                    <span class=\"#= item." + GeoBox.Layer.iconField + " # geobox-icon\" style=\"color:#= item." + GeoBox.Layer.colorField + " #\"></span>\n                # } else if (item." + GeoBox.Layer.iconField + " != null) { #\n                    <span class=\"#= item." + GeoBox.Layer.iconField + " # geobox-icon\"></span>\n                # } #\n                #= item." + GeoBox.Layer.titleField + " #\n                # if (item." + GeoBox.Layer.countField + " != null) { #\n                    <span class=\"geobox-layer-count\">#= item." + GeoBox.Layer.countField + " #</span>\n                # } #";
        };
        //#endregion Event Handlers
        LayerControl.prototype.destroyControlNode = function () {
            if (this._controlNode != null) {
                if (this._controlNode.parentElement != null) {
                    this._controlNode.parentElement.removeChild(this._controlNode);
                }
                this._controlNode = null;
            }
        };
        LayerControl.prototype.destroyTreeView = function () {
            if (this._treeView != null) {
                this._treeView.destroy();
                this._treeView = null;
            }
        };
        LayerControl.prototype.destroy = function () {
            this.destroyTreeView();
            this.destroyControlNode();
            this._map = null;
            this._stateManager = null;
        };
        LayerControl._templateId = 1;
        return LayerControl;
    }());
    GeoBox.LayerControl = LayerControl;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var LayerFactory = (function () {
        function LayerFactory() {
        }
        LayerFactory.createFromModel = function (model) {
            if (GeoBox.Layer.ClientClustering(model)) {
                return new GeoBox.ClusterLayer(model);
            }
            return new GeoBox.Layer(model);
        };
        return LayerFactory;
    }());
    GeoBox.LayerFactory = LayerFactory;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    (function (LayerMode) {
        LayerMode[LayerMode["Client"] = 1] = "Client";
        LayerMode[LayerMode["Server"] = 2] = "Server";
    })(GeoBox.LayerMode || (GeoBox.LayerMode = {}));
    var LayerMode = GeoBox.LayerMode;
})(GeoBox || (GeoBox = {}));
/// <reference path="size.ts" />
/// <reference path="events/geoobjectlocationeventargs.ts" />
var GeoBox;
(function (GeoBox) {
    var LocationEventArgs = GeoBox.Events.GeoObjectLocationEventArgs;
    var LMap = (function () {
        function LMap(centerOrBounds, zoom) {
            this._map = null;
            this._mapNode = null;
            this._clientSize = LMap._defaultClientSize;
            this._zoom = LMap._defaultZoom;
            this._center = null;
            this._bounds = null;
            this.isDefaultLayer = true;
            // Events
            this._click = new GeoBox.Common.Event();
            this._moveEnd = new GeoBox.Common.Event();
            this._zoomEnd = new GeoBox.Common.Event();
            if (centerOrBounds instanceof L.LatLngBounds) {
                this._bounds = centerOrBounds;
            }
            else if (centerOrBounds instanceof L.LatLng) {
                this._center = centerOrBounds;
            }
            else {
                throw new GeoBox.Common.Exception("Invalid argument: center or bounds");
            }
            if (zoom != null) {
                this._zoom = zoom;
            }
            this.handleClick = this.handleClick.bind(this);
            this.handleMoveEnd = this.handleMoveEnd.bind(this);
            this.handleZoomEnd = this.handleZoomEnd.bind(this);
        }
        Object.defineProperty(LMap.prototype, "click", {
            //#endregion Constructor
            //#region Events
            get: function () {
                return this._click;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "moveEnd", {
            get: function () {
                return this._moveEnd;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "zoomEnd", {
            get: function () {
                return this._zoomEnd;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "center", {
            //#endregion Events
            //#region Public Properties
            get: function () {
                return this._center;
            },
            set: function (value) {
                if (value != null) {
                    this._center = value;
                    if (this._map !== null) {
                        this._map.setView(this._center);
                    }
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "bounds", {
            get: function () {
                return this._bounds;
            },
            set: function (value) {
                if (value != null) {
                    this._bounds = value;
                    if (this._map !== null) {
                        this._map.fitBounds(this._bounds);
                    }
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "zoom", {
            get: function () {
                if (this._map !== null) {
                    this._zoom = this._map.getZoom();
                }
                return this._zoom;
            },
            set: function (value) {
                if (value != null) {
                    this._zoom = value;
                    if (this._map !== null) {
                        this._map.setZoom(this._zoom);
                    }
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "map", {
            get: function () {
                return this._map;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "originMap", {
            get: function () {
                return this._map;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "clientSize", {
            get: function () {
                return this._clientSize.clone();
            },
            set: function (value) {
                if (value != null &&
                    value instanceof GeoBox.Size &&
                    value.width > 0 &&
                    value.height > 0) {
                    this._clientSize = value.clone();
                    if (this._map !== null) {
                        this.updateClientSize(this._clientSize);
                    }
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(LMap.prototype, "isRendered", {
            get: function () {
                return this._map != null && this._mapNode != null;
            },
            enumerable: true,
            configurable: true
        });
        //#endregion Public Properties
        //#region Render Members
        LMap.prototype.renderTo = function (targetNode) {
            this.assertMapNotCreated();
            if (targetNode == null) {
                throw new GeoBox.Common.ArgumentNullException("containerNode");
            }
            try {
                var mapNode = this.createMapNode(this._clientSize);
                targetNode.appendChild(mapNode);
                this.createMap(mapNode);
                this.removeAttributionNode();
            }
            catch (e) {
                throw new GeoBox.Common.Exception("Error render LMap.", e);
            }
        };
        LMap.prototype.setView = function (center, zoom, options) {
            if (this._map != null) {
                this._map.setView(center, zoom, options);
            }
        };
        //#endregion Render Members
        //#region Private Members
        LMap.prototype.createMap = function (mapNode) {
            this._map = new L.Map(mapNode, {
                zoomAnimation: true
            });
            this._mapNode = this._map.getContainer();
            if (this._zoom !== null) {
                this._map.setZoom(this._zoom);
            }
            if (this._center !== null) {
                this._map.setView(this._center);
            }
            if (this._bounds !== null) {
                this._map.fitBounds(this._bounds);
            }
            if (this.isDefaultLayer) {
                var defaultLayer = new GeoBox.OpenStreetMapLayer();
                this._map.addLayer(defaultLayer);
            }
            this.bindEvents();
        };
        LMap.prototype.createMapNode = function (size) {
            var mapNode = document.createElement("div");
            mapNode.id = "map_" + LMap._mapNodeNextId++;
            mapNode.className = LMap._mapNodeClassName;
            mapNode.style.width = size.width + "px";
            mapNode.style.height = size.height + "px";
            return mapNode;
        };
        LMap.prototype.updateClientSize = function (size) {
            this._mapNode.style.width = size.width + "px";
            this._mapNode.style.height = size.height + "px";
            this._map.invalidateSize(false);
        };
        LMap.prototype.removeAttributionNode = function () {
            var attributionNode = document.querySelector(".leaflet-control-attribution");
            if (attributionNode.parentElement != null) {
                attributionNode.parentElement.removeChild(attributionNode);
            }
        };
        LMap.prototype.bindEvents = function () {
            this._map.on("click", this.handleClick);
            this._map.on("moveend", this.handleMoveEnd);
            this._map.on("zoomend", this.handleZoomEnd);
        };
        LMap.prototype.unbindEvents = function () {
            this._map.off("click", this.handleClick);
            this._map.off("moveend", this.handleMoveEnd);
            this._map.off("zoomend", this.handleZoomEnd);
        };
        LMap.prototype.handleClick = function (event) {
            if (!this._click.empty) {
                var args = new LocationEventArgs();
                args.location = event.latlng;
                this._click.notify(this, args);
            }
        };
        LMap.prototype.handleMoveEnd = function (event) {
            if (!this._moveEnd.empty) {
                this._moveEnd.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        LMap.prototype.handleZoomEnd = function (event) {
            if (!this._zoomEnd.empty) {
                this._zoomEnd.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        //#endregion Private Members
        //#region Assert Helpers
        LMap.prototype.assertMapCreated = function () {
            if (this._map === null) {
                throw new GeoBox.Common.Exception("The map has not created. Please call renderTo() method.");
            }
        };
        LMap.prototype.assertMapNotCreated = function () {
            if (this._map !== null) {
                throw new GeoBox.Common.Exception("The map was created.");
            }
        };
        //#endregion Assert Helpers
        LMap.prototype.destroyEvents = function () {
            this._click.clear();
            this._moveEnd.clear();
            this._zoomEnd.clear();
            this._click = null;
            this._moveEnd = null;
            this._zoomEnd = null;
        };
        LMap.prototype.destroyMap = function () {
            if (this._map !== null) {
                this.unbindEvents();
                this._map.remove();
                this._map = null;
            }
        };
        LMap.prototype.destroyMapNode = function () {
            if (this._mapNode != null) {
                if (this._mapNode.parentElement != null) {
                    this._mapNode.parentElement.removeChild(this._mapNode);
                }
                this._mapNode = null;
            }
        };
        //#region IDestroyable
        LMap.prototype.destroy = function () {
            this.destroyMap();
            this.destroyMapNode();
            this.destroyEvents();
            this._clientSize = null;
            this._bounds = null;
            this._center = null;
        };
        LMap._mapNodeNextId = 1;
        LMap._mapNodeClassName = "geobox-map";
        LMap._defaultZoom = 5;
        LMap._defaultClientSize = new GeoBox.Size(500, 400);
        return LMap;
    }());
    GeoBox.LMap = LMap;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var MarkerClusterGroup = (function (_super) {
        __extends(MarkerClusterGroup, _super);
        function MarkerClusterGroup(options) {
            _super.call(this, options);
        }
        Object.defineProperty(MarkerClusterGroup.prototype, "featureGroup", {
            get: function () {
                return this._featureGroup;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(MarkerClusterGroup.prototype, "nonPointGroup", {
            get: function () {
                return this._nonPointGroup;
            },
            enumerable: true,
            configurable: true
        });
        return MarkerClusterGroup;
    }(L.MarkerClusterGroup));
    GeoBox.MarkerClusterGroup = MarkerClusterGroup;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var ProgressBar = (function () {
        function ProgressBar() {
            this._isRendered = false;
        }
        Object.defineProperty(ProgressBar.prototype, "isRendered", {
            //#region Public Members
            get: function () {
                return this._isRendered;
            },
            enumerable: true,
            configurable: true
        });
        ProgressBar.prototype.show = function () {
            if (this._isRendered) {
                this._progressBarNode.style.display = "block";
            }
        };
        ProgressBar.prototype.hide = function () {
            if (this._isRendered) {
                this._progressBarNode.style.display = "none";
            }
        };
        ProgressBar.prototype.renderTo = function (targetNode) {
            if (targetNode == null) {
                throw new GeoBox.Common.ArgumentNullException("targetNode");
            }
            if (this._isRendered) {
                throw new GeoBox.Common.Exception("The ProgressBar was rendered.");
            }
            this._progressBarNode = document.createElement("div");
            this._progressBarNode.className = "geobox-progressbar";
            this._progressBarNode.style.display = "none";
            this._progressBarNode.innerHTML = "\n                <svg width=\"32px\" height=\"32px\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 100 100\" preserveAspectRatio=\"xMidYMid\" class=\"geobox-progressbar-svg\">\n                    <rect x=\"0\" y=\"0\" width=\"100\" height=\"100\" fill=\"none\"></rect>\n                    <circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"#c5c5cf\" fill=\"none\" stroke-width=\"10\" stroke-linecap=\"round\"></circle>\n                    <circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"#3f51b5\" fill=\"none\" stroke-width=\"6\" stroke-linecap=\"round\">\n                        <animate attributeName=\"stroke-dashoffset\" dur=\"2s\" repeatCount=\"indefinite\" from=\"0\" to=\"502\"></animate>\n                        <animate attributeName=\"stroke-dasharray\" dur=\"2s\" repeatCount=\"indefinite\" values=\"150.6 100.4;1 250;150.6 100.4\"></animate>\n                    </circle>\n                </svg>";
            targetNode.appendChild(this._progressBarNode);
            this._isRendered = true;
        };
        ProgressBar.prototype.destroy = function () {
            if (this._progressBarNode != null) {
                if (this._progressBarNode.parentNode != null) {
                    this._progressBarNode.parentNode.removeChild(this._progressBarNode);
                }
                this._progressBarNode = null;
            }
        };
        //#endregion Public Members
        //#region Static Members
        ProgressBar.show = function (domNode) {
            var instance = ProgressBar.findInstanceForNode(domNode);
            if (instance != null) {
                instance.show();
            }
            else {
                var newInstance = new ProgressBar();
                newInstance.renderTo(domNode);
                newInstance.show();
                this._instances.push({
                    key: domNode,
                    value: newInstance
                });
            }
        };
        ProgressBar.hide = function (domNode) {
            var instance = ProgressBar.findInstanceForNode(domNode);
            if (instance != null) {
                instance.hide();
            }
        };
        ProgressBar.findInstanceForNode = function (domNode) {
            var result = this._instances.filter(function (v) { return v.key === domNode; });
            return result.length > 0 ? result[0].value : null;
        };
        ProgressBar.destroyAll = function () {
            for (var _i = 0, _a = this._instances; _i < _a.length; _i++) {
                var pair = _a[_i];
                pair.value.destroy();
            }
            this._instances = [];
        };
        ProgressBar._instances = [];
        return ProgressBar;
    }());
    GeoBox.ProgressBar = ProgressBar;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var EventArgs = (function () {
            function EventArgs() {
            }
            Object.defineProperty(EventArgs, "empty", {
                get: function () {
                    return new EventArgs();
                },
                enumerable: true,
                configurable: true
            });
            return EventArgs;
        }());
        Common.EventArgs = EventArgs;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var DataSources;
    (function (DataSources) {
        var DataSource = kendo.data.DataSource;
        var GeoObjectCountDataSource = (function (_super) {
            __extends(GeoObjectCountDataSource, _super);
            function GeoObjectCountDataSource(url, layerIdField) {
                if (url == null) {
                    throw new GeoBox.Common.Exception("Url is required.");
                }
                var options = {
                    transport: {
                        read: {
                            url: url,
                            dataType: "json"
                        }
                    },
                    schema: {
                        data: function (response) {
                            return [response];
                        },
                        errors: "error"
                    }
                };
                _super.call(this, options);
                this.transport.options.read.data = this.handleGetRequestQuery.bind(this);
                this.layerIdField = layerIdField == null ? null : layerIdField;
            }
            GeoObjectCountDataSource.prototype.handleGetRequestQuery = function () {
                var requestQuery = {};
                if (this.layerId != null) {
                    this.layerIdField = this.layerIdField !== null ? this.layerIdField : DataSources.GeoObjectDataSource.defaultLayerIdField;
                    if (this.layerIdField === null) {
                        throw new GeoBox.Common.Exception("layerIdField property could not be emtpy.");
                    }
                    if (this.layerId instanceof Array) {
                        var layerIds_1 = {};
                        this.layerId.forEach(function (v, k) { return layerIds_1[k] = v; });
                        requestQuery[this.layerIdField] = layerIds_1;
                    }
                    else {
                        requestQuery[this.layerIdField] = this.layerId;
                    }
                }
                return requestQuery;
            };
            GeoObjectCountDataSource.defaultLayerIdField = null;
            return GeoObjectCountDataSource;
        }(DataSource));
        DataSources.GeoObjectCountDataSource = GeoObjectCountDataSource;
    })(DataSources = GeoBox.DataSources || (GeoBox.DataSources = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var DataSources;
    (function (DataSources) {
        var HierarchicalDataSource = kendo.data.HierarchicalDataSource;
        var GeoLayerHierarchicalDataSource = (function (_super) {
            __extends(GeoLayerHierarchicalDataSource, _super);
            function GeoLayerHierarchicalDataSource(url) {
                if (url == null || typeof url !== "string") {
                    throw new GeoBox.Common.ArgumentNullException("url");
                }
                var options = {
                    transport: {
                        read: {
                            url: url,
                            dataType: "json"
                        }
                    },
                    schema: {
                        model: GeoBox.LayerModel
                    },
                    filter: {
                        field: GeoBox.Layer.searchOnClickField,
                        operator: "neq",
                        value: true
                    }
                };
                _super.call(this, options);
            }
            return GeoLayerHierarchicalDataSource;
        }(HierarchicalDataSource));
        DataSources.GeoLayerHierarchicalDataSource = GeoLayerHierarchicalDataSource;
    })(DataSources = GeoBox.DataSources || (GeoBox.DataSources = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var DataSources;
    (function (DataSources) {
        var DataSource = kendo.data.DataSource;
        var GeoObjectDataSource = (function (_super) {
            __extends(GeoObjectDataSource, _super);
            function GeoObjectDataSource(urlOrCrudUrls, layerIdField) {
                var _this = this;
                if (urlOrCrudUrls == null) {
                    throw new GeoBox.Common.ArgumentNullException("urlOrCrudUrls");
                }
                if (typeof urlOrCrudUrls !== "string" && !Array.isArray(urlOrCrudUrls)) {
                    throw new GeoBox.Common.ArgumentNullException("urlOrCrudUrls must be string or array.");
                }
                if (Array.isArray(urlOrCrudUrls) && urlOrCrudUrls.length === 0) {
                    throw new GeoBox.Common.ArgumentNullException("urlOrCrudUrls must be not empty array.");
                }
                var readUrl = null;
                var createUrl = null;
                var updateUrl = null;
                var deleteUrl = null;
                if (typeof urlOrCrudUrls === "string") {
                    readUrl = urlOrCrudUrls;
                }
                else {
                    if (typeof urlOrCrudUrls[0] === "string") {
                        createUrl = urlOrCrudUrls[0];
                    }
                    if (typeof urlOrCrudUrls[1] === "string") {
                        readUrl = urlOrCrudUrls[1];
                    }
                    if (typeof urlOrCrudUrls[2] === "string") {
                        updateUrl = urlOrCrudUrls[2];
                    }
                    if (typeof urlOrCrudUrls[3] === "string") {
                        deleteUrl = urlOrCrudUrls[3];
                    }
                }
                if (readUrl === null) {
                    throw new GeoBox.Common.Exception("Read url is required.");
                }
                var options = {
                    transport: {
                        read: {
                            url: readUrl,
                            dataType: "json"
                        },
                        create: function (e) {
                            e.success(e.data);
                        },
                        update: function (e) {
                            e.success();
                        },
                        destroy: function (e) {
                            e.success();
                        },
                        parameterMap: function (opt, operation) {
                            if (operation !== "read" && opt.models) {
                                var params = _this.handleGetRequestQuery();
                                params["models"] = kendo.stringify(opt.models);
                                return params;
                            }
                            return opt;
                        }
                    },
                    batch: true,
                    schema: {
                        model: GeoBox.GeoObjectModel,
                        errors: "error"
                    }
                };
                if (createUrl !== null) {
                    options.transport.create = {
                        url: createUrl,
                        type: "post",
                        dataType: "json"
                    };
                }
                if (updateUrl !== null) {
                    options.transport.update = {
                        url: updateUrl,
                        type: "post",
                        dataType: "json"
                    };
                }
                if (deleteUrl !== null) {
                    options.transport.destroy = {
                        url: deleteUrl,
                        type: "post",
                        dataType: "json"
                    };
                }
                _super.call(this, options);
                this.transport.options.read.data = this.handleGetRequestQuery.bind(this);
                this._readUrl = readUrl;
                this._createUrl = createUrl;
                this._updateUrl = updateUrl;
                this._deleteUrl = deleteUrl;
                this.layerIdField = layerIdField == null ? null : layerIdField;
            }
            GeoObjectDataSource.prototype.handleGetRequestQuery = function () {
                var requestQuery = {};
                if (this.layerId != null) {
                    this.layerIdField = this.layerIdField !== null ? this.layerIdField : GeoObjectDataSource.defaultLayerIdField;
                    if (this.layerIdField === null) {
                        throw new GeoBox.Common.Exception("layerIdField property could not be emtpy.");
                    }
                    requestQuery[this.layerIdField] = this.layerId;
                }
                if (this.bbox != null) {
                    var bbox_1 = {};
                    this.bbox.forEach(function (v, k) { return bbox_1[k] = v; });
                    requestQuery["bbox"] = bbox_1;
                }
                if (this.point != null) {
                    var point_1 = {};
                    this.point.forEach(function (v, k) { return point_1[k] = v; });
                    requestQuery["point"] = point_1;
                }
                if (this.zoom != null) {
                    requestQuery["zoom"] = this.zoom;
                }
                if (this.single != null) {
                    requestQuery["single"] = this.single;
                }
                return requestQuery;
            };
            GeoObjectDataSource.prototype.clone = function () {
                var crudUrls = [this._createUrl, this._readUrl, this._updateUrl, this._deleteUrl];
                return new GeoObjectDataSource(crudUrls, this.layerIdField);
            };
            GeoObjectDataSource.defaultLayerIdField = null;
            return GeoObjectDataSource;
        }(DataSource));
        DataSources.GeoObjectDataSource = GeoObjectDataSource;
    })(DataSources = GeoBox.DataSources || (GeoBox.DataSources = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="common/event.ts" />
/// <reference path="common/eventargs.ts" />
/// <reference path="events/geoobjectlocationeventargs.ts" />
/// <reference path="datasources/geoobjectcountdatasource.ts" />
/// <reference path="datasources/geolayerhierarchicaldatasource.ts" />
/// <reference path="datasources/geoobjectdatasource.ts" />
var GeoBox;
(function (GeoBox) {
    var DataSource = kendo.data.DataSource;
    var HierarchicalDataSource = kendo.data.HierarchicalDataSource;
    var LocationEventArgs = GeoBox.Events.GeoObjectLocationEventArgs;
    var GeoObjectDataSource = GeoBox.DataSources.GeoObjectDataSource;
    var GeoObjectCountDataSource = GeoBox.DataSources.GeoObjectCountDataSource;
    var DataSourceEventArgs = (function () {
        function DataSourceEventArgs() {
            this.origin = null;
        }
        return DataSourceEventArgs;
    }());
    GeoBox.DataSourceEventArgs = DataSourceEventArgs;
    var LayerEventArgs = (function () {
        function LayerEventArgs() {
            this.layer = null;
        }
        return LayerEventArgs;
    }());
    GeoBox.LayerEventArgs = LayerEventArgs;
    var GeoObjectEventArgs = (function () {
        function GeoObjectEventArgs() {
            this.geoObject = null;
        }
        return GeoObjectEventArgs;
    }());
    GeoBox.GeoObjectEventArgs = GeoObjectEventArgs;
    var GeoObjectRequestEventArgs = (function () {
        function GeoObjectRequestEventArgs() {
            this.layer = null;
        }
        return GeoObjectRequestEventArgs;
    }());
    GeoBox.GeoObjectRequestEventArgs = GeoObjectRequestEventArgs;
    var StateManager = (function () {
        function StateManager(map) {
            this._layerDataSource = null;
            this._geoObjectCountDataSource = null;
            this._geoObjectDataSourcePrototype = null;
            this._geoObjectDataSources = {};
            this._layers = {};
            this._geoObjects = {};
            // Layer Events
            this._layerRequestStart = new GeoBox.Common.Event();
            this._layerRequestEnd = new GeoBox.Common.Event();
            this._layerShow = new GeoBox.Common.Event();
            this._layerHide = new GeoBox.Common.Event();
            this._layerBatchLoadStart = new GeoBox.Common.Event();
            this._layerBatchLoadEnd = new GeoBox.Common.Event();
            // GeoObject Events
            this._geoObjectRequestStart = new GeoBox.Common.Event();
            this._geoObjectRequestEnd = new GeoBox.Common.Event();
            this._geoObjectDoubleClick = new GeoBox.Common.Event();
            this._geoObjectPolygonClick = new GeoBox.Common.Event();
            this._geoObjectEditButtonClick = new GeoBox.Common.Event();
            this._geoObjectDeleteButtonClick = new GeoBox.Common.Event();
            this._geoObjectClick = new GeoBox.Common.Event();
            this._requestError = new GeoBox.Common.Event();
            if (map == null) {
                throw new GeoBox.Common.ArgumentNullException("map");
            }
            this._map = map;
            this._editableLayer = GeoBox.EditableLayer.create();
            this.handleChangeLayerDataSource = this.handleChangeLayerDataSource.bind(this);
            this.handleLayerRequestStart = this.handleLayerRequestStart.bind(this);
            this.handleLayerRequestEnd = this.handleLayerRequestEnd.bind(this);
            this.handleLayerBatchLoadStart = this.handleLayerBatchLoadStart.bind(this);
            this.handleLayerBatchLoadEnd = this.handleLayerBatchLoadEnd.bind(this);
            this.handleChangeGeoObjectDataSource = this.handleChangeGeoObjectDataSource.bind(this);
            this.handleGeoObjectRequestStart = this.handleGeoObjectRequestStart.bind(this);
            this.handleGeoObjectRequestEnd = this.handleGeoObjectRequestEnd.bind(this);
            this.handleGeoObjectDoubleClick = this.handleGeoObjectDoubleClick.bind(this);
            this.handleGeoObjectPolygonClick = this.handleGeoObjectPolygonClick.bind(this);
            this.handleGeoObjectEditButtonClick = this.handleGeoObjectEditButtonClick.bind(this);
            this.handleGeoObjectDeleteButtonClick = this.handleGeoObjectDeleteButtonClick.bind(this);
            this.handleGeoObjectClick = this.handleGeoObjectClick.bind(this);
            this.handleRequestError = this.handleRequestError.bind(this);
        }
        Object.defineProperty(StateManager.prototype, "layerRequestStart", {
            //#region Layer Events
            get: function () {
                return this._layerRequestStart;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "layerRequestEnd", {
            get: function () {
                return this._layerRequestEnd;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "layerShow", {
            get: function () {
                return this._layerShow;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "layerHide", {
            get: function () {
                return this._layerHide;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "layerBatchLoadStart", {
            get: function () {
                return this._layerBatchLoadStart;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "layerBatchLoadEnd", {
            get: function () {
                return this._layerBatchLoadEnd;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectRequestStart", {
            //#endregion Layer Events
            //#region GeoObject Events
            get: function () {
                return this._geoObjectRequestStart;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectRequestEnd", {
            get: function () {
                return this._geoObjectRequestEnd;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectDoubleClick", {
            get: function () {
                return this._geoObjectDoubleClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectPolygonClick", {
            get: function () {
                return this._geoObjectPolygonClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectEditButtonClick", {
            get: function () {
                return this._geoObjectEditButtonClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectDeleteButtonClick", {
            get: function () {
                return this._geoObjectDeleteButtonClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectClick", {
            get: function () {
                return this._geoObjectClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "requestError", {
            get: function () {
                return this._requestError;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "editableLayer", {
            //#endregion GeoObject Events
            //#region Public Properties
            get: function () {
                return this._editableLayer;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "layerDataSource", {
            get: function () {
                return this._layerDataSource;
            },
            set: function (value) {
                if (value == null || !(value instanceof HierarchicalDataSource)) {
                    throw new GeoBox.Common.ArgumentNullException("value");
                }
                if (this._layerDataSource !== null) {
                    this.unbindLayerDataSource(this._layerDataSource);
                }
                this._layerDataSource = value;
                this.bindLayerDataSource(this._layerDataSource);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectDataSource", {
            get: function () {
                return this._geoObjectDataSourcePrototype;
            },
            set: function (value) {
                if (value == null || !(value instanceof DataSource)) {
                    throw new GeoBox.Common.ArgumentNullException("value");
                }
                this._geoObjectDataSourcePrototype = value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(StateManager.prototype, "geoObjectCountDataSource", {
            get: function () {
                return this._geoObjectCountDataSource;
            },
            set: function (value) {
                if (value == null || !(value instanceof DataSource)) {
                    throw new GeoBox.Common.ArgumentNullException("value");
                }
                if (this._geoObjectCountDataSource !== null) {
                    this.unbindGeoObjectCountDataSource(this._geoObjectCountDataSource);
                }
                this._geoObjectCountDataSource = value;
                this.bindGeoObjectCountDataSource(this._geoObjectCountDataSource);
            },
            enumerable: true,
            configurable: true
        });
        //#endregion Public Properties
        //#region Public Members
        StateManager.prototype.getGeoObjectDataSource = function (layerId) {
            if (layerId == null) {
                throw new GeoBox.Common.ArgumentNullException("layerId");
            }
            return this._geoObjectDataSources[layerId] || null;
        };
        //#endregion Public Members
        StateManager.prototype.bindLayerDataSource = function (dataSource) {
            dataSource.bind("change", this.handleChangeLayerDataSource);
            dataSource.bind("requestStart", this.handleLayerRequestStart);
            dataSource.bind("requestEnd", this.handleLayerRequestEnd);
            dataSource.bind("error", this.handleRequestError);
        };
        StateManager.prototype.unbindLayerDataSource = function (dataSource) {
            dataSource.unbind("change", this.handleChangeLayerDataSource);
            dataSource.unbind("requestStart", this.handleLayerRequestStart);
            dataSource.unbind("requestEnd", this.handleLayerRequestEnd);
            dataSource.unbind("error", this.handleRequestError);
        };
        StateManager.prototype.bindLayerEvents = function (layer) {
            layer.batchLoadStart.add(this.handleLayerBatchLoadStart);
            layer.batchLoadEnd.add(this.handleLayerBatchLoadEnd);
        };
        StateManager.prototype.unbindLayerEvents = function (layer) {
            layer.batchLoadStart.remove(this.handleLayerBatchLoadStart);
            layer.batchLoadEnd.remove(this.handleLayerBatchLoadEnd);
        };
        StateManager.prototype.bindGeoObjectDataSource = function (dataSource) {
            dataSource.bind("change", this.handleChangeGeoObjectDataSource);
            dataSource.bind("requestStart", this.handleGeoObjectRequestStart);
            dataSource.bind("requestEnd", this.handleGeoObjectRequestEnd);
            dataSource.bind("error", this.handleRequestError);
        };
        StateManager.prototype.unbindGeoObjectDataSource = function (dataSource) {
            dataSource.unbind("change", this.handleChangeGeoObjectDataSource);
            dataSource.unbind("requestStart", this.handleGeoObjectRequestStart);
            dataSource.unbind("requestEnd", this.handleGeoObjectRequestEnd);
            dataSource.unbind("error", this.handleRequestError);
        };
        StateManager.prototype.bindGeoObjectEvents = function (geoObject) {
            geoObject.doubleClick.add(this.handleGeoObjectDoubleClick);
            geoObject.polygonClick.add(this.handleGeoObjectPolygonClick);
            geoObject.editButtonClick.add(this.handleGeoObjectEditButtonClick);
            geoObject.deleteButtonClick.add(this.handleGeoObjectDeleteButtonClick);
            geoObject.click.add(this.handleGeoObjectClick);
        };
        StateManager.prototype.unbindGeoObjectEvents = function (geoObject) {
            geoObject.doubleClick.remove(this.handleGeoObjectDoubleClick);
            geoObject.polygonClick.remove(this.handleGeoObjectPolygonClick);
            geoObject.editButtonClick.remove(this.handleGeoObjectEditButtonClick);
            geoObject.deleteButtonClick.remove(this.handleGeoObjectDeleteButtonClick);
            geoObject.click.remove(this.handleGeoObjectClick);
        };
        StateManager.prototype.bindGeoObjectCountDataSource = function (dataSource) {
            dataSource.bind("error", this.handleRequestError);
        };
        StateManager.prototype.unbindGeoObjectCountDataSource = function (dataSource) {
            dataSource.unbind("error", this.handleRequestError);
        };
        //#region Event Handlers
        StateManager.prototype.handleChangeLayerDataSource = function (event) {
            if (event.action === undefined) {
                this.removeAllLayers();
                this.addLayersIfNotExists(event.items);
            }
            else {
                switch (event.action) {
                    case "itemchange":
                        this.addLayersIfNotExists(event.items);
                        if (event.field === "checked") {
                            this.checkLayers(event.items);
                        }
                        break;
                    case "itemloaded":
                        this.addLayersIfNotExists(event.items);
                        break;
                    case "add":
                        this.addLayersIfNotExists(event.items);
                        break;
                    case "remove":
                        this.removeLayers(event.items);
                        break;
                    case "sync":
                        break;
                }
            }
        };
        StateManager.prototype.handleChangeGeoObjectDataSource = function (event) {
            if (!(event.sender instanceof GeoObjectDataSource)) {
                return;
            }
            var layerId = event.sender.layerId;
            if (event.action === undefined) {
                this.removeAllGeoObjectsFromLayer(layerId);
                this.addGeoObjectsIfNotExists(layerId, event.items);
            }
            else {
                switch (event.action) {
                    case "itemchange":
                        break;
                    case "add":
                        this.addGeoObjectsIfNotExists(layerId, event.items);
                        break;
                    case "remove":
                        this.removeGeoObjectsFromLayer(layerId, event.items);
                        break;
                    case "sync":
                        break;
                }
            }
        };
        StateManager.prototype.handleLayerRequestStart = function (event) {
            if (!this._layerRequestStart.empty) {
                this._layerRequestStart.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        StateManager.prototype.handleLayerRequestEnd = function (event) {
            if (!this._layerRequestEnd.empty) {
                this._layerRequestEnd.notify(this, GeoBox.Common.EventArgs.empty);
            }
        };
        StateManager.prototype.handleLayerBatchLoadStart = function (sender) {
            if (!this._layerBatchLoadStart.empty) {
                var args = new LayerEventArgs();
                args.layer = sender;
                this._layerBatchLoadStart.notify(this, args);
            }
        };
        StateManager.prototype.handleLayerBatchLoadEnd = function (sender) {
            if (!this._layerBatchLoadEnd.empty) {
                var args = new LayerEventArgs();
                args.layer = sender;
                this._layerBatchLoadEnd.notify(this, args);
            }
        };
        StateManager.prototype.handleGeoObjectRequestStart = function (event) {
            if (!this._geoObjectRequestStart.empty) {
                var args = new GeoObjectRequestEventArgs();
                args.layer = this.getLayer(event.sender.layerId);
                this._geoObjectRequestStart.notify(this, args);
            }
        };
        StateManager.prototype.handleGeoObjectRequestEnd = function (event) {
            if (!this._geoObjectRequestEnd.empty) {
                var args = new GeoObjectRequestEventArgs();
                args.layer = this.getLayer(event.sender.layerId);
                this._geoObjectRequestEnd.notify(this, args);
            }
        };
        StateManager.prototype.handleRequestError = function (event) {
            if (!this._requestError.empty) {
                var args = new DataSourceEventArgs();
                args.origin = event;
                this._requestError.notify(this, args);
            }
        };
        StateManager.prototype.handleGeoObjectDoubleClick = function (sender, event) {
            if (!this._geoObjectDoubleClick.empty) {
                var args = new GeoObjectEventArgs();
                args.geoObject = sender;
                this._geoObjectDoubleClick.notify(this, args);
            }
        };
        StateManager.prototype.handleGeoObjectPolygonClick = function (sender, event) {
            if (!this._geoObjectPolygonClick.empty) {
                var args = new LocationEventArgs();
                args.location = event.location;
                this._geoObjectPolygonClick.notify(this, args);
            }
        };
        StateManager.prototype.handleGeoObjectEditButtonClick = function (sender, event) {
            if (!this._geoObjectEditButtonClick.empty) {
                var args = new GeoObjectEventArgs();
                args.geoObject = sender;
                this._geoObjectEditButtonClick.notify(this, args);
            }
        };
        StateManager.prototype.handleGeoObjectDeleteButtonClick = function (sender, event) {
            if (!this._geoObjectDeleteButtonClick.empty) {
                var geoObject = sender;
                var args = new GeoBox.Events.GeoObjectDeleteEventArgs();
                args.layer = geoObject.layer;
                args.model = geoObject.model;
                args.dataSource = this.getGeoObjectDataSource(geoObject.layer.id);
                args.dataSource.remove(geoObject.model);
                this._geoObjectDeleteButtonClick.notify(this, args);
            }
        };
        StateManager.prototype.handleGeoObjectClick = function (sender, event) {
            if (!this._geoObjectClick.empty) {
                var args = new GeoObjectEventArgs();
                args.geoObject = sender;
                this._geoObjectClick.notify(this, args);
            }
        };
        StateManager.prototype.onShowLayer = function (layer) {
            if (!this._layerShow.empty) {
                var args = new LayerEventArgs();
                args.layer = layer;
                this._layerShow.notify(this, args);
            }
        };
        StateManager.prototype.onHideLayer = function (layer) {
            if (!this._layerHide.empty) {
                var args = new LayerEventArgs();
                args.layer = layer;
                this._layerHide.notify(this, args);
            }
        };
        //#endregion Event Handlers
        //#region Manage Layer
        StateManager.prototype.addLayersIfNotExists = function (items) {
            var _this = this;
            this.forEachItems(items, function (item) {
                var id = item.id;
                if (_this._layers[id] === undefined) {
                    var layer = GeoBox.LayerFactory.createFromModel(item);
                    _this._layers[layer.id] = layer;
                    //this._editableLayer.watch(layer);
                    _this.bindLayerEvents(layer);
                    _this.createGeoObjectDataSource(layer.id);
                    layer.needReload = true;
                    layer.isVisible = layer.searchable || layer.checked;
                }
            });
        };
        StateManager.prototype.removeLayers = function (items) {
            var _this = this;
            this.forEachItems(items, function (item) {
                var id = item.id;
                if (_this._layers[id] !== undefined) {
                    var layer = _this._layers[id];
                    layer.removeFromMap(_this._map);
                    //this._editableLayer.unwatch(layer);
                    _this.removeGeoObjectDataSource(layer.id);
                    _this.removeAllGeoObjectsFromLayer(layer.id);
                    layer.destroy();
                    delete _this._layers[id];
                }
            });
        };
        StateManager.prototype.removeAllLayers = function () {
            this.removeLayers(this.getLayers().map(function (l) { return l.model; }));
        };
        StateManager.prototype.clearLayer = function (layer) {
            this.removeAllGeoObjectsFromLayer(layer.id);
        };
        StateManager.prototype.layerInZoom = function (layer) {
            return GeoBox.Utils.inZoom(this._map.zoom, layer.minSearchZoom, layer.maxSearchZoom);
        };
        StateManager.prototype.getLayers = function () {
            var _this = this;
            return Object.keys(this._layers).map(function (key) { return _this._layers[key]; });
        };
        StateManager.prototype.getLayer = function (id) {
            return this._layers[id] || null;
        };
        StateManager.prototype.checkLayers = function (items) {
            var _this = this;
            this.forEachItems(items, function (item) {
                var id = item.id;
                if (_this._layers[id] !== undefined) {
                    var layer = _this._layers[id];
                    if (layer.isVisible) {
                        layer.addToMap(_this._map);
                        _this.onShowLayer(layer);
                        if (_this.layerNeedRefresh(layer)) {
                            layer.needReload = true;
                        }
                        _this.reloadLayerIfNeeded(layer);
                    }
                    else {
                        layer.removeFromMap(_this._map);
                        _this.onHideLayer(layer);
                    }
                }
            });
        };
        StateManager.prototype.reloadLayerIfNeeded = function (layer) {
            var needReload = layer.needReload &&
                layer.isVisible &&
                layer.loadable &&
                !layer.searchable;
            if (needReload) {
                if (layer.updatable && this._map.isRendered) {
                    var zoom = this._map.zoom;
                    var bbox = GeoBox.Utils.boundsToArray(this._map.map.getBounds());
                    layer.currentZoom = zoom;
                    this.loadGeoObjects(layer, null, bbox, zoom);
                }
                else {
                    this.loadGeoObjects(layer);
                }
                layer.needReload = false;
            }
        };
        StateManager.prototype.layerNeedRefresh = function (layer) {
            return layer.updatable &&
                layer.currentZoom != null &&
                layer.currentZoom !== this._map.zoom;
        };
        StateManager.prototype.findSearchableLayers = function () {
            var _this = this;
            return Object.keys(this._layers)
                .filter(function (k) { return _this._layers[k].searchable; })
                .map(function (k) { return _this._layers[k]; });
        };
        StateManager.prototype.findLayersByFilter = function (predicateFunc) {
            var _this = this;
            return Object.keys(this._layers)
                .filter(function (k) { return predicateFunc(_this._layers[k]); })
                .map(function (k) { return _this._layers[k]; });
        };
        StateManager.prototype.findSelectedLayer = function () {
            var layers = this.findLayersByFilter(function (l) { return l.isSelected; });
            return layers.length > 0 ? layers[0] : null;
        };
        //#endregion Manage Layer
        //#region Manage GeoObjects
        StateManager.prototype.addGeoObjectsIfNotExists = function (layerId, items) {
            var layer = this._layers[layerId];
            if (layer === undefined || layer === null) {
                return;
            }
            if (this._geoObjects[layer.id] === undefined) {
                this._geoObjects[layer.id] = {};
            }
            if (items == null || items.length === undefined || items.length <= 0) {
                return;
            }
            var needsAdding = [];
            var geoObjects = this._geoObjects[layer.id];
            for (var i = 0; i < items.length; i++) {
                var model = items[i];
                if (geoObjects[model.uid] === undefined) {
                    var geoObject = GeoBox.GeoObjectFactory.createFromModel(model);
                    geoObjects[geoObject.uid] = geoObject;
                    this.bindGeoObjectEvents(geoObject);
                    needsAdding.push(geoObject);
                }
            }
            if (needsAdding.length > 0) {
                layer.addGeoObjects(needsAdding);
                needsAdding = null;
            }
        };
        StateManager.prototype.removeGeoObjectsFromLayer = function (layerId, items) {
            var layer = this._layers[layerId];
            if (layer === undefined || layer === null) {
                return;
            }
            if (this._geoObjects[layer.id] === undefined) {
                this._geoObjects[layer.id] = {};
            }
            if (items == null || items.length === undefined || items.length <= 0) {
                return;
            }
            var needsRemoving = [];
            var geoObjects = this._geoObjects[layer.id];
            for (var i = 0; i < items.length; i++) {
                var uid = items[i].uid;
                if (geoObjects[uid] !== undefined) {
                    needsRemoving.push(geoObjects[uid]);
                    delete geoObjects[uid];
                }
            }
            if (needsRemoving.length > 0) {
                layer.removeGeoObjects(needsRemoving);
                for (var _i = 0, needsRemoving_1 = needsRemoving; _i < needsRemoving_1.length; _i++) {
                    var geoObject = needsRemoving_1[_i];
                    geoObject.destroy();
                }
                needsRemoving = null;
            }
        };
        StateManager.prototype.removeAllGeoObjectsFromLayer = function (layerId) {
            var layer = this._layers[layerId];
            if (layer === undefined || layer === null) {
                return;
            }
            if (this._geoObjects[layer.id] === undefined) {
                this._geoObjects[layer.id] = {};
            }
            var geoObjects = this._geoObjects[layer.id];
            var needRemoval = false;
            Object.keys(geoObjects).map(function (k) {
                var obj = geoObjects[k];
                obj.layer = null;
                obj.destroy();
                delete geoObjects[k];
                needRemoval = true;
            });
            if (needRemoval) {
                layer.removeAllGeoObjects();
            }
        };
        StateManager.prototype.forEachItems = function (items, callback) {
            if (items != null && items.length !== undefined && items.length > 0) {
                for (var i = 0; i < items.length; i++) {
                    callback(items[i]);
                }
            }
        };
        StateManager.prototype.findGeoObjectsForLayer = function (layerOrId) {
            var geoObjects = null;
            if (typeof layerOrId === "string") {
                geoObjects = this._geoObjects[layerOrId];
            }
            else {
                geoObjects = this._geoObjects[layerOrId.id];
            }
            if (geoObjects != null) {
                return Object.keys(geoObjects).map(function (k) { return geoObjects[k]; });
            }
            return [];
        };
        StateManager.prototype.findGeoObjectByLobject = function (lobject) {
            var result = null;
            var layers = this.getLayers();
            for (var _i = 0, layers_1 = layers; _i < layers_1.length; _i++) {
                var layer = layers_1[_i];
                if (this._geoObjects[layer.id] !== undefined) {
                    var geoObjects = this._geoObjects[layer.id];
                    var uid = Object.keys(geoObjects).filter(function (k) { return geoObjects[k].lobject === lobject; });
                    if (uid.length > 0) {
                        result = geoObjects[uid[0]];
                        break;
                    }
                }
            }
            return result;
        };
        StateManager.prototype.findGeoObjectByLGroup = function (lgroup) {
            var _this = this;
            if (lgroup == null) {
                throw new GeoBox.Common.ArgumentNullException("lgroup");
            }
            var result = [];
            lgroup.eachLayer(function (l) {
                var geoObject = _this.findGeoObjectByLobject(l);
                if (geoObject !== null) {
                    result.push(geoObject);
                }
            });
            return result;
        };
        //#endregion Manage GeoObjects
        //#region Load Members
        StateManager.prototype.loadGeoObjects = function (layer, point, bbox, zoom, single) {
            var ds = this.getGeoObjectDataSource(layer.id);
            if (ds != null) {
                if (point != null) {
                    ds.point = point;
                }
                if (zoom != null) {
                    ds.zoom = zoom;
                }
                if (bbox != null) {
                    ds.bbox = bbox;
                }
                if (single != null) {
                    ds.single = single;
                }
                ds.read();
            }
        };
        StateManager.prototype.loadGeoObjectCounts = function () {
            var _this = this;
            if (this._geoObjectCountDataSource != null) {
                if (!(this._geoObjectCountDataSource instanceof GeoObjectCountDataSource)) {
                    throw new GeoBox.Common.InvalidOperationException("GeoObjectCountDataSource must be instance or subclass of GeoObjectCountDataSource.");
                }
                var layers_2 = this.getLayers();
                if (layers_2.length === 0) {
                    return;
                }
                this._geoObjectCountDataSource.layerId = layers_2.map(function (layer) { return layer.id; });
                this._geoObjectCountDataSource.read().then(function () {
                    var data = _this._geoObjectCountDataSource.data();
                    if (data.length > 0) {
                        var counts_1 = data[0];
                        layers_2.forEach(function (layer) {
                            var count = counts_1.get(layer.id);
                            if (count != null) {
                                layer.count = count;
                            }
                        });
                    }
                });
            }
        };
        //#endregion Load Members
        //#region Manage DataSource
        StateManager.prototype.removeGeoObjectDataSource = function (layerId) {
            if (this._geoObjectDataSources[layerId] !== undefined) {
                this.unbindGeoObjectDataSource(this._geoObjectDataSources[layerId]);
                delete this._geoObjectDataSources[layerId];
            }
        };
        StateManager.prototype.createGeoObjectDataSource = function (layerId) {
            if (this._geoObjectDataSources[layerId] === undefined) {
                this.checkGeoObjectDataSource();
                var dataSource = this._geoObjectDataSourcePrototype.clone();
                dataSource.layerId = layerId;
                this.bindGeoObjectDataSource(dataSource);
                this._geoObjectDataSources[layerId] = dataSource;
            }
            return this._geoObjectDataSources[layerId];
        };
        StateManager.prototype.checkGeoObjectDataSource = function () {
            if (this._geoObjectDataSourcePrototype == null) {
                throw new GeoBox.Common.Exception("GeoObjectDataSource is required. Please set geoObjectDataSource property.");
            }
            if (!(this._geoObjectDataSourcePrototype instanceof GeoObjectDataSource)) {
                throw new GeoBox.Common.Exception("GeoObjectDataSource must be instance or subclass of GeoObjectDataSource.");
            }
        };
        //#endregion Manage DataSource
        //#region Destroy Members
        StateManager.prototype.destroyLayerDataSource = function () {
            if (this._layerDataSource != null) {
                this.unbindLayerDataSource(this._layerDataSource);
            }
            this._layerDataSource = null;
        };
        StateManager.prototype.destroyGeoObjectDataSources = function () {
            var geoObjectDataSources = this._geoObjectDataSources;
            for (var layerId in geoObjectDataSources) {
                if (geoObjectDataSources.hasOwnProperty(layerId)) {
                    this.unbindGeoObjectDataSource(geoObjectDataSources[layerId]);
                }
            }
            this._geoObjectDataSources = null;
            this._geoObjectDataSourcePrototype = null;
        };
        StateManager.prototype.destroyGeoObjectCountDataSource = function () {
            if (this._geoObjectCountDataSource != null) {
                this.unbindGeoObjectCountDataSource(this._geoObjectCountDataSource);
                this._geoObjectCountDataSource = null;
            }
        };
        StateManager.prototype.destroyEvents = function () {
            this._layerRequestStart.clear();
            this._layerRequestEnd.clear();
            this._layerShow.clear();
            this._layerHide.clear();
            this._layerBatchLoadStart.clear();
            this._layerBatchLoadEnd.clear();
            this._layerRequestStart = null;
            this._layerRequestEnd = null;
            this._layerShow = null;
            this._layerHide = null;
            this._layerBatchLoadStart = null;
            this._layerBatchLoadEnd = null;
            this._geoObjectRequestStart.clear();
            this._geoObjectRequestEnd.clear();
            this._geoObjectDoubleClick.clear();
            this._geoObjectPolygonClick.clear();
            this._geoObjectEditButtonClick.clear();
            this._geoObjectDeleteButtonClick.clear();
            this._geoObjectClick.clear();
            this._requestError.clear();
            this._geoObjectRequestStart = null;
            this._geoObjectRequestEnd = null;
            this._geoObjectDoubleClick = null;
            this._geoObjectPolygonClick = null;
            this._geoObjectEditButtonClick = null;
            this._geoObjectDeleteButtonClick = null;
            this._geoObjectClick = null;
            this._requestError = null;
        };
        StateManager.prototype.destroy = function () {
            this.destroyLayerDataSource();
            this.destroyGeoObjectDataSources();
            this.destroyGeoObjectCountDataSource();
            this.destroyEvents();
            this._layers = null;
            this._geoObjects = null;
            this._map = null;
            this._editableLayer = null;
        };
        return StateManager;
    }());
    GeoBox.StateManager = StateManager;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Utils = (function () {
        function Utils() {
        }
        Utils.createBoundsAroundPoint = function (latlng, meters) {
            var d = meters / 2;
            //Coordinate offsets in radians
            var dLat1 = d / Utils.R;
            var dLon1 = d / (Utils.R * Math.cos(Utils.PI * latlng.lat / 180));
            var dLat2 = -d / Utils.R;
            var dLon2 = -d / (Utils.R * Math.cos(Utils.PI * latlng.lat / 180));
            //OffsetPosition, decimal degrees
            var latO1 = latlng.lat + dLat1 * 180 / Utils.PI;
            var lonO1 = latlng.lng + dLon1 * 180 / Utils.PI;
            var latO2 = latlng.lat + dLat2 * 180 / Utils.PI;
            var lonO2 = latlng.lng + dLon2 * 180 / Utils.PI;
            return new L.LatLngBounds([[latO1, lonO1], [latO2, lonO2]]);
        };
        Utils.createBoundsAroundPointByZoom = function (latlng, zoom) {
            var meters = (-7 + 1.5 * (zoom > 6 ? zoom : 6)) * (1 << (18 - zoom));
            return Utils.createBoundsAroundPoint(latlng, meters);
        };
        Utils.boundsToArray = function (bounds) {
            return [
                bounds.getSouthWest().lat, bounds.getSouthWest().lng,
                bounds.getNorthEast().lat, bounds.getNorthEast().lng
            ];
        };
        Utils.rangeInclude = function (value, rangeStart, rangeEnd) {
            if (value == null || (rangeStart == null && rangeEnd == null)) {
                return false;
            }
            return !((rangeStart != null && value < rangeStart) || (rangeEnd != null && value > rangeEnd));
        };
        Utils.inZoom = function (zoom, minZoom, maxZoom) {
            return (zoom != null && minZoom == null && maxZoom == null) || Utils.rangeInclude(zoom, minZoom, maxZoom);
        };
        //#region Manage ProgressBar
        Utils.showProgress = function (domNode) {
            kendo.ui.progress($(domNode), true);
        };
        Utils.hideProgress = function (domNode) {
            kendo.ui.progress($(domNode), false);
        };
        Utils.showMapProgress = function (domNode) {
            GeoBox.ProgressBar.show(domNode);
        };
        Utils.hideMapProgress = function (domNode) {
            GeoBox.ProgressBar.hide(domNode);
        };
        Utils.R = 6378137;
        Utils.PI = 3.14159265359;
        return Utils;
    }());
    GeoBox.Utils = Utils;
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Events;
    (function (Events) {
        var GeoObjectCreatedEventArgs = (function () {
            function GeoObjectCreatedEventArgs(geoObject, stateManager) {
                this.model = null;
                if (geoObject == null) {
                    throw new GeoBox.Common.ArgumentNullException("geoObject");
                }
                if (stateManager == null) {
                    throw new GeoBox.Common.ArgumentNullException("stateManager");
                }
                this._geoObject = geoObject;
                this._stateManager = stateManager;
            }
            GeoObjectCreatedEventArgs.prototype.cancelCreate = function () {
                this._geoObject.destroy();
            };
            GeoObjectCreatedEventArgs.prototype.applyCreate = function (layerId, syncChanges) {
                if (syncChanges === void 0) { syncChanges = false; }
                if (layerId == null) {
                    throw new GeoBox.Common.ArgumentNullException("geoObject");
                }
                var dataSource = this._stateManager.getGeoObjectDataSource(layerId);
                if (dataSource == null) {
                    throw new GeoBox.Common.InvalidOperationException("Couldn't found geo object data source for layer id: " + layerId);
                }
                this._geoObject.removeFromLayer(this._stateManager.editableLayer);
                dataSource.add(this._geoObject.model);
                if (syncChanges) {
                    dataSource.sync();
                }
            };
            GeoObjectCreatedEventArgs.prototype.getDataSource = function (layerId) {
                if (layerId == null) {
                    throw new GeoBox.Common.ArgumentNullException("geoObject");
                }
                return this._stateManager.getGeoObjectDataSource(layerId);
            };
            GeoObjectCreatedEventArgs.prototype.getSelectedLayer = function () {
                var layer = this._stateManager.findSelectedLayer();
                return layer != null ? layer.model : null;
            };
            return GeoObjectCreatedEventArgs;
        }());
        Events.GeoObjectCreatedEventArgs = GeoObjectCreatedEventArgs;
    })(Events = GeoBox.Events || (GeoBox.Events = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Events;
    (function (Events) {
        var GeoObjectDeletedEventArgs = (function () {
            function GeoObjectDeletedEventArgs(deletedItems) {
                this._deleted = [];
                if (deletedItems == null) {
                    throw new GeoBox.Common.ArgumentNullException("deletedItems");
                }
                this._deleted = deletedItems;
            }
            Object.defineProperty(GeoObjectDeletedEventArgs.prototype, "deleted", {
                get: function () {
                    return this._deleted;
                },
                enumerable: true,
                configurable: true
            });
            GeoObjectDeletedEventArgs.prototype.syncChanges = function () {
                if (this._deleted.length > 0) {
                    this._deleted.forEach(function (item) {
                        item.dataSource.sync();
                    });
                }
            };
            return GeoObjectDeletedEventArgs;
        }());
        Events.GeoObjectDeletedEventArgs = GeoObjectDeletedEventArgs;
    })(Events = GeoBox.Events || (GeoBox.Events = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Events;
    (function (Events) {
        var GeoObjectEditedEventArgs = (function () {
            function GeoObjectEditedEventArgs(editedItems) {
                this._edited = [];
                if (editedItems == null) {
                    throw new GeoBox.Common.ArgumentNullException("editedItems");
                }
                this._edited = editedItems;
            }
            Object.defineProperty(GeoObjectEditedEventArgs.prototype, "edited", {
                get: function () {
                    return this._edited;
                },
                enumerable: true,
                configurable: true
            });
            GeoObjectEditedEventArgs.prototype.syncChanges = function () {
                if (this._edited.length > 0) {
                    this._edited.forEach(function (item) {
                        item.dataSource.sync();
                    });
                }
            };
            return GeoObjectEditedEventArgs;
        }());
        Events.GeoObjectEditedEventArgs = GeoObjectEditedEventArgs;
    })(Events = GeoBox.Events || (GeoBox.Events = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Events;
    (function (Events) {
        var GeoObjectModelEventArgs = (function () {
            function GeoObjectModelEventArgs() {
                this.model = null;
                this.layerId = null;
                this.dataSource = null;
            }
            return GeoObjectModelEventArgs;
        }());
        Events.GeoObjectModelEventArgs = GeoObjectModelEventArgs;
    })(Events = GeoBox.Events || (GeoBox.Events = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="geoobjectmodeleventargs.ts" />
var GeoBox;
(function (GeoBox) {
    var Events;
    (function (Events) {
        var GeoObjectEditEventArgs = (function (_super) {
            __extends(GeoObjectEditEventArgs, _super);
            function GeoObjectEditEventArgs(viewController, button) {
                _super.call(this);
                this._viewController = viewController;
                this._button = button;
            }
            GeoObjectEditEventArgs.prototype.wait = function () {
                this._button.enable(false);
                this._viewController.waitProgress();
            };
            GeoObjectEditEventArgs.prototype.endWait = function () {
                this._button.enable(true);
                this._viewController.progressEnd();
            };
            GeoObjectEditEventArgs.prototype.destroy = function () {
                this._viewController = null;
                this._button = null;
                this.model = null;
                this.layerId = null;
                this.dataSource = null;
            };
            return GeoObjectEditEventArgs;
        }(Events.GeoObjectModelEventArgs));
        Events.GeoObjectEditEventArgs = GeoObjectEditEventArgs;
    })(Events = GeoBox.Events || (GeoBox.Events = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="events/geoobjectcreatedeventargs.ts" />
/// <reference path="events/geoobjectdeletedeventargs.ts" />
/// <reference path="events/geoobjecteditedeventargs.ts" />
/// <reference path="events/geoobjectlocationeventargs.ts" />
/// <reference path="events/geoobjectmodeleventargs.ts" />
/// <reference path="events/geoobjectediteventargs.ts" />
var GeoBox;
(function (GeoBox) {
    var GeoObjectCreatedEventArgs = GeoBox.Events.GeoObjectCreatedEventArgs;
    var GeoObjectEditedEventArgs = GeoBox.Events.GeoObjectEditedEventArgs;
    var GeoObjectDeletedEventArgs = GeoBox.Events.GeoObjectDeletedEventArgs;
    var GeoObjectModelEventArgs = GeoBox.Events.GeoObjectModelEventArgs;
    var GeoObjectEditEventArgs = GeoBox.Events.GeoObjectEditEventArgs;
    var ViewController = (function () {
        function ViewController(map, layout, drawControl, stateManager) {
            this._waitCount = 0;
            // Events
            this._geoObjectCreated = new GeoBox.Common.Event();
            this._geoObjectEdited = new GeoBox.Common.Event();
            this._geoObjectDeleted = new GeoBox.Common.Event();
            this._geoObjectDoubleClick = new GeoBox.Common.Event();
            this._geoObjectEditButtonClick = new GeoBox.Common.Event();
            if (map == null)
                throw new GeoBox.Common.ArgumentNullException("map");
            if (drawControl == null)
                throw new GeoBox.Common.ArgumentNullException("drawControl");
            if (stateManager == null)
                throw new GeoBox.Common.ArgumentNullException("stateManager");
            if (layout == null)
                throw new GeoBox.Common.ArgumentNullException("layout");
            this._map = map;
            this._drawControl = drawControl;
            this._stateManager = stateManager;
            this._layout = layout;
            this._updateTimer = new GeoBox.Common.Timer(ViewController._updateTimeout);
            this._loadCompleteTimer = new GeoBox.Common.Timer(ViewController._loadCompleteTimeout);
            // Request Hanlers
            this.handleLayerRequestStart = this.handleLayerRequestStart.bind(this);
            this.handleLayerRequestEnd = this.handleLayerRequestEnd.bind(this);
            this.handleGeoObjectRequestStart = this.handleGeoObjectRequestStart.bind(this);
            this.handleGeoObjectRequestEnd = this.handleGeoObjectRequestEnd.bind(this);
            // Draw Hanlers
            this.handleDrawCreated = this.handleDrawCreated.bind(this);
            this.handleDrawEdited = this.handleDrawEdited.bind(this);
            this.handleDrawDeleted = this.handleDrawDeleted.bind(this);
            // Click Handlers
            this.handleGeoObjectDoubleClick = this.handleGeoObjectDoubleClick.bind(this);
            this.handleGeoObjectEditButtonClick = this.handleGeoObjectEditButtonClick.bind(this);
            this.handleClusterClick = this.handleClusterClick.bind(this);
            // Search Handlers
            this.handleSearchOnClick = this.handleSearchOnClick.bind(this);
            this.handleEndMoveOnMap = this.handleEndMoveOnMap.bind(this);
            this.handleUpdateLayers = this.handleUpdateLayers.bind(this);
            this.handleLoadCompleteLayer = this.handleLoadCompleteLayer.bind(this);
            // Batch Load Handlers
            this.handleLayerBatchLoadStart = this.handleLayerBatchLoadStart.bind(this);
            this.handleLayerBatchLoadEnd = this.handleLayerBatchLoadEnd.bind(this);
            // Bind events
            this.bindEvents();
        }
        ViewController.prototype.bindEvents = function () {
            // Layer Events
            this._stateManager.layerRequestStart.add(this.handleLayerRequestStart);
            this._stateManager.layerRequestEnd.add(this.handleLayerRequestEnd);
            this._stateManager.layerBatchLoadStart.add(this.handleLayerBatchLoadStart);
            this._stateManager.layerBatchLoadEnd.add(this.handleLayerBatchLoadEnd);
            // GeoObject Events
            this._stateManager.geoObjectRequestStart.add(this.handleGeoObjectRequestStart);
            this._stateManager.geoObjectRequestEnd.add(this.handleGeoObjectRequestEnd);
            this._stateManager.geoObjectPolygonClick.add(this.handleSearchOnClick);
            this._stateManager.geoObjectDoubleClick.add(this.handleGeoObjectDoubleClick);
            this._stateManager.geoObjectEditButtonClick.add(this.handleGeoObjectEditButtonClick);
            this._stateManager.geoObjectClick.add(this.handleClusterClick);
            // Draw Control Events
            this._drawControl.created.add(this.handleDrawCreated);
            this._drawControl.edited.add(this.handleDrawEdited);
            this._drawControl.deleted.add(this.handleDrawDeleted);
            // Map Events
            this._map.click.add(this.handleSearchOnClick);
            this._map.moveEnd.add(this.handleEndMoveOnMap);
            // Timer Events
            this._updateTimer.elapsed.add(this.handleUpdateLayers);
            this._loadCompleteTimer.elapsed.add(this.handleLoadCompleteLayer);
        };
        ViewController.prototype.unbindEvents = function () {
            // Layer Events
            this._stateManager.layerRequestStart.remove(this.handleLayerRequestStart);
            this._stateManager.layerRequestEnd.remove(this.handleLayerRequestEnd);
            this._stateManager.layerBatchLoadStart.remove(this.handleLayerBatchLoadStart);
            this._stateManager.layerBatchLoadEnd.remove(this.handleLayerBatchLoadEnd);
            // GeoObject Events
            this._stateManager.geoObjectRequestStart.remove(this.handleGeoObjectRequestStart);
            this._stateManager.geoObjectRequestEnd.remove(this.handleGeoObjectRequestEnd);
            this._stateManager.geoObjectPolygonClick.remove(this.handleSearchOnClick);
            this._stateManager.geoObjectDoubleClick.remove(this.handleGeoObjectDoubleClick);
            this._stateManager.geoObjectEditButtonClick.remove(this.handleGeoObjectEditButtonClick);
            this._stateManager.geoObjectClick.remove(this.handleClusterClick);
            // Draw Control Events
            this._drawControl.created.remove(this.handleDrawCreated);
            this._drawControl.edited.remove(this.handleDrawEdited);
            this._drawControl.deleted.remove(this.handleDrawDeleted);
            // Map Events
            this._map.click.remove(this.handleSearchOnClick);
            this._map.moveEnd.remove(this.handleEndMoveOnMap);
            // Timer Events
            this._updateTimer.elapsed.remove(this.handleUpdateLayers);
            this._loadCompleteTimer.elapsed.remove(this.handleLoadCompleteLayer);
        };
        Object.defineProperty(ViewController.prototype, "geoObjectCreated", {
            //#region Events
            get: function () {
                return this._geoObjectCreated;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ViewController.prototype, "geoObjectEdited", {
            get: function () {
                return this._geoObjectEdited;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ViewController.prototype, "geoObjectDeleted", {
            get: function () {
                return this._geoObjectDeleted;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ViewController.prototype, "geoObjectDoubleClick", {
            get: function () {
                return this._geoObjectDoubleClick;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ViewController.prototype, "geoObjectEditButtonClick", {
            get: function () {
                return this._geoObjectEditButtonClick;
            },
            enumerable: true,
            configurable: true
        });
        //#endregion Events
        //#region ProgressBar
        ViewController.prototype.waitProgress = function () {
            this._waitCount++;
            GeoBox.Utils.showMapProgress(this._layout.mapPanelNode);
        };
        ViewController.prototype.progressEnd = function () {
            this._waitCount--;
            if (this._waitCount <= 0) {
                GeoBox.Utils.hideMapProgress(this._layout.mapPanelNode);
            }
        };
        //#endregion ProgressBar
        //#region Request Handles
        ViewController.prototype.handleLayerRequestStart = function (sender) {
            GeoBox.Utils.showProgress(this._layout.layerPanelNode);
        };
        ViewController.prototype.handleLayerRequestEnd = function (sender) {
            GeoBox.Utils.hideProgress(this._layout.layerPanelNode);
        };
        ViewController.prototype.handleLayerBatchLoadStart = function (sender, args) {
            args.layer.disable();
            this.waitProgress();
        };
        ViewController.prototype.handleLayerBatchLoadEnd = function (sender, args) {
            this._loadCompleteTimer.restart();
            args.layer.enable();
            this.progressEnd();
        };
        ViewController.prototype.handleGeoObjectRequestStart = function (sender, args) {
            args.layer.disable();
            this.waitProgress();
        };
        ViewController.prototype.handleGeoObjectRequestEnd = function (sender, args) {
            this._loadCompleteTimer.restart();
            args.layer.enable();
            this.progressEnd();
        };
        //#endregion Request Handles
        //#region Internal Handlers
        ViewController.prototype.handleSearchOnClick = function (sender, event) {
            if (this._drawControl.activated) {
                return;
            }
            var searchableLayers = this._stateManager.findSearchableLayers();
            if (searchableLayers.length === 0) {
                return;
            }
            var bounds = GeoBox.Utils.createBoundsAroundPointByZoom(event.location, this._map.zoom);
            for (var _i = 0, searchableLayers_1 = searchableLayers; _i < searchableLayers_1.length; _i++) {
                var layer = searchableLayers_1[_i];
                this._stateManager.loadGeoObjects(layer, null, GeoBox.Utils.boundsToArray(bounds), this._map.zoom, true);
            }
        };
        ViewController.prototype.handleEndMoveOnMap = function (sender) {
            if (this._drawControl.activated) {
                this._updateTimer.stop();
                return;
            }
            this._updateTimer.restart();
        };
        ViewController.prototype.handleUpdateLayers = function (sender) {
            if (this._drawControl.activated) {
                return;
            }
            if (!this._map.isRendered) {
                return;
            }
            var updatableLayers = this._stateManager.findLayersByFilter(function (layer) {
                return layer.updatable && layer.loadable && layer.isVisible;
            });
            if (updatableLayers.length === 0) {
                return;
            }
            for (var _i = 0, updatableLayers_1 = updatableLayers; _i < updatableLayers_1.length; _i++) {
                var layer = updatableLayers_1[_i];
                if (layer.searchByZoom) {
                    if (this._stateManager.layerInZoom(layer)) {
                        layer.needReload = true;
                        this._stateManager.reloadLayerIfNeeded(layer);
                    }
                    else {
                        this._stateManager.clearLayer(layer);
                    }
                }
                else {
                    layer.needReload = true;
                    this._stateManager.reloadLayerIfNeeded(layer);
                }
            }
        };
        ViewController.prototype.handleClusterClick = function (sender, event) {
            var geoObject = event.geoObject;
            if (!(geoObject instanceof GeoBox.ClusterObject) || !geoObject.isMarker || !this._map.isRendered) {
                return;
            }
            this._map.setView(geoObject.getLatLng(), this._map.zoom + 1);
        };
        ViewController.prototype.handleLoadCompleteLayer = function (sender) {
            if (this._waitCount <= 0) {
                GeoBox.GeoObject.closeUnbindedPopups();
                GeoBox.GeoObject.spiderfyObjectIfOpenPopup();
            }
        };
        //#endregion Internal Handlers
        //#region CRUD Event Handlers
        ViewController.prototype.handleDrawCreated = function (sender, event) {
            var geoObject = GeoBox.GeoObject.create();
            geoObject.geometry = event.layer.toGeoJSON().geometry;
            geoObject.addToLayer(this._stateManager.editableLayer);
            // Raise Event
            this.onGeoObjectCreated(geoObject);
        };
        ViewController.prototype.handleDrawEdited = function (sender, event) {
            var _this = this;
            var geoObjects = this._stateManager.findGeoObjectByLGroup(event.layers);
            var editedItems = {};
            geoObjects.forEach(function (geoObject) {
                if (geoObject.layer !== null && editedItems[geoObject.layer.id] === undefined) {
                    editedItems[geoObject.layer.id] = {
                        layerId: geoObject.layer.id,
                        dataSource: _this._stateManager.getGeoObjectDataSource(geoObject.layer.id),
                        models: []
                    };
                }
                geoObject.syncModelGeometry();
                editedItems[geoObject.layer.id].models.push(geoObject.model);
            });
            var editedItemsArray = Object.keys(editedItems).map(function (k) { return editedItems[k]; });
            // Raise Event
            this.onGeoObjectEdited(editedItemsArray);
        };
        ViewController.prototype.handleDrawDeleted = function (sender, event) {
            var _this = this;
            var geoObjects = this._stateManager.findGeoObjectByLGroup(event.layers);
            var deletedItems = {};
            geoObjects.forEach(function (geoObject) {
                if (geoObject.layer !== null && deletedItems[geoObject.layer.id] === undefined) {
                    deletedItems[geoObject.layer.id] = {
                        layerId: geoObject.layer.id,
                        dataSource: _this._stateManager.getGeoObjectDataSource(geoObject.layer.id),
                        models: []
                    };
                }
                var deletedItem = deletedItems[geoObject.layer.id];
                deletedItem.models.push(geoObject.model);
                deletedItem.dataSource.remove(geoObject.model);
            });
            var deletedItemsArray = Object.keys(deletedItems).map(function (k) { return deletedItems[k]; });
            // Raise Event
            this.onGeoObjectDeleted(deletedItemsArray);
        };
        ViewController.prototype.handleGeoObjectDoubleClick = function (sender, event) {
            this.onGeoObjectDoubleClick(event.geoObject);
        };
        ViewController.prototype.handleGeoObjectEditButtonClick = function (sender, event) {
            this.onGeoObjectEditButtonClick(event.geoObject);
        };
        ViewController.prototype.onGeoObjectCreated = function (geoObject) {
            if (!this._geoObjectCreated.empty) {
                var args = new GeoObjectCreatedEventArgs(geoObject, this._stateManager);
                args.model = geoObject.model;
                this._geoObjectCreated.notify(this, args);
            }
        };
        ViewController.prototype.onGeoObjectEdited = function (editedItems) {
            if (!this._geoObjectEdited.empty) {
                var args = new GeoObjectEditedEventArgs(editedItems);
                this._geoObjectEdited.notify(this, args);
            }
        };
        ViewController.prototype.onGeoObjectDeleted = function (deletedItems) {
            if (!this._geoObjectDeleted.empty) {
                var args = new GeoObjectDeletedEventArgs(deletedItems);
                this._geoObjectDeleted.notify(this, args);
            }
        };
        ViewController.prototype.onGeoObjectDoubleClick = function (geoObject) {
            if (!this._geoObjectDoubleClick.empty) {
                var args = new GeoObjectModelEventArgs();
                args.model = geoObject.model;
                if (geoObject.layer !== null) {
                    args.layerId = geoObject.layer.id;
                    args.dataSource = this._stateManager.getGeoObjectDataSource(args.layerId);
                }
                this._geoObjectDoubleClick.notify(this, args);
            }
        };
        ViewController.prototype.onGeoObjectEditButtonClick = function (geoObject) {
            if (!this._geoObjectEditButtonClick.empty) {
                var args = new GeoObjectEditEventArgs(this, geoObject.getEditButton());
                args.model = geoObject.model;
                if (geoObject.layer !== null) {
                    args.layerId = geoObject.layer.id;
                    args.dataSource = this._stateManager.getGeoObjectDataSource(args.layerId);
                }
                this._geoObjectEditButtonClick.notify(this, args);
            }
        };
        //#endregion CRUD Event Handlers
        ViewController.prototype.destroyEvents = function () {
            this._geoObjectCreated.clear();
            this._geoObjectEdited.clear();
            this._geoObjectDeleted.clear();
            this._geoObjectDoubleClick.clear();
            this._geoObjectEditButtonClick.clear();
            this._geoObjectCreated = null;
            this._geoObjectEdited = null;
            this._geoObjectDeleted = null;
            this._geoObjectDoubleClick = null;
            this._geoObjectEditButtonClick = null;
        };
        ViewController.prototype.destroyTimers = function () {
            this._updateTimer.destroy();
            this._updateTimer = null;
            this._loadCompleteTimer.destroy();
            this._loadCompleteTimer = null;
        };
        ViewController.prototype.destroy = function () {
            this.unbindEvents();
            this.destroyEvents();
            this.destroyTimers();
            this._drawControl = null;
            this._stateManager = null;
            this._map = null;
            this._layout = null;
        };
        ViewController._updateTimeout = 300;
        ViewController._loadCompleteTimeout = 0;
        return ViewController;
    }());
    GeoBox.ViewController = ViewController;
})(GeoBox || (GeoBox = {}));
/// <reference path="geoobjectmodeleventargs.ts" />
var GeoBox;
(function (GeoBox) {
    var Events;
    (function (Events) {
        var GeoObjectDeleteEventArgs = (function (_super) {
            __extends(GeoObjectDeleteEventArgs, _super);
            function GeoObjectDeleteEventArgs() {
                _super.apply(this, arguments);
            }
            GeoObjectDeleteEventArgs.prototype.syncChanges = function () {
                if (this.dataSource) {
                    this.dataSource.sync();
                }
            };
            return GeoObjectDeleteEventArgs;
        }(Events.GeoObjectModelEventArgs));
        Events.GeoObjectDeleteEventArgs = GeoObjectDeleteEventArgs;
    })(Events = GeoBox.Events || (GeoBox.Events = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var Exception = (function (_super) {
            __extends(Exception, _super);
            function Exception(message, innerException) {
                _super.call(this);
                if (!(this instanceof Exception)) {
                    return new Exception(message, innerException);
                }
                if (typeof Error.captureStackTrace === "function") {
                    Error.captureStackTrace(this, arguments.callee);
                }
                this.name = "Exception";
                message = message == null ? "" : message;
                if (innerException !== null && innerException !== undefined) {
                    if (innerException instanceof Error) {
                        this.innerException = innerException;
                        this.message = message + ", innerException: " + this.innerException.message;
                    }
                    else if (typeof innerException === "string") {
                        this.innerException = new Exception(innerException);
                        this.message = message + ", innerException: " + this.innerException.message;
                    }
                    else {
                        this.innerException = innerException;
                        this.message = message + ", innerException: " + this.innerException;
                    }
                }
                else {
                    this.message = message;
                }
            }
            Exception.prototype.toString = function () {
                var resultString;
                if (this.message == null ||
                    (typeof this.message === "string" && this.message.length <= 0)) {
                    resultString = this.name;
                }
                else {
                    resultString = this.name + ": " + this.message;
                }
                if (this.innerException != null) {
                    resultString += " ---> " + this.innerException.toString();
                }
                if (this.stack != null) {
                    resultString += "\n" + this.stack;
                }
                return resultString;
            };
            return Exception;
        }(Error));
        Common.Exception = Exception;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="exception.ts" />
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var ArgumentException = (function (_super) {
            __extends(ArgumentException, _super);
            function ArgumentException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "ArgumentException";
            }
            return ArgumentException;
        }(Common.Exception));
        Common.ArgumentException = ArgumentException;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="exception.ts" />
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var ArgumentNullException = (function (_super) {
            __extends(ArgumentNullException, _super);
            function ArgumentNullException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "ArgumentNullException";
            }
            return ArgumentNullException;
        }(Common.Exception));
        Common.ArgumentNullException = ArgumentNullException;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var ArgumentOutOfRangeException = (function (_super) {
            __extends(ArgumentOutOfRangeException, _super);
            function ArgumentOutOfRangeException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "ArgumentOutOfRangeException";
            }
            return ArgumentOutOfRangeException;
        }(Common.Exception));
        Common.ArgumentOutOfRangeException = ArgumentOutOfRangeException;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="exception.ts" />
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var AssertException = (function (_super) {
            __extends(AssertException, _super);
            function AssertException(message) {
                _super.call(this, message);
                this.name = "AssertException";
            }
            return AssertException;
        }(Common.Exception));
        Common.AssertException = AssertException;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var Debug = (function () {
            function Debug() {
            }
            Debug.assert = function (condition, message) {
                if (!condition) {
                    message = message || "Assertion failed";
                    throw new Common.AssertException(message);
                }
            };
            return Debug;
        }());
        Common.Debug = Debug;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="exception.ts" />
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var InvalidOperationException = (function (_super) {
            __extends(InvalidOperationException, _super);
            function InvalidOperationException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "InvalidOperationException";
            }
            return InvalidOperationException;
        }(Common.Exception));
        Common.InvalidOperationException = InvalidOperationException;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var List = (function () {
            function List() {
                this._items = [];
            }
            Object.defineProperty(List.prototype, "size", {
                get: function () {
                    return this._items.length;
                },
                enumerable: true,
                configurable: true
            });
            List.prototype.add = function (item) {
                this._items.push(item);
            };
            List.prototype.remove = function (item) {
                var index = this._items.indexOf(item);
                if (index >= 0) {
                    this._items.splice(index, 1);
                }
            };
            List.prototype.contains = function (item) {
                return this._items.indexOf(item) >= 0;
            };
            List.prototype.indexOf = function (item) {
                return this._items.indexOf(item);
            };
            List.prototype.get = function (index) {
                if (index >= this._items.length) {
                    throw new Common.ArgumentOutOfRangeException("index");
                }
                return this._items[index];
            };
            List.prototype.set = function (index, item) {
                if (index >= this._items.length) {
                    throw new Common.ArgumentOutOfRangeException("index");
                }
                this._items[index] = item;
            };
            List.prototype.clear = function () {
                this._items = [];
            };
            List.prototype.toArray = function () {
                return this._items.slice();
            };
            return List;
        }());
        Common.List = List;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="exception.ts" />
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var NotImplementedException = (function (_super) {
            __extends(NotImplementedException, _super);
            function NotImplementedException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "NotImplementedException";
            }
            return NotImplementedException;
        }(Common.Exception));
        Common.NotImplementedException = NotImplementedException;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
/// <reference path="exception.ts" />
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var ObjectDisposedException = (function (_super) {
            __extends(ObjectDisposedException, _super);
            function ObjectDisposedException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "ObjectDisposedException";
            }
            return ObjectDisposedException;
        }(Common.Exception));
        Common.ObjectDisposedException = ObjectDisposedException;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
var GeoBox;
(function (GeoBox) {
    var Common;
    (function (Common) {
        var ElapsedEventArgs = (function (_super) {
            __extends(ElapsedEventArgs, _super);
            function ElapsedEventArgs() {
                _super.apply(this, arguments);
                this.payload = null;
            }
            return ElapsedEventArgs;
        }(Common.EventArgs));
        Common.ElapsedEventArgs = ElapsedEventArgs;
        var Timer = (function () {
            function Timer(timeout) {
                // Events
                this._elapsed = new Common.Event();
                this._timeout = timeout;
                this._running = false;
                this._windowTimers = window;
                this._timerDescriptor = -1;
                this.handleTimeout = this.handleTimeout.bind(this);
            }
            Object.defineProperty(Timer.prototype, "elapsed", {
                get: function () {
                    return this._elapsed;
                },
                enumerable: true,
                configurable: true
            });
            Timer.prototype.start = function (payload) {
                var _this = this;
                if (!this._running) {
                    this._timerDescriptor = this._windowTimers.setTimeout(function () {
                        _this._running = false;
                        _this._timerDescriptor = -1;
                        _this.handleTimeout(payload);
                    }, this._timeout);
                }
            };
            Timer.prototype.stop = function () {
                if (this._timerDescriptor > 0) {
                    this._windowTimers.clearTimeout(this._timerDescriptor);
                    this._timerDescriptor = -1;
                }
                this._running = false;
            };
            Timer.prototype.restart = function (payload) {
                this.stop();
                this.start(payload);
            };
            Timer.prototype.handleTimeout = function (payload) {
                if (!this._elapsed.empty) {
                    var eventArgs = new ElapsedEventArgs();
                    eventArgs.payload = payload;
                    this._elapsed.notify(this, eventArgs);
                }
            };
            Timer.prototype.destroy = function () {
                this.stop();
                this._elapsed.clear();
                this._elapsed = null;
                this._windowTimers = null;
            };
            return Timer;
        }());
        Common.Timer = Timer;
    })(Common = GeoBox.Common || (GeoBox.Common = {}));
})(GeoBox || (GeoBox = {}));
