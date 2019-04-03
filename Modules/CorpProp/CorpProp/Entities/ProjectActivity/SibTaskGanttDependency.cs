using Base;
using Base.Attributes;
using CorpProp.Helpers;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using Base.Task.Entities;

namespace CorpProp.Entities.ProjectActivity
{
    public class SibTaskGanttDependency : BaseTaskDependency
    {
        [ListView(Visible = false, Hidden = true)]
        public int? PredecessorTaskID { get; set; }

        [ListView(Visible = false, Hidden = true)]
        public int? SuccessorTaskID { get; set; }
    }
}
