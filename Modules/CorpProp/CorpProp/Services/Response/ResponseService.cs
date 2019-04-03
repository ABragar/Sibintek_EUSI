using System;
using System.Diagnostics;
using System.Linq;
using Base.DAL;
using Base.Entities.Complex;
using Base.Notification.Service.Abstract;
using Base.Service;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Request;
using CorpProp.Entities.Request.ResponseCells;
using CorpProp.Entities.Security;
using CorpProp.Services.Response.Fasade;
using CorpProp.Entities.ManyToMany;
using CorpProp.Extentions;
using AppContext = Base.Ambient.AppContext;

namespace CorpProp.Services.Response
{
    public class ResponseService: BaseObjectService<Entities.Request.Response>
    {
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public ResponseService(
            IBaseObjectServiceFacade facade
            , INotificationService notificationService
            , IEmailService emailService) : base(facade)
        {
            _notificationService = notificationService;
            _emailService = emailService;
        }

        private readonly string ResponseMnemonic = "Response";
        private readonly string NotifyExecutorWhenReadyTitle = "Подготовлен ответ на запрос";
        private readonly string NotifyExecutorWhenReadyMessage = "Подготовлен ответ на запрос";

        void GetResponseStateChange(
            IUnitOfWork unitOfWork,
            Entities.Request.Response updatedResponse,
            out string outdatedRequestStatusCode,
            out string updatedRequestStatusCode)
        {
            var outdatedResponseRepo = unitOfWork.GetRepository<Entities.Request.Response>().Find(updatedResponse.ID);
            var outdatedResponseStatusCodeId = outdatedResponseRepo?.ResponseStatus?.ID;
            var updatedRequestStatusCodeId = updatedResponse.ResponseStatusID ?? updatedResponse?.ResponseStatus?.ID;

            outdatedRequestStatusCode = outdatedResponseStatusCodeId == null
                ? null
                : unitOfWork.GetRepository<ResponseStatus>().FindAsNoTracking((int) outdatedResponseStatusCodeId)?.Code;
            updatedRequestStatusCode = updatedRequestStatusCodeId == null
                ? null
                : unitOfWork.GetRepository<ResponseStatus>().FindAsNoTracking((int) updatedRequestStatusCodeId)?.Code;
        }

        bool IsStateChangedTo(
            IUnitOfWork unitOfWork,
            Entities.Request.Response updatedResponse,
            RequestInitializer.ResponseStates state)
        {
            string outdatedResponseStatusCode;
            string updatedResponseStatusCode;
            GetResponseStateChange(
                                   unitOfWork,
                                   updatedResponse,
                                   out outdatedResponseStatusCode,
                                   out updatedResponseStatusCode);
            if (updatedResponseStatusCode == ((int) state).ToString())
            {
                if (outdatedResponseStatusCode == null)
                    return true;
                if (outdatedResponseStatusCode != ((int) state).ToString())
                    return true;
            }
            return false;
        }


        void SendNotification(IUnitOfWork unitOfWork
            , string title
            , string message
            , int? recipientId
            , Entities.Request.Response response
            , bool byEmail = false)
        {
            if (response == null)
            {
                Debug.WriteLine("Нет ответа для отправки уведомления");
                return;
            }

            if (recipientId == null)
            {
                Debug.WriteLine("Получатель не найден, отправка уведомления не возможна");
                return;
            }
            var recipient = unitOfWork.GetRepository<SibUser>().Find(recipientId);
            var email = recipient?.Email;
            int? uid = recipient?.UserID;
            if (uid != null)
            {
                LinkBaseObject linkedObject = new LinkBaseObject(response);
                linkedObject.Mnemonic = linkedObject.Mnemonic ?? ResponseMnemonic;
                _notificationService.CreateNotification(unitOfWork, new[] { uid.Value }, linkedObject, title, message);

                if (!String.IsNullOrEmpty(email) && byEmail)
                {
                    _emailService.SendMail(email, title, message);
                }
            }
            else
            {
                Debug.WriteLine("Получатель не найден, отправка уведомления не возможна");
            }
        }

        void NotifyExecutorWhenReady(IUnitOfWork unitOfWork, Entities.Request.Response response)
        {
            if (IsStateChangedTo(unitOfWork, response, RequestInitializer.ResponseStates.Ready))
                SendNotification(unitOfWork, NotifyExecutorWhenReadyMessage, NotifyExecutorWhenReadyTitle, response.Executor?.ID, response);
        }

        void NotifyRequestInitiator(IUnitOfWork unitOfWork, Entities.Request.Response response)
        {
            if (response.ResponseStatus == null || response.Request == null)
                return;

            var status = unitOfWork.GetRepository<ResponseStatus>().FindAsNoTracking(response.ResponseStatus.ID);

            var request = unitOfWork.GetRepository<Entities.Request.Request>().FindAsNoTracking(response.Request.ID);

            if (status?.Code == "102" || status?.Code == "103" || status?.Code == "105" && request?.ResponsibleID != null)
                SendNotification(unitOfWork
                    , $"Ответ на запрос {request?.Name}  подготовлен статус ответа: {status.Name}"
                    , $"Ответ на запрос {request?.Name}  подготовлен статус ответа: {status.Name}"
                    , request?.ResponsibleID, response, true);
        }

        private bool HasResponseStatusCode(IUnitOfWork unitOfWork, Entities.Request.Response response, RequestInitializer.ResponseStates state)
        {
            if (response.ResponseStatusID == null)
                return false;

            var requestStatusCode =
                unitOfWork.GetRepository<ResponseStatus>()
                          .FindAsNoTracking((int)response.ResponseStatusID)?.Code;
            return requestStatusCode != null
                   && requestStatusCode == ((int)state).ToString();
        }

        void SetTerm(IUnitOfWork unitOfWork, Entities.Request.Response response)
        {
            if (HasResponseStatusCode(unitOfWork, response, RequestInitializer.ResponseStates.Ready))
            {
                response.Date = DateTime.Now;
            }
        }

        public override Entities.Request.Response Create(IUnitOfWork unitOfWork, Entities.Request.Response obj)
        {
            NotifyExecutorWhenReady(unitOfWork, obj);
            NotifyRequestInitiator(unitOfWork, obj);
            SetTerm(unitOfWork,obj);
            return base.Create(unitOfWork, obj);
        }

        public override Entities.Request.Response Update(IUnitOfWork unitOfWork, Entities.Request.Response obj)
        {
            NotifyExecutorWhenReady(unitOfWork, obj);
            NotifyRequestInitiator(unitOfWork, obj);
            SetTerm(unitOfWork, obj);
            return base.Update(unitOfWork, obj);
        }

        public override IQueryable<Entities.Request.Response> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            var result = base.GetAll(unitOfWork, hidden);
            var sibUser = AppContext.SecurityUser.GetSibUser(unitOfWork);
            if (sibUser != null && !sibUser.IsFromCauk())
            {
                var mainUserSocietyId = sibUser.Society?.ID;
                var subsidiaries = unitOfWork.GetRepository<SocietySubsidiaries>().All()
                    .Where(x => x.ObjLeftId == mainUserSocietyId);

                result = result.Where(x => x.ExecutorSocietyID == mainUserSocietyId).Union(
                result.Join(subsidiaries, resp => resp.ExecutorSocietyID, sub => sub.ObjRigthId, (resp, sub) => resp));
            }
            return result;
        }
    }
}
