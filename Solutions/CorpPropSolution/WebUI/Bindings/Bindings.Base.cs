using System;
using System.Collections.Generic;
using Base;
using Base.Ambient;
using Base.ComplexKeyObjects.Common;
using Base.ComplexKeyObjects.Unions;
using Base.ComplexKeyObjects.Unions.Implementation;
using Base.DAL;
using Base.Events;
using Base.Helpers;
using Base.Security.Service;
using Base.Service;
using Base.Settings;
using Base.SysRegistry;
using Base.Validation;
using Common.Data.Service.Concrete;
using CorpProp;
using CorpProp.Common;
using SimpleInjector;
using WebUI.Concrete;

namespace WebUI.Bindings
{
    public class BaseBindings
    {
        public static void Bind(Container container)
        {
            //Service Locator
            container.RegisterSingleton<IServiceLocator>(new ServiceLocator(container));
            container.Register(typeof(IServiceFactory<>), typeof(ServiceFactory<>), Lifestyle.Singleton);

            container.Register<IEventBus, EventBus>(Lifestyle.Singleton);

            //Validation
            container.Register<IValidationConfigManager, ValidationConfigManager>(Lifestyle.Singleton);
            container.Register<IValidationService, ValidationService>(Lifestyle.Singleton);
            container.RegisterInitializer<ValidationConfigManager>(x => { x.Load(); });

            //Service
            container.Register<IBaseObjectServiceFacade, BaseObjectServiceFacade>(Lifestyle.Singleton);
            container.Register(typeof(IBaseObjectService<>), typeof(BaseObjectService<>), Lifestyle.Singleton);
            container.Register(typeof(IBaseCategoryService<>), typeof(BaseCategoryService<>), Lifestyle.Singleton);
            container.Register(typeof(IBaseCategorizedItemService<>), typeof(BaseCategorizedItemService<>), Lifestyle.Singleton);


            container.Register(typeof(UnionConfig<>),typeof(UnionConfig<>),Lifestyle.Singleton);
            container.Register(typeof(IUnionService<>),typeof(UnionService<>),Lifestyle.Singleton);
            container.Register<ITypeRelationService, TypeRelationService>(Lifestyle.Singleton);
            
            //DAL
            container.Register<IUnitOfWorkFactory, UnitOfWorkFactory>(Lifestyle.Singleton);
            container.Register<IRepositoryManager, RepositoryManager>();

            //Ambient
            container.Register<IAppContextBootstrapper, AppContextBootstrapper>(Lifestyle.Singleton);
            container.Register<IUserContextScope, UserContextScope>(Lifestyle.Scoped);
            container.Register<IDateTimeProvider, DefaultDateTimeProvider>();

            //Helpers
            container.Register<IHelperJsonConverter, HelperJsonConverter>(Lifestyle.Singleton);
            container.Register<IPathHelper, PathHelper>(Lifestyle.Singleton);

            container.Register<IApplicationInitializer, ApplicationInitializer>();

            //FileSystem
            container.Register<IFileSystemService, FileSystemService>(Lifestyle.Singleton);
            container.Register<IFileManager, DefaultFileManager>(Lifestyle.Singleton);

            //Settings

            container.Register(typeof(ISettingService<>), typeof(SettingService<>), Lifestyle.Singleton);

            //SignalR
            container.Register<IBroadcaster, Hubs.Broadcaster>(Lifestyle.Singleton);

            //Mapper           
            container.Register<IAutoMapperCloner, AutoMapperCloner>();

            //SystemRegistry
            container.Register<ISystemRegistryService, SystemRegistryService>(Lifestyle.Singleton);

            //ImportChecker
            container.Register<IExcelImportChecker, ImportChecker>();
        }

        private class ServiceFactory<TService> : IServiceFactory<TService>
            where TService : class
        {
            private readonly InstanceProducer _registration;

            public ServiceFactory(Container container)
            {
                if (container == null)
                    throw new ArgumentNullException(nameof(container));

                try
                {
                    _registration = container.GetRegistration(typeof(TService), true);
                }
                catch (ActivationException ex)
                {
                    throw new Base.Exceptions.ActivationException(typeof(TService), ex);

                }

            }

            public TService GetService()
            {
                try
                {
                    return (TService)_registration.GetInstance();
                }
                catch (ActivationException ex)
                {
                    throw new Base.Exceptions.ActivationException(typeof(TService), ex);

                }
                
            }
        }

        private class ServiceLocator : IServiceLocator
        {
            private readonly Container _container;

            public ServiceLocator(Container container)
            {
                if (container == null)
                {
                    throw new ArgumentNullException(nameof(container));
                }

                _container = container;
            }

            public T GetService<T>() where T : class
            {
                try
                {
                    return _container.GetInstance<T>();
                }
                catch (ActivationException ex)
                {
                    throw new Base.Exceptions.ActivationException(typeof(T), ex);
                }

            }

            public object GetService(Type type)
            {
                try
                {
                    return _container.GetInstance(type);
                }
                catch (ActivationException ex)
                {
                    throw new Base.Exceptions.ActivationException(type, ex);
                }
            }

            public IEnumerable<object> GetServices(Type type)
            {
                try
                {
                    return _container.GetAllInstances(type);
                }
                catch (ActivationException ex)
                {
                    throw new Base.Exceptions.ActivationException(type, ex);
                }

            }
        }
    }
}