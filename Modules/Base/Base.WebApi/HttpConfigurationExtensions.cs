using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Base.WebApi.Filters;
using Base.WebApi.Services;
using WebApiProxy.Server;

namespace Base.WebApi
{
    public static class HttpConfigurationExtensions
    {
        private class CustomAssembliesResolver : IAssembliesResolver
        {

            private readonly ReadOnlyCollection<Assembly> _assemblies;

            public CustomAssembliesResolver(params Assembly[] assemblies)
            {
                _assemblies = assemblies.ToList().AsReadOnly();
            }

            public ICollection<Assembly> GetAssemblies()
            {
                return _assemblies;


            }
        }

        private class CustomHttpControllerTypeResolver : IHttpControllerTypeResolver
        {
            private readonly Func<Type, bool> _predicate;

            public CustomHttpControllerTypeResolver(Func<Type, bool> predicate)
            {
                _predicate = predicate;
            }

            public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
            {
                return assembliesResolver.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => x != null && x.IsClass && typeof(IHttpController).IsAssignableFrom(x) && _predicate(x))
                    .ToArray();
            }
        }

        public static void UseCustomAssembliesResolver(this HttpConfiguration configuration, params Assembly[] assemblies)
        {
            configuration.Services.Replace(typeof(IAssembliesResolver), new CustomAssembliesResolver(assemblies));
        }


        public static void UseCustomControllerTypeResolver(this HttpConfiguration configuration, bool is_public, params Assembly[] assemblies)
        {
            var hash = new HashSet<Assembly>(assemblies);
            var check_assembly = hash.Count > 0;

            configuration.Services.Replace(typeof(IHttpControllerTypeResolver), new CustomHttpControllerTypeResolver(
                type => (type.IsPublic || !is_public)
                        && !type.IsGenericType
                        && !type.IsAbstract
                        && (!check_assembly || hash.Contains(type.Assembly))
                        && type.Name.EndsWith(DefaultHttpControllerSelector.ControllerSuffix)
                ));

        }

        public static void UseDefaultConfiguration(this HttpConfiguration configuration)
        {

            configuration.Filters.Add(new ForbiddenFilter());
            configuration.Filters.Add(new BadRequestFilter());

            var json = configuration.Formatters.JsonFormatter;

            json.UseDataContractJsonSerializer = false;


            configuration.Formatters.Clear();
            configuration.Formatters.Add(json);
            
            configuration.RegisterProxyRoutes();
            configuration.Routes.MapHttpRoute("metadata", "metadata", null, null, new MetadataHandler(configuration));
        }



        public static void MapHttpAttributeRoutesFiltered(this HttpConfiguration configuration, Func<HttpControllerDescriptor, bool> filter)
        {

            configuration.MapHttpAttributeRoutes(new FilteredDirectRouteProvider(filter));

        }
    }
}