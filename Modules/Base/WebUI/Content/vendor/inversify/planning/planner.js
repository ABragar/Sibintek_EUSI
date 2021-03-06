"use strict";
var plan_1 = require("./plan");
var context_1 = require("./context");
var request_1 = require("./request");
var target_1 = require("./target");
var binding_type_1 = require("../bindings/binding_type");
var binding_count_1 = require("../bindings/binding_count");
var reflection_utils_1 = require("./reflection_utils");
var metadata_1 = require("../planning/metadata");
var serialization_1 = require("../utils/serialization");
var ERROR_MSGS = require("../constants/error_msgs");
var METADATA_KEY = require("../constants/metadata_keys");
function _createTarget(isMultiInject, targetType, serviceIdentifier, name, key, value) {
    var metadataKey = isMultiInject ? METADATA_KEY.MULTI_INJECT_TAG : METADATA_KEY.INJECT_TAG;
    var injectMetadata = new metadata_1.Metadata(metadataKey, serviceIdentifier);
    var target = new target_1.Target(targetType, name, serviceIdentifier, injectMetadata);
    if (key !== undefined) {
        var tagMetadata = new metadata_1.Metadata(key, value);
        target.metadata.push(tagMetadata);
    }
    return target;
}
function _getActiveBindings(avoidConstraints, context, parentRequest, target) {
    var bindings = getBindings(context.container, target.serviceIdentifier);
    var activeBindings = [];
    if (bindings.length > 1 && avoidConstraints === false) {
        activeBindings = bindings.filter(function (binding) {
            var request = new request_1.Request(binding.serviceIdentifier, context, parentRequest, binding, target);
            return binding.constraint(request);
        });
    }
    else {
        activeBindings = bindings;
    }
    _validateActiveBindingCount(target.serviceIdentifier, activeBindings, target, context.container);
    return activeBindings;
}
function _validateActiveBindingCount(serviceIdentifier, bindings, target, container) {
    switch (bindings.length) {
        case binding_count_1.BindingCount.NoBindingsAvailable:
            var serviceIdentifierString = serialization_1.getServiceIdentifierAsString(serviceIdentifier);
            var msg = ERROR_MSGS.NOT_REGISTERED;
            msg += serialization_1.listMetadataForTarget(serviceIdentifierString, target);
            msg += serialization_1.listRegisteredBindingsForServiceIdentifier(container, serviceIdentifierString, getBindings);
            throw new Error(msg);
        case binding_count_1.BindingCount.OnlyOneBindingAvailable:
            if (target.isArray() === false) {
                return bindings;
            }
        case binding_count_1.BindingCount.MultipleBindingsAvailable:
        default:
            if (target.isArray() === false) {
                var serviceIdentifierString_1 = serialization_1.getServiceIdentifierAsString(serviceIdentifier), msg_1 = ERROR_MSGS.AMBIGUOUS_MATCH + " " + serviceIdentifierString_1;
                msg_1 += serialization_1.listRegisteredBindingsForServiceIdentifier(container, serviceIdentifierString_1, getBindings);
                throw new Error(msg_1);
            }
            else {
                return bindings;
            }
    }
}
function _createSubRequests(avoidConstraints, serviceIdentifier, context, parentRequest, target) {
    try {
        var activeBindings = void 0;
        var childRequest_1;
        if (parentRequest === null) {
            activeBindings = _getActiveBindings(avoidConstraints, context, null, target);
            childRequest_1 = new request_1.Request(serviceIdentifier, context, null, activeBindings, target);
            var plan_2 = new plan_1.Plan(context, childRequest_1);
            context.addPlan(plan_2);
        }
        else {
            activeBindings = _getActiveBindings(avoidConstraints, context, parentRequest, target);
            childRequest_1 = parentRequest.addChildRequest(target.serviceIdentifier, activeBindings, target);
        }
        activeBindings.forEach(function (binding) {
            var subChildRequest = null;
            if (target.isArray()) {
                subChildRequest = childRequest_1.addChildRequest(binding.serviceIdentifier, binding, target);
            }
            else {
                subChildRequest = childRequest_1;
            }
            if (binding.type === binding_type_1.BindingType.Instance) {
                var dependencies = reflection_utils_1.getDependencies(binding.implementationType);
                dependencies.forEach(function (dependency) {
                    _createSubRequests(false, dependency.serviceIdentifier, context, subChildRequest, dependency);
                });
            }
        });
    }
    catch (error) {
        if (error instanceof RangeError) {
            serialization_1.circularDependencyToException(parentRequest.parentContext.plan.rootRequest);
        }
        else {
            throw new Error(error.message);
        }
    }
}
function getBindings(container, serviceIdentifier) {
    var bindings = [];
    var bindingDictionary = container._bindingDictionary;
    if (bindingDictionary.hasKey(serviceIdentifier)) {
        bindings = bindingDictionary.get(serviceIdentifier);
    }
    else if (container.parent !== null) {
        bindings = getBindings(container.parent, serviceIdentifier);
    }
    return bindings;
}
function plan(container, isMultiInject, targetType, serviceIdentifier, key, value, avoidConstraints) {
    if (avoidConstraints === void 0) { avoidConstraints = false; }
    var context = new context_1.Context(container);
    var target = _createTarget(isMultiInject, targetType, serviceIdentifier, "", key, value);
    _createSubRequests(avoidConstraints, serviceIdentifier, context, null, target);
    return context;
}
exports.plan = plan;
