"use strict";
var BindingScope;
(function (BindingScope) {
    BindingScope[BindingScope["Transient"] = 0] = "Transient";
    BindingScope[BindingScope["Singleton"] = 1] = "Singleton";
})(BindingScope || (BindingScope = {}));
exports.BindingScope = BindingScope;
