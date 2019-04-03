using Base;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Settings;
using CorpProp.Extentions;
using EUSI.Entities.Accounting;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using EUSI.Entities.Report;
using EUSI.Services.Monitor;
using System;
using System.Linq;

namespace EUSI.Services.Accounting
{

    public interface IDraftOSPassBusService : IBaseObjectService<DraftOS>
    {
       
    }

    public class DraftOSPassBusService : BaseObjectService<DraftOS>, IDraftOSPassBusService
    {
        IUnitOfWorkFactory _unitOfWorkFactory;
        IAccountingObjectExtService _osService;

        public DraftOSPassBusService(
              IAccountingObjectExtService osService
            , IUnitOfWorkFactory unitOfWorkFactory
            , IBaseObjectServiceFacade facade) : base(facade)
        {
            _osService = osService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public override IQueryable<DraftOS> GetAll(IUnitOfWork uow, bool? hidden = false)
        {
            var oss = _osService.GetAllByDate(uow, DateTime.Now)
                .Where(f => f.StateObjectRSBU != null && f.StateObjectRSBU.Code == "OUTBUS");
            
            var originatorNotices = uow.GetRepository<UserNotification>()
                .Filter(f => !f.Hidden 
                && f.IsSentByEmail == true && f.Template != null && f.Template.Code == "OS_DraftOSPassBuss_Originator")
                .Include(i => i.Entity)
                .GroupBy(g => new {
                     g.Entity.ID
                    , g.IsSentByEmail
                    , g.Date})
                    .Select(s =>
                       new {
                           s.Key.ID
                         , s.Key.IsSentByEmail
                         , s.Key.Date
                       });

            var busNotices = uow.GetRepository<UserNotification>()
                .Filter(f => !f.Hidden 
                && f.IsSentByEmail == true && f.Template != null && f.Template.Code == "OS_DraftOSPassBuss_BUS")
                .Include(i => i.Entity)
                .GroupBy(g => new {
                    g.Entity.ID
                    , g.IsSentByEmail
                    , g.Date })                    
                    .Select( s =>
                        new {                       
                            s.Key.ID 
                           ,s.Key.IsSentByEmail
                           ,s.Key.Date
                    }
                );

            var links = uow.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                .Filter(f => !f.Hidden)
                .DefaultIfEmpty();

            var grLink = from link in links
                         group link by link.ObjLeftId into grp
                         select new
                         {
                             osID = grp.Key
                             ,
                             er = grp.OrderByDescending(g => g.ObjRigth.Date).DefaultIfEmpty().FirstOrDefault().ObjRigth
                         };

            var q =
                from os in oss
                join gl in grLink on os.ID equals gl.osID into outer
                from gr in outer.DefaultIfEmpty()   
                join originatorNotice in originatorNotices on os.ID equals originatorNotice.ID into nOuter
                from orNotice in nOuter.DefaultIfEmpty()
                join busNotice in busNotices on os.ID equals busNotice.ID into bOuter
                from bNotice in bOuter.DefaultIfEmpty()
                select new DraftOS()
                {
                    ID = os.ID,
                    Hidden = os.Hidden,
                    RowVersion = os.RowVersion,
                    SortOrder = os.SortOrder,

                    EUSINumber = os.EUSINumber,
                    InventoryNumber = os.InventoryNumber,
                    Consolidation = os.Consolidation,
                    ConsolidationID = os.ConsolidationID,
                    NameEUSI = os.NameEUSI,
                    StateObjectRSBU = os.StateObjectRSBU,
                    StateObjectRSBUID = os.StateObjectRSBUID,
                    TransferBUSDate = os.TransferBUSDate,
                    ERNumber = gr.er.Number,
                    ERContactName = (!String.IsNullOrEmpty(gr.er.ContacName)) ? gr.er.ContacName : gr.er.Originator.Name,
                    ERContactEmail = gr.er.ContactEmail,
                    ERContactPhone = gr.er.ContactPhone,
                    DateVerification = gr.er.ERControlDateAttributes != null ? gr.er.ERControlDateAttributes.DateVerification : null,
                    Comment = os.Comment,
                    NotifyOriginator = (orNotice != null )? orNotice.IsSentByEmail : false,
                    NotifyOriginatorDate = orNotice.Date,
                    NotifyBUS = (bNotice != null) ? bNotice.IsSentByEmail : false,
                    NotifyBUSDate = bNotice.Date
                };
           
            return q;
        }

       
    }
}
