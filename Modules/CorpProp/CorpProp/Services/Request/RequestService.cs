using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Base;
using Base.DAL;
using Base.Entities.Complex;
using Base.Extensions;
using Base.Notification.Service.Abstract;
using Base.Service;
using Base.Utils.Common.Maybe;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Request;
using CorpProp.Entities.Request.ResponseCells;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Services.Response;
using AppContext = Base.Ambient.AppContext;

namespace CorpProp.Services.Request
{
    public class RequestService : BaseObjectService<Entities.Request.Request>
    {
        private readonly string ResponseMnemonic = "Response";
        private readonly string NotificationMessageTitle = "Вам направлен запрос";
        private readonly string NotificationMessage = "Вам направлен запрос - перейдите во \"Входящие\" модуля \"Запросы\"";
        const string HasNoResponse = "Нет ответа для отправки уведомления автору";
        const string HasNoRequest = "Нет запроса ответа для отправки уведомления автору";
        const string HasNoResponsable = "Нет автора запроса для отправки уведомления";
        const string HasNoSibUserForResponsable = "Пользователь автора не найден, отправка уведомления не возможна";

        private readonly INotificationService _notificationService;

        public RequestService(IBaseObjectServiceFacade facade, INotificationService notificationService) : base(facade)
        {
            _notificationService = notificationService;
        }

        private static IQueryable<int> ResponseIDsByUsers(IUnitOfWork secondaryUnitOfWork, IEnumerable<SibUser> sibUsers)
        {
            var responses =
                from response in secondaryUnitOfWork.GetRepository<Entities.Request.Response>().All().Where(response=> response.Hidden == false)
                join deletedExecutor in sibUsers.Select(user => user.ID) on response.ExecutorID equals deletedExecutor
                select response.ID;
            return responses.Distinct();
        }
        private void DeleteResponses(IUnitOfWork secondaryUnitOfWork, int requestId, IEnumerable<Society> deletedSocietys)
        {
            if (deletedSocietys == null)
                deletedSocietys = new List<Society>();

            var responsesToDeleteIds = deletedSocietys.Where(r=>r != null).Select(rid => rid.ID).ToList();

            var responseRepo = secondaryUnitOfWork.GetRepository<Entities.Request.Response>();

            var responsesToDelete =
                responseRepo.All()
                            .Where(
                                   response =>
                                       response.ExecutorSociety != null && response.Hidden == false &&
                                       responsesToDeleteIds.Contains(response.ExecutorSociety.ID));
            foreach (var response in responsesToDelete)
            {
                response.Hidden = true;
                responseRepo.Update(response);
            }
        }

        //IQueryable<SibUser> GetExecutorsByRequestID(IUnitOfWork secondaryUnitOfWork, int requestId)
        //{
        //    return secondaryUnitOfWork.GetRepository<RequestAndSociety>().Filter(user => user.ObjLeftId == requestId && user.Hidden == false).Select(user => user.ObjRigth);
        //}

        private Entities.Request.Response CreateResponse(Entities.Request.Request request, Society society, ResponseStatus defaultStatus)
        {
            
            var response = new Entities.Request.Response()
            {
                Term = request.UniqueTerm,
                Date = null,
                Name = request.Name,
                ResponseStatusID = defaultStatus.ID,
                RequestID = request.ID,
                ExecutorID = society.ResponsableForResponseID,
                ExecutorSocietyID = society.ID,
                ResponseStatus = null,
            };
            return response;
        }

        private List<Entities.Request.Response> CreateResponses(IUnitOfWork secondaryUnitOfWork, Entities.Request.Request request, IEnumerable<Society> newSocieties)
        {
            var responseStatesDraft = secondaryUnitOfWork.GetRepository<ResponseStatus>().Find(requestStatus => requestStatus.Code == ((int)RequestInitializer.ResponseStates.Draft).ToString());
            var responsesToCreateIds = newSocieties.ToList();
            var createdResponses = responsesToCreateIds.Select(society => CreateResponse(request, society, responseStatesDraft));
            var creator = createdResponses.Select(response => secondaryUnitOfWork.GetRepository<Entities.Request.Response>().Create(response));
            return creator.ToList();
        }

        IEnumerable<TLeft> LeftOuterJoin<TLeft, TRight, TCleft, TCright>(TCleft leftQueryable, TCright rightQueryable)
            where TLeft: BaseObject
            where TRight: BaseObject
            where TCleft: IQueryable<TLeft>
            where TCright : IQueryable<TRight>
        {
            var leftJoin =
                from left in leftQueryable
                join right in rightQueryable on left.ID
                equals right.ID into equalLr
                from rightEmpty in equalLr.DefaultIfEmpty()
                where rightEmpty == null
                select left;
            return leftJoin.Distinct();
        }

