using Base.DAL;
using Base.Service;
using CorpProp.Entities.ProjectActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Entities.Complex;
using Base.Notification.Service.Abstract;
using CorpProp.Entities.Security;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Common;
using CorpProp.Entities.Settings;
using CorpProp.Entities.ManyToMany;
using CorpProp.Helpers;
using System.Reflection;
using Base.Security;
using Base.Task.Entities;
using Base.Utils.Common;

namespace CorpProp.Services.ProjectActivity
{   
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - задача.
    /// </summary>
    public interface ISibTaskService : IBaseObjectService<SibTask>, ISibNotification
    {
        
    }

    /// <summary>
    /// Представляет сервис для работы с объектом - задача.
    /// </summary>
    public class SibTaskService : BaseObjectService<SibTask>, ISibTaskService
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Инициализирует новый экземпляр класса SibTaskService.
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="notificationService"></param>
        public SibTaskService(IBaseObjectServiceFacade facade, INotificationService notificationService) : base(facade)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>задача.</returns>
        public override SibTask Create(IUnitOfWork unitOfWork, SibTask obj)
        {
            if (!obj.IsTemplate)
                Checks(unitOfWork, obj);

            SibTask createdTask = base.Create(unitOfWork, obj);

            if (createdTask.NotificationEnabled && !obj.IsTemplate)
                CreateSibNotification(unitOfWork, createdTask);

            return createdTask;
        }

