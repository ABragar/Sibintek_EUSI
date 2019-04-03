using Base.Attributes;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Document.Entities;

namespace Base.CRM.Entities
{
    /// <summary>
    /// Скидка базовая
    /// </summary>
    public class DiscountBase<T> : BaseObject
    {
        [ListView]
        [MaxLength(255)]
        [DetailView("Наименование", Required = true, Order = -10)]
        public string Title { get; set; }

        [DetailView("Описание", Order = -5)]
        public string Description { get; set; }

        [ListView]
        [DetailView("Срок действия", Order = -1)]
        public DateTime? EndDate { get; set; }
    }
}
