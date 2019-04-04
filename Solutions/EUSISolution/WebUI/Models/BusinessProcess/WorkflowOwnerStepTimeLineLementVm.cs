using Base.BusinessProcesses.Entities;
using Base.UI;
using System;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities.Steps;

namespace WebUI.Models.BusinessProcess
{
    public class WorkflowOwnerStepTimeLineLementVm : TimeLineElementVm 
    {
        public WorkflowOwnerStepTimeLineLementVm(ChangeHistory history)            
            : base(history)
        {
            var workflowOwnerStep = history.Step as WorkflowOwnerStep;
            if(workflowOwnerStep == null)
                throw new ArgumentNullException(nameof(history));
            ChildWorkflowID = workflowOwnerStep.ChildWorkflowImplementation.ID;
            Date = history.Date;
            Color = "#f0ad4e";
        }

        

        public int ChildWorkflowID { get; set; }       

        public DateTime? EndDate { get; set; }
    }
}