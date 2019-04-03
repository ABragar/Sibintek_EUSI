using Base;
using Base.Attributes;
using Base.DAL;
using Base.Enums;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.NSI;
using System;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет нематериальный актив.
    /// </summary>
    [EnableFullTextSearch]
    public class IntangibleAsset : Estate
    {
       

        /// <summary>
        /// Получает или задает ИД изображения.
        /// </summary>
        public int? ImageID { get; set; }
        //TODO: убрать изображение в ОИ
        /// <summary>
        /// Получает или задает изображение.
        /// </summary>
        [DetailView(Visible = false)]
        [Image(DefaultImage = DefaultImage.NoImage, Crop = false)]
        //[FullTextSearchProperty]
        ////[ListView]
        public  FileData Image { get; set; }

        /// <summary>
        /// Получате или задает ИД типа НМА.
        /// </summary>
        public int? IntangibleAssetTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип НМА.
        /// </summary>
        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        public  IntangibleAssetType IntangibleAssetType { get; set; }

        public int? IntangibleAssetStatusID { get; set; }

        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        public  IntangibleAssetStatus IntangibleAssetStatus { get; set; }

        /// <summary>
        /// Получает или задает автора.
        /// </summary>        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        
        public String Author { get; set; }


        /// <summary>
        /// Получает или задает правообладателя.
        /// </summary>        
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        // TODO: добавить логику вычисления правообладателей.
        public String RightHolder { get; set; }


        [PropertyDataType(PropertyDataType.Text)]       
        public String RightHolderAddress { get; set; }

        /// <summary>
        /// Получает или задает лицензиата.
        /// </summary>        
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        // TODO: добавить логику вычисления лицензиата.
        public String Licensee { get; set; }

        /// <summary>
        /// номер заявки
        /// </summary>
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RequestNumber { get; set; }
        /// <summary>
        /// срок действия истекает
        /// </summary>
        [DetailView(Visible = false)]
        public DateTime? RightDateTo { get; set; }

        /// <summary>
        /// дата регистрации
        /// </summary>
        [DetailView(Visible = false)]
        public DateTime? RightDateFrom { get; set; }

        /// <summary>
        /// приоритет товарного знака
        /// </summary>
        [DetailView(Visible = false)]
        public DateTime? PrioritySign { get; set; }

        public int? SignTypeID { get; set; }

        [DetailView(Visible = false)]
        public  SignType SignType { get; set; }


        /// <summary>
        /// номер регистрации
        /// </summary>
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightRegNumber { get; set; }

        

        /// <summary>
        /// Инициализирует новый экземпляр класса IntangibleAsset.
        /// </summary>
        public IntangibleAsset()
        {
            
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса IntangibleAsset из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public IntangibleAsset(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {
           
        }
    }
}