        /// <summary>
        /// Переопределяет метод при событии обновления объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>задача.</returns>
        public override SibTask Update(IUnitOfWork unitOfWork, SibTask obj)
        {
            if (!obj.IsTemplate)
            {
                Checks(unitOfWork, obj);
                CreateSibNotification(unitOfWork, obj);
            }

            return base.Update(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<SibTask> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SibTask> objectSaver)
        {

            bool isStatusChanged = objectSaver.Src.SibStatus?.ID != objectSaver.Dest.SibStatus?.ID;
            bool isDateEndChanged = objectSaver.Src.End != objectSaver.Original?.End;
            bool isProjectChange = objectSaver.Src.Project?.ID != objectSaver.Dest.Project?.ID;
            bool isParentTaskChange = objectSaver.Src.Parent?.ID != objectSaver.Dest.Parent?.ID;
            bool isNumberEmpty = objectSaver.Src.Number == null;

            if (isStatusChanged || isDateEndChanged)
            {
                NotificationHelper.ChangeType changeType = NotificationHelper.ChangeType.Both;

                if (isStatusChanged && isDateEndChanged)
                    changeType = NotificationHelper.ChangeType.Both;
                else if (isStatusChanged)
                    changeType = NotificationHelper.ChangeType.Status;
                else
                    changeType = NotificationHelper.ChangeType.DateEnd;

                ObjectChangeNotification(unitOfWork, objectSaver.Src, changeType);
            }

            if (isProjectChange || isParentTaskChange || isNumberEmpty)
            {
                string number = ProjectActivityHelper.SetTaskNumber(unitOfWork, objectSaver.Src);
                objectSaver.Dest.InternalNumber = int.Parse(number.Split('.').Last());
                objectSaver.Dest.Number = number;
            }

            return
                base.GetForSave(unitOfWork, objectSaver)
                   .SaveOneObject(t => t.Project)
                                   .SaveOneObject(t => t.Responsible)
                                   .SaveOneObject(t => t.SibStatus)
                                   .SaveOneObject(t => t.Parent_)
                                   .SaveOneObject(t => t.Initiator)
                                   .SaveOneObject(t => t.Project)
                                   .SaveOneObject(t => t.TaskParent)
                    ;
        }

        /// <summary>
        /// Проверка возможности смены статуса в зависимости от текущей связи.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Задача.</param>
        private void CheckDependesies(IUnitOfWork uofw, SibTask obj)
        {
            List<SibTaskGanttDependency> dependenciesList = uofw.GetRepository<SibTaskGanttDependency>()
                .Filter(f => f.SuccessorTaskID == obj.ID && f.PredecessorTaskID != null).ToList();

            if (dependenciesList.Count == 0) 
                return;

            foreach (var dependency in dependenciesList)
            {
                if (dependency.PredecessorTaskID == null)
                    continue;

                SibTask predecessorTask = this.Get(uofw, (int)dependency.PredecessorTaskID);
                if (predecessorTask != null)
                {
                    string statusCode = predecessorTask.SibStatus?.Code;
                    SibTaskStatus statusCode2 = uofw.GetRepository<SibTaskStatus>().Filter(f => f.ID == obj.SibStatus.ID).FirstOrDefault();

                    if (statusCode != null && statusCode2 != null)
                    {
                        switch (dependency.Type)
                        {
                            case 0:
                                if (statusCode.ToLower() != "completed" && statusCode2.Code.ToLower() == "completed")
                                    throw new Exception("Не выполнена одна из предыдущих задач.");
                                break;
                            case 1:
                                if (statusCode.ToLower() != "completed" && statusCode2.Code.ToLower() == "appoint")
                                    throw new Exception("Не выполнена одна из предыдущих задач.");
                                break;
                            case 2:
                                if (statusCode.ToLower() != "appoint" && statusCode2.Code.ToLower() == "completed")
                                    throw new Exception("Не отправлена одна из предыдущих задач.");
                                break;
                            case 3:
                                if (statusCode.ToLower() != "appoint" && statusCode2.Code.ToLower() == "appoint")
                                    throw new Exception("Не выполнена одна из предыдущих задач.");
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверка даты создания/окончания.
        /// </summary>
        /// <param name="obj">Задача.</param>
        private static void CheckDates (SibTask obj)
        {
            if (obj == null) 
                return;
            //if (obj.Start != null && DateTime.Compare(obj.Start.Date, DateTime.Now.Date) < 0 )
            //    throw new Exception("Дата начала задачи меньше текущей даты.");

            if ((obj.Start != null && obj.End != null) && DateTime.Compare(obj.Start.Date, obj.End.Date) > 0)
                throw new Exception("Дата окончания задачи не может быть меньше даты начала.");
        }

        /// <summary>
        /// Проверка сроков задачи относительно родительской задачи.
        /// Согласно требованию:
        /// Начало нижестоящей задачи не должно быть ранее начала вышестоящей +1 день,
        /// а также окончание нижестоящей задачи не должно быть позже окончания вышестоящей -1 день.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Задача.</param>
        private void CheckParent(IUnitOfWork unitOfWork, SibTask obj)
        {
            if (obj.Project != null)
                this.CheckProjectDates(unitOfWork, obj);

            if (obj.Parent == null)
                return;

            var parentTask = this.Get(unitOfWork, obj.Parent.ID);

            if (DateTime.Compare(parentTask.Start.Date.AddDays(1), obj.Start.Date) > 0)
                throw new Exception("Дата начала задачи раньше даты начала родительской задачи.");

            if (DateTime.Compare(parentTask.End.Date.AddDays(-1), obj.End.Date) < 0)
                throw new Exception("Дата окончания задачи позже даты окончания родительской задачи.");

            if (parentTask.Parent != null)
                this.CheckParent(unitOfWork, parentTask);
        }

        /// <summary>
        /// Проверка сроков задачи относительно проекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Задача.</param>
        private void CheckProjectDates(IUnitOfWork unitOfWork, SibTask obj)
        {
            if (obj.Project == null)
                return;

            var project = unitOfWork.GetRepository<SibProject>().Find(obj.Project.ID);

            if (project.DateFrom != null && DateTime.Compare(project.DateFrom.Value.Date, obj.Start.Date) > 0)
                throw new Exception("Проект начинается позже задачи.");

            if (project.DateTo != null && DateTime.Compare(project.DateTo.Value.Date, obj.End.Date) < 0)
                throw new Exception("Проект заканчивается раньше задачи.");
        }

        /// <summary>
        /// Вход для проверок.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Задача.</param>
        private void Checks (IUnitOfWork uofw, SibTask obj)
        {
            CheckDependesies(uofw, obj);
            CheckDates(obj);
            CheckParent(uofw, obj);
        }

        /// <summary>
        /// Переопределение метода задающего значения по умолчанию при открытии объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <returns>Задача.</returns>
        public override SibTask CreateDefault(IUnitOfWork unitOfWork)
        {

            var obj = base.CreateDefault(unitOfWork);

            if (obj.Initiator == null)
            {

                var uid = AppContext.SecurityUser.ID;
                if (uid != 0)
                {
                    int? profileId = unitOfWork.GetRepository<User>().Find(f => f.ID == uid && !f.Hidden).BaseProfileID;

                    var currentSibUser = profileId != null ? unitOfWork.GetRepository<SibUser>().Find(f => f.ID == profileId && !f.Hidden) : null;

                    if (currentSibUser != null)
                    {
                        obj.Initiator = currentSibUser;
                        obj.InitiatorID = currentSibUser.ID;
                    }
                }
            }

            if (obj.SibStatus == null && obj.ID == 0)
            {
                SibTaskStatus defaultStatus = unitOfWork.GetRepository<SibTaskStatus>()
                     .Filter(f => !f.Hidden && f.Code.ToLower() == "draft")
                     .FirstOrDefault();
                obj.SibStatus = defaultStatus;
                obj.SibStatusID = defaultStatus?.ID;
            }

            if (obj.Period == null || obj.Period.Start == DateTime.MinValue)
            {
                obj.Period = new Period()
                {
                    Start = DateTime.Now,
                    End = DateTime.Now
                };
            }

            return obj;
        }


        /// <summary>
        /// Создает список объектов уведомления.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="notification">Уведомление.</param>
        /// <returns></returns>
        public List<SibNotificationObject> PrepareLinkedObject(IUnitOfWork unitOfWork, SibNotification notification)
        {
            DateTime now = DateTime.Now;
            DateTime dtNow = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            PropertyInfo propertyInfo = typeof(SibTask).GetProperty(notification.PropertyName);
            List<SibTask> tasks = new List<SibTask>();

            if (propertyInfo != null && propertyInfo.DeclaringType == typeof(BaseTask))
                propertyInfo = typeof(BaseTask).GetProperty(notification.PropertyName);


            if (notification.ItemID != null && notification.ItemID > 0)
                tasks.Add(this.Get(unitOfWork, (int)notification.ItemID));
            else
                tasks = this.GetAll(unitOfWork)
                    .Where(NotificationHelper.PropertyEquals<SibTask, DateTime>(propertyInfo, dtNow.Date))
                    .Where(w => !w.Hidden && !w.IsTemplate && w.ResponsibleID != null).ToList();

            List<SibNotificationObject> notificationObjects = new List<SibNotificationObject>();

            try
            {
                if (tasks.Count > 0)
                {
                    foreach (SibTask task in tasks)
                    {
                        List<int> responsiblesIds = new List<int>();

                        if (propertyInfo == null)
                            continue;

                        DateTime? remindDate = NotificationHelper.CalculateRemindDateTime(notification.RemindPeriod, ((DateTime)propertyInfo.GetValue(task, null)));

                        if (remindDate == null || remindDate.Value.Date != dtNow.Date)
                            continue;

                        if (notification.SendToResponsibles)
                            responsiblesIds = unitOfWork.GetRepository<SibTaskAndSibUser>().All().Where(w => w.ObjLeftId == task.ID && w.ObjRigth.User != null).Select(s => s.ObjRigth.User.ID).ToList();
                                                
                        if (task.Responsible?.UserID != null)
                            responsiblesIds.Add((int)task.Responsible.UserID);

                        if (notification.Reciever?.UserID != null)
                            responsiblesIds.Add((int)notification.Reciever.UserID);


                        SibNotificationObject nObject = new SibNotificationObject()
                        {
                            Subject = notification.Subject,
                            Message = notification.Message,
                            LinkBaseObject = NotificationHelper.GetLinkedObj(task, "SibTaskMenuList"),
                            Recipients = responsiblesIds
                        };

                        notificationObjects.Add(nObject);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToStringWithInner());
            }

            return notificationObjects;
        }

        /// <summary>
        /// Создание уведомления при смене статуса и/или времени окончания задачи.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="task">Задача.</param>
        /// <param name="changeType">Тип изменений.</param>
        private void ObjectChangeNotification(IUnitOfWork unitOfWork, SibTask task, NotificationHelper.ChangeType changeType)
        {
            if (task?.SibStatus == null || task.Responsible == null)
                return;

            DateTime? dateChange = task.End;
            SibUser responsibleUser = unitOfWork.GetRepository<SibUser>().Find(f => f.ID == task.Responsible.ID);

            string taskName = task.Title;
            string statusName = unitOfWork.GetRepository<SibTaskStatus>().Find(f => f.ID == task.SibStatus.ID).Name;
            string message = $"В задаче {taskName} измены статус на {statusName} и дата окончания на {dateChange.Value.Date.ToShortDateString()}.";
            
            switch (changeType)
            {
                case NotificationHelper.ChangeType.Status:
                    message = $"В задаче {taskName} изменен статус на {statusName}.";
                    break;
                case NotificationHelper.ChangeType.DateEnd:
                    message = $"В задаче {taskName} изменена дата окончания на {dateChange.Value.Date.ToShortDateString()}.";
                    break;
            }

            if (responsibleUser.User == null)
                return;

            List<int> recipients = new List<int>()
            {
                responsibleUser.User.ID
            };

            _notificationService.CreateNotification(unitOfWork, recipients.ToArray(), NotificationHelper.GetLinkedObj(task, "SibTaskMenuList"), $"Изменена задача {task.Title}.", message);
        }

        /// <summary>
        /// Создание/изменение уведомления из задачи.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="task"></param>
        private static void CreateSibNotification(IUnitOfWork unitOfWork, SibTask task)
        {
            var notificationRepo = unitOfWork.GetRepository<SibNotification>();

            var notification = notificationRepo.Find(f => f.ItemID == task.ID);

            if (notification != null)
            {
                notification.RemindPeriod = task.RemindPeriod;
                notification.IsEnabled = task.NotificationEnabled;
                notification.Reciever = task.Responsible;
                notification.SendToResponsibles = true;
                notification.PropertyName = task.PropertyName;
                notification.Subject = task.NotificationSubject;
                notification.Message = task.NotificationMessage;

                notificationRepo.Update(notification);
            }
            else
            {
                notification = new SibNotification()
                {
                    Mnemonic = $"{typeof(SibTask).Name}MenuList",
                    RemindPeriod = task.RemindPeriod,
                    IsEnabled = task.NotificationEnabled,
                    Reciever = task.Responsible,
                    SendToResponsibles = true,
                    PropertyName = task.PropertyName,
                    Subject = task.NotificationSubject,
                    Message = task.NotificationMessage,
                    ItemID = task.ID
                };

                notificationRepo.Create(notification);
            }
        }
    }
}
