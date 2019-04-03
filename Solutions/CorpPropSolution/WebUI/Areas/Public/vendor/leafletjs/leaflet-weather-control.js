(function(L) {
            L.Control.Weather = L.Control.extend({
                options: {
                    // topright, topleft, bottomleft, bottomright
                    position: "topright"
                },
                initialize: function (options) {
                    // constructor
                },
                onAdd: function (map) {
                    var controlDiv = L.DomUtil.create("div", "leaflet-control-weather");
                    var controlTemp = L.DomUtil.create("div", "leaflet-control-weather-temp", controlDiv);
                    var controlWind = L.DomUtil.create("div", "leaflet-control-weather-wind", controlDiv);

                    $.simpleWeather({
                        woeid: "2122265", //MO
                        unit: "c",
                        success: function (weather) {
                            console.log("Weather:", weather);

                            var $control = $(controlDiv);
                            var $temp = $(controlTemp);
                            var $wind = $(controlWind);

                            $control.css("background-image", "url(" + weather.image + ")");
                            $temp.html(weather.temp + "&deg;C");
                            $wind.html(weather.wind.chill + "&deg;C&nbsp;" + weather.wind.speed + "&nbsp;" + weather.wind.direction);
                        },
                        error: function (error) {
                            console.log("Weather error:", error);
                        }
                    });

                    return controlDiv;
                },
                onRemove: function (map) {
                    // when removed
                }
            });
})(L);