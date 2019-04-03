using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.DAL;
using Base.Service;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface IChangeHistoryService : IBaseObjectService<ChangeHistory>
    {
        IQueryable<ChangeHistory> GetChangeHistory(IUnitOfWork unitOfWork, IBPObject obj, int? implID = null, int? count = null);

        void WriteStepsBetweenStages(IUnitOfWork unitOfWork, List<Step> steps, ref double sortOrder, InvokeStageContext stageContext);

        void WriteStageToHistory(IUnitOfWork unitOfWork, StagePerform perform, InvokeStageContext stageContext, ref double sortOrder);
    }
}
