using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Document;
using CorpProp.Entities.Law;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CorpProp.Entities.ProjectActivity;

namespace CorpProp.Entities.DocumentFlow
{
    /// <summary>
    /// Регистрационная карточка документа
    /// </summary>
    [EnableFullTextSearch]
    public class Doc : TypeObject
    {
       
        /// <summary>
        /// Инициализирует новый экземпляр класса Doc.
        /// </summary>
        public Doc()
        {            
           
            
        }
       

        /// <summary>
        /// Получает или задает ИД источника информации.
        /// </summary>
        public int? SourseInformationID { get; set; }

        /// <summary>
        /// Получает или задает источник информации.
        /// </summary>       
        [FullTextSearchProperty]
        [DetailView(Name = "Источник информации", Order = 0, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SourceInformation SourseInformation { get; set; }

        /// <summary>
        /// Получает или задает ID во внешней системе.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView(Name = "ID во внешней системе", Order = 1, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExternalSystemIdentifier { get; set; }

        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Order = 2, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает номер.
        /// </summary>
        [ListView (Order =0)]
        [DetailView(Name = "Номер", Order = 1, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number { get; set; }

        /// <summary>
        /// Получает или задает полный номер.
        /// </summary>
        [DetailView(Name = "Полный номер", Order = 4, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string FullNumber { get; set; }

        /// <summary>
        /// Получает или задает дату документа.
        /// </summary>
        [DetailView(Name = "Дата документа", Order = 5, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DateDoc { get; set; }

        /// <summary>
        /// Получает или задает дату регистрации.
        /// </summary>
        [DetailView(Name = "Дата регистрации", Order = 6, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateRegistration { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView(Name = "Примечание", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает ИД вида документа.
        /// </summary>
        public int? DocKindID { get; set; }

        /// <summary>
        /// Получает или задает вид документа.
        /// </summary>        
        [DetailView(Name = "Вид", Order = 8, TabName = CaptionHelper.DefaultTabName)]
        public virtual DocKind DocKind { get; set; }

        /// <summary>
        /// Получает или задает ИД типа документа.
        /// </summary>
        public int? DocTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип документа.
        /// </summary>       
        [DetailView(Name = "Тип", Order = 9, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual DocType DocType { get; set; }


        /// <summary>
        /// Получает или задает идентификатор вышестоящего документа.
        /// </summary>
        public int? DocParentID { get; set; }

        /// <summary>
        /// Получает или задает вышестоящий документ.
        /// </summary>
        [DetailView(Name = "Вышестоящий документ", Order = 10, TabName = CaptionHelper.DefaultTabName)]
        public virtual Doc DocParent { get; set; }

       
    }    
}
