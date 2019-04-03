using System.Collections.Generic;
using System.Linq;
using Base.Attributes;
using Base.Security;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.Task.Entities
{
    [EnableFullTextSearch]
    public class Task : BaseTask
    {
        private static readonly CompiledExpression<Task, string> Comment = DefaultTranslationOf<Task>.Property(x => x.LastComment).Is(x => x.TaskChangeHistory != null && x.TaskChangeHistory.LastOrDefault() != null ? x.TaskChangeHistory.LastOrDefault().Сomment ?? "" : "");

        public string LastComment => Comment.Evaluate(this);

        [DetailView("Наблюдатели", Order = 42)]
        public virtual ICollection<TaskObserverUser> ObserverUsers { get; set; } = new List<TaskObserverUser>();

        [DetailView("Исполнитель может поменять дату завершения", Order = 100)]
        public bool CanPerformerChangeDate { get; set; }

        [DetailView("Исполнитель может поменять исполнителя", Order = 110)]
        public bool CanChangePerformer { get; set; }

        [DetailView("Тип", Order = 130, Required = true), ListView]
        public TaskType Type { get; set; }

        [DetailView(TabName = "[4]История", ReadOnly = true)]
        public virtual ICollection<TaskChangeHistory> TaskChangeHistory { get; set; }
    }

    public class TaskExecutiveUser : EasyCollectionEntry<User>
    {
        
    }

    public class TaskObserverUser : EasyCollectionEntry<User>
    {
        
    }
}
