using Base.Attributes;
using Base.UI.ViewModal;
using CorpProp.Analyze.Entities.NSI;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;

namespace CorpProp.Analyze.Entities.Accounting
{
    /// <summary>
    /// Представляет финансовый показатель общества
    /// </summary>
    public class FinancialIndicatorItem : TypeObject
    {
        public FinancialIndicatorItem() : base()
        {
        }

        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает и задаёт общество группы которому принадлежит значение финансового показателя
        /// </summary>
        [ListView(Name = "ОГ", Order = 0)]
        [DetailView(Name = "ОГ", Order = 0, ReadOnly = true)]
        public Society Owner { get; set; }

        public int? FinancialIndicatorID { get; set; }

        /// <summary>
        /// Получает и задаёт финансовый показатель
        /// </summary>
        [ListView(Name = "Финансовый показатель", Order = 1)]
        [DetailView(Name = "Финансовый показатель", Order = 1, ReadOnly = true)]
        public FinancialIndicator FinancialIndicator { get; set; }

        /// <summary>
        /// Получает и задаёт значение показателя
        /// </summary>
        [ListView(Name = "Показатель", Order = 2)]
        [DetailView(Name = "Показатель", Order = 2, ReadOnly = true)]
        public decimal? FinancialIndValue { get; set; }
    }
}