namespace App.Libs.Inversify {
    export function inject(...injectionKeys: Array<string>): (target: Function) => void {
        return (target: Function) => {
            target["__inject__"] = injectionKeys;
        };
    }
}