using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base.Attributes;
using Base.Utils.Common;
using Base.Utils.Common.Maybe;
using Newtonsoft.Json;

namespace Base.DAL.EF
{
    public class ObjectSaver<TEntity> : IObjectSaver<TEntity> where TEntity : BaseObject
    {
        private readonly bool _isNew;
        private readonly TEntity _objOriginal;
        private readonly TEntity _objSrc;
        private readonly TEntity _objDest;
        private readonly IUnitOfWork _unitOfWork;

        public ObjectSaver(IUnitOfWork unitOfWork, TEntity objSrc, TEntity objDest = null, bool copy = true)
        {
            #region Contract.Requires

            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            if (objSrc == null) throw new ArgumentNullException(nameof(objSrc));

            #endregion

            _unitOfWork = unitOfWork;

            _objSrc = objSrc;

            _isNew = objSrc.ID == 0;

            if (_isNew)
            {
                _objOriginal = null;

                if (_objDest != null) return;

                if (copy)
                    //NOTE: поучаем runtime type из-за наследования в агрегациях
                    _objDest =
                        (TEntity)
                            ObjectHelper.CreateAndCopyObject(_objSrc, _objSrc.GetType().GetBaseObjectType(),
                                new[] {typeof(IBaseObject)});
                else
                    _objDest = _objSrc;
            }
            else
            {
                var repositoty = _unitOfWork.GetRepository<TEntity>();

                _objOriginal = repositoty.GetOriginal(_objSrc.ID);
                _objDest = objDest ?? repositoty.Find(x => x.ID == _objSrc.ID);

                if (copy)
                {

                    var propsIgnore = typeof(TEntity).GetProperties()
                        .Where(
                            x =>
                                x.IsDefined(typeof(JsonIgnoreAttribute), false) ||
                                    (!x.IsDefined(typeof(SystemPropertyAttribute), false) && !x.IsDefined(typeof(DetailViewAttribute), false)))
                        .Select(x => x.Name).ToArray();

                    ObjectHelper.CopyObject(_objSrc, _objDest,
                        new[] { typeof(IBaseObject) }, propsIgnore);
                }
            }
        }

        public bool IsNew => _isNew;
        public TEntity Original => _objOriginal;
        public TEntity Src => _objSrc;
        public TEntity Dest => _objDest;

        /// <summary>
        /// Returns new ObjectSaver with current context for derrived Type
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public IObjectSaver<TObject> AsObjectSaver<TObject>(bool copySrcToDest = true) where TObject : BaseObject
        {
            return new ObjectSaver<TObject>(_unitOfWork, _objSrc as TObject, _objDest as TObject, copySrcToDest);
        }

        #region Many to many

        /// <summary>
        /// Prepare object to save with Entity Framework for Many to Many relationship
        /// </summary>
        /// <typeparam name="TEntry">Object type that inherits from BaseObject, type of collection entry</typeparam>
        /// <param name="exprproperty">Many to Many navigation property</param>
        /// <returns>ObjectSaver for chain support</returns>
        public IObjectSaver<TEntity> SaveManyToMany<TEntry>(Expression<Func<TEntity, ICollection<TEntry>>> exprproperty) where TEntry : BaseObject
        {
            var property = exprproperty.Compile();

            var objSrcPropValue = property(_objSrc);

            if (objSrcPropValue != null)
            {
                var iDs = objSrcPropValue.Where(x => x.ID != 0).Select(x => x.ID).Distinct().ToArray();

                if (property(_objDest) == null)
                {
                    _objDest.SetPropertyValue(exprproperty, new List<TEntry>());
                }

                var objDestPropValue = property(_objDest);

                objDestPropValue.Clear();

                foreach (var t in _unitOfWork.GetRepository<TEntry>().Filter(x => iDs.Contains(x.ID)).AsEnumerable())
                    objDestPropValue.Add(t);

            }
            else
            {
                property(_objDest)?.Clear();
            }

            return this;
        }

        #endregion

        #region One object

