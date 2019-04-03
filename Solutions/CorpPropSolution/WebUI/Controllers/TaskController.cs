using Base.Task.Entities;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Base.Extensions;
using Base.Project.Entities;
using Base.Service;
using Base.Task.Services;
using Base.Task.Services.Abstract;
using Base.UI;
using WebUI.Models.Task;
using Base.UI.Extensions;
using Base.Utils.Common;
using WebUI.Helpers;
using static System.String;
using AppContext = Base.Ambient.AppContext;
using SystemTask = System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class TaskController : BaseController
    {
        private readonly ITaskService _taskService;

        public TaskController(IBaseControllerServiceFacade baseServiceFacade,
            IBaseTaskService<BaseTask> baseTaskService,
            ITaskService taskService)
            : base(baseServiceFacade)
        {
            _taskService = taskService;
        }

        private TaskVm GetTaskVm(int taskId)
        {
            if (taskId == 0) return null;

            using (var uofw = CreateUnitOfWork())
            {
                return _taskService.GetAll(uofw)
                    .Select(x => new TaskVm()
                    {
                        ID = x.ID,
                        Status = x.Status,
                        AssignedFromID = x.AssignedFromID,
                        AssignedToID = x.AssignedToID
                    })
                    .SingleOrDefault(x => x.ID == taskId);
            }
        }

        public ActionResult Toolbar(int taskID)
        {
            return PartialView("_Toolbar", new TaskToolbarVm(this, GetTaskVm(taskID)));
        }


        [System.Web.Mvc.HttpPost]
        public JsonNetResult ExecuteAction(int taskID, string actionID, string comment)
        {
            if (taskID == 0)
                return new JsonNetResult(new {error = 1, message = "Некорректный параметр"});

            var intError = 0;
            var strMsg = "";


            var task = GetTaskVm(taskID);

            if (task == null)
            {
                intError = 2;
                strMsg = "Напоминание не найдена";
            }
            else
            {
                var toolbar = new TaskToolbarVm(this, task);

                var action = toolbar.Actions.FirstOrDefault(x => x.Value == actionID);

                if (action == null)
                {
                    intError = 3;
                    strMsg = "Попытка выполнить недопустимое действие";
                }
                else if (action.СommentIsRequired && IsNullOrEmpty(comment))
                {
                    intError = 4;
                    strMsg = "Введите комментарий";
                }
                else
                {
                    try
                    {
                        TaskStatus status;

                        if (Enum.TryParse(actionID, out status))
                        {
                            _taskService.ChangeStatus(taskID, status, comment);
                        }
                    }
                    catch (Exception e)
                    {
                        intError = 1;
                        strMsg = e.ToStringWithInner();
                    }
                }
            }

            return new JsonNetResult(new {error = intError, message = strMsg});
        }
    }
}