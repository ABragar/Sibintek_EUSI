using Base.Attributes;
using Base.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Task.Entities;

namespace Base.BusinessProcesses.Entities
{ 
    [JsonObject]
    public class Workflow : BaseObject, ICategorizedItem
    {

        public int CategoryID { get; set; }

        [JsonIgnore]
        [ForeignKey("CategoryID")]
        public virtual WorkflowCategory Category_ { get; set; }

        #region ICategorizedItem
        HCategory ICategorizedItem.Category => Category_;
        #endregion


        [ListView]
        [MaxLength(255)]
        [DetailView(Name = "Наименование", Required = true)]
        public string Title { get; set; }

        [ListView]
        [DetailView(Name = "Описание")]
        public string Description { get; set; }

        [DetailView(Name = "Объект", Required = true)]
        [ListView]
        [PropertyDataType("ListWFObjects")]
        public string ObjectType { get; set; }

        public int? CuratorID { get; set; }

        [DetailView(Name = "Куратор", Required = true)]
        [ListView]
        public virtual User Curator { get; set; }

        public int? CuratorsCategoryID { get; set; }

        [DetailView(Name = "Категория кураторов")]
        [ListView]
        public virtual UserCategory CuratorsCategory { get; set; }

        public int? TaskCategoryID { get; set; }

        [DetailView(Name = "Категория задачи", Required = true, Order = 4)]
        [ForeignKey("TaskCategoryID")]
        public virtual BaseTaskCategory BaseTaskCategory { get; set; }

        [PropertyDataType(PropertyDataType.Duration), DetailView(Name = "Срок исполнения", Order = 7)]
        public int PerformancePeriod { get; set; }

        public int? CreatorID { get; set; }
        public virtual User Creator { get; set; }

        public DateTime? CreatedDate { get; set; }

        [ListView("Шаблон", Visible = false)]
        public bool IsTemplate { get; set; }

        public bool CreateTemplate { get; set; }

        [ListView]
        [MaxLength(255)]
        [DetailView(Name = "Системное имя")]
        public string SystemName { get; set; }

        [DetailView("Версии", TabName = "Версии", HideLabel = true)]
        [PropertyDataType("WorkflowVersions")]
        public virtual ICollection<WorkflowImplementation> WorkflowImplementations { get; set; }

        [DetailView("Для объекта по умолчанию")]
        [SystemProperty]
        public bool IsDefault { get; set; } = false;
    }
}