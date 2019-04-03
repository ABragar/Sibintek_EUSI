using Base.Attributes;
using Base.Task.Entities;
using Base.Utils.Common.Attributes;

namespace Base.BusinessProcesses.Entities
{
    [EnableFullTextSearch]
    public class BPTask : BaseTask
    {
        public BPTask()
        {
            Auto = true;
        }
        [SystemProperty]
        public int? StagePerformID { get; set; }

        [DetailView(Visible = false)]
        public virtual StagePerform StagePerform { get; set; }
        [DetailView(Visible = false)]
        public string ObjectType { get; set; }
        [SystemProperty]
        public int ObjectID { get; set; }

        public bool ForcedTask { get; set; } //Если создал куратор или админ когда взял на итсполнение
    }
}