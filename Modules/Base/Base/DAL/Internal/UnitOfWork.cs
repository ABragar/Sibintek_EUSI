using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.DAL.Internal
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IRepositoryManager _repository_manager;
        private readonly IEntityConfiguration _entityConfiguration;

        public UnitOfWork(IRepositoryManager repository_manager, IEntityConfiguration entityConfiguration)
        {
            _repository_manager = repository_manager;
            _entityConfiguration = entityConfiguration;
        }

        public Guid ID { get; } = Guid.NewGuid();

        protected IBaseContext GetContext<TObject>() where TObject : BaseObject
        {
            return _repository_manager.ContextOf<TObject>();
        }

        protected IReadOnlyList<IBaseContext> GetContexts()
        {
            return _repository_manager.GetContexts();
        }

        public int SaveChanges()
        {
            int i = 0;
            this.OnSavingChanges();//sib
            foreach (var context in this.GetContexts())
            {                
                i = i + context.SaveChanges();
            }

            return i;
        }

        public async Task<int> SaveChangesAsync()
        {
            int i = 0;

            foreach (var context in this.GetContexts())
            {
                this.OnSavingChanges();//sib
                i = i + await context.SaveChangesAsync();
            }

            return i;
        }

        public virtual IObjectSaver<TObject> GetObjectSaver<TObject>(TObject objSrc, TObject objDest) where TObject : BaseObject
        {
            var repository = GetRepository<TObject>();

            var config = _entityConfiguration.Get<TObject>();

            var objectSaver = repository.GetObjectSaver(this, objSrc, objDest);

            config.InvokeActionSave(objectSaver);

            foreach (var configurationItem in _entityConfiguration.GetParents<TObject>())
            {
                configurationItem.InvokeActionSave(objectSaver);
            }

            return objectSaver;
        }

        public virtual void Dispose()
        {
            _repository_manager.Dispose();
        }

        public virtual IRepository<TObject> GetRepository<TObject>() where TObject : BaseObject
        {
            return _repository_manager.RepositoryOf<TObject>();
        }

        public Dictionary<TEntity, BaseEntityState> GetModifiedEntities<TEntity>(bool recursive = true) where TEntity : BaseObject
        {
            var contex = this.GetContext<TEntity>();

            return contex != null ? contex.GetModifiedEntities<TEntity>(recursive) : new Dictionary<TEntity, BaseEntityState>();
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }


        public bool IsModifiedEntity<TEntity>(TEntity entity, BaseEntityState modif) where TEntity : BaseObject
        {
            var contex = this.GetContext<TEntity>();

            return contex != null && contex.IsModifiedEntity(entity, modif);
        }


        //sib
        private void OnSavingChanges()
        {
            foreach (var context in this.GetContexts())
            {
                context.OnSavingChanges(this);
            }
        }
        /// <summary>
        /// Перечитывание записи из БД.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Сущность, которая будет перечитана из БД.</param>
        /// <param name="collection">Св-во типа коллекция, которое будет также перечитано и загружено из БД.</param>
        public void ReloadEntity<TEntity>(TEntity entity, string collection = null) where TEntity : BaseObject
        {
            var context = this.GetContext<TEntity>();
            context.ReloadEntity(entity, collection);
        }

        //end sib
    }
}
