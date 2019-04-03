using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Entities.Complex;
using Base.Security;
using Base.Security.ObjectAccess;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.Task.Entities
{
    public class BaseTask : HCategory, ISuperObject<BaseTask>, IScheduler
    {
        private static readonly CompiledExpression<BaseTask, TaskDelayStatus> Delay = DefaultTranslationOf<BaseTask>.Property(x => x.TaskDelay).Is(x => TaskDelayStatus.NotImportant);

        private static readonly CompiledExpression<BaseTask, string> Message = DefaultTranslationOf<BaseTask>.Property(x => x.StatusMessage).Is(x => "Срок окончания не определен");

        private static readonly CompiledExpression<BaseTask, string> _color =
            DefaultTranslationOf<BaseTask>.Property(x => x.Color).Is(x => x.ColorPicker.Value);

        private static readonly CompiledExpression<BaseTask, DateTime> _end =
            DefaultTranslationOf<BaseTask>.Property(x => x.End).Is(x => x.Period.End != null ? x.Period.End.Value : x.Period.Start);

        private static readonly CompiledExpression<BaseTask, DateTime> _start =
            DefaultTranslationOf<BaseTask>.Property(x => x.Start).Is(x => x.Period.Start);

        private static readonly CompiledExpression<BaseTask, string> _title =
            DefaultTranslationOf<BaseTask>.Property(x => x.Title).Is(x => x.Name);

        [ListView(Order = 4)]
        [DetailView(Name = "Период", Required = true, Order = 10)]
        public Period Period { get; set; } = new Period();

        [ListView(Name = "% выполнения", Order = 5)]
        [DetailView(Name = "Процент выполнения", Order = 20)]
        [PropertyDataType(PropertyDataType.Percent)]
        public double PercentComplete { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Дата завершения", ReadOnly = true, Visible = false)]
        public DateTime? CompliteDate { get; set; }

        public int? AssignedFromID { get; set; }

        [FullTextSearchProperty]
        [ListView(Hidden = true, Order = 0)]
        [DetailView(Name = "Автор", ReadOnly = true, Order = 30)]
        public virtual User AssignedFrom { get; set; }

        public int? AssignedToID { get; set; }

        [FullTextSearchProperty]
        [ListView(Hidden = true, Order = 0)]
        [DetailView(Name = "Исполнитель", Order = 40)]
        public virtual User AssignedTo { get; set; }

        [DetailView("Соисполнители", Order = 41)]
        public virtual ICollection<TaskExecutiveUser> AssignedToUsers { get; set; } = new List<TaskExecutiveUser>();

        [ListView(Order = 3)]
        [DetailView(Name = "Приоритет", Order = 50)]
        public Priority Priority { get; set; } = Entities.Priority.Normal;

        [ListView(Order = 2)]
        [DetailView(Name = "Статус", ReadOnly = true, Order = 60)]
        public TaskStatus Status { get; set; }

        [DetailView("Вышестоящая задача", Order = 70)]
        [ForeignKey("ParentID")]
        public virtual BaseTask Parent_ { get; set; }
        public virtual ICollection<BaseTask> Children_ { get; set; }

        #region HCategory
        public override HCategory Parent => this.Parent_;
        public override IEnumerable<HCategory> Children => Children_ ?? new List<BaseTask>();
        #endregion

        [ForeignKey("CategoryID")]
        [DetailView("Категория", Order = 80), ListView]
        public virtual BaseTaskCategory BaseTaskCategory { get; set; }

        [FullTextSearchProperty]
        [DetailView("Описание", TabName = "[2]Описание", HideLabel = true)]
        [PropertyDataType(PropertyDataType.SimpleHtml)]
        [ListView(Hidden = true)]
        public string Description { get; set; }

        [FullTextSearchProperty]
        [DetailView(TabName = "[3]Файлы")]
        public virtual ICollection<BaseTaskFile> Files { get; set; }

        //[SystemProperty]
        //public bool System { get; set; }

        public bool Auto { get; set; }

        [SystemProperty]
        [NotMapped]
        public TaskDelayStatus TaskDelay => Delay.Evaluate(this);

        [NotMapped]
        [DetailView("Статус просрочки", Order = 90)]
        [ListView(Order = 3, Filterable = false, Sortable = false, Width = 240)]
        public string StatusMessage => Message.Evaluate(this);

        [DetailView("Цвет", Order = 200)]
        public Color ColorPicker { get; set; } = new Color();

        public int? CategoryID { get; set; }

        [ListView("Тип")]
        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; }

        #region IScheduler

        [SystemProperty]
        public string Title => _title.Evaluate(this);

        [SystemProperty]
        public DateTime Start => _start.Evaluate(this);

        [SystemProperty]
        public DateTime End => _end.Evaluate(this);

        [SystemProperty]
        public string StartTimezone { get; set; }

        [SystemProperty]
        public string EndTimezone { get; set; }

        [SystemProperty]
        public string RecurrenceRule { get; set; }

        [SystemProperty]
        public int? RecurrenceID { get; set; }

        [SystemProperty]
        public string RecurrenceException { get; set; }

        [SystemProperty]
        public bool IsAllDay { get; set; }

        [SystemProperty]
        public string Color => _color.Evaluate(this);

        #endregion
    }


    [UiEnum]
    public enum TaskDelayStatus
    {
        /// <summary>
        /// Статус задержки не важен (если задача, например, отменена или завершена)
        /// </summary>
        NotImportant = 10,
        /// <summary>
        /// Осталось менее двух дней до окончания срока
        /// </summary>
        Danger = 20,
        /// <summary>
        /// 
        /// </summary>
        Delay = 30
    }
}
