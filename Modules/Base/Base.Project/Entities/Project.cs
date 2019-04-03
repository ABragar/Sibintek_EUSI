using System;
using Base.Attributes;
using Base.Security;
using Base.Task.Entities;

namespace Base.Project.Entities
{
    public class Project : BaseTaskCategory
    {
        public int? UserID { get; set; }

        [DetailView("Руководитель", Required = true, Order = 5), ListView]
        public virtual User User { get; set; }

        [DetailView("Описание", Order = 10), ListView]
        public string Description { get; set; }

        [DetailView("Дата начала проекта", Required = true, Order = 20), ListView]
        public DateTime StartDate { get; set; }

        [DetailView("Статус", Required = true, Order = 30), ListView]
        public ProjectStatus Status { get; set; }
    }

    [UiEnum]
    public enum ProjectStatus
    {
        [UiEnumValue("Черновик")]
        Draft = 10,
        [UiEnumValue("Текущий")]
        Current = 20,
        [UiEnumValue("В архиве")]
        Arhcive = 30
        
    }
}