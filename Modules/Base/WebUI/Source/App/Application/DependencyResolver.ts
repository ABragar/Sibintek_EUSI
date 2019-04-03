namespace App.Application {
    export class DependencyResolver {
        public static current: IDependencyResolver = null;

        public static SetResolver(resolver: IDependencyResolver): void {
            DependencyResolver.current = resolver;
        }
    }
}

