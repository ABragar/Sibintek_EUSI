using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Concrete;
using Base.DAL;
using Base.Service;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface IStageInvoker
    {
        void InvokeStage(IUnitOfWork uofw, InvokeStageContext context);
        void ExecuteNextStage(IUnitOfWork unitOfWork, IBPObject baseObject, StageAction action, int? assignToUserID, ref double counter);
        ICollection<StagePerform> GetNextStage(IUnitOfWork unitOfWork, IBPObject baseObject, int actionID);
        StageJumper FindNextStage(IUnitOfWork unitOfWork, Output output, IBPObject obj);
    }
}
