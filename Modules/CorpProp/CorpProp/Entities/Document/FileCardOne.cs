using Base;
using Base.Attributes;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Document
{
    /// <summary>
    /// Представляет одиночный документ.
    /// </summary>
    public class FileCardOne : FileCard
    {
        
        /// <summary>
        /// Инициализирует новый экземпляр класса FileCard.
        /// </summary>
        public FileCardOne():base()
        {
        }
      

        /// <summary>
        /// Получает или задает ИД документа.
        /// </summary>
        public int? FileDataID { get; set; }

        /// <summary>
        /// Получает или задает документ.
        /// </summary>
        /// <remarks>
        /// Интерфейс получения / сохранения связанного файла из подсистемы "Хранилище документов".
        /// </remarks>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Файл", Order = 0, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.File, Params = "Select=false;SyncToField=Name")]
        public virtual FileData FileData { get; set; }

        /// <summary>
        /// Получает или задает ИД карточки одиночного документа.
        /// </summary>
        public int? PreviousVersionID { get; set; }

        /// <summary>
        /// Получает или задает карточку одиночного документа.
        /// </summary>
        /// <remark>
        /// Предыдущая версия.
        /// </remark>       
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView(Name = "Предыдущая версия", Order = 1, TabName = CaptionHelper.DefaultTabName)]
        public virtual FileCardOne PreviousVersion { get; set; }

       
    }
}
