using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.SysRegistry;

namespace Base
{
    public interface IProcessorContext
    {
        ITransactionUnitOfWork UnitOfWork { get; }

        T GetChildContext<T>() where T : class;
    }

    public interface IInitializerContext
    {
        ITransactionUnitOfWork UnitOfWork { get; }
        T GetChildContext<T>() where T : class;
        void ProcessConfigs(Action<IProcessorContext> processFunc);
        void DataInitializer(string module, string ver, Action action);
    }


    public interface IApplicationInitializer : IDisposable
    {
        void Init(IEnumerable<IModuleInitializer> modules);
    }

    public class ApplicationInitializer : IInitializerContext, IProcessorContext, IApplicationInitializer
    {
        private readonly Dictionary<Type, object> _configs;
        private readonly Dictionary<string, bool> _versions;
        private readonly IServiceLocator _serviceLocator;
        private readonly ISystemRegistryService _systemRegistryService;
        private const string KeyVersions = "Versions";

        public ApplicationInitializer(IUnitOfWorkFactory unitOfWorkFactory, IServiceLocator serviceLocator, ISystemRegistryService systemRegistryService)
        {
            UnitOfWork = unitOfWorkFactory.CreateSystemTransaction();

            _systemRegistryService = systemRegistryService;
            _serviceLocator = serviceLocator;
            _configs = new Dictionary<Type, object>();

            string vers = systemRegistryService.Get(UnitOfWork, KeyVersions);
            _versions = !string.IsNullOrEmpty(vers) ? vers.Split(';').ToDictionary(x => x, x => false) : new Dictionary<string, bool>();
        }

        public ITransactionUnitOfWork UnitOfWork { get; }

        public T GetChildContext<T>() where T : class
        {
            return (T)_configs.GetOrAdd(typeof (T), x => _serviceLocator.GetService<T>());
        }

        private readonly List<Action<IProcessorContext>> _processActions = new List<Action<IProcessorContext>>();
        public void ProcessConfigs(Action<IProcessorContext> processFunc)
        {
            if (processFunc == null)
                throw new ArgumentNullException(nameof(processFunc));

            _processActions.Add(processFunc);
        }

        public virtual void Dispose()
        {
            _processActions.Clear();
            UnitOfWork.Dispose();
            _configs.Clear();
        }

        public void Init(IEnumerable<IModuleInitializer> modules)
        {
            foreach (var module in modules)
            {
                module.Init(this);
            }

            foreach (var actions in _processActions)
            {
                actions(this);
            }

            if (_versions.Count > 0)
            {
                string val = string.Join(";", _versions.Keys);
                _systemRegistryService.AddOrUpdate(UnitOfWork, KeyVersions, () => val, (oldvalue) => val);
            }

            UnitOfWork.Commit();
        }

        public void DataInitializer(string module, string ver, Action action)
        {
            string key = $"{module}:{ver}";

            if (_versions.ContainsKey(key)) return;

            action();

            _versions.Add(key, true);
        }
    }
}
