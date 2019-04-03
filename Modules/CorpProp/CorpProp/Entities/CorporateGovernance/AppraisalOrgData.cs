using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет Данные оценочных организаций
    /// </summary>
    [EnableFullTextSearch]
    public class AppraisalOrgData : TypeObject
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
        [ListView(Order = 2)]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Получает или допуск оценщиков.
        /// </summary>
        [DetailView(Name = "Допуск оценщиков", Order = 3, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order = 3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AppAdmission { get; set; }


        /// <summary>
        /// Получает или задает признак Наличие замечаний
        /// </summary>
        [DetailView(Name = "Наличие замечаний", Order = 4, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [DefaultValue(false)]
        [ListView(Order = 4)]
        public bool HaveComment { get; set; }


        /// <summary>
        /// Получает или задает делового партнера (оценщика).
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Оценочная организация", Order = 8, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual Appraiser Appraiser { get; set; }

        
    }
}
