using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Validation;
using Base.Service;
using Base.UI;
using Base.WebApi;
using Base.WebApi.Services;
using EUSI.WebApi.Services;
using WebApi.Attributes;
using WebApi.Services;
using WA = WebApi;

namespace EUSI.WebApi
{
    internal class HttpConfigurationBuilder 
    {
        private readonly IViewModelConfigService _config_service;
        private readonly IJsonSettingsProvider _json_settings_provider;

        private readonly HttpConfiguration _configuration = new HttpConfiguration();

        private readonly IServiceLocator _locator;
        public HttpConfigurationBuilder(IViewModelConfigService config_service, IJsonSettingsProvider json_settings_provider, IServiceLocator locator)
        {
            _config_service = config_service;
            _json_settings_provider = json_settings_provider;
            _locator = locator;
        }


        private class GenericActionSelector : IHttpActionSelector
        {
            private readonly IHttpActionSelector _inner;
            private readonly IGenericTypeResolver _resolver;

            public GenericActionSelector(IHttpActionSelector inner, IGenericTypeResolver resolver)
            {
                _inner = inner;
                _resolver = resolver;
            }

            private struct CacheKey
            {
                private sealed class CacheKeyComparer : IEqualityComparer<CacheKey>
                {
                    public bool Equals(CacheKey x, CacheKey y)
                    {
                        return x.HttpActionDescriptor == y.HttpActionDescriptor && x.Key == y.Key;
                    }

                    public int GetHashCode(CacheKey obj)
                    {
                        unchecked
                        {
                            return (obj.HttpActionDescriptor.GetHashCode() * 397) ^ (obj.Key?.GetHashCode() ?? 0);
                        }
                    }
                }

                public static IEqualityComparer<CacheKey> Comparer { get; } = new CacheKeyComparer();

                public CacheKey(HttpActionDescriptor http_action_descriptor, object key)
                {
                    HttpActionDescriptor = http_action_descriptor;
                    Key = key;
                }

                public HttpActionDescriptor HttpActionDescriptor { get; }
                public object Key { get; }
            }

            private readonly ConcurrentDictionary<CacheKey, HttpActionDescriptor> _cache = new ConcurrentDictionary<CacheKey, HttpActionDescriptor>(CacheKey.Comparer);

            public HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
            {
                var result = (ReflectedHttpActionDescriptor)_inner.SelectAction(controllerContext);

                var method = result.MethodInfo;

                if (!method.IsGenericMethodDefinition)
                    return result;


                var attr = result.GetCustomAttributes<GenericActionAttribute>().Single();

                object value;




                if (controllerContext.RouteData.Values.TryGetValue(attr.Name, out value))
                {

                    var item = _cache.GetOrAdd(new CacheKey(result, value), x =>
                    {
                        var type = _resolver.Resolve(attr.Name, value.ToString());

                        if (type == null)
                            return null;


                        method = method.MakeGenericMethod(type);

                        return new ReflectedHttpActionDescriptor(controllerContext.ControllerDescriptor, method);
                    });

                    if (item != null)
                        return item;
                }


                throw new InvalidOperationException();
            }

            public ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
            {
                var result = _inner.GetActionMapping(controllerDescriptor);

                result.SelectMany(x => x).All(x => CheckGeneric(x));


                return result;
            }

            public bool CheckGeneric(HttpActionDescriptor action)
            {
                var x = (ReflectedHttpActionDescriptor)action;

                if (!x.MethodInfo.IsGenericMethod)
                    return true;


                var attr = x.GetCustomAttributes<GenericActionAttribute>().SingleOrDefault();

                if (attr == null)
                    throw new ArgumentException("Для генерик методов обязателен аттрибут");

                if (action.GetParameters().All(p => p.ParameterName != attr.Name && p.IsOptional == false))
                    throw new ArgumentException("Не найден обязаельный параметр из атрибута");

                //TODO from route data


                return true;
            }

        }

        private T ReplaceService<T>(Func<T, T> func)
        {
            var old = (T)_configuration.Services.GetService(typeof(T));
            var @new = func(old);

            _configuration.Services.Replace(typeof(T), @new);
            return @new;
        }


        private void ConfigureJsonFormatter()
        {



            var json = _configuration.Formatters.JsonFormatter.SerializerSettings;

            _json_settings_provider.ApplySettings(json);


        }


        public HttpConfiguration Build()
        {
            _configuration.UseDefaultConfiguration();




            ConfigureJsonFormatter();


            //TODO Для некоторых типов падает валидация, например DbGeometry
            _configuration.Services.Replace(typeof(IBodyModelValidator), null);

            var ass = new List<Assembly>() {
                GetType().Assembly,
                typeof(WA.Attributes.GenericActionAttribute).Assembly
            }.ToArray();

            _configuration.UseCustomAssembliesResolver(ass);//(GetType().Assembly);
            _configuration.UseCustomControllerTypeResolver(false);

            ReplaceService<IHttpControllerActivator>(x => new SimpleControllerActivator(_locator));

            var action = ReplaceService<IHttpActionSelector>(x => new GenericActionSelector(x, new ViewModelConfigTypeResolver(_config_service)));


            _configuration.MapHttpAttributeRoutesFiltered(d =>
            {

                try
                {

                    var factory = _locator.GetService(typeof(IServiceFactory<>).MakeGenericType(d.ControllerType));

                }
                catch (Exception e)
                {

                    throw new Exception("Не найдены все зависимости", e);
                }

                return true;



            });


            _configuration.EnsureInitialized();
            return _configuration;
        }



    }
}