using System;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.DAL;
using Base.Task.Entities;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface ITaskServiceFacade
    {
        ICollection<BPTask> UpdateTasks(IUnitOfWork uow, StagePerform currentPerformer);
        ICollection<BPTask> ProcessTasks(IUnitOfWork uow, ICollection<BPTask> tasks);
        BPTask CreateBPTask(IUnitOfWork uow, int userFromID, int userToID, Stage stage, IBPObject baseObject, DateTime dt);
        void CreateNotifications(IUnitOfWork uow, ICollection<BPTask> tasks, BaseEntityState status);
    }
}
