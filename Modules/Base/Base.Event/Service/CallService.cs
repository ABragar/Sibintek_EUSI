using System;
using System.Linq;
using Base.DAL;
using Base.Event.Entities;
using Base.Notification.Service.Abstract;
using Base.Security;
using Base.Security.Service;
using Base.Service;
using Base.UI;

namespace Base.Event.Service
{
    public class CallService : EventService<Call>, ICallService
    {
        public CallService(
            IBaseObjectServiceFacade facade, 
            IUserService<User> userService, 
            IUnitOfWorkFactory factory,
            IViewModelConfigService configService,
            INotificationService notificationService) 
            :base(facade, userService, factory, configService, notificationService)
        {
        }

        protected override IObjectSaver<Call> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<Call> objectSaver)
        {
            if (string.IsNullOrEmpty(objectSaver.Dest.Title) && objectSaver.Dest.Contact != null)
            {
                objectSaver.Dest.Title = $"Звонок: {objectSaver.Dest.Contact.Title} {objectSaver.Dest.Phone}";
            }

            return base.GetForSave(unitOfWork, objectSaver)
            .SaveOneObject(x => x.Contact);
        }

        public Call GetNewCall(IUnitOfWork uow, string phone, string code, bool outCall)
        {
            var call = CreateDefault(uow);

            call.CallType = CallType.In;
            if (outCall)
            {
                call.CallType = CallType.Out;
            }

            call.Phone = phone;
            call.Code = code;
            call.Title = $"Звонок: {phone}";

            Create(uow, call);

            return call;
        }
    }

    public interface ICallService : IEventService<Call>
    {
        Call GetNewCall(IUnitOfWork uow, string phone, string code, bool outCall);

    }
}