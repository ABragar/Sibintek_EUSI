using System.Collections.Generic;
using System.Linq;
using Base.Ambient;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.FIAS;
using CorpProp.Services.Base;
using CorpProp.Services.Settings;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;

namespace EUSI.Services.Estate
{
    public class EstateRegistrationSecurityService : TypeObjectService<EstateRegistration>, ISibUserNotification
    {
        private ISibEmailService _emailService;
        private readonly ILogService _logger;

        public ISibEmailService EmailService { get { return _emailService; } }

        public EstateRegistrationSecurityService(
            IBaseObjectServiceFacade facade
            , ISibEmailService emailService
            , ILogService logger
            ) : base(facade, logger)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                EstateStatesHelper.UpdateCodeIds(uow);
            }
            _emailService = emailService;
            _logger = logger;
        }

        public override IQueryable<EstateRegistration> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            var query = base.GetAll(unitOfWork, hidden);
            if (AppContext.SecurityUser.IsRoleCode("ImportEstateRegistration"))
            {
                query = query.Where(
                        registration =>
                            registration.StateID != EstateStatesHelper.DirectedStateID);
            }
            if (AppContext.SecurityUser.IsRoleCode("ResponsibleER"))
            {
                query = query.Where(
                        registration =>
                            registration.StateID != EstateStatesHelper.CreatedStateID);
            }
            return query;
        }

        public override EstateRegistration Create(IUnitOfWork unitOfWork, EstateRegistration obj)
        {
            return base.Create(unitOfWork, obj);
        }

        public override EstateRegistration Update(IUnitOfWork unitOfWork, EstateRegistration obj)
        {
            return base.Update(unitOfWork, obj);
        }

        public override EstateRegistration CreateDefault(IUnitOfWork unitOfWork)
        {
            var obj = base.CreateDefault(unitOfWork);
            obj.State = unitOfWork.GetRepository<EstateRegistrationStateNSI>()
                                    .Filter(f => !f.Hidden && !f.IsHistory && f.Code == "CREATED")
                                    .FirstOrDefault();
            obj.StateID = obj.State?.ID;

            return obj;
        }

        /// <summary>
        /// Отправка уведомления пользователю.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="ids"></param>
        public void SendUserNotification(
            IUnitOfWork uow
            , int[] ids)
        {
            var erList = uow.GetRepository<EstateRegistration>()
                .Filter(f => ids.Contains(f.ID))
                .Include(inc => inc.Contragent)
                .Include(inc => inc.ERType)
                .Include(inc => inc.State)
                .Include(inc => inc.ERControlDateAttributes)
                .Include(inc => inc.ERReceiptReason)
                .Include(inc => inc.Society)
                .ToList();

            var items = from er in erList
                        where ids.Contains(er.ID)
                        select new
                        {
                            Link = er,
                            er.ID,
                            er.ContactEmail,
                            Number = (er.Number == 0) ? er.ID : er.Number,//TODO: заглушка, решить проблему с Number
                            er.NumberCDS,
                            er.ERControlDateAttributes?.DateCDS,
                            er.Date,
                            er.LastComment,
                            State = (er.State == null) ? "" : er.State.Code,
                            er.StateID,
                            CreatedOSCount = GetOSCount(uow, er.ID),
                            CreatedOICount = GetOICount(uow, er.ID),
                            CreatedOSS = GetOSS(uow, er.ID),
                            ERTypeName = (er.ERType != null) ? er.ERType.Name : "",
                            Contragent = (er.Contragent != null) ? er.Contragent.FullName : "",
                            ContactName = er.ContacName,
                            er.ContactPhone,
                            Society = (er.Society != null) ? er.Society.FullName : "",
                            ERReceiptReason = (er.ERReceiptReason != null) ? er.ERReceiptReason.Name : "",
                            ERContractDate = er.ERContractDate?.ToString("dd.MM.yyyy") ?? "",
                            er.ERContractNumber,
                            DateNow = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                        };

            foreach (var item in items.ToList())
            {
                var tState = uow.GetRepository<EstateRegistrationStateNSI>()
                                    .Filter(f => !f.Hidden && !f.IsHistory && f.ID == item.StateID)
                                    .FirstOrDefault();

                var templateCode = "EstateRegistration_" + item.State;
                var notificationStrategy = new EstateRegistrationNotificationStrategy();
                notificationStrategy.Init(templateCode);
                _emailService.SetNotificationStrategy(notificationStrategy);
                _emailService.SendNotice(
                    uow
                    , item
                    , null
                    , null
                    , templateCode.ToLower()
                    , item.Link);
            }
        }

        private int GetOSCount(IUnitOfWork uow, int erID)
        {
            return uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
              .FilterAsNoTracking(f => f.ObjRigthId == erID && !f.Hidden && f.IsPrototype)
              .Count();
        }

        private int GetOICount(IUnitOfWork uow, int erID)
        {
            return uow.GetRepository<EstateAndEstateRegistrationObject>()
              .FilterAsNoTracking(f => f.ObjRigthId == erID && !f.Hidden && f.IsPrototype)
              .Count();
        }

        private ICollection<object> GetOSS(IUnitOfWork uow, int erID)
        {
            var createdOSList = uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
               .FilterAsNoTracking(f => f.ObjRigthId == erID && !f.Hidden && f.IsPrototype);

            return
                (from oss in createdOSList
                 where oss.ObjRigthId == erID
                 select new
                 {
                     Position = oss.ObjLeft.CreatingFromERPosition,
                     oss.ObjLeft.EUSINumber,
                     oss.ObjLeft.NameEUSI,
                     EstateDefinitionType = (oss.ObjLeft.EstateDefinitionType == null) ? "" : oss.ObjLeft.EstateDefinitionType.Name,
                     oss.ObjLeft.Comment
                 }).ToList<object>();
        }

        private int GetCountRows(IUnitOfWork uow, int erID)
        {
            return uow.GetRepository<EstateRegistrationRow>()
               .FilterAsNoTracking(f => f.EstateRegistrationID == erID && !f.Hidden)
               .Count();
        }

        private ICollection<object> GetRows(IUnitOfWork uow, int erID)
        {
            var rows = uow.GetRepository<EstateRegistrationRow>()
               .FilterAsNoTracking(f => f.EstateRegistrationID == erID && !f.Hidden)
               .Include(inc => inc.EstateDefinitionType);

            return
                (from row in rows
                 select new
                 {
                     row.Position,
                     row.EUSINumber,
                     row.NameEstateByDoc,
                     EstateDefinitionType = (row.EstateDefinitionType == null) ? "" : row.EstateDefinitionType.Name,
                     row.Comment
                 }).DefaultIfEmpty()
                 .OrderBy(s => s.Position)
                 .ToList<object>();
        }

        /// <summary>
        /// Отправка уведомления по шаблону.
        /// </summary>
        public void SendNotification(
            IUnitOfWork uow
            , int[] ids
            , string email
            , string template)
        {
            var erList = uow.GetRepository<EstateRegistration>()
                .Filter(f => ids.Contains(f.ID))
                .Include(inc => inc.Contragent)
                .Include(inc => inc.ERType)
                .Include(inc => inc.State)
                .Include(inc => inc.ERReceiptReason)
                .Include(inc => inc.Society)
                .Include(inc => inc.ERControlDateAttributes)
                .ToList();

            var items = from er in erList
                        where ids.Contains(er.ID)
                        select new
                        {
                            Link = er,
                            er.ID,
                            er.ContactEmail,
                            ContactName = er.ContacName,
                            er.ContactPhone,
                            Number = (er.Number == 0) ? er.ID : er.Number,//TODO: заглушка, решить проблему с Number
                            er.NumberCDS,
                            DateCDS = (er.ERControlDateAttributes?.DateCDS != null) ? er.ERControlDateAttributes.DateCDS.Value.ToString("dd.MM.yyyy") : "",
                            Date = (er.Date != null) ? er.Date.Value.ToString("dd.MM.yyyy") : "",
                            er.LastComment,
                            State = (er.State == null) ? "" : er.State.Code,
                            StateID = (er.StateID == null) ? null : er.StateID,
                            CreatedOSCount = GetOSCount(uow, er.ID),
                            CreatedOICount = GetOICount(uow, er.ID),
                            CreatedOSS = GetOSS(uow, er.ID),
                            CountRows = GetCountRows(uow, er.ID),
                            Rows = GetRows(uow, er.ID),
                            ERTypeName = (er.ERType != null) ? er.ERType.Name : "",
                            ERReceiptReason = (er.ERReceiptReason != null) ? er.ERReceiptReason.Name : "",
                            Contragent = (er.Contragent != null) ? er.Contragent.FullName : "",
                            Society = (er.Society != null) ? er.Society.FullName : "",
                            ERContractDate = (er.ERContractDate != null) ? er.ERContractDate.Value.ToString("dd.MM.yyyy") : "",
                            er.ERContractNumber,
                            DateNow = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                        };

            foreach (var item in items.ToList())
            {
                _emailService.SendNotice(
                    uow
                    , item
                    , null
                    , email
                    , template.ToLower()
                    , item.Link);
            }
        }
    }
}