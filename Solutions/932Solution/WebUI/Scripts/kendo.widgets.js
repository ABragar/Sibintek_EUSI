(function (kendo, $, application, pbaAPI) {
    'use strict';

    var DATABINDING = "dataBinding", DATABOUND = "dataBound", CHANGE = "change", FOCUS = "focus";

    var EnumView = kendo.ui.Widget.extend({
        _enums: {},
        _value: null,
        _template: null,

        init: function(element, options) {
            kendo.ui.Widget.fn.init.call(this, element, options);
            this._template = kendo.template(this.options.template && this.options.template.length ? this.options.template : '<label class="property-icon" style="background-color: #: Color #"><span class="#: Icon #"></span></label>#: Title #');
        },

        options: {
            name: "EnumView",
            template: "",
            type: ""
        },

        refresh: function() {
            var self = this;

            self.trigger(DATABINDING);

            application.UiEnums.get(self.options.type, function(data) {
                if (data && data.Values[self._value]) {
                    self.element.html(self._template(data.Values[self._value]));
                }
                self.trigger(DATABOUND);
            });
        },

        value: function(value) {
            if (value !== undefined) {
                this._value = value;
                return this.refresh();
            } else {
                return this._value;
            }
        },

        destroy: function() {
            self.element.html('');
        },

        events: [DATABINDING, DATABOUND]

    });

    var getBaseObjectTemplate = function(options) {
     return '\
            # if(data.hasOwnProperty("' + options.lookupimage + '") && data["' + options.lookupimage + '"]) { # \
                <img src="#= pbaAPI.imageHelpers.getsrc(data["' + options.lookupimage + '"].FileID, 50, 50, "NoPhoto") #" alt="" />\
            # } else if (data.BoType && data.BoType.Icon) { # \
            \
            # } #\
            #: data["' + options.lookuptext + '"] #';
    };

    var BaseObjectOneView = kendo.ui.Widget.extend({
        _value: null,
        _template: null,

        init: function (element, options) {
            kendo.ui.Widget.fn.init.call(this, element, options);
            //console.log(getBaseObjectTemplate(this.options));
            this._template = kendo.template(this.options.template && this.options.template.length ? this.options.template : getBaseObjectTemplate(this.options));
        },

        options: {
            name: "BaseObjectOneView",
            template: "",
            lookuptext: "Title",
            lookupimage: "Image"
        },

        refresh: function () {
            var self = this;
            //console.log('Base Object One View:', self._value);
            self.trigger(DATABINDING);
            self.element.html(self._template(self._value));
            self.trigger(DATABOUND);
        },

        value: function (value) {
            if (value !== undefined) {
                this._value = value;
                return this.refresh();
            } else {
                return this._value;
            }
        },

        destroy: function () {
            self.element.html('');

        },

        events: [DATABINDING, DATABOUND]

    });

    var locationTemplate = '\
                            <p>Адрес: #: data.Address #</p>\
                            # if(data.Disposition) { #\
                            <p>Тип геометрии: #= pbaAPI.geo.namedGeometryType(data) # </p>\
                            # if(data.Disposition.crs && data.Disposition.crs.properties && data.Disposition.crs.properties.name) { #\
                            <p>Система координат: #: data.Disposition.crs.properties.name #</p>\
                            # } #\
                            # } #\
                           ';


    var LocationView = kendo.ui.Widget.extend({
        _value: null,
        _template: null,

        init: function (element, options) {
            kendo.ui.Widget.fn.init.call(this, element, options);
            //console.log(getBaseObjectTemplate(this.options));
            this._template = kendo.template(this.options.template && this.options.template.length ? this.options.template : locationTemplate);
        },

        options: {
            name: "LocationView",
            template: ""
        },

        refresh: function () {
            var self = this;
            console.log('Location View:', self._value);
            self.trigger(DATABINDING);
            self.element.html(self._template(self._value));
            self.trigger(DATABOUND);
        },

        value: function (value) {
            if (value !== undefined) {
                this._value = value;
                return this.refresh();
            } else {
                return this._value;
            }
        },

        destroy: function () {
            self.element.html('');

        },

        events: [DATABINDING, DATABOUND]

    });


    kendo.ui.plugin(EnumView);
    kendo.ui.plugin(BaseObjectOneView);
    kendo.ui.plugin(LocationView);

})(window.kendo, window.jQuery, window.application, window.pbaAPI);