        void SendNotification(IUnitOfWork unitOfWork, string title, string message, Entities.Request.Response response)
        {
            if (response == null)
            {
                Debug.WriteLine(HasNoResponse);
                return;
            }

            if (response.ExecutorID == null)
            {
                Debug.WriteLine("Не указан исполнитель ответа.");
                return;
            }

            var responsable = unitOfWork.GetRepository<SibUser>().Find(response.ExecutorID);
            int? uid = responsable?.UserID;
            if (uid != null)
            {
                LinkBaseObject linkedObject = new LinkBaseObject(response);
                linkedObject.Mnemonic = linkedObject.Mnemonic ?? ResponseMnemonic;
                _notificationService.CreateNotification(unitOfWork, new[] {uid.Value}, linkedObject, title, message);
            }
            else
            {
                Debug.WriteLine(HasNoSibUserForResponsable);
            }
        }

        void SendMassResponseNotification(IUnitOfWork unitOfWork,  ICollection<Entities.Request.Response> notifyResponses)
        {
            foreach (var notifyResponse in notifyResponses)
            {
                SendNotification(unitOfWork, NotificationMessageTitle, NotificationMessage, notifyResponse);
            }
        }

        void GetRequestStateChange(
            IUnitOfWork unitOfWork,
            Entities.Request.Request updatedRequest,
            out string outdatedRequestStatusCode,
            out string updatedRequestStatusCode)
        {
            var outdatedRequestRepo = unitOfWork.GetRepository<Entities.Request.Request>().Find(updatedRequest.ID);
            var outdatedRequestStatusCodeId = outdatedRequestRepo?.RequestStatus?.ID;
            var updatedRequestStatusCodeId = updatedRequest.RequestStatusID ?? updatedRequest?.RequestStatus?.ID;

            outdatedRequestStatusCode = outdatedRequestStatusCodeId == null? null:
                unitOfWork.GetRepository<RequestStatus>().Find(outdatedRequestStatusCodeId)?.Code;
            updatedRequestStatusCode = updatedRequestStatusCodeId == null ? null:
                unitOfWork.GetRepository<RequestStatus>().Find(updatedRequestStatusCodeId)?.Code;
        }

        bool IsStateChangedTo(
            IUnitOfWork unitOfWork,
            Entities.Request.Request updatedRequest,
            RequestInitializer.RequestStates state)
        {
            string outdatedRequestStatusCode;
            string updatedRequestStatusCode;
            GetRequestStateChange(unitOfWork, updatedRequest, out outdatedRequestStatusCode, out updatedRequestStatusCode);
            if (updatedRequestStatusCode == ((int)state).ToString())
            {
                if (outdatedRequestStatusCode == null)
                    return true;
                if (outdatedRequestStatusCode != ((int)state).ToString())
                    return true;
            }
            return false;
        }

        private IQueryable<Society> GetAddrRequestSocietyes(IUnitOfWork unitOfWork, Entities.Request.Request request)
        {
            var requestAddrSocietyes = from requestAndSociety in unitOfWork.GetRepository<RequestAndSociety>().All()
                      where requestAndSociety.ObjLeftId == request.ID
                      where !requestAndSociety.Hidden
                      join society in unitOfWork.GetRepository<Society>().All() on requestAndSociety.ObjRigthId equals society.ID
                      where !society.Hidden
                      join responsableForResponse in unitOfWork.GetRepository<SibUser>().All() on requestAndSociety.ObjRigthId equals responsableForResponse.SocietyID
                      where !responsableForResponse.Hidden
                      where responsableForResponse.ResponsibleOnRequest
                      select society;

            return requestAddrSocietyes;
        }

        IQueryable<Society> GetResponsesSociety(IUnitOfWork unitOfWork, Entities.Request.Request request)
        {
            return unitOfWork.GetRepository<Entities.Request.Response>()
                             .Filter(response => !response.Hidden && response.Request.ID == request.ID)
                             .Select(response => response.ExecutorSociety);
        }

        private IEnumerable<Society> GetNewResponseSocietyes(
            IUnitOfWork unitOfWork,
            Entities.Request.Request request)
        {
            IQueryable<Society> responsesSociety = GetResponsesSociety(unitOfWork, request);
            IQueryable<Society> addresed = GetAddrRequestSocietyes(unitOfWork, request);
            var added = LeftOuterJoin<Society, Society, IQueryable<Society>, IQueryable<Society>>(addresed, responsesSociety);
            return added;
        }

