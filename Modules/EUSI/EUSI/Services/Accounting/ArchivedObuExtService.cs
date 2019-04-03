using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Accounting;
using CorpProp.Services.Accounting;

namespace EUSI.Services.Accounting
{
    public interface IArchivedObuExtService : IAccountingObjectService
    {
    }

   public class ArchivedObuExtService : AccountingObjectService, IArchivedObuExtService
    {
        private readonly ILogService _logger;

        public ArchivedObuExtService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
            IPathHelper pathHelper, IWorkflowService workflowService, IAccessService accessService, ILogService logger
        ) : base(facade, securityUserService, pathHelper, workflowService, accessService, logger)
        {
            _logger = logger;
        }

        public override IQueryable<AccountingObject> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden).Where(obu => obu.IsArchived.HasValue && obu.IsArchived.Value);
        }

        public override IQueryable<AccountingObject> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            return base.GetAll(uow, false).Where(obu => obu.IsArchived.HasValue && obu.IsArchived.Value); 
        }
    }
}
