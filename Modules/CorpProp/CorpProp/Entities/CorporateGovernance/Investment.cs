using SubjectObject = CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Helpers;
using CorpProp.Entities.Base;
using CorpProp.Attributes;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет сведения об Акциях / долях.
    /// </summary>
    [EnableFullTextSearch]
    public class Investment : TypeObject
    {
        /// <summary>
        /// Получает или задает ИД ЕУП.
        /// </summary>
        [DetailView(Name = "ИД ЕУП", Order = 0, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDEUP { get; set; }

        /// <summary>
        /// Получает или задает Тип.
        /// </summary>
        [DetailView(Name = "Тип", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [FullTextSearchProperty]
        [ListView]
        public virtual InvestmentType InvestmentType { get; set; }

        /// <summary>
        /// Получает или задает количество.
        /// </summary>
        [DetailView(Name = "Количество", Order = 2, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [ListView(Hidden = true)]
        [PropertyDataType("Sib_Decimal2")]
        [DecimalPrecision(28, 4)]
        public decimal NumberShares { get; set; }

        /// <summary>
        /// Получает или задает номинальную стоимость.
        /// </summary>
        [DetailView(Name = "Номинальная стоимость", Order = 3, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [SibDisplayFormat("c")]
        [DecimalPrecision(31, 8)]
        public decimal NominalShares { get; set; }

        /// <summary>
        /// Получает или задает стоимость за ед..
        /// </summary>
        [DetailView(Name = "Стоимость за единицу", Order = 4, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [SibDisplayFormat("c")]
        [DecimalPrecision(31, 8)]
        public decimal SharePrice { get; set; }

        /// <summary>
        /// Получает или задает дату начала.
        /// </summary>
        [DetailView(Name = "Дата начала", Order = 5, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания.
        /// </summary>
        [DetailView(Name = "Дата окончания", Order = 6, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Получает или задает дату выпуска.
        /// </summary>
        [DetailView(Name = "Дата выпуска", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateRelease { get; set; }

        /// <summary>
        /// Получает или задает регистрационный номер выпуска.
        /// </summary>
        [DetailView(Name = "Регистрационный номер выпуска", Order = 8, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [ListView]
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Получает или задает ИД эмитента.
        /// </summary>
        public int? SocietyIssuerID { get; set; }

        /// <summary>
        /// Получает или задает эмитента.
        /// </summary>
        [DetailView(Name = "Эмитент", Order = 9, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [FullTextSearchProperty]
        [ListView]
        public virtual SubjectObject.Society SocietyIssuer { get; set; }
    }
}
