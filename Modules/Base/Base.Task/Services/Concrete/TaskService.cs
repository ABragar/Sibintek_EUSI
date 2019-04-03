using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Entities.Complex;
using Base.Notification.Service.Abstract;
using Base.Security;
using Base.Service;
using Base.Settings;
using Base.Task.Entities;
using Base.Task.Services.Abstract;
using Base.Utils.Common;
using AppContext = Base.Ambient.AppContext;

namespace Base.Task.Services.Concrete
{
    public class TaskService : BaseTaskService<Entities.Task>, ITaskService
    {
        private readonly INotificationService _notificationService;

        public TaskService(IBaseObjectServiceFacade facade, INotificationService notificationService)
            : base(facade, notificationService)
        {
            _notificationService = notificationService;
        }

        private void AddItemToChangeHistory(Entities.Task task, string comment = null)
        {
            AddItemToChangeHistory(task, task.Status, comment);
        }

        private void AddItemToChangeHistory(Entities.Task task, TaskStatus status, string comment = null)
        {
            if (task.TaskChangeHistory == null)
                task.TaskChangeHistory = new List<TaskChangeHistory>();

            task.TaskChangeHistory.Add(new TaskChangeHistory()
            {
                Date = DateTime.Now,
                UserID = AppContext.SecurityUser.ID,
                Status = status,
                Сomment = comment
            });
        }

        public void ChangeStatus(int taskId, TaskStatus status, string comment = null)
        {
            using (var uofw = UnitOfWorkFactory.Create())
            {
                var task = Get(uofw, taskId);

                if (task == null || task.Status == status) return;
                
                task.Status = status;

                AddItemToChangeHistory(task, comment);

                var update_task = Update(uofw, task);

                CreateNotification(uofw, update_task, BaseEntityState.Modified);
            }
        }

        protected override LinkBaseObject GetLinkedObj(Entities.Task obj)
        {
            var linked = base.GetLinkedObj(obj);
            linked.Mnemonic = "Task";
            return linked;
        }

        public override Entities.Task Get(IUnitOfWork unitOfWork, int id)
        {
            var task = base.Get(unitOfWork, id);

            if (task == null || task.Status != TaskStatus.New || task.AssignedToID != AppContext.SecurityUser?.ID)
                return task;

            task.Status = TaskStatus.Viewed;

            AddItemToChangeHistory(task);

            return Update(unitOfWork, task);
        }

        protected override IObjectSaver<Entities.Task> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<Entities.Task> objectSaver)
        {
            if (objectSaver.IsNew)
            {
                AddItemToChangeHistory(objectSaver.Dest, objectSaver.Src.Status);
            }

            if (objectSaver.Dest.AssignedTo?.ID != objectSaver.Src.AssignedTo?.ID)
            {
                AddItemToChangeHistory(objectSaver.Dest, TaskStatus.Redirection);
            }

            var newObj = objectSaver.Dest;

            if (!string.IsNullOrEmpty(newObj.sys_all_parents))
            {
                var parentIds = newObj.sys_all_parents.Split(HCategory.Seperator).Select(HCategory.IdToInt);

                var rep = unitOfWork.GetRepository<Entities.Task>();

                var tasks = rep.All().Where(w => parentIds.Contains(w.ID));

                foreach (var parentTask in tasks)
                {
                    bool changed = false;

                    var parentIdStr = HCategory.IdToString(parentTask.ID);
                    if (rep.All().Where(x => x.sys_all_parents.Contains(parentIdStr))
                        .All(w => w.Status == TaskStatus.Complete))
                    {
                        parentTask.Status = TaskStatus.Complete;
                        changed = true;
                    }

                    if (parentTask.Period.Start > newObj.Period.Start)
                    {
                        parentTask.Period.Start = newObj.Period.Start;
                        changed = true;
                    }
                    if (parentTask.Period.End.HasValue && newObj.Period.End.HasValue && parentTask.Period.End < newObj.Period.End.Value)
                    {
                        parentTask.Period.End = newObj.Period.End;
                        changed = true;
                    }

                    if (changed)
                        rep.Update(parentTask);
                }
            }

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneToMany(x => x.TaskChangeHistory, x => x.SaveOneObject(z => z.User))
                .SaveOneToMany(x => x.ObserverUsers, x => x.SaveOneObject(z => z.Object));
        }

