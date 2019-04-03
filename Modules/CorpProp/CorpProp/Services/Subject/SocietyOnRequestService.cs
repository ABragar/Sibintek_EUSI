using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Extensions;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Subject;
using CorpProp.Services.Security;

namespace CorpProp.Services.Subject
{
    public interface ISocietyOnRequestService : ISocietyService
    {

    }

    public class SocietyOnRequestService : SocietyService, ISocietyOnRequestService
    {
        private readonly ILogService _logger;
        public SocietyOnRequestService(IBaseObjectServiceFacade facade, ISecurityUserService securityUserService, IPathHelper pathHelper, IWorkflowService workflowService, IAccessService accessService, ILogService logger) : base(facade, securityUserService, pathHelper, workflowService, accessService, logger)
        {
            _logger = logger;
        }
    }
}
