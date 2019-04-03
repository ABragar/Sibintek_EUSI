"use strict";
var binding_type_1 = require("./binding_type");
var guid_1 = require("../utils/guid");
var Binding = (function () {
    function Binding(serviceIdentifier, defaultScope) {
        this.guid = guid_1.guid();
        this.activated = false;
        this.serviceIdentifier = serviceIdentifier;
        this.scope = defaultScope;
        this.type = binding_type_1.BindingType.Invalid;
        this.constraint = function (request) { return true; };
        this.implementationType = null;
        this.cache = null;
        this.factory = null;
        this.provider = null;
        this.onActivation = null;
    }
    Binding.prototype.clone = function () {
        var clone = new Binding(this.serviceIdentifier, this.scope);
        clone.activated = false;
        clone.implementationType = this.implementationType;
        clone.dynamicValue = this.dynamicValue;
        clone.scope = this.scope;
        clone.type = this.type;
        clone.factory = this.factory;
        clone.provider = this.provider;
        clone.constraint = this.constraint;
        clone.onActivation = this.onActivation;
        clone.cache = this.cache;
        return clone;
    };
    return Binding;
}());
exports.Binding = Binding;
