using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.Security;
using Base.Task.Entities;
using Base.UI.Wizard;

namespace Base.BusinessProcesses.Wizard
{
    /// <summary>
    /// Мастер создания бизнес-процесса
    /// </summary>
    public class WorkflowWizard : DecoratedWizardObject<Workflow>
    {
        public override Workflow GetObject()
        {
            return new Workflow
            {
                Title = this.Title,
                ObjectType = this.ObjectType,
                TaskCategoryID = this.BaseTaskCategory.ID,
                CuratorID = this.Curator.ID,
                Description = this.Description,
                CuratorsCategoryID = this.CuratorsCategory?.ID,
                PerformancePeriod = this.PerformancePeriod,
                SystemName = this.SystemName,
                IsDefault = this.IsDefault,
                CategoryID = this.CategoryID,
                IsTemplate = true
            };
        }

        [ListView]
        [MaxLength(255)]
        [DetailView(Name = "Наименование", Required = true)]
        public string Title { get; set; }

        [DetailView(Name = "Объект", Required = true)]
        [ListView]
        [PropertyDataType("ListWFObjects")]
        public string ObjectType { get; set; }

        [DetailView(Name = "Категория задачи", Required = true)]
        public virtual BaseTaskCategory BaseTaskCategory { get; set; }

        [DetailView(Name = "Куратор", Required = true)]
        [ListView]
        public virtual User Curator { get; set; }

        [SystemProperty]
        public int CategoryID { get; set; }

        [ListView]
        [DetailView(Name = "Описание")]
        public string Description { get; set; }

        [DetailView(Name = "Категория кураторов")]
        [ListView]
        public virtual UserCategory CuratorsCategory { get; set; }

        [PropertyDataType(PropertyDataType.Duration), DetailView(Name = "Срок исполнения", Order = 7)]
        public int PerformancePeriod { get; set; }

        [ListView]
        [MaxLength(255)]
        [DetailView(Name = "Системное имя")]
        public string SystemName { get; set; }

        [DetailView("Для объекта по умолчанию")]
        [SystemProperty]
        public bool IsDefault { get; set; } = true;

    }
}
