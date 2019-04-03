using System;
using Base;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;

namespace EUSI.Entities.Estate
{
    /// <summary>
    /// Атрибуты контрольных дат заявки на регистрацию ОИ.
    /// </summary>
    public class ERControlDateAttributes : BaseObject
    {
        private static readonly CompiledExpression<ERControlDateAttributes, TimeSpan> _eRRemainingTime =
            DefaultTranslationOf<ERControlDateAttributes>
                .Property(x => x.ERRemainingTime)
                .Is(x=>new TimeSpan(1, 20, 30)); //TODO Убрать заглушку (Задача 11995)

        /// <summary>
        /// Инициализирует новый экземпляр класса ERControlDateAttributes.
        /// </summary>
        public ERControlDateAttributes() : base()
        {

        }

        /// <summary>
        /// Получает или задает дату заявки ЦДС.
        /// </summary>
        [DetailView("Дата заявки ЦДС", ReadOnly = false, Visible = false)]
        [ListView("Дата заявки ЦДС", Visible = false)]
        [FullTextSearchProperty]
        public DateTime? DateCDS { get; set; }

        /// <summary>
        /// Дата создания заявки.
        /// </summary>
        [ListView("Дата создания заявки", Visible = false)]
        [DetailView("Дата создания заявки", Visible = false, ReadOnly = true)]
        public DateTime? DateСreation { get; set; }

        /// <summary>
        /// Дата отправки заявки на проверку.
        /// </summary>
        [ListView("Дата отправки заявки на проверку", Visible = false)]
        [DetailView("Дата отправки заявки на проверку", Visible = false, ReadOnly = true)]
        public DateTime? DateToVerify { get; set; }

        /// <summary>
        /// Дата проверки (выполнения) заявки.
        /// </summary>
        [ListView("Дата проверки (выполнения) заявки", Visible = false)]
        [DetailView("Дата проверки (выполнения) заявки", Visible = false, ReadOnly = true)]
        public DateTime? DateVerification { get; set; }

        /// <summary>
        /// Дата отклонения заявки.
        /// </summary>
        [ListView("Дата отклонения заявки", Visible = false)]
        [DetailView("Дата отклонения заявки", Visible = false, ReadOnly = true)]
        public DateTime? DateRejection { get; set; }

        /// <summary>
        /// Дата отправки на уточнение.
        /// </summary>
        [ListView("Дата отправки на уточнение", Visible = false)]
        [DetailView("Дата отправки на уточнение", Visible = false, ReadOnly = true)]
        public DateTime? DateToСlarify { get; set; }

        /// <summary>
        /// Оставшееся время на выполнение заявки(Вычисляемый атрибут).
        /// </summary>
        [ListView("Оставшееся время на выполнение заявки", Visible = false)]
        [DetailView("Оставшееся время на выполнение заявки", Visible = false, ReadOnly = true)]
        public TimeSpan ERRemainingTime => _eRRemainingTime.Evaluate(this);
    }
}