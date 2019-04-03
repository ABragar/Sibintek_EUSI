using System;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security;
using Base.Service;
using System.Linq;
using Base.BusinessProcesses.Entities.Steps;
using Base.Entities.Complex;
using Base.Notification.Entities;
using Base.Notification.Service.Abstract;
using Base.Task.Entities;
using Base.Task.Services.Concrete;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class BPTaskService : BaseTaskService<BPTask>
    {
        private readonly INotificationService _notificationService;

        public BPTaskService(IBaseObjectServiceFacade facade, INotificationService notificationService) : base(facade, notificationService)
        {
            _notificationService = notificationService;
        }

        protected override IObjectSaver<BPTask> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<BPTask> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.StagePerform);
        }

        public override BPTask Get(IUnitOfWork unitOfWork, int id)
        {
            var task = base.Get(unitOfWork, id);

            if (task == null || task.Status != TaskStatus.New || task.AssignedToID != Ambient.AppContext.SecurityUser.ID)
                return task;

            task.Status = TaskStatus.Viewed;

            return Update(unitOfWork, task);
        }


        protected override LinkBaseObject GetLinkedObj(BPTask obj)
        {
            var linked = base.GetLinkedObj(obj);
            linked.Mnemonic = "BPTask";
            return linked;
        }

        public override BPTask Update(IUnitOfWork unitOfWork, BPTask obj)
        {
            var task = base.Update(unitOfWork, obj);

            //if (task.Status == TaskStatus.New || task.Status == TaskStatus.Abolished)
            //    CreateNotification(unitOfWork, task, BaseEntityState.Modified);

            return task;
        }

        public override void CreateNotification(IUnitOfWork unitOfWork, BPTask task, BaseEntityState state)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (task.AssignedTo == null)
                throw new ArgumentNullException("AssignedToID");

            var entity = GetLinkedObj(task);
            string title = task.Status != TaskStatus.Abolished ? "Новая задача" : "Задача изменена";
            string description = $"{task.AssignedFrom?.FullName ?? "Пользователь"}";

            if (task.Status == TaskStatus.Abolished)
                description += " отменил задачу ";
            else
                description += " назначил задачу ";

            description += $"{task.Name}";

            _notificationService.CreateNotification(unitOfWork, new[] { task.AssignedTo.ID }, entity, title, description);
        }
    }
}
