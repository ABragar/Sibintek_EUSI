using System;

namespace Base.DAL
{
    public abstract class EntityConfigurationItem
    {
        internal EntityConfigurationItem()
        {

        }

        public string Name { get; set; }
        public abstract Type ContextType { get; }
        public abstract Type EntityType { get; }
        public abstract void InvokeActionSave<T>(IObjectSaver<T> objectSaver) where T : IBaseObject;
    }

    public abstract class EntityConfigurationItem<T> : EntityConfigurationItem
    where T : BaseObject
    {
        public abstract IRepository<T> CreateRepository(IBaseContext context);
    }

    internal sealed class EntityConfigurationItem<TContext, TEntity>: EntityConfigurationItem<TEntity>
        where TContext: IBaseContext
        where TEntity : BaseObject
    {
        private readonly IRepositoryFactory<TContext> _repository_factory;

        internal EntityConfigurationItem(IRepositoryFactory<TContext> repository_factory)
        {
            _repository_factory = repository_factory;
        }

        public override IRepository<TEntity> CreateRepository(IBaseContext context)
        {
            return _repository_factory.CreateRepository<TEntity>((TContext)context);
        }

        public override Type ContextType => typeof(TContext);
        public override Type EntityType => typeof(TEntity);

        public override void InvokeActionSave<T>(IObjectSaver<T> objectSaver)
        {
            if (typeof (TEntity) != typeof (T))
                ActionSave?.Invoke(objectSaver.AsObjectSaver<TEntity>(copySrcToDest: false));
            else
                ActionSave?.Invoke(((IObjectSaver<TEntity>) objectSaver));
        }

        public Action<IObjectSaver<TEntity>> ActionSave { get; set; }
    }
}