        public override Entities.Task CreateDefault(IUnitOfWork unitOfWork)
        {
            var dtm = DateTime.Now;
            dtm = dtm.AddMilliseconds(-dtm.Millisecond);

            return new Entities.Task
            {
                AssignedFrom = unitOfWork.GetRepository<User>().Find(u => u.ID == AppContext.SecurityUser.ID),
                Period = new Period() { Start = dtm, End = dtm.AddMinutes(15) }
            };
        }

        public override void CreateNotification(IUnitOfWork unitOfWork, Entities.Task task, BaseEntityState state)
        {
            var assignedFrom = unitOfWork.GetRepository<User>().Find(task.AssignedFromID);

            var linkBaseObject = GetLinkedObj(task);

            string title = null;
            string description = null;
            User user;

            switch (state)
            {
                case BaseEntityState.Added:
                    if (task.AssignedToID != null && task.AssignedFromID != null)
                    {
                        user = task.AssignedTo;
                        title = $"Новое напоминание от {assignedFrom.FullName}";
                        description = task.Name.TruncateAtWord(100);
                        _notificationService.CreateNotification(unitOfWork, new [] { user.ID}, linkBaseObject, title, description);
                    }

                    break;

                case BaseEntityState.Modified:
                    if (task.AssignedToID != null && task.AssignedFromID != null)
                    {
                        switch (task.Status)
                        {
                            case TaskStatus.InProcess:
                                {
                                    user = task.AssignedTo;
                                    description = task.Name.TruncateAtWord(100);
                                    title = $"Новое напоминание от {assignedFrom.FullName}";
                                    _notificationService.CreateNotification(unitOfWork, new[] { user.ID }, linkBaseObject, title, description);
                                }

                                break;

                            case TaskStatus.New:
                                {
                                    user = task.AssignedTo;
                                    title = $"Новое напоминание от {assignedFrom.FullName}";
                                    description = task.Name.TruncateAtWord(100);
                                    _notificationService.CreateNotification(unitOfWork, new[] { user.ID }, linkBaseObject, title, description);
                                    break;
                                }
                            case TaskStatus.Refinement:
                                {
                                    user = task.AssignedFrom;
                                    title = $"{task.AssignedTo.FullName} задал(а) вопрос";
                                    description = task.Name.TruncateAtWord(100);
                                    _notificationService.CreateNotification(unitOfWork, new[] { user.ID }, linkBaseObject, title, description);
                                    break;
                                }

                            case TaskStatus.Revise:
                                {
                                    user = task.AssignedFrom;
                                    title = $"{task.AssignedTo.FullName} просит проверить";
                                    description = task.Name.TruncateAtWord(100);
                                    _notificationService.CreateNotification(unitOfWork, new[] { user.ID }, linkBaseObject, title, description);
                                    break;
                                }
                            case TaskStatus.Rework:
                                {
                                    user = task.AssignedTo;
                                    title = $"{assignedFrom.FullName} просит уточнить";
                                    description = task.Name.TruncateAtWord(100);
                                    _notificationService.CreateNotification(unitOfWork, new[] { user.ID }, linkBaseObject, title, description);
                                    break;
                                }
                            case TaskStatus.NotRelevant:
                                break;

                            case TaskStatus.Complete:
                                {
                                    user = task.AssignedTo;
                                    title = $"{assignedFrom.FullName} завершил задачу";
                                    description = task.Name.TruncateAtWord(100);
                                    _notificationService.CreateNotification(unitOfWork, new[] { user.ID }, linkBaseObject, title, description);
                                    break;
                                }
                        }
                    }

                    break;
            }
        }
    }
}
