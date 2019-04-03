using System;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Entities.Complex;
using Base.Security;
using Base.Task.Entities;
using Base.Task.Services.Abstract;
using Base.Utils.Common;
using AppContext = Base.Ambient.AppContext;
using TaskStatus = Base.Task.Entities.TaskStatus;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class TaskServiceFacade : ITaskServiceFacade
    {
        private readonly IBaseTaskService<BPTask> _taskService;
        private readonly ITemplateRenderer _renderer;

        public TaskServiceFacade(IBaseTaskService<BPTask> taskService, ITemplateRenderer renderer)
        {
            _taskService = taskService;
            _renderer = renderer;
        }

        public ICollection<BPTask> UpdateTasks(IUnitOfWork uow, StagePerform currentPerformer)
        {
            foreach (var task in currentPerformer.Tasks.Where(x => x.AssignedToID != AppContext.SecurityUser.ID))
            {
                task.Status = TaskStatus.NotRelevant;
            }

            var performerTask = currentPerformer.Tasks.FirstOrDefault(x => x.AssignedToID == AppContext.SecurityUser.ID);

            if (performerTask != null)
            {
                performerTask.Status = TaskStatus.Complete;
                performerTask.CompliteDate = AppContext.DateTime.Now;
                performerTask.PercentComplete = 100;
            }

            return ProcessTasks(uow, currentPerformer.Tasks);
        }

        private string Render(string template, IBPObject obj, IDictionary<string, string> additional = null)
        {
            return _renderer.Render(template, obj, additional);
        }

        public BPTask CreateBPTask(IUnitOfWork uow, int userFromID, int userToID, Stage stage, IBPObject baseObject, DateTime dt)
        {
            var assignedFrom = uow.GetRepository<User>().Find(userFromID);
            var assignedTo = uow.GetRepository<User>().Find(userToID);

            var task = new BPTask
            {
                Name = Render(stage.TitleTemplate, baseObject).TruncateAtWord(150),
                CategoryID = stage.WorkflowImplementation.Workflow.TaskCategoryID.GetValueOrDefault(-1),
                AssignedFrom = assignedFrom,
                AssignedTo = assignedTo,
                Period = new Period
                {
                    Start = dt,
                    End = dt + TimeSpan.FromMinutes(stage.PerformancePeriod)
                },
                Status = TaskStatus.New,
                ObjectID = baseObject.ID,
                ObjectType = baseObject.GetType().GetBaseObjectType().GetTypeName(),
                Description = Render(stage.DescriptionTemplate, baseObject)
            };
         
         //  _taskService.Create(uow, task);

            return task;
        }

        public void CreateNotifications(IUnitOfWork uow, ICollection<BPTask> tasks, BaseEntityState status)
        {
            foreach (var bpTask in tasks)
            {
                _taskService.CreateNotification(uow, bpTask, status);
            }
        }



        public ICollection<BPTask> ProcessTasks(IUnitOfWork uow, ICollection<BPTask> tasks)
        {            
            return _taskService.UpdateCollection(uow, tasks.ToList()).ToList();
        }
    }
}
