using System;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Concrete;
using Base.DAL;
using Base.Security;
using Base.Service;
using Base.Service.Crud;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface IWorkflowContextService : IBaseObjectService<WorkflowContext>
    {
        /// <summary>
        /// Получить текущие этапы
        /// </summary>
        /// <param name="uow">Unit of work</param>
        /// <param name="obj">Объект</param>
        /// <returns></returns>
        List<StagePerform> GetCurrentStages(IUnitOfWork uow, IBPObject obj);
        
        /// <summary>
        /// Возвращает тип доступа
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="perform">Этап</param>
        /// <param name="obj">Объект</param>
        /// <returns></returns>
        PerformerType GetPerformerType(IUnitOfWork unitOfWork, StagePerform perform, InvokeStageContext stageContext);

        void TakeForPerform(IUnitOfWork unitOfWork, IBaseObjectCrudService objectService, int? userID, int? peroformID, int objectID);
        void ReleasePerform(IUnitOfWork unitOfWork, IBaseObjectCrudService objectService, int peroformID, int objectID);

        IEnumerable<BPTask> GetTasksForAllStageUsers(IUnitOfWork unitOfWork, Stage stage, IBPObject baseObject, DateTime dt);

        void AutoTakeForPerform(IUnitOfWork unitOfWork, StagePerform perform, InvokeStageContext stageContext);

        void UpdateContext(IUnitOfWork uow, WorkflowContext context, StagePerform oldPerform, ICollection<StagePerform> newPerforms);        
        bool CanSelectPreformer(IUnitOfWork uow, int stageID);
    }
}
