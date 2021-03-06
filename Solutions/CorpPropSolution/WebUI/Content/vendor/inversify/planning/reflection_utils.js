"use strict";
var serialization_1 = require("../utils/serialization");
var target_type_1 = require("./target_type");
var target_1 = require("./target");
var ERROR_MSGS = require("../constants/error_msgs");
var METADATA_KEY = require("../constants/metadata_keys");
function getDependencies(func) {
    var constructorName = serialization_1.getFunctionName(func);
    var targets = getTargets(func, false);
    var baseClassDepencencyCount = getBaseClassDepencencyCount(func);
    if (targets.length < baseClassDepencencyCount) {
        var error = ERROR_MSGS.ARGUMENTS_LENGTH_MISMATCH_1 +
            constructorName + ERROR_MSGS.ARGUMENTS_LENGTH_MISMATCH_2;
        throw new Error(error);
    }
    return targets;
}
exports.getDependencies = getDependencies;
function getTargets(func, isBaseClass) {
    var constructorName = serialization_1.getFunctionName(func);
    var serviceIdentifiers = Reflect.getMetadata(METADATA_KEY.PARAM_TYPES, func);
    if (serviceIdentifiers === undefined) {
        var msg = ERROR_MSGS.MISSING_INJECTABLE_ANNOTATION + " " + constructorName + ".";
        throw new Error(msg);
    }
    var constructorArgsMetadata = Reflect.getMetadata(METADATA_KEY.TAGGED, func) || [];
    var targets = (getConstructorArgsAsTargets(isBaseClass, constructorName, serviceIdentifiers, constructorArgsMetadata, func.length)).concat((getClassPropsAsTargets(func)));
    return targets;
}
function getConstructorArgsAsTargets(isBaseClass, constructorName, serviceIdentifiers, constructorArgsMetadata, constructorLength) {
    var targets = [];
    for (var i = 0; i < constructorLength; i++) {
        var targetMetadata = constructorArgsMetadata[i.toString()] || [];
        var metadata = formatTargetMetadata(targetMetadata);
        var serviceIndentifier = serviceIdentifiers[i];
        var hasInjectAnnotations = (metadata.inject || metadata.multiInject);
        serviceIndentifier = (hasInjectAnnotations) ? (hasInjectAnnotations) : serviceIndentifier;
        var isObject = serviceIndentifier === Object;
        var isFunction = serviceIndentifier === Function;
        var isUndefined = serviceIndentifier === undefined;
        var isUnknownType = (isObject || isFunction || isUndefined);
        if (isBaseClass === false && isUnknownType) {
            var msg = ERROR_MSGS.MISSING_INJECT_ANNOTATION + " argument " + i + " in class " + constructorName + ".";
            throw new Error(msg);
        }
        var target = new target_1.Target(target_type_1.TargetType.ConstructorArgument, metadata.targetName, serviceIndentifier);
        target.metadata = targetMetadata;
        targets.push(target);
    }
    return targets;
}
function getClassPropsAsTargets(func) {
    var classPropsMetadata = Reflect.getMetadata(METADATA_KEY.TAGGED_PROP, func) || [];
    var targets = [];
    var keys = Object.keys(classPropsMetadata);
    for (var i = 0; i < keys.length; i++) {
        var key = keys[i];
        var targetMetadata = classPropsMetadata[key];
        var metadata = formatTargetMetadata(classPropsMetadata[key]);
        var targetName = metadata.targetName || key;
        var serviceIndentifier = (metadata.inject || metadata.multiInject);
        var target = new target_1.Target(target_type_1.TargetType.ClassProperty, targetName, serviceIndentifier);
        target.metadata = targetMetadata;
        targets.push(target);
    }
    var baseConstructor = Object.getPrototypeOf(func.prototype).constructor;
    if (baseConstructor !== Object) {
        var baseTargets = getClassPropsAsTargets(baseConstructor);
        targets = targets.concat(baseTargets);
    }
    return targets;
}
function getBaseClassDepencencyCount(func) {
    var baseConstructor = Object.getPrototypeOf(func.prototype).constructor;
    if (baseConstructor !== Object) {
        var targets = getTargets(baseConstructor, true);
        var metadata = targets.map(function (t) {
            return t.metadata.filter(function (m) {
                return m.key === METADATA_KEY.UNMANAGED_TAG;
            });
        });
        var unmanagedCount = [].concat.apply([], metadata).length;
        var dependencyCount = targets.length - unmanagedCount;
        if (dependencyCount > 0) {
            return dependencyCount;
        }
        else {
            return getBaseClassDepencencyCount(baseConstructor);
        }
    }
    else {
        return 0;
    }
}
function formatTargetMetadata(targetMetadata) {
    var targetMetadataMap = {};
    targetMetadata.forEach(function (m) {
        targetMetadataMap[m.key.toString()] = m.value;
    });
    return {
        inject: targetMetadataMap[METADATA_KEY.INJECT_TAG],
        multiInject: targetMetadataMap[METADATA_KEY.MULTI_INJECT_TAG],
        targetName: targetMetadataMap[METADATA_KEY.NAME_TAG],
        unmanaged: targetMetadataMap[METADATA_KEY.UNMANAGED_TAG]
    };
}
