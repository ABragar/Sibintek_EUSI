using Base.DAL;
using Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Service.Crud;

namespace Base.Security.ObjectAccess.Services
{
    public class ObjectAccessItemService : IObjectAccessItemService, IEventSource
    {
        public ObjectAccessItemService(IEventBus event_bus)
        {
            EventBus = event_bus;

            _on_get_all = EventBus.GetTrigger<OnGetAll<ObjectAccessItem>>(this);
            _on_get = EventBus.GetTrigger<OnGet<ObjectAccessItem>>(this);
            _on_create = EventBus.GetTrigger<OnCreate<ObjectAccessItem>>(this);
            _on_update = EventBus.GetTrigger<OnUpdate<ObjectAccessItem>>(this);
            _on_delete = EventBus.GetTrigger<OnDelete<ObjectAccessItem>>(this);

        }

        private readonly IEventTrigger<OnGetAll<ObjectAccessItem>> _on_get_all;

        protected IEventBus EventBus { get; private set; }

        public virtual void OnBeforeSave(IUnitOfWork unitOfWork, ObjectAccessItem src, ObjectAccessItem dest)
        {
            if (src.UserCategories != null)
            {
                src.UserCategories = src.UserCategories.GroupBy(x => x.UserCategoryID).Select(x =>
                {
                    var category = x.FirstOrDefault(c => c.UserCategoryID == x.Key);

                    if (category == null) return null;

                    category.Read = x.Any(c => c.Read);

                    category.Update = x.Any(c => c.Update);

                    category.Delete = x.Any(c => c.Delete);

                    return category;
                }).ToList();
            }

            if (src.Users != null)
            {
                src.Users = src.Users.GroupBy(x => x.UserID).Select(x =>
                {
                    var category = x.FirstOrDefault(c => c.UserID == x.Key);

                    if (category == null) return null;

                    category.Read = x.Any(c => c.Read);

                    category.Update = x.Any(c => c.Update);

                    category.Delete = x.Any(c => c.Delete);

                    return category;
                }).ToList();
            }
        }

        protected virtual IObjectSaver<ObjectAccessItem> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<ObjectAccessItem> objectSaver)
        {
            return objectSaver
                .SaveOneToMany(x => x.UserCategories, x => x.SaveOneObject(a => a.UserCategory))
                .SaveOneToMany(x => x.Users, x => x.SaveOneObject(a => a.User));
        }        

        public virtual IQueryable<ObjectAccessItem> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            IQueryable<ObjectAccessItem> q = unitOfWork.GetRepository<ObjectAccessItem>().All();

            if (hidden != null)
            {
                if ((bool)hidden)
                    q = q.Where(a => a.Hidden == true);
                else
                    q = q.Where(a => a.Hidden == false);
            }



            _on_get_all.Raise(() => new OnGetAll<ObjectAccessItem>(null, unitOfWork));

            return q;
        }


        private readonly IEventTrigger<OnGet<ObjectAccessItem>> _on_get;
        public virtual ObjectAccessItem Get(IUnitOfWork unitOfWork, int id)
        {
            var obj = this.GetAll(unitOfWork).FirstOrDefault(m => m.ID == id);


            _on_get.Raise(() => new OnGet<ObjectAccessItem>(obj, unitOfWork));

            return obj;
        }

        protected virtual void InitSortOrder(IUnitOfWork unitOfWork, ObjectAccessItem obj)
        {
            if (obj.SortOrder == -1)
            {
                obj.SortOrder = (this.GetAll(unitOfWork).Max(m => (int?)m.SortOrder) ?? 0) + 1;
            }
        }


        private readonly IEventTrigger<OnCreate<ObjectAccessItem>> _on_create;

        public virtual ObjectAccessItem Create(IUnitOfWork unitOfWork, ObjectAccessItem obj)
        {

            this.InitSortOrder(unitOfWork, obj);

            this.OnBeforeSave(unitOfWork, obj, null);

            var saver = this.GetForSave(unitOfWork, unitOfWork.GetObjectSaver(obj, null));

            unitOfWork.GetRepository<ObjectAccessItem>().Create(saver.Dest);

            unitOfWork.SaveChanges();



            _on_create.Raise(() => new OnCreate<ObjectAccessItem>(saver.Dest, unitOfWork));


            return saver.Dest;
        }

        private readonly IEventTrigger<OnUpdate<ObjectAccessItem>> _on_update;

        public virtual ObjectAccessItem Update(IUnitOfWork unitOfWork, ObjectAccessItem obj)
        {
            var repository = unitOfWork.GetRepository<ObjectAccessItem>();

            var objDest = repository.Find(obj.ID);

            this.OnBeforeSave(unitOfWork, obj, objDest);

            var saver = this.GetForSave(unitOfWork, unitOfWork.GetObjectSaver(obj, objDest));

            repository.Update(saver.Dest);

            unitOfWork.SaveChanges();

            _on_update.Raise(() => new OnUpdate<ObjectAccessItem>(saver.Original, saver.Dest, unitOfWork));

            return saver.Dest;
        }

        private readonly IEventTrigger<OnDelete<ObjectAccessItem>> _on_delete;

        public virtual void Delete(IUnitOfWork unitOfWork, ObjectAccessItem obj)
        {
            var repository = unitOfWork.GetRepository<ObjectAccessItem>();

            obj.Hidden = true;

            repository.Update(obj);

            unitOfWork.SaveChanges();

            _on_delete.Raise(() => new OnDelete<ObjectAccessItem>(obj, unitOfWork));

        }

        public virtual void ChangeSortOrder(IUnitOfWork unitOfWork, ObjectAccessItem obj, int posId)
        {
        }

        public virtual ObjectAccessItem CreateDefault(IUnitOfWork unitOfWork)
        {
            return null;
        }

        public ObjectAccessItem Clone(BaseObject source)
        {
            throw new NotImplementedException();
        }


        public IReadOnlyCollection<ObjectAccessItem> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<ObjectAccessItem> collection)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<ObjectAccessItem> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<ObjectAccessItem> collection)
        {
            throw new NotImplementedException();
        }

        public void DeleteCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<ObjectAccessItem> collection)
        {
            throw new NotImplementedException();
        }

        public bool DisableEvents { get; protected set; }
        public IBaseObjectCrudService GetCrudService()
        {
            throw new NotImplementedException();
        }

        public Type EntityType => typeof(ObjectAccessItem);

        public IQueryable<ObjectAccessItem> GetById(IUnitOfWork unitOfWork, bool? hidden, int id)
        {
            return this.GetAll(unitOfWork, hidden).Where(x => x.ID == id);
        }
    }
}
