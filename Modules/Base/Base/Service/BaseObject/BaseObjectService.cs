using Base.DAL;
using Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Enums;
using Base.Service.Crud;
using AppContext = Base.Ambient.AppContext;

namespace Base.Service
{


    public class BaseObjectService<T> : IBaseObjectService<T>, IEventSource
        where T : BaseObject
    {

        Type IBaseObjectCrudService.EntityType => typeof(T);

        private readonly IBaseObjectServiceFacade _facade;

        public BaseObjectService(IBaseObjectServiceFacade facade)
        {
            _facade = facade;

            _on_get_all = EventBus.GetTrigger<OnGetAll<T>>(this);

            OnGet = EventBus.GetTrigger<OnGet<T>>(this);
            OnCreate = EventBus.GetTrigger<OnCreate<T>>(this);
            OnUpdate = EventBus.GetTrigger<OnUpdate<T>>(this);
            OnDelete = EventBus.GetTrigger<OnDelete<T>>(this);
            _on_change_sort_order = EventBus.GetTrigger<OnChangeSortOrder<T>>(this);
        }

        protected IAccessService SecurityService => _facade.AccessService;
        protected IEventBus EventBus => _facade.EventBus;
        protected IUnitOfWorkFactory UnitOfWorkFactory => _facade.UnitOfWorkFactory;

        public virtual IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Read);

            IQueryable<T> q = unitOfWork.GetRepository<T>().All();

            //TODO: access
            //q = SecurityService.FilterByAccess(q, unitOfWork);

            if (hidden != null)
            {
                if ((bool)hidden)
                    q = q.Where(a => a.Hidden == true);
                else
                    q = q.Where(a => a.Hidden == false);
            }

            _on_get_all.Raise(() => new OnGetAll<T>(null, unitOfWork));

