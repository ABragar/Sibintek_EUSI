using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.DAL.Internal
{
    internal abstract class WrapperUnitOfWork: IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        protected WrapperUnitOfWork(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Guid ID
        {
            get { return _unitOfWork.ID; }
        }

        public int SaveChanges()
        {
            return _unitOfWork.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _unitOfWork.SaveChangesAsync();
        }

        public IObjectSaver<TObject> GetObjectSaver<TObject>(TObject objSrc, TObject objDest) where TObject : BaseObject
        {
            return _unitOfWork.GetObjectSaver(objSrc, objDest);
        }

        public IRepository<TObject> GetRepository<TObject>() where TObject : BaseObject
        {
            return _unitOfWork.GetRepository<TObject>();
        }
        
        public Dictionary<TEntity, BaseEntityState> GetModifiedEntities<TEntity>(bool recursive = true) where TEntity : BaseObject
        {
            return _unitOfWork.GetModifiedEntities<TEntity>(recursive);
        }
        
        public override int GetHashCode()
        {
            return _unitOfWork.GetHashCode();
        }

        public void Dispose()
        {
            
        }

        public bool IsModifiedEntity<TEntity>(TEntity entity, BaseEntityState modif) where TEntity : BaseObject
        {
            return _unitOfWork.IsModifiedEntity(entity, modif);
        }


        //sib        
        /// <summary>
        /// Перечитывание записи из БД.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Сущность, которая будет перечитана из БД.</param>
        /// <param name="collection">Св-во типа коллекция, которое будет также перечитано и загружено из БД.</param>
        public void ReloadEntity<TEntity>(TEntity entity, string collection = null) where TEntity : BaseObject
        {
            _unitOfWork.ReloadEntity<TEntity>(entity, collection);
        }

        //end sib
    }
}
