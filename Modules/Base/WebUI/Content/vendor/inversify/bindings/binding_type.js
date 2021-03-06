"use strict";
var BindingType;
(function (BindingType) {
    BindingType[BindingType["Invalid"] = 0] = "Invalid";
    BindingType[BindingType["Instance"] = 1] = "Instance";
    BindingType[BindingType["ConstantValue"] = 2] = "ConstantValue";
    BindingType[BindingType["DynamicValue"] = 3] = "DynamicValue";
    BindingType[BindingType["Constructor"] = 4] = "Constructor";
    BindingType[BindingType["Factory"] = 5] = "Factory";
    BindingType[BindingType["Function"] = 6] = "Function";
    BindingType[BindingType["Provider"] = 7] = "Provider";
})(BindingType || (BindingType = {}));
exports.BindingType = BindingType;
