using Base.DAL;
using Base.Entities.Complex;
using Base.Notification.Entities;
using Base.Notification.Service.Abstract;
using Base.Service;
using Base.Task.Entities;
using Base.Task.Services.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Base.Task.Services.Concrete
{
    public class BaseTaskService<T> : BaseCategoryService<T>, IBaseTaskService<T> where T : BaseTask, new()
    {
        private readonly INotificationService _notificationService;

        public BaseTaskService(IBaseObjectServiceFacade facade, INotificationService notificationService) : base(facade)
        {
            _notificationService = notificationService;
        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            if (objectSaver.Dest.Status != objectSaver.Src.Status && (objectSaver.Src.Status == TaskStatus.Complete || objectSaver.Src.Status == TaskStatus.NotRelevant))
            {
                objectSaver.Dest.CompliteDate = DateTime.Now;
            }

            if (objectSaver.Src.Parent_ != null)
            {
                var catId = unitOfWork.GetRepository<BaseTask>()
                    .All()
                    .Where(x => x.ID == objectSaver.Src.Parent_.ID)
                    .Select(w => w.CategoryID).Single();

                if (catId != null)
                {
                    objectSaver.Src.BaseTaskCategory = new BaseTaskCategory() { ID = catId.Value };
                }
            }

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Parent_)
                .SaveOneObject(x => x.BaseTaskCategory)
                .SaveOneObject(x => x.AssignedFrom)
                .SaveOneObject(x => x.AssignedTo)
                .SaveOneToMany(x => x.AssignedToUsers, x => x.SaveOneObject(z => z.Object))
                .SaveOneToMany(x => x.Files, x => x.SaveOneObject(z => z.Object));
        }

        public override T Create(IUnitOfWork unitOfWork, T obj)
        {
            var task = base.Create(unitOfWork, obj);
            CreateNotification(unitOfWork, task, BaseEntityState.Added);
            return task;
        }

        protected virtual LinkBaseObject GetLinkedObj(T obj)
        {
            var ret = new LinkBaseObject(obj);
            return ret;
        }

        public virtual void CreateNotification(IUnitOfWork unitOfWork, T task, BaseEntityState state)
        {
            if (task.AssignedToID != null)
                _notificationService.CreateNotification(unitOfWork, new[] { task.AssignedToID.Value }, GetLinkedObj(task), task.Name, task.Description);
        }
    }
}
