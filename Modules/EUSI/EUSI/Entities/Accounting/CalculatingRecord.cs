using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Accounting
{

    /// <summary>
    /// Реестр расчетов.
    /// </summary>
    [EnableFullTextSearch]
    public class CalculatingRecord : BaseObject
    {
        public CalculatingRecord() : base()
        {

            PeriodCalculatedNU = PeriodCalculatedNU.Year;  
        }

        /// <summary>
        /// Результат выполнения.
        /// </summary>
        [DetailView(Name = "Результат выполнения")]
        [ListView(Name = "Результат выполнения")]
        public string Result { get; set; }

        /// <summary>
        /// Дата расчета.
        /// </summary>
        [DetailView(Name = "Дата расчета")]
        [ListView(Name = "Дата расчета")]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime CalculatingDate { get; set; }

        /// <summary>
        /// Инициатор расчета.
        /// </summary>
        [DetailView(Name = "Инициатор расчета")]
        [ListView(Name = "Инициатор расчета")]
        public SibUser Initiator { get; set; }

        public int? InitiatorID { get; set; }

        /// <summary>
        /// Год.
        /// </summary>
        [DetailView(Name = "Год")]
        [ListView(Name = "Год")]
        [PropertyDataType("Sib_Year")]
        public int? Year { get; set; }

        /// <summary>
        /// БЕ.
        /// </summary>
        [DetailView(Name = "БЕ")]
        [ListView(Name = "БЕ")]
        public Consolidation Consolidation { get; set; }

        /// <summary>
        /// ИД БЕ.
        /// </summary>
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Группа ОС/НМА по РСБУ.
        /// </summary>
        [DetailView(Name = "Группа ОС/НМА по РСБУ", Visible = false)]
        [ListView(Name = "Группа ОС/НМА по РСБУ", Visible = false)]
        public PositionConsolidation PositionConsolidation { get; set; }

        /// <summary>
        /// Период расчета.
        /// </summary>
        [DetailView(Name = "Период расчета")]
        [ListView(Name = "Период расчета")]
        public PeriodCalculatedNU PeriodCalculatedNU { get; set; }

        /// <summary>
        /// ИД Группы ОС/НМА по РСБУ.
        /// </summary>
        public int? PositionConsolidationID { get; set; }

        /// <summary>
        /// Вид налога.
        /// </summary>
        [DetailView(Name = "Вид налога")]
        [ListView(Name = "Вид налога")]
        public TaxRateType TaxRateType { get; set; }
        public int? TaxRateTypeID { get; set; }

        /// <summary>
        /// Получает или задает уникальный ИД объекта.
        /// </summary>
        [SystemProperty]
        public System.Guid Oid { get; set; }
    }
}
