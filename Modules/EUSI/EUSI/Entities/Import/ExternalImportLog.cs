using CorpProp.Entities.Subject;
using System;
using Base;
using Base.Attributes;
using Base.Translations;

namespace EUSI.Entities.Import
{
    /// <summary>
    /// Представляет класс журнала результатов загрузки данных.
    /// </summary>
    public class ExternalImportLog : BaseObject
    {
        private static readonly CompiledExpression<ExternalImportLog, DateTime?> _externalImportDate =
            DefaultTranslationOf<ExternalImportLog>.Property(x => x.ExternalImportDate).Is(x => x.ExternalImportDateTime);


        /// <summary>
        /// Идентификатор объекта имущества в ИР ЕУСИ. 
        /// </summary>
        [DetailView]
        [ListView]
        public string EUSINumber { get; set; }

        /// <summary>
        /// Дата загрузки данных в БУС (ИСУП РН).
        /// Для отображения в UI.
        /// </summary>
        [DetailView]
        [ListView]
        public DateTime? ExternalImportDate => _externalImportDate.Evaluate(this);

        /// <summary>
        /// Дата и время загрузки данных в БУС (ИСУП РН).
        /// </summary>
        [DetailView]
        [ListView]
        public DateTime? ExternalImportDateTime { get; set; }

        /// <summary>
        /// Дата и время загрузки данных в ЕУСИ.
        /// </summary>
        [DetailView]
        [ListView]
        public DateTime? ImportDate { get; set; }

        /// <summary>
        /// Признак успешного завершения загрузки.
        /// </summary>
        [DetailView]
        [ListView]
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// ИД ОГ, направившего журнал загрузки в ЕУСИ.
        /// </summary>
        public int? SocietyID { get; set; }

        /// <summary>
        /// ОГ, направившее журнал загрузки в ЕУСИ.
        /// </summary>
        [DetailView]
        [ListView]
        public Society Society { get; set; }
    }
}
