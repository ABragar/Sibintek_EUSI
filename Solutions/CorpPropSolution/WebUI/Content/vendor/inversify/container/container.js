"use strict";
var binding_1 = require("../bindings/binding");
var binding_scope_1 = require("../bindings/binding_scope");
var lookup_1 = require("./lookup");
var planner_1 = require("../planning/planner");
var resolver_1 = require("../resolution/resolver");
var binding_to_syntax_1 = require("../syntax/binding_to_syntax");
var target_type_1 = require("../planning/target_type");
var serialization_1 = require("../utils/serialization");
var container_snapshot_1 = require("./container_snapshot");
var guid_1 = require("../utils/guid");
var ERROR_MSGS = require("../constants/error_msgs");
var METADATA_KEY = require("../constants/metadata_keys");
var Container = (function () {
    function Container(containerOptions) {
        if (containerOptions !== undefined) {
            if (typeof containerOptions !== "object") {
                throw new Error("" + ERROR_MSGS.KERNEL_OPTIONS_MUST_BE_AN_OBJECT);
            }
            else if (containerOptions.defaultScope === undefined) {
                throw new Error("" + ERROR_MSGS.KERNEL_OPTIONS_INVALID_DEFAULT_SCOPE);
            }
            else if (containerOptions.defaultScope !== "singleton" && containerOptions.defaultScope !== "transient") {
                throw new Error("" + ERROR_MSGS.KERNEL_OPTIONS_INVALID_DEFAULT_SCOPE);
            }
            this.options = {
                defaultScope: containerOptions.defaultScope
            };
        }
        else {
            this.options = {
                defaultScope: "transient"
            };
        }
        this.guid = guid_1.guid();
        this._bindingDictionary = new lookup_1.Lookup();
        this._snapshots = [];
        this._middleware = null;
        this._parentContainer = null;
    }
    Container.merge = function (container1, container2) {
        var container = new Container();
        var bindingDictionary = container._bindingDictionary;
        var bindingDictionary1 = container1._bindingDictionary;
        var bindingDictionary2 = container2._bindingDictionary;
        function copyDictionary(origing, destination) {
            origing.traverse(function (key, value) {
                value.forEach(function (binding) {
                    destination.add(binding.serviceIdentifier, binding.clone());
                });
            });
        }
        copyDictionary(bindingDictionary1, bindingDictionary);
        copyDictionary(bindingDictionary2, bindingDictionary);
        return container;
    };
    Container.prototype.load = function () {
        var _this = this;
        var modules = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            modules[_i - 0] = arguments[_i];
        }
        var getBindFunction = function (moduleId) {
            return function (serviceIdentifier) {
                var _bind = _this.bind.bind(_this);
                var bindingToSyntax = _bind(serviceIdentifier);
                bindingToSyntax._binding.moduleId = moduleId;
                return bindingToSyntax;
            };
        };
        modules.forEach(function (module) {
            var bindFunction = getBindFunction(module.guid);
            module.registry(bindFunction);
        });
    };
    Container.prototype.unload = function () {
        var _this = this;
        var modules = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            modules[_i - 0] = arguments[_i];
        }
        var conditionFactory = function (expected) { return function (item) {
            return item.moduleId === expected;
        }; };
        modules.forEach(function (module) {
            var condition = conditionFactory(module.guid);
            _this._bindingDictionary.removeByCondition(condition);
        });
    };
    Container.prototype.bind = function (serviceIdentifier) {
        var defaultScope = (this.options.defaultScope === "transient") ? binding_scope_1.BindingScope.Transient : binding_scope_1.BindingScope.Singleton;
        var binding = new binding_1.Binding(serviceIdentifier, defaultScope);
        this._bindingDictionary.add(serviceIdentifier, binding);
        return new binding_to_syntax_1.BindingToSyntax(binding);
    };
    Container.prototype.unbind = function (serviceIdentifier) {
        try {
            this._bindingDictionary.remove(serviceIdentifier);
        }
        catch (e) {
            throw new Error(ERROR_MSGS.CANNOT_UNBIND + " " + serialization_1.getServiceIdentifierAsString(serviceIdentifier));
        }
    };
    Container.prototype.unbindAll = function () {
        this._bindingDictionary = new lookup_1.Lookup();
    };
    Container.prototype.isBound = function (serviceIdentifier) {
        return this._bindingDictionary.hasKey(serviceIdentifier);
    };
    Container.prototype.snapshot = function () {
        this._snapshots.push(container_snapshot_1.ContainerSnapshot.of(this._bindingDictionary.clone(), this._middleware));
    };
    Container.prototype.restore = function () {
        if (this._snapshots.length === 0) {
            throw new Error(ERROR_MSGS.NO_MORE_SNAPSHOTS_AVAILABLE);
        }
        var snapshot = this._snapshots.pop();
        this._bindingDictionary = snapshot.bindings;
        this._middleware = snapshot.middleware;
    };
    Container.prototype.createChild = function () {
        var child = new Container();
        child.parent = this;
        return child;
    };
    Object.defineProperty(Container.prototype, "parent", {
        get: function () {
            return this._parentContainer;
        },
        set: function (container) {
            this._parentContainer = container;
        },
        enumerable: true,
        configurable: true
    });
    Container.prototype.applyMiddleware = function () {
        var middlewares = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            middlewares[_i - 0] = arguments[_i];
        }
        var initial = (this._middleware) ? this._middleware : this._planAndResolve();
        this._middleware = middlewares.reduce(function (prev, curr) {
            return curr(prev);
        }, initial);
    };
    Container.prototype.get = function (serviceIdentifier) {
        return this._get(false, false, target_type_1.TargetType.Variable, serviceIdentifier);
    };
    Container.prototype.getTagged = function (serviceIdentifier, key, value) {
        return this._get(false, false, target_type_1.TargetType.Variable, serviceIdentifier, key, value);
    };
    Container.prototype.getNamed = function (serviceIdentifier, named) {
        return this.getTagged(serviceIdentifier, METADATA_KEY.NAMED_TAG, named);
    };
    Container.prototype.getAll = function (serviceIdentifier) {
        return this._get(true, true, target_type_1.TargetType.Variable, serviceIdentifier);
    };
    Container.prototype.getAllTagged = function (serviceIdentifier, key, value) {
        return this._get(false, true, target_type_1.TargetType.Variable, serviceIdentifier, key, value);
    };
    Container.prototype.getAllNamed = function (serviceIdentifier, named) {
        return this.getAllTagged(serviceIdentifier, METADATA_KEY.NAMED_TAG, named);
    };
    Container.prototype._get = function (avoidConstraints, isMultiInject, targetType, serviceIdentifier, key, value) {
        var result = null;
        var args = {
            avoidConstraints: avoidConstraints,
            contextInterceptor: function (context) { return context; },
            isMultiInject: isMultiInject,
            key: key,
            serviceIdentifier: serviceIdentifier,
            targetType: targetType,
            value: value
        };
        if (this._middleware) {
            result = this._middleware(args);
            if (result === undefined || result === null) {
                throw new Error(ERROR_MSGS.INVALID_MIDDLEWARE_RETURN);
            }
        }
        else {
            result = this._planAndResolve()(args);
        }
        return result;
    };
    Container.prototype._planAndResolve = function () {
        var _this = this;
        return function (args) {
            var context = planner_1.plan(_this, args.isMultiInject, args.targetType, args.serviceIdentifier, args.key, args.value, args.avoidConstraints);
            var result = resolver_1.resolve(args.contextInterceptor(context));
            return result;
        };
    };
    return Container;
}());
exports.Container = Container;
