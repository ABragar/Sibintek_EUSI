using Base;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Enums;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет право на НМА.
    /// </summary>    
    [EnableFullTextSearch]
    public class IntangibleAssetRight : TypeObject
    {     

        private static readonly CompiledExpression<IntangibleAssetRight, string> _IntangibleAssetType =
         DefaultTranslationOf<IntangibleAssetRight>.Property(x => x.IntangibleAssetTypeName)
            .Is(x => (x.IntangibleAsset != null && x.IntangibleAsset.IntangibleAssetType != null) ? 
            x.IntangibleAsset.IntangibleAssetType.Name : "");

        /// <summary>
        /// Инициализирует новый экземпляр класса IntangibleAssetRight.
        /// </summary>
        public IntangibleAssetRight()
        {

        }

        /// <summary>
        /// Получает или задает заголовок.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Visible = false, Name = "Вид, номер, дата")]
        public string Title { get; set; }
        

        /// <summary>
        /// Получает или задает ИД типа права.
        /// </summary>
        public int? IntangibleAssetRightTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип права на НМА.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Тип права", TabName = CaptionHelper.DefaultTabName, Required = true, Order = 1)]
        public IntangibleAssetRightType IntangibleAssetRightType { get; set; }

        /// <summary>
        /// Получает тип НМА.
        /// </summary>        
        [ListView(Visible = false)]
        [DetailView(Name = "Тип НМА", TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String IntangibleAssetTypeName => _IntangibleAssetType.Evaluate(this);

        /// <summary>
        /// Получате или задает ИД НМА.
        /// </summary>
        public int? IntangibleAssetID { get; set; }

        /// <summary>
        /// Получает или задает НМА.
        /// </summary>
        //[FullTextSearchProperty]
        ////[ListView]
        [DetailView(Name = "НМА", TabName = CaptionHelper.DefaultTabName, Required = true, Order = 10)]
        public virtual IntangibleAsset IntangibleAsset { get; set; }


        /// <summary>
        /// Получает или задает автора.
        /// </summary>        
        [ListView(Hidden = true)]
        [DetailView(Name = "Автор", TabName = CaptionHelper.DefaultTabName, Order =3)]
        [PropertyDataType(PropertyDataType.Text)]
        
        public String Author { get; set; }


        /// <summary>
        /// Получает или задает правообладателя.
        /// </summary>        
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Правообладатель", TabName = CaptionHelper.DefaultTabName, Required = true, Order =4)]
        [PropertyDataType(PropertyDataType.Text)]      
        public String RightHolder { get; set; }

        /// <summary>
        /// Получает или задает номер регистрации.
        /// </summary>
        [ListView(Hidden = true)]
        [FullTextSearchProperty]
        [DetailView(Name = "Номер регистрации", Order = 2, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegNumber { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Приоритет товарного знака", Order = 6, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? PrioritySign { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата регистрации", Order = 7, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Срок действия истекает", Order = 8, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Получает или задает ИД изображения.
        /// </summary>
        public int? ImageID { get; set; }

        /// <summary>
        /// Получает или задает изображение.
        /// </summary>
        [DetailView("Изображение", TabName = CaptionHelper.DefaultTabName, Order = 0)]       
        [FullTextSearchProperty]
        [ListView]
        [Image(DefaultImage = DefaultImage.NoImage, Crop = false)]
        public virtual FileData Image { get; set; }


        /// <summary>
        /// Получает или задает номер заявки.
        /// </summary>        
        [ListView(Hidden = true)]
        [DetailView(Name = "Заявка №", TabName = CaptionHelper.DefaultTabName, Order =5)]
        [PropertyDataType(PropertyDataType.Text)]
        
        public String Number { get; set; }

    }
}
