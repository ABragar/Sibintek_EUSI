using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.UI.DetailViewSetting;
using Base.Utils.Common.Maybe;

namespace Base.BusinessProcesses.Entities.Steps
{    
    public class Stage : Step
    {
        public Stage()
        {
            AssignedToCategory= new List<StageUserCategory>();
            AssignedToUsers = new List<StageUser>();
            StepType = FlowStepType.Stage;
        }

        [PropertyDataType(PropertyDataType.Color)]
        [DetailView(Name = "Цвет", Order = 2)]
        [ListView]
        public string Color { get; set; }

        

        [DetailView(Name = "Создавать задачу", Order = 3)]
        [ListView]
        public bool CreateTask { get; set; }
       
        [DetailView(Name = "Шаблон заголовка задачи", Required = true, Order = 5)]
        [MaxLength(255)]
        public string TitleTemplate { get; set; }

        [DetailView(Name = "Шаблон описания задачи", Order = 6)]
        [PropertyDataType(PropertyDataType.MultilineText)]
        public string DescriptionTemplate { get; set; }

        [DetailView("Доступные свойства", TabName = "Помощь")]
        [NotMapped]
        [PropertyDataType("StepHelp")]
        public string Help { get; set; }

        [PropertyDataType(PropertyDataType.Duration), DetailView(Name = "Срок исполнения", Order = 7)]
        public int PerformancePeriod { get; set; }

        [DetailView(Name = "Тип расчета срока исполнения", Order = 7)]
        public PerfomancePeriodType PerfomancePeriodType { get; set; }

        [DetailView("Автопереход по истечении времени", Order = 8)]
        public bool AutoProcess { get; set; }

        public int? DvSettingID { get; set; }

        [DetailView(Name = "Настройка формы объекта", Order = 8)]
        [PropertyDataType(PropertyDataType.DetailViewSetting)]
        public virtual DvSettingForType DvSetting { get; set; }


        [DetailView(Name = "Отдел исполнителей", TabName = "[1]Ответственные")]
        public virtual ICollection<StageUserCategory> AssignedToCategory { get; set; }

        [DetailView(Name = "Исполнители", TabName = "[1]Ответственные")]
        public virtual ICollection<StageUser> AssignedToUsers { get; set; }
        
        [DetailView(Name = "Выбор исполнителя", TabName = "[1]Ответственные")]
        public bool IsCustomPerformer { get; set; }

        [NotMapped]
        [DetailView(Name = "Действия", HideLabel = true, TabName = "[1]Действия")]
        public ICollection<StageAction> Outputs
        {
            get { return BaseOutputs.With(x => x.OfType<StageAction>()).With(x => x.ToList()); }
            set { BaseOutputs = value.With(x => x.OfType<Output>()).With(x => x.ToList()); }
        }

        [DetailView(Name = "Стратегия выбора исполнителей", TabName = "[1]Ответственные")]
        [PropertyDataType("StakeholdersSelectionStrategy")]
        public string StakeholdersSelectionStrategy { get; set; }
    }
}