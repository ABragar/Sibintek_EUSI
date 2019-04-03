using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Base.DAL
{
    internal class EntityConfiguration : IEntityConfiguration
    {
        private readonly ConfigContext _context;

        public EntityConfiguration(ConfigContext context)
        {
            _context = context;
        }

        public EntityConfigurationItem<T> Get<T>() where T : BaseObject
        {
            return (EntityConfigurationItem<T>)_context.Get(typeof(T));
        }

        public IEnumerable<EntityConfigurationItem> GetContextConfig(Type contextType)
        {
            return _context.GetContextConfig(contextType);
        }

        public IEnumerable<EntityConfigurationItem> GetContextConfig(IBaseContext context)
        {
            return GetContextConfig(context.GetType());
        }

        public IEnumerable<EntityConfigurationItem> GetParents<T>() where T : BaseObject
        {
            return _context.GetParents(typeof(T));
        }

    }

    internal class ConfigContext
    {
        private readonly Dictionary<Type, EntityConfigurationItem> _configs;

        public ConfigContext()
        {
            _configs = new Dictionary<Type, EntityConfigurationItem>();
        }

        public void Add(EntityConfigurationItem entityConfiguration)
        {
            _configs.Add(entityConfiguration.EntityType, entityConfiguration);
        }

        public EntityConfigurationItem Get(Type type)
        {
            if (!_configs.ContainsKey(type))
                throw new InvalidOperationException($"The entity configuration is not configured for type \"{type.FullName}\"");

            return _configs[type];
        }

        public IEnumerable<EntityConfigurationItem> GetContextConfig(Type contextType)
        {
            return _configs.Select(x => x.Value).Where(x => x.ContextType == contextType).ToList();
        }
        public IEnumerable<EntityConfigurationItem> GetParents(Type type)
        {
            return _configs.Where(x => type.IsSubclassOf(x.Key)).Select(x => x.Value);
        }
    }

    public class EntityConfigurationBuilder
    {
        private readonly ConfigContext _context = new ConfigContext();


        public RepositoryBuilder<TContext> Context<TContext>(IRepositoryFactory<TContext> repository_factory)
            where TContext : IBaseContext
        {
            return new RepositoryBuilder<TContext>(_context, repository_factory);
        }

        public IEntityConfiguration Build()
        {
            return new EntityConfiguration(_context);
        }
    }

    public sealed class RepositoryBuilder<TContext>
        where TContext : IBaseContext
    {
        private readonly IRepositoryFactory<TContext> _repository_factory;
        private readonly ConfigContext _config;


        internal RepositoryBuilder(ConfigContext config, IRepositoryFactory<TContext> repository_factory)
        {
            _config = config;
            _repository_factory = repository_factory;
        }

        public RepositoryBuilder<TContext> Entity<TEntity>()
            where TEntity : BaseObject
        {
            _config.Add(new EntityConfigurationItem<TContext, TEntity>(_repository_factory));

            return this;
        }

        public RepositoryBuilder<TContext> Entity<TEntity>(Action<EntityBuilder<TContext, TEntity>> action)
            where TEntity : BaseObject
        {
            var item = new EntityConfigurationItem<TContext, TEntity>(_repository_factory);

            _config.Add(item);

            action(new EntityBuilder<TContext, TEntity>(item));

            return this;
        }
    }

    public sealed class EntityBuilder<TContext, TEntity>
        where TContext : IBaseContext
        where TEntity : BaseObject
    {
        private readonly EntityConfigurationItem<TContext, TEntity> _entityConfig;

        internal EntityBuilder(EntityConfigurationItem<TContext, TEntity> configuration)
        {
            _entityConfig = configuration;
        }

        public EntityBuilder<TContext, TEntity> Name(string name)
        {
            _entityConfig.Name = name;

            return this;
        }

        public EntityBuilder<TContext, TEntity> Save(Action<IObjectSaver<TEntity>> objectSaver)
        {
            _entityConfig.ActionSave = objectSaver;
            return this;
        }
    }
}
