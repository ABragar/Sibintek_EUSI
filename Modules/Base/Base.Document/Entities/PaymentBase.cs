using Base.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using Base.Translations;

namespace Base.Document.Entities
{
    /// <summary>
    /// Платеж
    /// </summary>
    public abstract class PaymentBase : BaseObject
    {
        private static readonly CompiledExpression<PaymentBase, int> _delayDays = DefaultTranslationOf<PaymentBase>.Property(x => x.DelayDays).Is(x => x.FactDate.HasValue ? (x.FactDate - x.PlanDate).Value.Days : 0);
        
        [ListView]
        [DetailView("Фактическая дата платежа")]
        public DateTime? FactDate { get; set; }

        [ListView]
        [DetailView("Планируемая дата платежа")]
        public DateTime PlanDate { get; set; }

        [ListView]
        [DetailView("Количество дней просрочки")]
        public int DelayDays => _delayDays.Evaluate(this);

        [ListView]
        [DetailView("Фактическая сумма платежа")]
        [DataType(DataType.Currency)]
        public decimal FactSumm { get; set; }

        [ListView]
        [DetailView("Планируемая сумма платежа")]
        [DataType(DataType.Currency)]
        public decimal PlanSumm { get; set; }

        [ListView]
        [DetailView("Процент просрочки")]
        [DataType(DataType.Currency)]
        public decimal Percent { get; set; }

        [ListView]
        [DetailView("Поступил")]
        public bool Complete { get; set; }
    }
}
