using System;
using Base.Attributes;
using CorpProp.Analyze.Entities.NSI;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;

namespace CorpProp.Analyze.Entities.Accounting
{
    /// <summary>
    /// 
    /// </summary>
    public class RecordBudgetLine : TypeObject
    {
        public RecordBudgetLine() : base()
        {
        }


        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает и задаёт общество группы которому принадлежит значение строки бюджета
        /// </summary>
        [ListView(Name = "ОГ", Order = 0)]
        [DetailView(Name = "ОГ", Order = 0, ReadOnly = true)]
        public Society Owner { get; set; }


        public int? BudgetLineID { get; set; }

        /// <summary>
        /// Получает и задаёт общество группы которому принадлежит значение строки бюджета
        /// </summary>
        [ListView(Name = "Строка бюджета", Order = 1)]
        [DetailView(Name = "Строка бюджета", Order = 1, ReadOnly = true)]
        public BudgetLine BudgetLine { get; set; }

        /// <summary>
        /// получает и задаёт дату за которую было значение строки бюджета
        /// </summary>
        [ListView(Name = "Дата", Order = 2)]
        [DetailView(Name = "Дата", Order = 2, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime? DateOfValue { get; set; }

        /// <summary>
        /// получает и задаёт значение строки бюджета
        /// </summary>
        [ListView(Name = "Сумма", Order = 3)]
        [DetailView(Name = "Сумма", Order = 3, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Number)]
        public decimal? Amount { get; set; }
    }
}