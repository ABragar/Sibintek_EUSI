/////<reference path="./interfaces.d.ts" />

//import {TypeBindingScopeEnum} from "./TypeBindingScope";
//import {Lookup} from "./Lookup";

namespace App.Libs.Inversify {
    declare var Map;

    export class Kernel implements IKernel {
        private _bindingDictionary: ILookup<ITypeBinding<any>>;

        constructor() {
            this._bindingDictionary = new Lookup<ITypeBinding<any>>();
        }

        public bind(typeBinding: ITypeBinding<any>): void {
            this._bindingDictionary.add(typeBinding.runtimeIdentifier, typeBinding);
        }

        public unbind(runtimeIdentifier: string): void {
            try {
                this._bindingDictionary.remove(runtimeIdentifier);
            } catch (e) {
                throw new Error(`Could not resolve service ${runtimeIdentifier}`);
            }
        }

        public unbindAll(): void {
            this._bindingDictionary = new Lookup<ITypeBinding<any>>();
        }

        public resolve<TImplementationType>(runtimeIdentifier: string): TImplementationType {
            let bindings: ITypeBinding<TImplementationType>[];

            if (this._bindingDictionary.hasKey(runtimeIdentifier)) {
                bindings = this._bindingDictionary.get(runtimeIdentifier);
            } else {
                return null;
            }

            const binding = bindings[0];

            if ((binding.scope === TypeBindingScopeEnum.Singleton) && (binding.cache !== null)) {
                return binding.cache;
            } else {
                const result = this._injectDependencies<TImplementationType>(binding.implementationType);
                binding.cache = result;
                return result;
            }
        }

        private _getConstructorArguments(func: Function): Array<string> {
            let result: Array<string>;

            const STRIP_COMMENTS = /((\/\/.*$)|(\/\*[\s\S]*?\*\/))/mg;
            const ARGUMENT_NAMES = /([^\s,]+)/g;

            const fnStr = func.toString().replace(STRIP_COMMENTS, '');
            const argsInit = fnStr.indexOf('(') + 1;
            const argsEnd = fnStr.indexOf(')');

            // If using ES6 classes and there is no constructor
            // there is no need to parser constructor args
            if ("function" === typeof Map &&
                fnStr.indexOf("class") !== -1 &&
                fnStr.indexOf("constructor") === -1) {
                result = null;
            } else {
                result = fnStr.slice(argsInit, argsEnd).match(ARGUMENT_NAMES);
            }

            if (result === null) {
                result = [];
            }

            return result;
        }

        private _getConstructorArgumentsFromStaticProperty(func: Function, propertyName: string): Array<string> {
            let result: Array<string> = null;

            if (func[propertyName] != null && func[propertyName] instanceof Array) {
                result = func[propertyName] as Array<string>;
            }

            if (result === null) {
                result = [];
            }

            return result;
        }

        private _injectDependencies<TImplementationType>(func: { new(): TImplementationType; }): TImplementationType {
            let args = this._getConstructorArgumentsFromStaticProperty(func, "__inject__");

            if (args.length === 0) {
                args = this._getConstructorArguments(func);
            }

            if (args.length === 0) {
                return new func();
            } else {
                const injections: Object[] = [];
                let implementation: any = null;

                for (let service of args) {
                    implementation = this.resolve<any>(service);
                    injections.push(implementation);
                }

                return this._construct<TImplementationType>(func, injections);
            }
        }

        private _construct<TImplementationType>(constr: { new(): TImplementationType; }, args: Object[]): TImplementationType {
            return new (Function.prototype.bind.apply(constr, [null].concat(args)));
        }
    }

}