        /// <summary>
        /// Prepare object to save with Entity Framework for One object relationship
        /// </summary>
        /// <typeparam name="TProperty">Object type that inherits from BaseObject, type of entry</typeparam>
        /// <param name="property">One object navigation property</param>
        /// <returns>ObjectSaver for chain support</returns>
        public IObjectSaver<TEntity> SaveOneObject<TProperty>(Expression<Func<TEntity, TProperty>> property)
            where TProperty : BaseObject
        {
            #region Contract.Requires

            //TODO: implement with contracts!

            if (property == null)
                throw new ArgumentNullException("property");

            var propertyFunc = property.Compile();
            var propertyExpression = property.Body as MemberExpression;

            if (propertyExpression == null)
                throw new Exception("propertyExpression");

            var propertyInfo = propertyExpression.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new Exception("propertyInfo");

            #endregion

            TProperty objSrcPropValue = propertyFunc(_objSrc);

            propertyInfo.GetValue(_objDest); // awesome entity framework lazy loading impl :)

            //propertyInfo.SetValue(_objDest, objSrcPropValue != null ? _unitOfWork.GetRepository<TProperty>().Find(x => x.ID == objSrcPropValue.ID) : null);

            var newValue = objSrcPropValue;

            if (objSrcPropValue != null && objSrcPropValue.ID != 0)
            {
                var repository = _unitOfWork.GetRepository<TProperty>();

                newValue = repository.Find(x => x.ID == objSrcPropValue.ID);

                if (_unitOfWork.IsModifiedEntity(objSrcPropValue, BaseEntityState.Added))
                {
                    foreach (var modifiedEntity in _unitOfWork.GetModifiedEntities<TProperty>().Where(x => x.Value == BaseEntityState.Added || x.Key.ID == objSrcPropValue.ID))
                    {
                        repository.Detach(modifiedEntity.Key);
                    }
                }
            }

            propertyInfo.SetValue(_objDest, newValue);

            return this;
        }

        #endregion

        //public ObjectSaver(IUnitOfWork unitOfWork, TEntity objSrc)
        //{
        //    #region Contract.Requires

        //    if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");

        //    if (unitOfWork == null) throw new ArgumentNullException("objSrc");

        //    #endregion

        //    _unitOfWork = unitOfWork;

        //    _objSrc = objSrc;

        //    _isNew = objSrc.ID == 0;

        //    if (_isNew)
        //    {
        //        _objDest = objSrc;
        //    }
        //    else
        //    {
        //        _objDest = _unitOfWork.GetRepository<TEntity>().Find(x => x.ID == _objSrc.ID);

        //        _objSrc.ToObject(_objDest);
        //    }
        //}

        public IObjectSaver<TOtherEntity> Create<TOtherEntity>(TOtherEntity objSrc, TOtherEntity objDest = null) where TOtherEntity : BaseObject
        {
            return new ObjectSaver<TOtherEntity>(_unitOfWork, objSrc, objDest);
        }

        #region One to many

        /// <summary>
        /// Prepare object to save with Entity Framework for One to Many relationship
        /// </summary>
        /// <typeparam name="TEntry">Object type that inherits from BaseObject, type of collection entry</typeparam>
        /// <param name="property">Many to Many navigation property</param>
        /// <returns>ObjectSaver for chain support</returns>
        public IObjectSaver<TEntity> SaveOneToMany<TEntry>(Expression<Func<TEntity, ICollection<TEntry>>> property) where TEntry : BaseObject
        {
            return this.SaveOneToManyMethodImplementation(property, false, null);
        }

        /// <summary>
        /// Prepare object to save with Entity Framework for One to Many relationship
        /// </summary>
        /// <typeparam name="TEntry">Object type that inherits from BaseObject, type of collection entry</typeparam>
        /// <param name="property">Many to Many navigation property</param>
        /// <param name="entrySaverDelegate">Entry saver delegate</param>
        /// <returns>ObjectSaver for chain support</returns>
        public IObjectSaver<TEntity> SaveOneToMany<TEntry>(Expression<Func<TEntity, ICollection<TEntry>>> property,
            Action<IObjectSaver<TEntry>> entrySaverDelegate) where TEntry : BaseObject
        {
            return this.SaveOneToManyMethodImplementation(property, false, entrySaverDelegate);
        }

