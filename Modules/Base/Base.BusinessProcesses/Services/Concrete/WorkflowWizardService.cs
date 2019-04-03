using System;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Services.Abstract;
using Base.BusinessProcesses.Wizard;
using Base.DAL;
using Base.Service;
using Base.UI.Wizard;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class WorkflowWizardService : BaseWizardService<WorkflowWizard, Workflow>, IWorkflowWizardService
    {
        private readonly IWorkflowImplementationService _workflowImplementationService;
        private readonly IWorkflowService _workflowService;

        public WorkflowWizardService(IBaseObjectService<Workflow> objectUserService, IAccessService accessService, IWorkflowImplementationService workflowImplementationService, IWorkflowService workflowService) : base(objectUserService, accessService)
        {
            _workflowImplementationService = workflowImplementationService;
            _workflowService = workflowService;
        }

        public override Workflow Complete(IUnitOfWork unitOfWork, WorkflowWizard obj)
        {
            var workflow = obj.GetObject();

            var stepsDictionary = new Dictionary<string, string>()
                {
                    {"Cтарт",  Guid.NewGuid().ToString("N") },
                    {"Точка выхода",Guid.NewGuid().ToString("N") }
                };


            var curWf = _workflowService.Create(unitOfWork, workflow);

            var wfImplementation = new WorkflowImplementation()
            {
                CreatorID = Base.Ambient.AppContext.SecurityUser.ID,
                Version = 1,
                CreateDate = Base.Ambient.AppContext.DateTime.Now,
                LastChangeDate = Base.Ambient.AppContext.DateTime.Now,
                EditorUserID = Base.Ambient.AppContext.SecurityUser.ID,


                Scheme = "{\"backgroundColor\":\"rgba(0, 0, 0, 0)\",\"steps\":{\""
                         + stepsDictionary["Cтарт"] +
                         "\":{\"size\":{\"width\":\"140px\",\"height\":\"32px\"},\"position\":{\"top\":\"20px\",\"left\":\"20px\"},\"backgroundColor\":\"rgb(92, 184, 92)\"},\""
                         + stepsDictionary["Точка выхода"] +
                         "\":{\"size\":{\"width\":\"140px\",\"height\":\"32px\"},\"position\":{\"top\":\"111.997px\",\"left\":\"615.993px\"},\"backgroundColor\":\"rgb(240, 128, 128)\"}}}",

                Steps = new List<Step>() {
                    new EntryPointStep()
                    {
                        Title = "Старт",
                        StepType = FlowStepType.EntryPointStep,
                        ViewID = stepsDictionary["Cтарт"],
                        Outputs = new List<StageAction>()
                            {
                                new StageAction()
                                {
                                    Title = "Точка входа",
                                    NextStepViewID = stepsDictionary["Точка выхода"]
                                }
                            },
                    },
                    new EndStep()
                    {
                        Title = "Точка выхода",
                        StepType = FlowStepType.EndStep,
                        ViewID = stepsDictionary["Точка выхода"],
                        BaseOutputs = new List<Output>()
                    }},
                WorkflowID = curWf.ID
            };

            _workflowImplementationService.Create(unitOfWork, wfImplementation);

            return curWf;
        }
    }
}
