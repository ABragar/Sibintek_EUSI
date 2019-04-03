using System;
using System.Linq;
using Base.DAL;
using Base.Event;
using Base.Event.Entities;
using Base.Event.Service;
using Base.Events;
using Base.Hangfire;
using Base.Notification.Service.Abstract;
using Base.Notification.Entities;

namespace WebUI.Concrete
{

    public interface IEventNotificationService : IHangFireClient, IEventBusHandler<ChangeObjectEvent<Event>>
    {

    }

    public class EventNotificationService : IEventNotificationService
    {
        private readonly IUnitOfWorkFactory _factory;
        private readonly IEventService<Event> _event_service;        
        private readonly IHangFireService _hangfire_service;

        public EventNotificationService(IUnitOfWorkFactory factory, IEventService<Event> event_service, IHangFireService hangfire_service)
        {
            _factory = factory;
            _event_service = event_service;
            
            _hangfire_service = hangfire_service;
        }

        public void Process(int id)
        {
            using (var uow = _factory.CreateSystem())
            {
                var evnt = _event_service.GetAll(uow).FirstOrDefault(x => x.ID == id);

                var notify = new Notification();

                //if (ProcessEvent(id, evnt))
                //    _manager_notification.CreateNotice(uow, notify, BaseEntityState.Unchanged);

            }
        }

        public bool ProcessEvent(int id, Event evnt)
        {
            //TODO
            return false;

            if (evnt == null)
            {
                RegisterEvent(id, null, true);
                return false;
            }

            var parser = new CalendarParser(evnt);
            try
            {
                return parser.IsCurrent(DateTime.Now);
            }
            finally
            {
                RegisterEvent(id, parser, true);
            }

        }

        private void RegisterEvent(int id, CalendarParser parser, bool raise)
        {

            if (parser == null)
                _hangfire_service.Register<IEventNotificationService>(id, null);
            else
                _hangfire_service.Register<IEventNotificationService>(id, parser.ToCron(DateTime.Now, raise));
        }

        public void OnEvent(ChangeObjectEvent<Event> evnt)
        {
            //TODO


            //RegisterEvent(evnt.Object.ID, evnt is OnDelete<Event> ? null : new CalendarParser(evnt.Object),false);

        }
    }
}