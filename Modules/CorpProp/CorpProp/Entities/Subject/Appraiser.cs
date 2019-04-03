using Base.Attributes;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SubjectObject = CorpProp.Entities.Subject;

namespace CorpProp.Entities.Subject
{
    /// <summary>
    /// Представляет оценщика.
    /// </summary>
    public class Appraiser : Subject
    {
        #region Constructor
        /// <summary>
        /// Инициализирует новый экземпляр класса Subject.
        /// </summary>
        public Appraiser()
        {
        }
        #endregion
        /// <summary>
        /// Получает или задает дату заполнения анкеты.
        /// </summary>
        [DetailView(Name = "Дата заполнения анкеты", Order = 2, TabName = CaptionHelper.AppraiserTabName)]
        public DateTime? DateFilling { get; set; }

        /// <summary>
        /// Получает или задает среднесписочную численность персонала за год.
        /// </summary>
        [DetailView(Name = "Среднесписочная численность персонала за год", Order = 3, TabName = CaptionHelper.AppraiserTabName)]
        public int? AverageHeadcountForYear { get; set; }

        /// <summary>
        /// Получает или задает кол-во оценщиков с ЕКЭ.
        /// </summary>
        [DetailView(Name = "Кол-во оценщиков с ЕКЭ", Order = 4, TabName = CaptionHelper.AppraiserTabName)]
        public int? CountAppraisersWithConfirmation { get; set; }

        /// <summary>
        /// Получает или задает количество сертиф.оценщиков.
        /// </summary>
        [DetailView(Name = "Количество сертиф.оценщиков", Order = 5, TabName = CaptionHelper.AppraiserTabName)]
        public int? CountAppraisersWithCertificate { get; set; }

        /// <summary>
        /// Получает или задает лимит по договору страхования.
        /// </summary>
        [DetailView(Name = "Лимит по договору страхования", Order = 6, TabName = CaptionHelper.AppraiserTabName)]
        public decimal? LimitInsuranceContract { get; set; }

        /// <summary>
        /// Получает или задает ИД валюты.
        /// </summary>
        public int? CurrencyID { get; set; }

        /// <summary>
        /// Получает или задает валюту.
        /// </summary>        
        [DetailView(Name = "Валюта", Order = 7, TabName = CaptionHelper.AppraiserTabName)]
        public Currency Currency { get; set; }

        /// <summary>
        /// Получает или задает срок существования организации.
        /// </summary>
        [DetailView(Name = "Лет существования организации", Order = 8, TabName = CaptionHelper.AppraiserTabName)]
        public int? YearsCompany { get; set; }

        /// <summary>
        /// Получает или задает количество оценок активов стоимостью более 1 млрд.руб.
        /// </summary>
        [DetailView(Name = "Оценок активов стоимостью более 1 млрд.руб.", Order = 9, TabName = CaptionHelper.AppraiserTabName)]
        public int? CountEstimatesLargeAssets { get; set; }

        /// <summary>
        /// Получает или задает признак наличия ISO9001
        /// </summary>
        [DetailView(Name = "Наличие ISO9001", Order = 10, TabName = CaptionHelper.AppraiserTabName)]
        [DefaultValue(false)]
        public bool ISO9001 { get; set; }
    }
}
