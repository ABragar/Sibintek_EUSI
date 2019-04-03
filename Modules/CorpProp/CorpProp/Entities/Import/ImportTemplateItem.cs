using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Import
{
    /// <summary>
    /// Представляет элемент (ячейку) шаблона файла импорта.
    /// </summary>
    [EnableFullTextSearch]
    public class ImportTemplateItem : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ImportTemplateItem.
        /// </summary>
        public ImportTemplateItem(): base()
        {

        }

        /// <summary>
        /// Получает или задает номер строки.
        /// </summary>
        public int? Row { get; set; }

        /// <summary>
        /// Получает или задает номер колонки.
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// Получает или задает значение.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Получает или задает ИД шаблона.
        /// </summary>
        public int? ImportTemplateID { get; set; }

        /// <summary>
        /// Получает или задает шаблон.
        /// </summary>
        public ImportTemplate ImportTemplate { get; set; }
    }
}
