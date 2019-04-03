using Base;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using System;
using Base.Attributes;

namespace EUSI.Entities.Accounting
{
    /// <summary>
    /// Протокол сверки сальдо.
    /// </summary>
    public class BalanceReconciliationReport : BaseObject
    {
        public BalanceReconciliationReport() : base()
        {

        }

        public int? PositionConsolidationID {get;set;}

        /// <summary>
        /// Позиция консолидации.
        /// </summary>
        [DetailView(Name = "Позиция консолидации")]
        [ListView(Name = "Позиция консолидации")]
        public PositionConsolidation PositionConsolidation {get;set;}

        public int? ConsolidationID {get;set;} 

        /// <summary>
        /// Наименование ОГ.
        /// </summary>
        [DetailView(Name = "ЕК")]
        [ListView(Name = "ЕК")]
        public Consolidation Consolidation {get;set;}

        /// <summary>
        /// Период.
        /// </summary>
        [DetailView(Name = "Период")]
        [ListView(Name = "Период")]
        [PropertyDataType(PropertyDataType.Month)]
        public DateTime? Period {get;set;}

        /// <summary>
        /// Дополнительная аналитика 1.
        /// </summary>
        [DetailView(Name = "Дополнительная аналитика 1")]
        [ListView(Name = "Дополнительная аналитика 1")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AdditionalAnalitics_1 {get;set;}

        /// <summary>
        /// Дополнительная аналитика 2.
        /// </summary>
        [DetailView(Name = "Дополнительная аналитика 2")]
        [ListView(Name = "Дополнительная аналитика 2")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AdditionalAnalitics_2 {get;set;}

        /// <summary>
        /// Первоначальная стоимость по РСБУ на начало периода.
        /// </summary>
        [DetailView(Name = "Первоначальная стоимость по РСБУ на начало периода")]
        [ListView(Name = "Первоначальная стоимость по РСБУ на начало периода")]
        public decimal? StartPeriodPriceRSBU {get;set;}

        /// <summary>
        /// Накопленный износ по РСБУ на начало периода.
        /// </summary>
        [DetailView(Name = "Накопленный износ по РСБУ на начало периода")]
        [ListView(Name = "Накопленный износ по РСБУ на начало периода")]
        public decimal? StartPeriodWearRSBU {get;set;}

        /// <summary>
        /// Первоначальная стоимость по РСБУ на конец периода.
        /// </summary>
        [DetailView(Name = "Первоначальная стоимость по РСБУ на конец периода")]
        [ListView(Name = "Первоначальная стоимость по РСБУ на конец периода")]
        public decimal? EndPeriodPriceRSBU {get;set;}

        /// <summary>
        /// Накопленный износ по РСБУ на конец периода.
        /// </summary>
        [DetailView(Name = "Накопленный износ по РСБУ на конец периода")]
        [ListView(Name = "Накопленный износ по РСБУ на конец периода")]
        public decimal? EndPeriodWearRSBU {get;set;}
    }
}
