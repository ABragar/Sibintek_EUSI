(function(cluster) {

    cluster.include({
        _addChild: function(new1, isNotificationFromChild) {
            this._iconNeedsUpdate = true;
            this._expandBounds(new1);

            if (new1 instanceof L.MarkerCluster) {
                if (!isNotificationFromChild) {
                    this._childClusters.push(new1);
                    new1.__parent = this;
                }
                this._childCount += new1._childCount;
            } else {
                if (!isNotificationFromChild) {
                    this._markers.push(new1);
                }
                //console.log("Cluster calc", this, new1);

                if (!new1.options.type || new1.options.type !== 3) {
                    this._childCount++;
                } else {
                    this._childCount += new1.options.count;
                }

            }

            if (this.__parent) {
                this.__parent._addChild(new1, true);
            }
        },
        _animationUnspiderfy: function (zoomDetails) {
            var group = this._group,
                map = group._map,
                fg = group._featureGroup,
                thisLayerPos = zoomDetails ? map._latLngToNewLayerPoint(this._latlng, zoomDetails.zoom, zoomDetails.center) : map.latLngToLayerPoint(this._latlng),
                childMarkers = this.getAllChildMarkers(),
                svg = L.Path.SVG && this.SVG_ANIMATION,
                m, i, a;

            group._animationStart();

            //Make us visible and bring the child markers back in
            this.setOpacity(1);
            for (i = childMarkers.length - 1; i >= 0; i--) {
                m = childMarkers[i];

                //Marker was added to us after we were spidified
                if (!m._preSpiderfyLatlng) {
                    continue;
                }

                //Fix up the location to the real one
                m.setLatLng(m._preSpiderfyLatlng);
                delete m._preSpiderfyLatlng;
                //Hack override the location to be our center
                if (m.setOpacity) {
                    m._setPos(thisLayerPos);
                    m.setOpacity(0);
                } else {
                    fg.removeLayer(m);
                }

                //Animate the spider legs back in
                if (svg && m._spiderLeg) {
                    a = m._spiderLeg._path.childNodes[0];
                    a.setAttribute('to', a.getAttribute('from'));
                    a.setAttribute('from', 0);
                    a.beginElement();

                    a = m._spiderLeg._path.childNodes[1];
                    a.setAttribute('from', 0.5);
                    a.setAttribute('to', 0);
                    a.setAttribute('stroke-opacity', 0);
                    a.beginElement();

                    m._spiderLeg._path.setAttribute('stroke-opacity', 0);
                }
            }

            setTimeout(function () {
                //If we have only <= one child left then that marker will be shown on the map so don't remove it!
                var stillThereChildCount = 0;
                for (i = childMarkers.length - 1; i >= 0; i--) {
                    m = childMarkers[i];
                    if (m._spiderLeg) {
                        stillThereChildCount++;
                    }
                }


                for (i = childMarkers.length - 1; i >= 0; i--) {
                    m = childMarkers[i];

                    if (!m._spiderLeg) { //Has already been unspiderfied
                        continue;
                    }


                    if (m.setOpacity) {
                        m.setOpacity(1);
                        m.setZIndexOffset(0);
                    }

                    if (stillThereChildCount > 1) {
                        fg.removeLayer(m);
                    }

                    map.removeLayer(m._spiderLeg);
                    delete m._spiderLeg;
                }
                group._animationEnd();
            }, 200);
        },
        _noanimationUnspiderfy: function () {
            var group = this._group,
                map = group._map,
                fg = group._featureGroup,
                childMarkers = this.getAllChildMarkers(),
                m, i;

            this.setOpacity(1);

            for (i = childMarkers.length - 1; i >= 0; i--) {
                m = childMarkers[i];

                fg.removeLayer(m);

                if (m._preSpiderfyLatlng) {
                    m.setLatLng(m._preSpiderfyLatlng);
                    delete m._preSpiderfyLatlng;
                }
                if (m.setZIndexOffset) {
                    m.setZIndexOffset(0);
                }

                if (m._spiderLeg) {
                    if (map) {
                        //console.warn('No map:', m._spiderLeg);
                        map.removeLayer(m._spiderLeg);
                    } else {
                        if (m._spiderLeg._map) {
                            m._spiderLeg._map.removeLayer(m._spiderLeg);
                        }
                        //console.warn('No map:', m._spiderLeg);
                    }
                    delete m._spiderLeg;
                }
            }

            group._spiderfied = null;
        }
    });

})(L.MarkerCluster);