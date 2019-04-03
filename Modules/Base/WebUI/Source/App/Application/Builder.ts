namespace App.Application {
    export function Run() {
        Builder.InitResolver();
        console.log('Run:', DependencyResolver.current);
    }

    class Builder {
        public static InitResolver(): IDependencyResolver {
            let resolver = new InversifyDependencyResolver();
            DependencyResolver.SetResolver(resolver);
            return DependencyResolver.current;
        }
    }
}

