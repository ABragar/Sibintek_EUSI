using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Estate;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Services.Estate;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Import;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using CorpProp.Services.Accounting;
using EUSI.Entities.Accounting;
using EUSI.Helpers;
using EUSI.Services.Estate;
using System.Reflection;
using Base.Service.Log;

namespace EUSI.Services.Accounting
{
    public interface INoImportDataOSService : IAccountingObjectService
    {

    }

    public class NoImportDataOSService : AccountingObjectService, INoImportDataOSService
    {
        private readonly ILogService _logger;

        public NoImportDataOSService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
            IPathHelper pathHelper, IWorkflowService workflowService, IAccessService accessService, ILogService logger
            ) : base(facade, securityUserService, pathHelper, workflowService, accessService, logger)
        {
            _logger = logger;
        }

        public override bool HistoryActionVisible => false;

        public override IQueryable<AccountingObject> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            var states = new List<string>() { "DRAFT", "OUTBUS" };
            var dt = DateTime.Now;

            return base.GetAll(unitOfWork, hidden)
                .Where(f => 
                    ((f.StateObjectRSBU != null && !states.Contains(f.StateObjectRSBU.Code)) || f.StateObjectRSBU == null)
                && f.LeavingDate == null
                && f.NonActualDate == null
                && f.ActualDate != null && f.ActualDate.Value.Month != dt.Month && f.ActualDate.Value.Year != dt.Year
                );            
           
        }

        public override IQueryable<AccountingObject> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            var states = new List<string>() { "DRAFT", "OUTBUS" };
            var dt = DateTime.Now;

            return base.GetAllByDate(uow, date)
                .Where(f =>
                    ((f.StateObjectRSBU != null && !states.Contains(f.StateObjectRSBU.Code)) || f.StateObjectRSBU == null)
                && f.LeavingDate == null
                && f.NonActualDate == null
                && f.ActualDate != null && f.ActualDate.Value.Month != dt.Month && f.ActualDate.Value.Year != dt.Year
                );
        }
    }

  
}
