using System.Linq;
using Base;
using Base.Conference.Entities;
using Base.Conference.Service;
using Base.DAL;
using Base.Events;
using Base.Mail.Entities;
using Base.Mail.Service;
using Base.Notification.Entities;

namespace WebUI.Concrete
{
    public class BroadcasterHandler : IEventBusHandler<IObjectEvent<PrivateMessage>>, IEventBusHandler<IObjectEvent<PublicMessage>>, IEventBusHandler<IChangeObjectEvent<Notification>>

    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IConferenceService _conferenceService;
        private readonly IBroadcaster _broadcaster;

        public BroadcasterHandler(IUnitOfWorkFactory unitOfWorkFactory, IConferenceService conferenceService, IBroadcaster broadcaster)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _conferenceService = conferenceService;
            _broadcaster = broadcaster;
        }

        public void OnEvent(IObjectEvent<PrivateMessage> evnt)
        {
            if (evnt.Modified.ToUserId != null)
            {
                _broadcaster.UpdateCounters(typeof(PrivateMessage), _unitOfWorkFactory.CreateSystem(), (int)evnt.Modified.ToUserId);
            }
        }

        public void OnEvent(IObjectEvent<PublicMessage> evnt)
        {
            var obj = evnt.Modified;

            if (obj.ToConferenceId == null) return;

            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var conf = _conferenceService.Get(uofw, (int)obj.ToConferenceId);

                var users = conf?.Members.Where(mem => mem.ObjectID != obj.FromId).Select(mem => mem.Object).ToList();

                if (users?.Count > 0)
                {
                    _broadcaster.UpdateCounters(typeof(PublicMessage), users);
                }
            }
        }

        public void OnEvent(IChangeObjectEvent<Notification> evnt)
        {
            _broadcaster.UpdateCounters(typeof(Notification),evnt.UnitOfWork,evnt.Modified.UserID.Value);
        }
    }
}
