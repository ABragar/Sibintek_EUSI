namespace App.Application {
    export interface IDependencyResolver {
        GetService<TService>(serviceName: string): TService;
    }
}

