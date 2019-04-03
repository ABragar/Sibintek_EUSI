using System;
using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandAccountingDt5 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }
        /// <summary>
        /// Получает или задает внутренний ИД ОБУ в БУС ОГ.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string ExternalID { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер 2.
        /// </summary>
        /// <remarks>
        /// Например, инв. № 1C в КИС САП РН.
        /// </remarks>  
        //[ListView(Order = 2)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber2 { get; set; }

        /// <summary>
        /// Получает или задает идентификатор балансодержателя.
        /// </summary> 
        [SystemProperty]
        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает или задает балансодержателя.
        /// </summary>
        //[ListView(Order = 13)]
        [DetailView(Visible = false)]
        public Society Owner { get; set; }

        /// <summary>
        /// Получает или задает номер счета.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Получает или задает ИД класса ОС.
        /// </summary>        
        [DetailView(Visible = false)]
        public int? ClassFixedAssetID { get; set; }

        /// <summary>
        /// Получает или задает класс БУ.
        /// </summary>
        //[ListView(Order = 3)]
        [DetailView(Visible = false)]
        public ClassFixedAsset ClassFixedAsset { get; set; }

        /// <summary>
        ///Получает или задает наименование.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        //[DetailView(Name = "Примечание", Order = 2, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает ИД бизнес-сферы.
        /// </summary>       
        public int? BusinessAreaID { get; set; }

        /// <summary>
        /// Получает или задает бизнес-сферу.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public BusinessArea BusinessArea { get; set; }

        /// <summary>
        /// Получает или задает идентификатор ОГ - пользователя ОБУ.
        /// </summary> 
        [SystemProperty]
        public int? WhoUseID { get; set; }

        /// <summary>
        /// Получает или задает ОГ - пользователя ОБУ.
        /// </summary>  
        [DetailView(Visible = false)]
        public Society WhoUse { get; set; }

        /// <summary>
        /// Получает или задает признак является ли инвентарный объект недвижимым.
        /// </summary>
//        [Historical]
        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        public bool? IsRealEstate { get; set; }
        /// <summary>
        /// Получает или задает материально-ответственное лицо.
        /// </summary>       
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string MOL { get; set; }

        /// <summary>
        /// Получает или задает дату оприходования.
        /// </summary>   
        [DetailView(Visible = false)]
        public DateTime? DateOfReceipt { get; set; }
        /// <summary>
        /// Получает или задает ИД причины поступления.
        /// </summary>       
        public int? ReceiptReasonID { get; set; }

        /// <summary>
        /// Получает или задает причину поступления.
        /// </summary>       
        [DetailView(Visible = false)]
        public ReceiptReason ReceiptReason { get; set; }

        /// <summary>
        /// Получает или задает дату списания.
        /// </summary>     
        [DetailView(Visible = false)]
        public DateTime? LeavingDate { get; set; }
        /// <summary>
        /// Получает или задает ИД причины выбытия.
        /// </summary>       
        public int? LeavingReasonID { get; set; }

        /// <summary>
        /// Получает или задает причину выбытия.
        /// </summary>        
        [DetailView(Visible = false)]
        public LeavingReason LeavingReason { get; set; }
    }
}
