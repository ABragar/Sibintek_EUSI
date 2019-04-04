using Base.BusinessProcesses.Entities;
using Base.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Security;

namespace WebUI.Models.BusinessProcess
{

    public class EndStepTimeLineElement : TimeLineElementVm
    {
        public EndStepTimeLineElement(ChangeHistory history)
            : base(history)
        {
         
        }
    }

    public class StageTimeLineElement : TimeLineElementVm
    {
        

        public StageTimeLineElement(ChangeHistory history)
            : base(history)
        {
            AgreementItem = history.AgreementItem;
            Comment = AgreementItem.Comment;
            Color = AgreementItem.Action.Color;
            Action = AgreementItem.Action;
            Performer = AgreementItem.User;
            FromUser = AgreementItem.FromUser;         
            EndDateFact = history.AgreementItem.Date;
            if (EndDateFact.HasValue)
            {
                ElapsedTime = (EndDateFact ?? DateTime.Now) - Date;
                ElapsedString += ElapsedTime.Days != 0 ? ElapsedTime.Days + " д " : "";
                ElapsedString += ElapsedTime.Hours != 0 ? ElapsedTime.Hours + " ч " : "";
                if (Date.Date == EndDateFact.GetValueOrDefault().Date)
                {
                    ElapsedString += ElapsedTime.Minutes != 0 ? ElapsedTime.Minutes + " м " : "1 м ";
                }
            }
        }

        public DateTime? EndDateFact { get; set; }

        public AgreementItem AgreementItem { get; set; }

        public TimeSpan ElapsedTime { get; set; }
        public string ElapsedString { get; set; }


        public string Comment { get; set; }
        public StageAction Action { get; set; }

        public User Performer { get; set; }
        public User FromUser { get; set; }
    }
}