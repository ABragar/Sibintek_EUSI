"use strict";
var metadata_1 = require("../planning/metadata");
var METADATA_KEY = require("../constants/metadata_keys");
var traverseAncerstors = function (request, constraint) {
    var parent = request.parentRequest;
    if (parent !== null) {
        return constraint(parent) ? true : traverseAncerstors(parent, constraint);
    }
    else {
        return false;
    }
};
exports.traverseAncerstors = traverseAncerstors;
var taggedConstraint = function (key) { return function (value) {
    var constraint = function (request) {
        return request.target.matchesTag(key)(value);
    };
    constraint.metaData = new metadata_1.Metadata(key, value);
    return constraint;
}; };
exports.taggedConstraint = taggedConstraint;
var namedConstraint = taggedConstraint(METADATA_KEY.NAMED_TAG);
exports.namedConstraint = namedConstraint;
var typeConstraint = function (type) { return function (request) {
    var binding = request.bindings[0];
    if (typeof type === "string") {
        var serviceIdentifier = binding.serviceIdentifier;
        return serviceIdentifier === type;
    }
    else {
        var constructor = request.bindings[0].implementationType;
        return type === constructor;
    }
}; };
exports.typeConstraint = typeConstraint;
