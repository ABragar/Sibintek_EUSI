using Base.Attributes;
using CorpProp.Helpers;
using System;
using Base.Utils.Common.Attributes;
using System.ComponentModel;
using CorpProp.Entities.Security;
using Base;
using Base.Entities.Complex;
using Base.Translations;
using Base.Task.Entities;

namespace CorpProp.Entities.ProjectActivity
{
    /// <summary>
    /// Представляет задачу.
    /// </summary>
    [EnableFullTextSearch]
    public class SibTask : BaseTask, ITreeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibTask.
        /// </summary>
        public SibTask()
        {
        }

        private static readonly CompiledExpression<SibTask, string> _projectName =
            DefaultTranslationOf<SibTask>.Property(x => x.ProjectName).Is(x => (x.Project != null) ? x.Project.Title : "");

        private static readonly CompiledExpression<SibTask, DateTime?> _projectDateFrom =
            DefaultTranslationOf<SibTask>.Property(x => x.ProjectDateFrom).Is(x => (x.Project != null) ? x.Project.DateFrom : null);

        private static readonly CompiledExpression<SibTask, DateTime?> _projectDateTo =
            DefaultTranslationOf<SibTask>.Property(x => x.ProjectDateTo).Is(x => (x.Project != null) ? x.Project.DateTo : null);

        private static readonly CompiledExpression<SibTask, string> _projectInitiator =
            DefaultTranslationOf<SibTask>.Property(x => x.ProjectInitiator).Is(x => (x.Project != null) ? ((x.Project.Initiator != null) ? x.Project.Initiator.FullName : null): null);

        public override BaseTaskCategory BaseTaskCategory => Project;

        public override BaseTask Parent_ => TaskParent;

        public int? TaskParentID { get; set; }

        [DetailView(Name = "Вышестоящая задача", Order = 1, TabName = CaptionHelper.DefaultTabName, Visible = true)]
        public virtual SibTask TaskParent { get; set; }

        [ListView(Name = "Наименование проекта", Visible = true)]
        [FullTextSearchProperty]
        [DetailView(Visible = false)]
        public string ProjectName => _projectName.Evaluate(this);

        [ListView(Name = "Дата начала проекта", Visible = true)]
        [FullTextSearchProperty]
        [DetailView(Visible = false)]
        public DateTime? ProjectDateFrom => _projectDateFrom.Evaluate(this);

        [ListView(Name = "Дата окончания проекта", Visible = true)]
        [FullTextSearchProperty]
        [DetailView(Visible = false)]
        public DateTime? ProjectDateTo => _projectDateTo.Evaluate(this);

        /// <summary>
        /// Получает или задает пользователя.
        /// </summary>
        /// <remarks>
        /// Инициатор проекта.
        /// </remarks>
        [ListView(Name = "Инициатор проекта", Visible = true)]
        [FullTextSearchProperty]
        [DetailView(Visible = false)]
        public string ProjectInitiator => _projectInitiator.Evaluate(this);

        /// <summary>
        /// Номер
        /// </summary>
        [ListView]        
        [FullTextSearchProperty]        
        [DetailView(Name = "Номер", Order = 1, TabName = CaptionHelper.DefaultTabName, Visible = true, ReadOnly = true)]
        public string Number { get; set; }

        /// <summary>
        /// Внутренний номер задачи для идентификации задачи в пределах проекта. 
        /// </summary>
        [ListView(Hidden = true, Visible = false)]
        //[FullTextSearchProperty]
        [DetailView(Name = "Внутренний номер", Order = 1, TabName = CaptionHelper.DefaultTabName, Visible = false, ReadOnly = true)]
        public int? InternalNumber { get; set; }

