using System;
using Base.Attributes;
using Base.UI.ViewModal;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;
using CorpProp.Helpers;

namespace EUSI.Entities.NSI
{
    /// <summary>
    /// Справочник объектов недвижимого имущества, налоговая база в отношении которых определяется как их кадастровая стоимость
    /// </summary>
    public class PropertyListTaxBaseCadastral : DictObject
    {
        /// <summary>
        /// Кадастровый номер здания (строения, сооружения)
        /// </summary>
        [DetailView(Name = "Кадастровый номер здания (строения, сооружения)", TabName = CaptionHelper.DefaultTabName, Order = 1)]
        [ListView(Name = "Кадастровый номер здания (строения, сооружения)", Order = 1)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Кадастровый номер помещения
        /// </summary>
        [DetailView(Name = "Кадастровый номер помещения", TabName = CaptionHelper.DefaultTabName, Order = 2)]
        [ListView(Name = "Кадастровый номер помещения", Order = 2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RoomCadastralNumber { get; set; }

        /// <summary>
        /// Условный номер единого недвижимого комплекса
        /// </summary>
        [DetailView(Name = "Условный номер единого недвижимого комплекса", TabName = CaptionHelper.DefaultTabName, Order = 3)]
        [ListView(Name = "Условный номер единого недвижимого комплекса", Order = 3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ConditionalNumber { get; set; }

        /// <summary>
        /// Номер и дата документа, утвердившего перечень
        /// </summary>
        [DetailView(Name = "Номер документа, утвердившего перечень", TabName = CaptionHelper.DefaultTabName, Order = 6)]
        [ListView(Name = "Номер документа, утвердившего перечень", Order = 6)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ApprovingDocNumber { get; set; }

        /// <summary>
        /// Номер и дата документа, утвердившего перечень
        /// </summary>
        [DetailView(Name = "Дата документа, утвердившего перечень", TabName = CaptionHelper.DefaultTabName, Order = 7)]
        [ListView(Name = "Дата документа, утвердившего перечень", Order = 7)]
        public DateTime? ApprovingDocDate { get; set; }

        /// <summary>
        /// Получает или задает ИД региона.
        /// </summary>
        [SystemProperty]
        public int? SibRegionID { get; set; }

        /// <summary>
        /// Получает или задает Федеральный округ.
        /// </summary>
        [DetailView(Name = "Регион", TabName = CaptionHelper.DefaultTabName, Order = 8)]
        [ListView(Name = "Регион", Order = 8)]
        public SibRegion SibRegion { get; set; }

        /// <summary>
        /// Адрес объекта
        /// </summary>
        [DetailView(Name = "Адрес объекта", TabName = CaptionHelper.DefaultTabName, Order = 9)]
        [ListView(Name = "Адрес объекта", Order = 9)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Address { get; set; }

        /// <summary>
        /// Обновление атрибутов ОИ
        /// </summary>
        [DetailView(Name = "Обновление налоговой базы в ОИ", TabName = CaptionHelper.DefaultTabName, Order = 10)]
        [ListView(Name = "Обновление налоговой базы в ОИ", Order = 10)]
        public bool? IsCadastralEstateUpdated { get; set; }

        /// <summary>
        /// Дата обновления атрибутов ОИ
        /// </summary>
        [DetailView(Name = "Дата обновления налоговой базы в ОИ", TabName = CaptionHelper.DefaultTabName, Order = 11)]
        [ListView(Name = "Дата обновления налоговой базы в ОИ", Order = 11)]
        public DateTime? CadastralEstateUpdatedDate { get; set; }

        
    }
}