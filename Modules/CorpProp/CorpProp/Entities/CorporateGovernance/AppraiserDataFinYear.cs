using Base.Attributes;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;
using SubjectObject = CorpProp.Entities.Subject;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет Данные оценщика за финансовый год
    /// </summary>
    [EnableFullTextSearch]
    public class AppraiserDataFinYear : TypeObject
    {
        /// <summary>
        /// Получает или задает дату начала действия.
        /// </summary>

        [DetailView(Name = "Действует с", Order = 1, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order = 1)]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия.
        /// </summary>
        [DetailView(Name = "Действует по", Order = 2, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order =2)]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Получает или задает выручку от оценочной деятельности (с НДС).
        /// </summary>
        [DetailView(Name = "Выручка от оценочной деятельности (с НДС)", Order = 3, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order = 3)]
        public decimal ProceedsValuationActivities { get; set; }

                /// <summary>
        /// Получает или задает ИД валюты.
        /// </summary>        
        public int? CurrencyID { get; set; }

        /// <summary>
        /// Получает или задает валюту.
        /// </summary>
        [DetailView(Name = "Валюта", Order = 4, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order = 4)]
        public virtual Currency Currency { get; set; }

        /// <summary>
        /// Получает или задает выручку от оценочной деятельности (с НДС).
        /// </summary>
        [DetailView(Name = "Количество выполненных оценок (отчетов, шт.)", Order = 5, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "Оценок всего (отчет, шт.)", Order = 5)]
        public int CompletedEvaluationsCount { get; set; }

        /// <summary>
        /// Получает или задает выручку от оценочной деятельности (с НДС).
        /// </summary>
        [DetailView(Name = "Количество оценок недвижимости (отчет, шт.)", Order = 6, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "Оценок недвижимости (отчет, шт.)", Order = 6)]
        public int RealEstateEvaluationsCount { get; set; }

        /// <summary>
        /// Получает или задает выручку от оценочной деятельности (с НДС).
        /// </summary>
        [DetailView(Name = "Количество оценок акций и долей (отчет, шт.)", Order = 7, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "Оценок акций и долей (отчет, шт.)", Order = 7)]
        public int SharesEvaluationsCount { get; set; }


        /// <summary>
        /// Получает или задает делового партнера (оценщика).
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Оценивающая организация", Order =8, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual Appraiser Appraiser { get; set; }
    }
}
