(function(L) {
            L.Control.Focus = L.Control.extend({
                options: {
                    // topright, topleft, bottomleft, bottomright
                    position: "topright"
                    //latlng: {
                    //    lat: 55.654347641744216,
                    //    lng: 36.485595703125
                    //},
                    //zoom: 8
                },
                initialize: function (options) {
                    // constructor
                    this.options = L.extend(this.options, options);
                },
                onAdd: function (map) {
                    var self = this;

                    var controlDiv = L.DomUtil.create("div", "leaflet-control-focus");

                    if (self.options.navigator && self.options.navigator.length) {
                        var controlTitle = L.DomUtil.create("div", "leaflet-control-focus-title", controlDiv);

                        var nav = self.options.navigator[0];

                        controlTitle.innerHTML = nav.name;

                        if(this.options.focusOnInit)
                            map.setView(nav.latlng, nav.zoom);

                        controlDiv.onclick = function (e) {
                            e.stopPropagation();
                            map.setView(nav.latlng, nav.zoom);
                        };

                        if (nav.distance) {
                            var controlDistance = L.DomUtil.create("div", "leaflet-control-focus-distance", controlDiv);

                            var getDistanceString = function(distance, latlng) {
                                return "<p>" + distance.title + "</p><p>" + Math.round(L.latLng(distance.latlng).distanceTo(L.latLng(latlng)) / 1000) + "км.</p>";
                            };

                            //controlDistance.innerHTML = getDistanceString(nav.distance, map.getCenter());

                            map.on("move", function() {
                                controlDistance.innerHTML = getDistanceString(nav.distance, map.getCenter());
                            });
                        }
                    }

                    //console.log("Focus control:", this);



                    //console.log('Map from control:', map);



                    return controlDiv;
                },
                onRemove: function (map) {
                    // when removed
                },
                getCenter: function() {
                    return this.options.latlng;
                },
                getZoom: function() {
                    return this.zoom;
                }
            });
})(L);