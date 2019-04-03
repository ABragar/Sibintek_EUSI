using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;
using CorpProp.Helpers;
using System;
using System.ComponentModel;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandGeneralData : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        /// <summary>
        /// Получает или задает наименование по данным Росреестра. 
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Наименование по данным ЕГРН", TabName = EstateTabs.GeneralData, Visible = true, Required = true, Order = 1)]
        public string NameByRight { get; set; }

        /// <summary>
        /// Получает признак, является ли объект непрофильным или неэффективным.
        /// </summary>
        /// <remarks>
        /// "Да", если существует ННА, связанный с данным инвентарным объектом, 
        /// и этот ННА актуален (в соотв. с его статусом). "Нет" по умолчанию.
        /// </remarks>
        // TODO: добавить логику вычисления признака ННА.       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Является ННА", TabName = EstateTabs.GeneralData, Visible = false, Order = 2)]
        [DefaultValue(false)]
        public bool? IsNonCoreAsset { get; set; } = false;

        /// <summary>
        /// Получает или задает кадастровый номер.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Name = "Кадастровый номер", TabName = EstateTabs.GeneralData, Visible = false, Required = true, Order = 3)]
        [ListView(Name = "Кадастровый номер")]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает номер кадастрового квартала.
        /// </summary>
//        [Historical]
        [DetailView(Name = "Номер кадастрового квартала", TabName = EstateTabs.GeneralData, Visible = false, Order = 4)]
        [ListView(Name = "Номер кадастрового квартала")]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string BlocksNumber { get; set; }

        [DetailView(Name = "Адрес (местоположение)", TabName = EstateTabs.GeneralData, Visible = false, Order = 5)]
        [ListView(Name = "Адрес (местоположение)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Address { get; set; }

        /// <summary>
        /// Получает или задает особые отметки.
        /// </summary>
//        [Historical]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Name = "Особые отметки", TabName = EstateTabs.GeneralData, Visible = true, Order = 6)]
        public string SpecialMarks { get; set; }

        /// <summary>
        /// Получает или задает дату постановки на учет/ регистрации.
        /// </summary>
        [DetailView(Name = "Дата постановки на учет/регистрации", TabName = EstateTabs.GeneralData, Visible = true, Order = 7)]
        [ListView(Name = "Дата постановки на учет/регистрации")]
        public DateTime? RegDate { get; set; }

        /// <summary>
        /// Получает или задает дату снятия с учета/регистрации.
        /// </summary>
        [DetailView(Name = "Дата снятия с учета/регистрации", TabName = EstateTabs.GeneralData, Visible = true, Order = 8)]
        [ListView(Name = "Дата снятия с учета/регистрации")]
        public DateTime? DeRegDate { get; set; }

        /// <summary>
        /// Получает или задает сведения об изъятии.
        /// </summary>
        [DetailView(Name = "Сведения об изъятии", TabName = EstateTabs.GeneralData, Visible = true, Order = 9,
            Description = "Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд")]
        [PropertyDataType(PropertyDataType.Text)]
//        [Historical]
        public string Confiscation { get; set; }

        /// <summary>
        /// Дата постановки на кадастровый учет по документу
        /// </summary>
        [DetailView(Name = "Дата постановки на кад.учет по документу", TabName = EstateTabs.GeneralData, Visible = true, Order = 10)]
        public DateTime? RegDateByDoc { get; set; }

        [DetailView(Name = "Номер гос. учета в лесном реестре", TabName = EstateTabs.GeneralData, Visible = true, Order = 11)]
        [PropertyDataType(PropertyDataType.Text)]
        public string GroundCadastralNumber { get; set; }

        [DetailView(Name = "Лесной участок", TabName = EstateTabs.GeneralData, Visible = true, Order = 12)]
        public bool? Wood { get; set; } = false;

        [DetailView(Name = "Налог с кад.стоимости", TabName = EstateTabs.GeneralData, Visible = true, Order = 13)]
        [ListView(Hidden = true)]
        [DefaultValue(false)]
        public bool? IsTaxCadastral { get; set; }
    }
}
