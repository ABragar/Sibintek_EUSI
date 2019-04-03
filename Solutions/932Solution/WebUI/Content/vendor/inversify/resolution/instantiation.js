"use strict";
var target_type_1 = require("../planning/target_type");
function _injectProperties(instance, childRequests, resolveRequest) {
    var propertyInjectionsRequests = childRequests.filter(function (childRequest) {
        return childRequest.target.type === target_type_1.TargetType.ClassProperty;
    });
    var propertyInjections = propertyInjectionsRequests.map(function (childRequest) {
        return resolveRequest(childRequest);
    });
    propertyInjectionsRequests.forEach(function (r, index) {
        var injection = propertyInjections[index];
        instance[r.target.name.value()] = injection;
    });
    return instance;
}
function _createInstance(Func, injections) {
    return new (Func.bind.apply(Func, [void 0].concat(injections)))();
}
function resolveInstance(constr, childRequests, resolveRequest) {
    var result = null;
    if (childRequests.length > 0) {
        var constructorInjectionsRequests = childRequests.filter(function (childRequest) {
            return childRequest.target.type === target_type_1.TargetType.ConstructorArgument;
        });
        var constructorInjections = constructorInjectionsRequests.map(function (childRequest) {
            return resolveRequest(childRequest);
        });
        result = _createInstance(constr, constructorInjections);
        result = _injectProperties(result, childRequests, resolveRequest);
    }
    else {
        result = new constr();
    }
    return result;
}
exports.resolveInstance = resolveInstance;
