namespace App.Libs.Inversify {
    export class TypeBinding<TServiceType> implements ITypeBinding<TServiceType> {
        public runtimeIdentifier: string;
        public implementationType: { new (): TServiceType; };
        public cache: TServiceType;
        public scope: TypeBindingScopeEnum;

        constructor(runtimeIdentifier: string,
            implementationType: { new (...args: any[]): TServiceType; },
            scopeType?: TypeBindingScopeEnum) {
            this.runtimeIdentifier = runtimeIdentifier;
            this.implementationType = implementationType;
            this.cache = null;

            if (typeof scopeType === "undefined") {
                this.scope = TypeBindingScopeEnum.Transient;
            }
            else {
                if (TypeBindingScopeEnum[scopeType]) {
                    this.scope = scopeType;
                }
                else {
                    const msg = `Invalid scope type ${scopeType}`;
                    throw new Error(msg);
                }
            }
        }
    }
}
