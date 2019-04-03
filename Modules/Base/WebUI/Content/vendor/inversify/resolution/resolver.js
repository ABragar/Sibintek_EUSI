"use strict";
var binding_scope_1 = require("../bindings/binding_scope");
var binding_type_1 = require("../bindings/binding_type");
var serialization_1 = require("../utils/serialization");
var instantiation_1 = require("./instantiation");
var ERROR_MSGS = require("../constants/error_msgs");
function _resolveRequest(request) {
    var bindings = request.bindings;
    var childRequests = request.childRequests;
    var targetIsAnAray = request.target && request.target.isArray();
    var targetParentIsNotAnArray = !request.parentRequest ||
        !request.parentRequest.target ||
        !request.parentRequest.target.matchesArray(request.target.serviceIdentifier);
    if (targetIsAnAray && targetParentIsNotAnArray) {
        return childRequests.map(function (childRequest) {
            return _resolveRequest(childRequest);
        });
    }
    else {
        var result = null;
        var binding = bindings[0];
        var isSingleton = binding.scope === binding_scope_1.BindingScope.Singleton;
        if (isSingleton && binding.activated === true) {
            return binding.cache;
        }
        switch (binding.type) {
            case binding_type_1.BindingType.ConstantValue:
                result = binding.cache;
                break;
            case binding_type_1.BindingType.DynamicValue:
                result = binding.dynamicValue(request.parentContext);
                break;
            case binding_type_1.BindingType.Constructor:
                result = binding.implementationType;
                break;
            case binding_type_1.BindingType.Factory:
                result = binding.factory(request.parentContext);
                break;
            case binding_type_1.BindingType.Function:
                result = binding.cache;
                break;
            case binding_type_1.BindingType.Provider:
                result = binding.provider(request.parentContext);
                break;
            case binding_type_1.BindingType.Instance:
                result = instantiation_1.resolveInstance(binding.implementationType, childRequests, _resolveRequest);
                break;
            case binding_type_1.BindingType.Invalid:
            default:
                var serviceIdentifier = serialization_1.getServiceIdentifierAsString(request.serviceIdentifier);
                throw new Error(ERROR_MSGS.INVALID_BINDING_TYPE + " " + serviceIdentifier);
        }
        if (typeof binding.onActivation === "function") {
            result = binding.onActivation(request.parentContext, result);
        }
        if (isSingleton) {
            binding.cache = result;
            binding.activated = true;
        }
        return result;
    }
}
function resolve(context) {
    return _resolveRequest(context.plan.rootRequest);
}
exports.resolve = resolve;
