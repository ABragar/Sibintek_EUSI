using System;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;

namespace WebUI.Models.BusinessProcess
{
    public class TimeLineVm
    {        

        public List<TimeLineElementVm> Elements { get; } = new List<TimeLineElementVm>();

        public int WorkflowId { get; set; }

        public ICollection<StageVM> CurrnetStages { get; set; }

        public bool ShowCurrentStages { get; set; }

        private TimeLineElementVm GetStageTimeLineElement(ChangeHistory lastHistoryItem)
        {
            TimeLineElementVm result;
            Stage stage = lastHistoryItem.Step as Stage;

            if (stage.StepType == FlowStepType.EndStep)
            {
                result = new EndStepTimeLineElement(lastHistoryItem);
            }
            else
            {
                result = new StageTimeLineElement(lastHistoryItem);
            }
            return result;
        }

        public void InitCollection(List<ChangeHistory> changeHistories)
        {
            foreach (var historyItem in changeHistories)
            {
                TimeLineElementVm result = null;

                if (historyItem.Step is Stage)
                {
                    result = GetStageTimeLineElement(historyItem);
                }
                else if (historyItem.Step is WorkflowOwnerStep)
                {
                    result = new WorkflowOwnerStepTimeLineLementVm(historyItem);
                }
                if (result == null)
                    throw new Exception("reesult is null");

                Elements.Add(result);
            }

            for (var i = 0; i < Elements.Count; i++)
                if (i + 1 < Elements.Count)
                {
                    Elements[i].Previous = Elements[i + 1];
                    WorkflowOwnerStepTimeLineLementVm owner = Elements[i + 1] as WorkflowOwnerStepTimeLineLementVm;
                    if (owner != null)
                        owner.EndDate = Elements[i].Date;
                }
        }

        public IEnumerable<TimeLineElementVm> GetElements()
        {
            bool isOdd = false;

            foreach (TimeLineElementVm element in Elements)
            {
                element.IsOdd = isOdd = !isOdd;                

                yield return element;
            }
        }
    }
}