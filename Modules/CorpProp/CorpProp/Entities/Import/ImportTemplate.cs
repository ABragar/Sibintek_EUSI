using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Document;

namespace CorpProp.Entities.Import
{
    /// <summary>
    /// Представляет шаблон файла импорта Excel.
    /// </summary>
    [EnableFullTextSearch]
    public class ImportTemplate : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ImportTemplate.
        /// </summary>
        public ImportTemplate() : base()
        {
        }

        /// <summary>
        /// Получает или задает наименование.
        /// </summary>            
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }


        /// <summary>
        /// Получает или задает мнемонику импортируемого объекта.
        /// </summary>            
        [PropertyDataType(PropertyDataType.ExtraId)]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Получает или задает версию.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string Version { get; set; }

       /// <summary>
       /// Получает или задает активность.
       /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Получает или задает формат имени файла.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string FileNameFormat { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает номер строки начала данных.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public int? RowData { get; set; }

        /// <summary>
        /// Получает или задает номер колонки начала данных.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public int? ColumnData { get; set; }

        /// <summary>
        /// Получает или задает номер строки историчности.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public int? RowHistory { get; set; }

        /// <summary>
        /// Получает или задает номер строки системных свойств.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public int? RowFiledSystem { get; set; }

        /// <summary>
        /// Получает или задает номер строки наименований свойств.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public int? RowFiledName { get; set; }

        /// <summary>
        /// Получает или задает номер строки обязательности.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        public int? RowRequired { get; set; }

        /// <summary>
        /// Получает или задает ИД файла.
        /// </summary>
        public int? FileCardID { get; set; }

        /// <summary>
        /// Получает или задает файл для шаблона.
        /// </summary>
        public FileCard FileCard { get; set; }
    }
}
