//import inersify = App.Libs.Inversify
//import communication = App.Communication

namespace App.Application {
    export class InversifyDependencyResolver implements IDependencyResolver {

        private _kernel: IKernel;

        constructor() {
            this._kernel = new Libs.Inversify.Kernel(); 
            this.InitBindings();
        }

        public GetService<TService>(serviceName: string): TService {
            return this._kernel.resolve<TService>(serviceName);
        }

        private InitBindings(): void {


            //Telephony
            this._kernel.bind(new Libs.Inversify.TypeBinding<Communication.ITelephony>("ITelephony", Communication.Telephony, Libs.Inversify.TypeBindingScopeEnum.Singleton));
        }
    }}

