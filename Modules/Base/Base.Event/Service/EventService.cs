using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Entities.Complex;
using Base.Security;
using Base.Service;
using Base.UI;
using Base.Notification.Service.Abstract;
using Base.Service.Crud;

namespace Base.Event.Service
{
    public class EventService<T> : BaseObjectService<T>, IEventService<T>
        where T : Entities.Event, new()
    {
        private readonly IUserService<User> _userService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IViewModelConfigService _configService;
        private readonly INotificationService _notificationService;

        public EventService(IBaseObjectServiceFacade facade, IUserService<User> userService, IUnitOfWorkFactory unitOfWorkFactory, IViewModelConfigService configService, INotificationService notificationService) : base(facade)
        {
            _userService = userService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _configService = configService;
            _notificationService = notificationService;
        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            SetRemindDate(objectSaver.Dest);

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneToMany(x => x.Files, x => x.SaveOneObject(z => z.Object))
                .SaveOneObject(x => x.Creator);
        }

        public IQueryable<T> GetRange(IUnitOfWork uofw, DateTime start, DateTime end)
        {
            return GetAll(uofw).Where(x => x.Start >= start && x.End <= end);
        }

        public override T CreateDefault(IUnitOfWork unitOfWork)
        {
            var dtm = Ambient.AppContext.DateTime.Now;

            return new T()
            {
                Creator = _userService.GetAsync(unitOfWork, Ambient.AppContext.SecurityUser.ID).Result,
                Start = Ambient.AppContext.DateTime.Now,
                End = dtm.AddMinutes(15)
            };
        }


        private void SetRemindDate(T obj)
        {
            if (obj.RemindPeriod.RemindValue.HasValue)
            {

                switch (obj.RemindPeriod.RemindValueType)
                {
                    case RemindValueType.Day:
                        obj.RemindDate = obj.Start.AddDays(-obj.RemindPeriod.RemindValue.Value);
                        break;

                    case RemindValueType.Hour:
                        obj.RemindDate = obj.Start.AddHours(-obj.RemindPeriod.RemindValue.Value);
                        break;

                    case RemindValueType.Week:
                        obj.RemindDate = obj.Start.AddDays(-obj.RemindPeriod.RemindValue.Value * 7);
                        break;
                }
            }
        }

        public int GetEventsToRemind()
        {
            using (var uow = _unitOfWorkFactory.CreateSystem())
            {
                var events = GetAll(uow).Where(x => x.RemindDate.HasValue
                    && x.End > DateTime.Now
                    && x.RemindDate.Value.Date == DateTime.Now.Date
                    && x.RemindDate.Value.Hour == DateTime.Now.Hour);

                var d = events.Select(x => new
                {
                    x.ID,
                    x.ExtraID,
                    x.RemindDate
                }).GroupBy(x => x.ExtraID, x => x.ID, (key, g) => new { ExtraID = key, ObjectIds = g }).ToList();

                foreach (var keyPair in d)
                {
                    SendNotify(uow, keyPair.ExtraID, keyPair.ObjectIds);
                }

                return d.Count;
            }
        }

        private void SendNotify(IUnitOfWork uow, string extraID, IEnumerable<int> ids)
        {
            var vmCfg = _configService.Get(extraID);

            var service = vmCfg.GetService<IBaseObjectCrudService>();

            foreach (int objectId in ids)
            {
                var ev = (Entities.Event)service.Get(uow, objectId);
                var stakeholders = ev.GetStakeHolders();
                var linkObj = new LinkBaseObject(ev);

                _notificationService.CreateNotification(uow, stakeholders.Select(x => x.ID).ToArray(), linkObj, ev.Title, ev.Description);
            }
        }
    }
}
