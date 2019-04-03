using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.CorporateGovernance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.CorporateGovernance
{
    public interface IPredecessorService : Base.ITypeObjectService<Predecessor>, IWFObjectService
    {

    }

    public class PredecessorService : Base.TypeObjectService<Predecessor>, IPredecessorService
    {

        private readonly ILogService _logger;
        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;

        public PredecessorService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, ILogService logger
            ) : base(facade, logger)
        {
            _logger = logger;
            _securityUserService = securityUserService;
            _workflowService = workflowService;

        }
        public override Predecessor Get(IUnitOfWork unitOfWork, int id)
        {
            return base.Get(unitOfWork, id);
        }
        public override IQueryable<Predecessor> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }
        public override Predecessor Update(IUnitOfWork unitOfWork, Predecessor obj)
        {
            return base.Update(unitOfWork, obj);
        }

        public override Predecessor Create(IUnitOfWork unitOfWork, Predecessor obj)
        {
            return base.Create(unitOfWork, obj);
        }


        public void OnActionExecuting(ActionExecuteArgs args)
        {

        }



        public int GetWorkflowID(IUnitOfWork unitOfWork, BaseObject obj)
        {
            return 0;// Workflow.Default;
        }

        public void BeforeInvoke(BaseObject obj)
        {
        }



        protected override IObjectSaver<Predecessor> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<Predecessor> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.SuccessionType)
                    .SaveOneObject(x => x.SocietySuccessor)
                    
                    ;
        }
    }
}
