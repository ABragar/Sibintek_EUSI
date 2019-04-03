"use strict";
var TargetType;
(function (TargetType) {
    TargetType[TargetType["ConstructorArgument"] = 0] = "ConstructorArgument";
    TargetType[TargetType["ClassProperty"] = 1] = "ClassProperty";
    TargetType[TargetType["Variable"] = 2] = "Variable";
})(TargetType || (TargetType = {}));
exports.TargetType = TargetType;
