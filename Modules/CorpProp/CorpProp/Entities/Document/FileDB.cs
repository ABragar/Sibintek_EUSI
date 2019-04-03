using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Document
{
    /// <summary>
    /// Представляет файл для хранения в БД.
    /// </summary>
    [EnableFullTextSearch]
    public class FileDB : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса FileDB.
        /// </summary>
        public FileDB() : base()
        {

        }

        [ListView("Наименование")]        
        [DetailView("Наименование", Required = true)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        [ListView("Расширение")]
        [DetailView("Расширение")]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Ext { get; set; }

        
        
        [Column]
        public byte[] Content { get; set; }
    }
}
