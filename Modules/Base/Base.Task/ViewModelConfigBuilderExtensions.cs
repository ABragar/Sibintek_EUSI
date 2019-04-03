using Base.Task.Entities;
using Base.UI;

namespace Base.Task
{
    public static class ViewModelConfigBuilderExtensions
    {
        public static ViewModelConfigBuilder<T> AddTaskToolbar<T>(this ViewModelConfigBuilder<T> builder)
            where T : BaseTask
        {
            builder.DetailView(x => x.Toolbar(t => t.Add("TaskToolbar", "BusinessProcesses",

                a => a.Title("Действия").IsAjax(true).ListParams(l => l.Add("taskID", "[ID]")))));

            return builder;
        }

        public static ListViewBuilder<T> AddTaskConditionAppearence<T>(this ListViewBuilder<T> builder)
            where T : BaseTask
        {

            builder.ConditionAppearence(c => c
                .Add("TaskDelay == 0", "\\#5cb85c")
                .Add("TaskDelay == 1", "\\#5bc0de")
                .Add("TaskDelay == 2", "\\#f0ad4e")
                .Add("TaskDelay == 3", "\\#f0ad4e")
                .Add("TaskDelay == 4", "\\#f0ad4e")
                .Add("TaskDelay == 5", "\\#d9534f")
                .Add("TaskDelay == null", "\\#d9534f"));

            return builder;
        }
    }
}