        /// <summary>
        /// Prepare object to save with Entity Framework for One to Many relationship
        /// </summary>
        /// <typeparam name="TEntry">Object type that inherits from BaseObject, type of collection entry</typeparam>
        /// <param name="property">Many to Many navigation property</param>
        /// <param name="makeHiddenWhenDelete">If true object hides instead deleteing</param>
        /// <returns>ObjectSaver for chain support</returns>
        public IObjectSaver<TEntity> SaveOneToMany<TEntry>(Expression<Func<TEntity, ICollection<TEntry>>> property,
            bool makeHiddenWhenDelete) where TEntry : BaseObject
        {
            return this.SaveOneToManyMethodImplementation(property, makeHiddenWhenDelete, null);
        }

        /// <summary>
        /// Prepare object to save with Entity Framework for One to Many relationship
        /// </summary>
        /// <typeparam name="TEntry">Object type that inherits from BaseObject, type of collection entry</typeparam>
        /// <param name="property">Many to Many navigation property</param>
        /// <param name="makeHiddenWhenDelete">If true object hides instead deleteing</param>
        /// <param name="entrySaverDelegate">Entry saver delegate</param>
        /// <returns>ObjectSaver for chain support</returns>
        public IObjectSaver<TEntity> SaveOneToMany<TEntry>(Expression<Func<TEntity, ICollection<TEntry>>> property, bool makeHiddenWhenDelete,
            Action<IObjectSaver<TEntry>> entrySaverDelegate) where TEntry : BaseObject
        {
            return this.SaveOneToManyMethodImplementation(property, makeHiddenWhenDelete, entrySaverDelegate);
        }

        protected IObjectSaver<TEntity> SaveOneToManyMethodImplementation<TEntry>(Expression<Func<TEntity, ICollection<TEntry>>> exprproperty, bool makeHiddenWhenDelete,
            Action<IObjectSaver<TEntry>> entrySaverDelegate) where TEntry : BaseObject
        {
            #region Contract.Requires

            if (exprproperty == null) throw new ArgumentNullException(nameof(exprproperty));

            #endregion

            var property = exprproperty.Compile();

            ICollection<TEntry> newCollection = property(_objSrc).With(x => x.ToList());

            var entryRepository = _unitOfWork.GetRepository<TEntry>();

            if (property(_objDest) == null)
            {
                _objDest.SetPropertyValue(exprproperty, new List<TEntry>());
            }

            var objectEntriesDest = property(_objDest);

            if (newCollection != null)
            {
                if (!_isNew)
                {
                    #region Update existing entries

                    ICollection<TEntry> toUpdateSrc = newCollection.Where(x => x.ID != 0 && !x.Hidden).ToList();

                    foreach (var entry in toUpdateSrc)
                    {
                        var entrySaver = new ObjectSaver<TEntry>(_unitOfWork, entry);
                        entrySaverDelegate?.Invoke(entrySaver);
                    }

                    #endregion
                    #region Delete entries

                    var toUpdateIDs = toUpdateSrc.Select(x => x.ID);

                    ICollection<TEntry> toDeleteDest = objectEntriesDest.Where(x => !toUpdateIDs.Contains(x.ID) && x.ID != 0).ToList();

                    if (!makeHiddenWhenDelete)
                    {
                        foreach (var entry in toDeleteDest)
                        {
                            objectEntriesDest.Remove(entry);

                            newCollection.Remove(entry);

                            entryRepository.Delete(entry);
                        }
                    }
                    else
                    {
                        foreach (var entry in toDeleteDest)
                        {
                            entry.Hidden = true;

                            entryRepository.Update(entry);
                        }
                    }

                    #endregion
                }

                #region Create new entries

                ICollection<TEntry> toCreate = newCollection.Where(x => x.ID == 0).ToList();

                foreach (var entry in toCreate)
                {
                    var entrySaver = new ObjectSaver<TEntry>(_unitOfWork, entry, null, _objDest != _objSrc);

                    entrySaverDelegate?.Invoke(entrySaver);

                    if (!objectEntriesDest.Contains(entrySaver.Dest))
                        objectEntriesDest.Add(entrySaver.Dest);
                }

                #endregion
            }
            else if (!_isNew && objectEntriesDest != null)
            {
                foreach (var entry in objectEntriesDest.ToList())
                {
                    objectEntriesDest.Remove(entry);

                    entryRepository.Delete(entry);
                }
            }

            return this;
        }

        #endregion
    }
}