        private IEnumerable<Society> GetRemovedResponseSocietyes(
            IUnitOfWork unitOfWork,
            Entities.Request.Request request)
        {
            IQueryable<Society> responsesSociety = GetResponsesSociety(unitOfWork, request);
            IQueryable<Society> addresed = GetAddrRequestSocietyes(unitOfWork, request);
            var removed = LeftOuterJoin<Society, Society, IQueryable<Society>, IQueryable<Society>>(responsesSociety, addresed);
            return removed;
        }


        private bool HasRequestStatusCode(IUnitOfWork unitOfWork, Entities.Request.Request request, RequestInitializer.RequestStates state)
        {
            var t = request.RequestStatus?.ID;
            var requestStatusCode =
                unitOfWork.GetRepository<RequestStatus>()
                .Filter(f => !f.Hidden && !f.IsHistory
                && (f.ID == t || f.ID == request.RequestStatusID))
                         .FirstOrDefault()?.Code;
            return requestStatusCode != null
                   && requestStatusCode == ((int)state).ToString();
        }

        private void SendRequestTo(IUnitOfWork unitOfWork, SibUser authorSibUser, Entities.Request.Request request)
        {
            using (var secondaryUnitOfWork = UnitOfWorkFactory.CreateSystem(unitOfWork))
            {
                var createdResponsable = GetNewResponseSocietyes(secondaryUnitOfWork, request);
                var removedResponsables = GetRemovedResponseSocietyes(secondaryUnitOfWork, request);

                DeleteResponses(secondaryUnitOfWork, request.ID, removedResponsables);
                secondaryUnitOfWork.SaveChanges();

                if (HasRequestStatusCode(unitOfWork, request, RequestInitializer.RequestStates.Directed))
                {
                    var created = CreateResponses(secondaryUnitOfWork, request, createdResponsable);
                    secondaryUnitOfWork.SaveChanges();
                    if (IsStateChangedTo(unitOfWork, request, RequestInitializer.RequestStates.Directed))
                        SendMassResponseNotification(unitOfWork, created);
                }
            }
        }

        public static int GetStatusIdByCode(IUnitOfWork unitOfWork, string stateCode)
        {
            var requestStateId = unitOfWork.GetRepository<RequestStatus>().Find(state => state != null && state.Code == stateCode)?.ID;
            if (requestStateId == null)
                throw new InvalidDataException($"В справочнике {typeof(RequestStatus).Name} нет кода {stateCode}");
            return requestStateId.Value;
        }

        private SibUser GetCurrentSibUser()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var uid = AppContext.SecurityUser.ID;
                return uow.GetRepository<SibUser>().Find(sibUser => sibUser.UserID == uid);
            }
        }

        private SibUser CurrentSibUser => GetCurrentSibUser();

        public override Entities.Request.Request Create(IUnitOfWork unitOfWork, Entities.Request.Request obj)
        {
            //При формировании нового запроса автоматически присваивать ему статус #2559
            obj.RequestStatusID = GetStatusIdByCode(unitOfWork, stateCode: RequestInitializer.RequestDraftStatusCode);
            obj.RequestStatus = null;
            var result = base.Create(unitOfWork, obj);
            //При сохранении запроса автоматически создаются ответы на запрос всем адресатам. #2561
            SendRequestTo(
                          unitOfWork: unitOfWork,
                          authorSibUser: CurrentSibUser,
                          //При создании запроса поле автор заполнять автоматически текущим пользователем #2573
                          request: result
                         );
            return result;
        }

        public override Entities.Request.Request Update(IUnitOfWork unitOfWork, Entities.Request.Request obj)
        {
            SendRequestTo(
                          unitOfWork: unitOfWork,
                          authorSibUser: CurrentSibUser,
                          //При создании запроса поле автор заполнять автоматически текущим пользователем #2573
                          request: obj);
            var result = base.Update(unitOfWork, obj);

            return result;
        }

        public override Entities.Request.Request CreateDefault(IUnitOfWork unitOfWork)
        {
            var request = base.CreateDefault(unitOfWork);
            //При формировании нового запроса автоматически присваивать ему статус #2559
            Func<IUnitOfWork, string, RequestStatus> getStatusIDByCode = (uow, stateCode) =>
            {
                var requestState = uow.GetRepository<RequestStatus>().Find(state => state != null && state.Code == stateCode);
                return requestState;
            };
            var draftState = getStatusIDByCode(unitOfWork, RequestInitializer.RequestDraftStatusCode);
            request.RequestStatusID = draftState.ID;
            request.RequestStatus = draftState;
          
            if (CurrentSibUser != null)
            {
                request.Responsible = CurrentSibUser;
                request.ResponsibleID = CurrentSibUser.ID;
            }
            return request;
        }
    }
}
