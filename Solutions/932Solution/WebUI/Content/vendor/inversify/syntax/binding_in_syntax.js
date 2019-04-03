"use strict";
var binding_scope_1 = require("../bindings/binding_scope");
var binding_when_on_syntax_1 = require("./binding_when_on_syntax");
var BindingInSyntax = (function () {
    function BindingInSyntax(binding) {
        this._binding = binding;
    }
    BindingInSyntax.prototype.inSingletonScope = function () {
        this._binding.scope = binding_scope_1.BindingScope.Singleton;
        return new binding_when_on_syntax_1.BindingWhenOnSyntax(this._binding);
    };
    BindingInSyntax.prototype.inTransientScope = function () {
        this._binding.scope = binding_scope_1.BindingScope.Transient;
        return new binding_when_on_syntax_1.BindingWhenOnSyntax(this._binding);
    };
    return BindingInSyntax;
}());
exports.BindingInSyntax = BindingInSyntax;
