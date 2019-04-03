using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.NSI;
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

    public interface IDraftOSService : IBaseObjectService<DraftOS>, ICustomDataSource<DraftOS>
    {
        IQueryable<DraftOS> GetReportDS(
            IUnitOfWork uow
            , string beCode
            , DateTime? period);
    }

    public class DraftOSService : BaseObjectService<DraftOS>, IDraftOSService
    {
        IUnitOfWorkFactory _unitOfWorkFactory;
        IAccountingObjectExtService _osService;

        public DraftOSService(
              IAccountingObjectExtService osService
            , IUnitOfWorkFactory unitOfWorkFactory
            , IBaseObjectServiceFacade facade) : base(facade)
        {
            _osService = osService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public override IQueryable<DraftOS> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {           
            return 
                new System.Collections.Generic.List<DraftOS>() { }
                .AsQueryable();
                ;
        }

       
        public IQueryable<DraftOS> GetAllCustom(IUnitOfWork uow, params object[] objs)
        {
            var q = new System.Collections.Generic.List<DraftOS>() { }
                .AsQueryable();
            MonitorReportingImportService monitor = new MonitorReportingImportService();

            if (!(objs != null && objs.Length > 0))                 
                return q;

            var custParams = objs[0];

            if (custParams == null)
                return q;

            var arr = custParams.ToString().Split(';');
            if (!(arr != null && arr.Length == 2))
                return q;

            var beCode = arr[0];
            var period = arr[1].GetDate();
            if (period == null)
                return q;

            //отчетная форма с параметрами БЕ и период
            q = GetReportDS(uow, beCode, period);

            return q;
        }


        /// <summary>
        /// Источник отчетной формы
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="beCode"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public IQueryable<DraftOS> GetReportDS(
            IUnitOfWork uow
            , string beCode
            , DateTime? period)
        {
            var q = new System.Collections.Generic.List<DraftOS>() { }
                .AsQueryable();           
                                  
            if (period == null)
                return q;
                      

            //отчетная форма с параметрами БЕ и период

            var oss = _osService.GetAllByDate(uow, DateTime.Now)
                .Where(f => f.StateObjectRSBU != null
                    //TFS:15275
                   //&& (f.StateObjectRSBU.Code == "DRAFT" || f.StateObjectRSBU.Code == "OUTBUS")
                   && (f.StateObjectRSBU.Code == "OUTBUS")
                   && f.Consolidation != null && f.Consolidation.Code == beCode
                   )
                   ;

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

            q =
                from os in oss
                join gl in grLink on os.ID equals gl.osID into outer
                from gr in outer.DefaultIfEmpty()
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
                    NotifyOriginator = false,
                    NotifyOriginatorDate = DateTime.MinValue,
                    NotifyBUS = false,
                    NotifyBUSDate = DateTime.MinValue

                };
            //Параметр Период (ММ.ГГГГ  посл. число месяца) + 3 дня >= ОС.Даты выполнения заявки
            period = new DateTime(period.Value.Year, period.Value.Month, 1).AddMonths(1).AddDays(-1).AddDays(3);
            q = q.Where(f => f.DateVerification <= period);

            return q;
        }
    }
}
