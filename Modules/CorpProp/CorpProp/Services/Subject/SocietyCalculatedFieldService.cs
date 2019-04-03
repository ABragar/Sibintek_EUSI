using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Base.Extensions;
using CorpProp.Entities.Security;
using CorpProp.Services.Base;
using CorpProp.Services.Import.BulkMerge;
using CorpProp.Extentions;
using Base.Service.Log;

namespace CorpProp.Services.Subject
{
    public interface ISocietyCalculatedFieldService : ITypeObjectService<SocietyCalculatedField>
    {

    }

    public class SocietyCalculatedFieldService : TypeObjectService<SocietyCalculatedField>, ISocietyCalculatedFieldService
    {

        private readonly ILogService _logger;
        private readonly ISecurityUserService _securityUserService;
        private readonly IWorkflowService _workflowService;
        private readonly IBaseObjectServiceFacade _facade;
        private readonly IAccessService _accessService;

        public SocietyCalculatedFieldService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService,
             IPathHelper pathHelper, IWorkflowService workflowService, IAccessService accessService, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            _facade = facade;
            _securityUserService = securityUserService;
            _workflowService = workflowService;
            _accessService = accessService;
        }

        public override SocietyCalculatedField Get(IUnitOfWork unitOfWork, int id)
        {
            return base.Get(unitOfWork, id);
        }

        public override IQueryable<SocietyCalculatedField> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }

        public override IQueryable<SocietyCalculatedField> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            //return base.GetAllByDate(uow, date);
            var SocietyCalculatedField = base.GetAllByDate(uow, date);
            var q = SocietyCalculatedField.Where(f => (f.DateExclusionFromPerimeter != null && f.DateExclusionFromPerimeter > date) || (f.DateExclusionFromPerimeter == null));
            return q;
        }

        void SetResponsable(IUnitOfWork unitOfWork, SocietyCalculatedField obj)
        {
            if (obj.ResponsableForResponse != null)
            {
                var users = unitOfWork.GetRepository<SibUser>().Filter(user => user.SocietyID == obj.ID && user.ID != obj.ResponsableForResponse.ID);
                users.ForEach(user => user.ResponsibleOnRequest = false);
                obj.ResponsableForResponse.ResponsibleOnRequest = true;
                unitOfWork.SaveChanges();
            }
        }

        public override SocietyCalculatedField Update(IUnitOfWork unitOfWork, SocietyCalculatedField obj)
        {
            var result = base.Update(unitOfWork, obj);
            SetResponsable(unitOfWork, result);
            return result;
        }

        public override SocietyCalculatedField Create(IUnitOfWork unitOfWork, SocietyCalculatedField obj)
        {
            var result = base.Create(unitOfWork, obj);
            SetResponsable(unitOfWork, result);
            return result;
        }


        public void OnActionExecuting(ActionExecuteArgs args)
        {

        }



        public int GetWorkflowID(IUnitOfWork unitOfWork, BaseObject obj)
        {
            return 0;//Workflow.Default;
        }

        public void BeforeInvoke(BaseObject obj)
        {
        }
        
        protected override IObjectSaver<SocietyCalculatedField> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SocietyCalculatedField> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver) ;
        }

        public List<SocietyCalculatedField> FindObjects(IUnitOfWork uofw, string idEup)
        {
            List<SocietyCalculatedField> list = new List<SocietyCalculatedField>();
            list = uofw.GetRepository<SocietyCalculatedField>().Filter(x =>
            !x.Hidden &&
            !x.IsHistory &&
            x.IDEUP != null && x.IDEUP == idEup).ToList<SocietyCalculatedField>();
            return list;
        }

    }
}
