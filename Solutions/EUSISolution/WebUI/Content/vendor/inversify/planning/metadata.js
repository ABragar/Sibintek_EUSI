"use strict";
var METADATA_KEY = require("../constants/metadata_keys");
var Metadata = (function () {
    function Metadata(key, value) {
        this.key = key;
        this.value = value;
    }
    Metadata.prototype.toString = function () {
        if (this.key === METADATA_KEY.NAMED_TAG) {
            return "named: " + this.value + " ";
        }
        else {
            return "tagged: { key:" + this.key + ", value: " + this.value + " }";
        }
    };
    return Metadata;
}());
exports.Metadata = Metadata;