        /// <summary>
        /// Получает или задает дату создания задачи.
        /// </summary>
        [DetailView(Name = "Дата создания задачи", Order = 3, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public DateTime? DateCreate { get; set; }

        /// <summary>
        /// Получает или задает фактическую дату окончания задачи.
        /// </summary>
        [DetailView(Name = "Дата фактического выполнения", Order = 4, TabName = CaptionHelper.DefaultTabName, Visible = true)]
        public DateTime? DateEnd { get; set; }
        
        public int OrderId { get; set; }

        /// <summary>
        /// Дельта начала задачи (дней).
        /// </summary>
        //[DetailView(Name = "Дельта", Order = 10, TabName = CaptionHelper.DefaultTabName, Visible = true)]
        public int? Delta { get; set; }

        /// <summary>
        /// Продолжительность задачи (дней).
        /// </summary>
        //[DetailView(Name = "Продолжительность задачи", Order = 11, TabName = CaptionHelper.DefaultTabName, Visible = true)]
        public int? Duration { get; set; }

        /// <summary>
        /// Получает или задает признак требуется приложить документ к отчёту.
        /// </summary>
        [DetailView(Name = "Необходимо прикрепление отчетного документа", Order = 13, TabName = CaptionHelper.DefaultTabName)]
        [DefaultValue(false)]
        public bool IsRequiredLinkReportFile { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса проекта
        /// </summary>
        public int? SibStatusID { get; set; }

        /// <summary>
        /// Получает или задает статус задачи.
        /// </summary>       
        [ListView]
        [DetailView(Name = "Статус задачи", Order = 18, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SibTaskStatus SibStatus { get; set; }

        /// <summary>
        /// Получает или задает ИД проекта.
        /// </summary>
        public int? ProjectID { get; set; }

        /// <summary>
        /// Получает или задает проект.
        /// </summary>
        [DetailView(Name = "Проект", Order = 20, TabName = CaptionHelper.DefaultTabName, Required = false)]
        public virtual SibProject Project { get; set; }

        /// <summary>
        /// Получает или задает признак шаблона задачи.
        /// </summary>
        [DefaultValue(false)]
        [SystemProperty]
        public bool IsTemplate { get; set; }

        /// <summary>
        /// Получает или задает ИД шаблона задачи.
        /// </summary>
        [SystemProperty]
        public int? TemplateID { get; set; }

        /// <summary>
        /// Получает или задает шаблон используемый при создании задачи.
        /// </summary>
        //[DetailView(Name = "Шаблон задачи", Order = 21, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        //public virtual SibTask Template { get; set; }

        /// <summary>
        /// Получает или задает ИД инициатора.
        /// </summary>
        public int? InitiatorID { get; set; }

        /// <summary>
        /// Получает или задает инициатора задачи.
        /// </summary>
        /// <remarks>
        /// Инициатор проекта.
        /// </remarks>
        [ListView]
        [DetailView(Name = "Инициатор", Order = 23, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual SibUser Initiator { get; set; }

        /// <summary>
        /// Получает или задает ИД ответственного исполнителя.
        /// </summary>
        public int? ResponsibleID { get; set; }

        /// <summary>
        /// Получает или задает ответственного исполнитель.
        /// </summary>
        /// <remarks>
        /// Ответственный исполнитель.
        /// </remarks>       
        [ListView]
        [DetailView(Name = "Ответственный исполнитель", Order = 24, TabName = CaptionHelper.DefaultTabName, Required = false)]
        public virtual SibUser Responsible { get; set; }

        #region Notification

        [DetailView("Включить уведомление", TabName = "Уведомление")]
        [DefaultValue(false)]
        public bool NotificationEnabled { get; set; }

        [DetailView("Напоминание", TabName = "Уведомление")]
        public RemindPeriod RemindPeriod { get; set; } = new RemindPeriod();

        [DetailView("Напомнить до", Description = "Напоминание сработает за указанный период до наступления даты в выбранном поле.", TabName = "Уведомление")]
        [PropertyDataType("Sib_NotificationProperty")]
        public string PropertyName { get; set; }

        [DetailView("Тема", TabName = "Уведомление")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NotificationSubject { get; set; }

        [DetailView("Сообщение", TabName = "Уведомление")]
        public string NotificationMessage { get; set; }

        #endregion
    }
}
