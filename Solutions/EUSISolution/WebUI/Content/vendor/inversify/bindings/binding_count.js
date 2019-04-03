"use strict";
var BindingCount;
(function (BindingCount) {
    BindingCount[BindingCount["NoBindingsAvailable"] = 0] = "NoBindingsAvailable";
    BindingCount[BindingCount["OnlyOneBindingAvailable"] = 1] = "OnlyOneBindingAvailable";
    BindingCount[BindingCount["MultipleBindingsAvailable"] = 2] = "MultipleBindingsAvailable";
})(BindingCount || (BindingCount = {}));
exports.BindingCount = BindingCount;
