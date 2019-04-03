using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Base.UI;

namespace WebUI.Concrete
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        private readonly ConcurrentDictionary<string, Type> _controller_types = new ConcurrentDictionary<string, Type>();

        protected override Type GetControllerType(RequestContext request_context, string controller_name)
        {
            var controller_type = base.GetControllerType(request_context, controller_name);

            if (controller_type != null) return controller_type;

            object generic;

            request_context.RouteData.Values.TryGetValue("GenericControllerVariable", out generic);

            if (generic == null || (string) generic == string.Empty) return null;

            string generic_type_name = generic.ToString();

            string key = $"{controller_name}<{generic_type_name}>";

            if (_controller_types.TryGetValue(key, out controller_type)) return controller_type;

            var route_data = request_context.RouteData;

            IEnumerable<string> namespaces;
            object obj;

            if (route_data.DataTokens.TryGetValue("Namespaces", out obj))
                namespaces = obj as IEnumerable<string>;
            else
                namespaces = ControllerBuilder.Current.DefaultNamespaces;

            if(namespaces == null)
                throw new InvalidOperationException("route namespaces is null");

            var enumerable = namespaces as string[] ?? namespaces.ToArray();

            if (!enumerable.Any())
                throw new InvalidOperationException("route namespaces is null");

            return _controller_types.GetOrAdd(key, x =>
            {
                var vm_config_service = DependencyResolver.Current.GetService<IViewModelConfigService>();

                var generic_type = vm_config_service.Get(generic_type_name)?.TypeEntity;

                foreach (string @namespace in enumerable)
                {
                    controller_type = Type.GetType($"{@namespace}.{controller_name}Controller`1");
                    if (controller_type != null) return controller_type.MakeGenericType(generic_type);
                }

                return null;
            });
        }
    }
}