            return q.OrderByDescending(x => x.ID);
        }

        private readonly IEventTrigger<OnGetAll<T>> _on_get_all;

        public virtual T Get(IUnitOfWork unitOfWork, int id)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Read);

            var obj = unitOfWork.GetRepository<T>().All().SingleOrDefault(x => x.ID == id);

            if(obj != null)
                OnGet.Raise(() => new OnGet<T>(obj, unitOfWork));

            return obj;
        }

        protected readonly IEventTrigger<OnGet<T>> OnGet;

        protected virtual IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            return objectSaver;
        }


        protected virtual void InitSortOrder(IUnitOfWork unitOfWork, T obj)
        {
            if (obj.SortOrder != -1) return;

            obj.SortOrder = GetMaxSortOrder(unitOfWork) + 1;
        }

        protected virtual double GetMaxSortOrder(IUnitOfWork unitOfWork)
        {
            return unitOfWork.GetRepository<T>().All().Select(x => x.SortOrder).DefaultIfEmpty(0).Max();
        }

        protected virtual double GetMinSortOrder(IUnitOfWork unitOfWork)
        {
            return unitOfWork.GetRepository<T>().All().Select(x => x.SortOrder).DefaultIfEmpty(0).Min();
        }

        public virtual T Create(IUnitOfWork unitOfWork, T obj)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Create);

            this.InitSortOrder(unitOfWork, obj);

            var objectSaver = this.GetForSave(unitOfWork, unitOfWork.GetObjectSaver(obj, null));

            OnSave(objectSaver.Dest);

            unitOfWork.GetRepository<T>().Create(objectSaver.Dest);
            unitOfWork.SaveChanges();

            OnCreate.Raise(() => new OnCreate<T>(objectSaver.Dest, unitOfWork));

            return objectSaver.Dest;
        }

        protected readonly IEventTrigger<OnCreate<T>> OnCreate;

        public virtual IReadOnlyCollection<T> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Create);

            double sortOrder = GetMaxSortOrder(unitOfWork) + 1;

            var res = new List<T>();

            foreach (var obj in collection)
            {
                if (obj.SortOrder == -1)
                    obj.SortOrder = sortOrder++;

                var objectSaver = this.GetForSave(unitOfWork, unitOfWork.GetObjectSaver(obj, null));

                unitOfWork.GetRepository<T>().Create(objectSaver.Dest);

                res.Add(objectSaver.Dest);
            }

            unitOfWork.SaveChanges();


            if (!DisableEvents)
            {
                foreach (var obj in res)
                {
                    var item = obj;
                    OnCreate.Raise(() => new OnCreate<T>(item, unitOfWork));

                }
            }

            return res;
        }

        protected virtual void OnSave(T obj)
        {

        }

        public virtual T Update(IUnitOfWork unitOfWork, T obj)
        {
            if (obj.ID == 0) return this.Create(unitOfWork, obj);

            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Write);

            //TODO: access
            //SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), obj.ID, TypePermission.Write, AccessType.Update);

            var os = unitOfWork.GetObjectSaver(obj, null);

            var objectSaver = this.GetForSave(unitOfWork, os);

            OnSave(objectSaver.Dest);

            unitOfWork.GetRepository<T>().Update(objectSaver.Dest);

            unitOfWork.SaveChanges();

            OnUpdate.Raise(() => new OnUpdate<T>(objectSaver.Original, objectSaver.Dest, unitOfWork));

            return objectSaver.Dest;
        }

        protected readonly IEventTrigger<OnUpdate<T>> OnUpdate;

        public virtual IReadOnlyCollection<T> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection)
        {
            var res = new List<IObjectSaver<T>>();

            double sortOrder = -1;

            foreach (var obj in collection)
            {
                var objectSaver = this.GetForSave(unitOfWork, unitOfWork.GetObjectSaver(obj, null));

                if (objectSaver.IsNew)
                {
                    SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Create);

                    if (objectSaver.Dest.SortOrder == -1)
                    {
                        if (sortOrder == -1)
                            sortOrder = GetMaxSortOrder(unitOfWork) + 1;

                        objectSaver.Dest.SortOrder = sortOrder++;
                    }

                    unitOfWork.GetRepository<T>().Create(objectSaver.Dest);
                }
                else
                {
                    SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Write);
                    //TODO: access
                    //SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), obj.ID, TypePermission.Write, AccessType.Update);

                    unitOfWork.GetRepository<T>().Update(objectSaver.Dest);
                }

                res.Add(objectSaver);
            }

            unitOfWork.SaveChanges();



            if (!DisableEvents)
            {
                foreach (var objectSaver in res)
                {
                    var item = objectSaver.Dest;
                    if (objectSaver.IsNew)
                    {

                        OnCreate.Raise(() => new OnCreate<T>(item, unitOfWork));
                    }
                    else
                    {
                        OnUpdate.Raise(() => new OnUpdate<T>(objectSaver.Original, objectSaver.Dest, unitOfWork));
                    }
                }
            }

            return res.Select(x => x.Dest).ToList();
        }


        protected virtual void DeleteInternal(IUnitOfWork unitOfWork, T obj)
        {
            obj.Hidden = true;

            unitOfWork.GetRepository<T>().Update(obj);

        }

        public virtual void Delete(IUnitOfWork unitOfWork, T obj)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Delete);
            //TODO: access
            //SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), obj.ID, TypePermission.Delete, AccessType.Delete);

            DeleteInternal(unitOfWork, obj);

            unitOfWork.SaveChanges();

            OnDelete.Raise(() => new OnDelete<T>(obj, unitOfWork));


        }

        protected readonly IEventTrigger<OnDelete<T>> OnDelete;

        public virtual void DeleteCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Delete);

            foreach (var obj in collection)
            {
                //TODO: access
                //SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), obj.ID, AccessType.Delete);

                DeleteInternal(unitOfWork, obj);
            }

            unitOfWork.SaveChanges();

            if (!DisableEvents)
            {
                foreach (var obj in collection)
                {
                    var item = obj;
                    OnDelete.Raise(() => new OnDelete<T>(item, unitOfWork));
                }
            }
        }

        public virtual void ChangeSortOrder(IUnitOfWork unitOfWork, T obj, int posId)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Write);

            double oldOrder = obj.SortOrder;

            var repository = unitOfWork.GetRepository<T>();

            var updateObj = repository.All().FirstOrDefault(x => x.ID == obj.ID);

            if (updateObj == null) return;

            double newOrder = repository.All().Where(x => x.ID == posId).Select(x => x.SortOrder).FirstOrDefault();

            if (newOrder > oldOrder)
            {
                double order = repository.All().Where(x => x.SortOrder > newOrder)
                    .Select(x => x.SortOrder).DefaultIfEmpty(-1).Min();

                if (order == -1)
                    updateObj.SortOrder = GetMaxSortOrder(unitOfWork) + 1;
                else
                    updateObj.SortOrder = newOrder + (order - newOrder) / 2;
            }
            else
            {
                double order = repository.All().Where(x => x.SortOrder < newOrder)
                    .Select(x => x.SortOrder).DefaultIfEmpty(-1).Max();

                if (order == -1)
                    updateObj.SortOrder = GetMinSortOrder(unitOfWork) - 1;
                else
                    updateObj.SortOrder = newOrder - (newOrder - order) / 2;
            }

            repository.Update(updateObj);
            unitOfWork.SaveChanges();

            _on_change_sort_order.Raise(() => new OnChangeSortOrder<T>(obj, unitOfWork));
        }

        private readonly IEventTrigger<OnChangeSortOrder<T>> _on_change_sort_order;

        public virtual T CreateDefault(IUnitOfWork unitOfWork)
        {
            if (AppContext.SecurityUser.IsPermission<T>(TypePermission.Read))
            {
                return Activator.CreateInstance<T>();
            }

            throw new Exception("Не удалось создать объект на основании");
        }

        public IQueryable<T> GetById(IUnitOfWork unitOfWork, bool? hidden, int id)
        {
            return GetAll(unitOfWork, hidden).Where(x => x.ID == id);
        }

        public bool DisableEvents { get; protected set; }
    }
}
