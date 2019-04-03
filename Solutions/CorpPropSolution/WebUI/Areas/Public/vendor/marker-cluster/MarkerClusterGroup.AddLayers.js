L.MarkerClusterGroup.include({
    addLayers: function (layersArray) {
        var fg = this._featureGroup,
            npg = this._nonPointGroup,
            chunked = this.options.chunkedLoading,
            chunkInterval = this.options.chunkInterval,
            chunkProgress = this.options.chunkProgress,
            newMarkers, i, l, m;

        if (this._map) {
            var offset = 0,
                started = (new Date()).getTime();
            var process = L.bind(function () {
                var start = (new Date()).getTime();
                for (; offset < layersArray.length; offset++) {
                    if (chunked && offset % 200 === 0) {
                        // every couple hundred markers, instrument the time elapsed since processing started:
                        var elapsed = (new Date()).getTime() - start;
                        if (elapsed > chunkInterval) {
                            break; // been working too hard, time to take a break :-)
                        }
                    }

                    m = layersArray[offset];

                    //Not point data, can't be clustered
                    if (!m.getLatLng || m.options.ignorecluster) {
                        npg.addLayer(m);
                        continue;
                    }

                    if (this.hasLayer(m)) {
                        continue;
                    }

                    this._addLayer(m, this._maxZoom);

                    //If we just made a cluster of size 2 then we need to remove the other marker from the map (if it is) or we never will
                    if (m.__parent) {
                        if (m.__parent.getChildCount() === 2) {
                            var markers = m.__parent.getAllChildMarkers(),
                                otherMarker = markers[0] === m ? markers[1] : markers[0];
                            fg.removeLayer(otherMarker);
                        }
                    }
                }

                if (chunkProgress) {
                    // report progress and time elapsed:
                    chunkProgress(offset, layersArray.length, (new Date()).getTime() - started);
                }

                if (offset === layersArray.length) {
                    //Update the icons of all those visible clusters that were affected
                    this._featureGroup.eachLayer(function (c) {
                        if (c instanceof L.MarkerCluster && c._iconNeedsUpdate) {
                            c._updateIcon();
                        }
                    });

                    this._topClusterLevel._recursivelyAddChildrenToMap(null, this._zoom, this._currentShownBounds);
                } else {
                    setTimeout(process, this.options.chunkDelay);
                }
            }, this);

            process();
        } else {
            newMarkers = [];
            for (i = 0, l = layersArray.length; i < l; i++) {
                m = layersArray[i];

                //Not point data, can't be clustered
                if (!m.getLatLng || m.options.ignorecluster) {
                    npg.addLayer(m);
                    continue;
                }

                if (this.hasLayer(m)) {
                    continue;
                }

                newMarkers.push(m);
            }
            this._needsClustering = this._needsClustering.concat(newMarkers);
        }
        return this;
    }

});