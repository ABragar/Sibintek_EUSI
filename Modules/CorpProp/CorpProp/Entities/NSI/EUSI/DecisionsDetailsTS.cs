using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using Base.Attributes;
using CorpProp.Helpers;
using System;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник реквизитов решений органов МО ТС.
    /// </summary>
    [EnableFullTextSearch]
    public class DecisionsDetailsTS : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public DecisionsDetailsTS() : base()
        {

        }

        /// <summary>
        /// Получает или задает Номер документа.
        /// </summary>
        [ListView("Номер документа")]
        [DetailView("Номер документа", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string Number { get; set; }

        /// <summary>
        /// Получает или задает дату документа.
        /// </summary>
        [ListView("Дата документа")]
        [DetailView("Дата документа", TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Получает или задает ссылку на электронную копию.
        /// </summary>
        [ListView("Ссылка на электронную копию")]
        [DetailView("Ссылка на электронную копию", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Url)]
        [FullTextSearchProperty]
        public string Link { get; set; }
    }
}
