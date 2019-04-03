namespace App.Libs.Inversify {
    class KeyValuePair<T> implements IKeyValuePair<T> {
        public key: string;
        public value: Array<T>;

        constructor(key: string, value: T) {
            this.key = key;
            this.value = new Array<T>();
            this.value.push(value);
        }
    }

    export class Lookup<T> implements ILookup<T> {
        private _hashMap: { [key: string]: IKeyValuePair<T> };

        constructor() {
            this._hashMap = {};
        }

        public add(key: string, value: T): void {
            if (key === null || key === undefined) {
                throw new Error("Argument Null");
            }

            if (value === null || value === undefined) {
                throw new Error("Argument Null");
            }

            const k = `$${key}`;
            const previousPair = this._hashMap[k];

            if (previousPair != null) {
                previousPair.value.push(value);
            }
            else {
                this._hashMap[k] = new KeyValuePair(key, value);
            }
        }

        public get(key: string): Array<T> {
            if (key === null || key === undefined) {
                throw new Error("Argument Null");
            }

            const keyValuePair = this._hashMap[`$${key}`];

            if (keyValuePair == null) {
                throw new Error("Key Not Found");
            }

            return keyValuePair.value;
        }

        public remove(key: string): void {
            if (key === null || key === undefined) {
                throw new Error("Argument Null");
            }

            const k = `$${key}`;
            const previousPair = this._hashMap[k];

            if (previousPair != null) {
                delete this._hashMap[k];
            }
            else {
                throw new Error("Key Not Found");
            }
        }

        public hasKey(key: string): boolean {
            if (key === null || key === undefined) {
                throw new Error("Argument Null");
            }

            const keyValuePair = this._hashMap[`$${key}`];
            return keyValuePair != null;
        }
    }
}


