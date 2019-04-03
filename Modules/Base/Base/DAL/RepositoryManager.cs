using System;
using System.Collections.Generic;
using System.Linq;
using Base.Extensions;
using Base.Service;

namespace Base.DAL
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IEntityConfiguration _entityConfiguration;
        private readonly IServiceLocator _locator;

        private readonly Dictionary<Type, object> _ropositories;
        private readonly Dictionary<Type, IBaseContext> _contexts;

        public RepositoryManager(IEntityConfiguration entityConfiguration, IServiceLocator locator)
        {
            _entityConfiguration = entityConfiguration;
            _locator = locator;

            _ropositories = new Dictionary<Type, object>();
            _contexts = new Dictionary<Type, IBaseContext>();
        }


        private IBaseContext CreateContext(Type context_type)
        {

            return _locator.GetService(context_type) as IBaseContext;

        }

        private IRepository<T> CreateRepository<T>(EntityConfigurationItem<T> entityConfig, IBaseContext context)
            where T : BaseObject
        {
            return entityConfig.CreateRepository(context);
        }
       

        public IRepository<T> RepositoryOf<T>() where T : BaseObject
        {
            var entityConfig = _entityConfiguration.Get<T>();

            var context = _contexts.GetOrAdd(entityConfig.ContextType, CreateContext);

            return (IRepository<T>)_ropositories.GetOrAdd(entityConfig.EntityType, x => CreateRepository(entityConfig, context));
        }

        public IBaseContext ContextOf<T>() where T : BaseObject
        {
            var entityConfig = _entityConfiguration.Get<T>();

            return _contexts[entityConfig.ContextType];
        }


        public IReadOnlyList<IBaseContext> GetContexts()
        {
            return _contexts.Select(x => x.Value).ToList().AsReadOnly();
        }

        public void Dispose()
        {
            //TODO транзакции так никто и не опменил, а вместе с ней и залоченные записи

            foreach (var context in _contexts)
            {
                context.Value.Dispose();
            }

            _ropositories.Clear();
            _contexts.Clear();
        }
    }
}