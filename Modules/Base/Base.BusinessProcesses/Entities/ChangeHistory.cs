using Base.Security;
using System;
using System.Collections.Generic;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Entities
{
    public class ChangeHistory : BaseObject
    {
        public DateTime Date { get; set; }

        public int? UserID { get; set; }
        public virtual User User { get; set; }

        public int StepID { get; set; }
        public virtual Step Step { get; set; }

        public bool AutoInvoked { get; set; }

        public int? AgreementItemID { get; set; }
        public virtual AgreementItem AgreementItem { get; set; }

        public int? CreatedObjectID { get; set; }
        public virtual CreatedObject CreatedObject { get; set; }

        public int ObjectID { get; set; }
        public string ObjectType { get; set; }
        public int WorkflowContextID { get; set; }
        public int WorkflowVersionID { get; set; }
    }
}