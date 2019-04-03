using System;
using Base.DAL;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.Task.Entities;
using Base.Task.Services.Abstract;
using Base.UI.ViewModal;
using Base.UI.Wizard;

namespace Base.Task.Services
{
    public class TaskWizzardService : BaseWizardService<TaskWizzard,Entities.Task>, ITaskWizzardService
    {
        public TaskWizzardService(ITaskService baseService, IAccessService accessService) : base(baseService, accessService)
        {
        }

        public override void OnBeforeStart(IUnitOfWork unitOfWork, ViewModelConfig config, TaskWizzard taskWizzard)
        {
            taskWizzard.Title = "This is test task";
            taskWizzard.Period.Start = DateTime.Now;
            taskWizzard.Period.End = DateTime.Now.AddDays(5);
            taskWizzard.Step = "third";
        }

        //public override TaskWizzard Save(IUnitOfWork unitOfWork, TaskWizzard obj)
        //{
        //    obj.DecoratedObject.Status = TaskStatus.New;
        //    ((ICategorizedItem) obj.Object).CategoryID = obj.Category.ID;
        //    obj.DecoratedObject.BaseTaskCategory = null;
        //    return base.Save(unitOfWork, obj);
        //}
    }
}