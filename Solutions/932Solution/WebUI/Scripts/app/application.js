var App;
(function (App) {
    var Application;
    (function (Application) {
        function Run() {
            Builder.InitResolver();
            console.log('Run:', Application.DependencyResolver.current);
        }
        Application.Run = Run;
        var Builder = (function () {
            function Builder() {
            }
            Builder.InitResolver = function () {
                var resolver = new Application.InversifyDependencyResolver();
                Application.DependencyResolver.SetResolver(resolver);
                return Application.DependencyResolver.current;
            };
            return Builder;
        }());
    })(Application = App.Application || (App.Application = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Application;
    (function (Application) {
        var DependencyResolver = (function () {
            function DependencyResolver() {
            }
            DependencyResolver.SetResolver = function (resolver) {
                DependencyResolver.current = resolver;
            };
            DependencyResolver.current = null;
            return DependencyResolver;
        }());
        Application.DependencyResolver = DependencyResolver;
    })(Application = App.Application || (App.Application = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Application;
    (function (Application) {
        var InversifyDependencyResolver = (function () {
            function InversifyDependencyResolver() {
                this._kernel = new App.Libs.Inversify.Kernel();
                this.InitBindings();
            }
            InversifyDependencyResolver.prototype.GetService = function (serviceName) {
                return this._kernel.resolve(serviceName);
            };
            InversifyDependencyResolver.prototype.InitBindings = function () {
                this._kernel.bind(new App.Libs.Inversify.TypeBinding("ITelephony", App.Communication.Telephony, App.Libs.Inversify.TypeBindingScopeEnum.Singleton));
            };
            return InversifyDependencyResolver;
        }());
        Application.InversifyDependencyResolver = InversifyDependencyResolver;
    })(Application = App.Application || (App.Application = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Communication;
    (function (Communication) {
        var Telephony = (function () {
            function Telephony() {
            }
            Telephony.prototype.Call = function () {
            };
            Telephony.prototype.Drop = function () {
            };
            return Telephony;
        }());
        Communication.Telephony = Telephony;
    })(Communication = App.Communication || (App.Communication = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Libs;
    (function (Libs) {
        var Inversify;
        (function (Inversify) {
            function inject() {
                var injectionKeys = [];
                for (var _i = 0; _i < arguments.length; _i++) {
                    injectionKeys[_i - 0] = arguments[_i];
                }
                return function (target) {
                    target["__inject__"] = injectionKeys;
                };
            }
            Inversify.inject = inject;
        })(Inversify = Libs.Inversify || (Libs.Inversify = {}));
    })(Libs = App.Libs || (App.Libs = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Libs;
    (function (Libs) {
        var Inversify;
        (function (Inversify) {
            (function (TypeBindingScopeEnum) {
                TypeBindingScopeEnum[TypeBindingScopeEnum["Transient"] = 0] = "Transient";
                TypeBindingScopeEnum[TypeBindingScopeEnum["Singleton"] = 1] = "Singleton";
            })(Inversify.TypeBindingScopeEnum || (Inversify.TypeBindingScopeEnum = {}));
            var TypeBindingScopeEnum = Inversify.TypeBindingScopeEnum;
        })(Inversify = Libs.Inversify || (Libs.Inversify = {}));
    })(Libs = App.Libs || (App.Libs = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Libs;
    (function (Libs) {
        var Inversify;
        (function (Inversify) {
            var KeyValuePair = (function () {
                function KeyValuePair(key, value) {
                    this.key = key;
                    this.value = new Array();
                    this.value.push(value);
                }
                return KeyValuePair;
            }());
            var Lookup = (function () {
                function Lookup() {
                    this._hashMap = {};
                }
                Lookup.prototype.add = function (key, value) {
                    if (key === null || key === undefined) {
                        throw new Error("Argument Null");
                    }
                    if (value === null || value === undefined) {
                        throw new Error("Argument Null");
                    }
                    var k = "$" + key;
                    var previousPair = this._hashMap[k];
                    if (previousPair != null) {
                        previousPair.value.push(value);
                    }
                    else {
                        this._hashMap[k] = new KeyValuePair(key, value);
                    }
                };
                Lookup.prototype.get = function (key) {
                    if (key === null || key === undefined) {
                        throw new Error("Argument Null");
                    }
                    var keyValuePair = this._hashMap[("$" + key)];
                    if (keyValuePair == null) {
                        throw new Error("Key Not Found");
                    }
                    return keyValuePair.value;
                };
                Lookup.prototype.remove = function (key) {
                    if (key === null || key === undefined) {
                        throw new Error("Argument Null");
                    }
                    var k = "$" + key;
                    var previousPair = this._hashMap[k];
                    if (previousPair != null) {
                        delete this._hashMap[k];
                    }
                    else {
                        throw new Error("Key Not Found");
                    }
                };
                Lookup.prototype.hasKey = function (key) {
                    if (key === null || key === undefined) {
                        throw new Error("Argument Null");
                    }
                    var keyValuePair = this._hashMap[("$" + key)];
                    return keyValuePair != null;
                };
                return Lookup;
            }());
            Inversify.Lookup = Lookup;
        })(Inversify = Libs.Inversify || (Libs.Inversify = {}));
    })(Libs = App.Libs || (App.Libs = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Libs;
    (function (Libs) {
        var Inversify;
        (function (Inversify) {
            var Kernel = (function () {
                function Kernel() {
                    this._bindingDictionary = new Inversify.Lookup();
                }
                Kernel.prototype.bind = function (typeBinding) {
                    this._bindingDictionary.add(typeBinding.runtimeIdentifier, typeBinding);
                };
                Kernel.prototype.unbind = function (runtimeIdentifier) {
                    try {
                        this._bindingDictionary.remove(runtimeIdentifier);
                    }
                    catch (e) {
                        throw new Error("Could not resolve service " + runtimeIdentifier);
                    }
                };
                Kernel.prototype.unbindAll = function () {
                    this._bindingDictionary = new Inversify.Lookup();
                };
                Kernel.prototype.resolve = function (runtimeIdentifier) {
                    var bindings;
                    if (this._bindingDictionary.hasKey(runtimeIdentifier)) {
                        bindings = this._bindingDictionary.get(runtimeIdentifier);
                    }
                    else {
                        return null;
                    }
                    var binding = bindings[0];
                    if ((binding.scope === Inversify.TypeBindingScopeEnum.Singleton) && (binding.cache !== null)) {
                        return binding.cache;
                    }
                    else {
                        var result = this._injectDependencies(binding.implementationType);
                        binding.cache = result;
                        return result;
                    }
                };
                Kernel.prototype._getConstructorArguments = function (func) {
                    var result;
                    var STRIP_COMMENTS = /((\/\/.*$)|(\/\*[\s\S]*?\*\/))/mg;
                    var ARGUMENT_NAMES = /([^\s,]+)/g;
                    var fnStr = func.toString().replace(STRIP_COMMENTS, '');
                    var argsInit = fnStr.indexOf('(') + 1;
                    var argsEnd = fnStr.indexOf(')');
                    if ("function" === typeof Map &&
                        fnStr.indexOf("class") !== -1 &&
                        fnStr.indexOf("constructor") === -1) {
                        result = null;
                    }
                    else {
                        result = fnStr.slice(argsInit, argsEnd).match(ARGUMENT_NAMES);
                    }
                    if (result === null) {
                        result = [];
                    }
                    return result;
                };
                Kernel.prototype._getConstructorArgumentsFromStaticProperty = function (func, propertyName) {
                    var result = null;
                    if (func[propertyName] != null && func[propertyName] instanceof Array) {
                        result = func[propertyName];
                    }
                    if (result === null) {
                        result = [];
                    }
                    return result;
                };
                Kernel.prototype._injectDependencies = function (func) {
                    var args = this._getConstructorArgumentsFromStaticProperty(func, "__inject__");
                    if (args.length === 0) {
                        args = this._getConstructorArguments(func);
                    }
                    if (args.length === 0) {
                        return new func();
                    }
                    else {
                        var injections = [];
                        var implementation = null;
                        for (var _i = 0, args_1 = args; _i < args_1.length; _i++) {
                            var service = args_1[_i];
                            implementation = this.resolve(service);
                            injections.push(implementation);
                        }
                        return this._construct(func, injections);
                    }
                };
                Kernel.prototype._construct = function (constr, args) {
                    return new (Function.prototype.bind.apply(constr, [null].concat(args)));
                };
                return Kernel;
            }());
            Inversify.Kernel = Kernel;
        })(Inversify = Libs.Inversify || (Libs.Inversify = {}));
    })(Libs = App.Libs || (App.Libs = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Libs;
    (function (Libs) {
        var Inversify;
        (function (Inversify) {
            var TypeBinding = (function () {
                function TypeBinding(runtimeIdentifier, implementationType, scopeType) {
                    this.runtimeIdentifier = runtimeIdentifier;
                    this.implementationType = implementationType;
                    this.cache = null;
                    if (typeof scopeType === "undefined") {
                        this.scope = Inversify.TypeBindingScopeEnum.Transient;
                    }
                    else {
                        if (Inversify.TypeBindingScopeEnum[scopeType]) {
                            this.scope = scopeType;
                        }
                        else {
                            var msg = "Invalid scope type " + scopeType;
                            throw new Error(msg);
                        }
                    }
                }
                return TypeBinding;
            }());
            Inversify.TypeBinding = TypeBinding;
        })(Inversify = Libs.Inversify || (Libs.Inversify = {}));
    })(Libs = App.Libs || (App.Libs = {}));
})(App || (App = {}));
//# sourceMappingURL=application.js.map