"use strict";
var metadata_1 = require("../planning/metadata");
var decorator_utils_1 = require("./decorator_utils");
function tagged(metadataKey, metadataValue) {
    return function (target, targetKey, index) {
        var metadata = new metadata_1.Metadata(metadataKey, metadataValue);
        if (typeof index === "number") {
            return decorator_utils_1.tagParameter(target, targetKey, index, metadata);
        }
        else {
            return decorator_utils_1.tagProperty(target, targetKey, metadata);
        }
    };
}
exports.tagged = tagged;
