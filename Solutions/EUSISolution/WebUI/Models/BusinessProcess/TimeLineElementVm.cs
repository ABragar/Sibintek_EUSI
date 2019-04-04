using Base.BusinessProcesses.Entities;
using Base.UI;
using System;
using Base.BusinessProcesses.Entities.Steps;

namespace WebUI.Models.BusinessProcess
{
    public abstract class TimeLineElementVm
    {
        protected TimeLineElementVm(ChangeHistory history)
        {
            Title = history.Step.Title;
            Step = history.Step;
            Description = history.Step.Description;
            Date = history.Date;
            ID = "te_" + Guid.NewGuid().ToString("D").Split('-')[0];            
            WorkflowId = history.Step.WorkflowImplementationID;
            ObjectID = history.ObjectID;
            ObjectType = history.ObjectType;
        }

        public string ObjectType { get; set; }
        public int ObjectID { get; set; }

        public int WorkflowId { get; set; }

        public bool IsOdd { get; set; }

        public string ID { get; private set; }

        public string Color { get; set; }

        public DateTime Date { get; set; }
        public TimeLineElementVm Previous { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Step Step { get; set; }
    }
}