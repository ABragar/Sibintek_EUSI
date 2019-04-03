(function (L) {
    L.Control.PbaCopyright = L.Control.extend({
        options: {
            // topright, topleft, bottomleft, bottomright
            position: "bottomright"
        },
        initialize: function (options) {
            this.options = L.extend(this.options, options);
        },
        onAdd: function (map) {
            var controlDiv = L.DomUtil.create("div", "leaflet-control-attribution show");
            var pbaDiv = L.DomUtil.create("div", "pba-copyright", controlDiv);
            //var pbaLink = L.DomUtil.create("a", "pba-copyright-link", pbaDiv);

            //pbaLink.href = 'http://pba.su/';
            //pbaLink.rel = 'nofollow';
            //pbaLink.target = '_blank';

            return controlDiv;
        }
    });
})(L);