using Base;
using Base.Attributes;
using CorpProp.Common;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.NonPersistent
{
    /// <summary>
    /// Представляет форму с параметрами: БЕ и Период (месяц, год).
    /// </summary>
    /// <remarks>Не хранится в БД.</remarks>
    public class BEAndMonthPeriod : BaseObject, IDialogObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BEAndMonthPeriod.
        /// </summary>
        public BEAndMonthPeriod() : base()
        {

        }

        /// <summary>
        /// Получает или задает БЕ.
        /// </summary>
        [DetailView("БЕ", Required = true), ListView()]
        public virtual Consolidation Consolidation { get; set; }

        /// <summary>
        /// Получает или задает ИД БЕ.
        /// </summary>       
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает период (месяц, год).
        /// </summary>
        [DetailView("Период", Required = true), ListView()]   
        [PropertyDataType(PropertyDataType.Month)]
        public DateTime? MonthPeriod { get; set; }
    }
}
