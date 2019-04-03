using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Base.Attributes;
using Base.BusinessProcesses.Entities.Steps;
using Base.Security;
using Base.Translations;
using Newtonsoft.Json;

namespace Base.BusinessProcesses.Entities
{
    [JsonObject]
    public class WorkflowImplementation : BaseObject
    {

        private static readonly CompiledExpression<WorkflowImplementation, string> _title =
            DefaultTranslationOf<WorkflowImplementation>.Property(x => x.Title)
                .Is(x => (x.Workflow != null ? x.Workflow.Title : "") + " ver." + x.Version + " от " + x.CreateDate);

        [SystemProperty]
        public string Title => _title.Evaluate(this);

        [ListView]
        [DetailView("Версия", Required = true)]
        public int Version { get; set; }

        public int? CreatorID { get; set; }
        [ListView]
        [DetailView("Создал", ReadOnly = true)]
        public virtual User Creator { get; set; }

        [ListView]
        [DetailView("Дата создания", ReadOnly = true)]
        public DateTime CreateDate { get; set; }

        public int? EditorUserID { get; set; }
        [DetailView("Изменил", ReadOnly = true)]
        public virtual User EditorUser { get; set; }

        [ListView]
        [DetailView("Дата последнего изменения", ReadOnly = true)]
        public DateTime LastChangeDate { get; set; }

        [ListView]
        [DetailView("Черновик")]
        public bool IsDraft { get; set; }
        
        [DetailView(Visible = false)]
        public int WorkflowID { get; set; }
        public virtual Workflow Workflow { get; set; }

        [DetailView(Name = "Этапы", HideLabel = true, TabName = "[1]Схема бизнес-процесса")]
        [PropertyDataType("BPWorkflow")]
        public virtual ICollection<Step> Steps { get; set; }
        
        [DetailView(Visible = false)]
        public string Scheme { get; set; }
    }
}
