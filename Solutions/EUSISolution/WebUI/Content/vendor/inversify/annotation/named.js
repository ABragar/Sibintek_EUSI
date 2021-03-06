"use strict";
var metadata_1 = require("../planning/metadata");
var decorator_utils_1 = require("./decorator_utils");
var METADATA_KEY = require("../constants/metadata_keys");
function named(name) {
    return function (target, targetKey, index) {
        var metadata = new metadata_1.Metadata(METADATA_KEY.NAMED_TAG, name);
        if (typeof index === "number") {
            return decorator_utils_1.tagParameter(target, targetKey, index, metadata);
        }
        else {
            return decorator_utils_1.tagProperty(target, targetKey, metadata);
        }
    };
}
exports.named = named;
