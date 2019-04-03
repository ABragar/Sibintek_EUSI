using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Export
{

    /// <summary>
    /// Строки шаблона экспорта.
    /// </summary>
    public class ExportTemplateItem : TypeObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса ExportTemplateItem.
        /// </summary>
        public ExportTemplateItem() : base()
        {

        }

        /// <summary>
        /// Получает или задает номер строки.
        /// </summary>
        [ListView("Номер строки")]
        [DetailView("Номер строки")]       
        public int? Row { get; set; }

        /// <summary>
        /// Получает или задает номер колонки.
        /// </summary>
        [ListView("Номер колонки")]
        [DetailView("Номер колонки")]
        [PropertyDataType(PropertyDataType.Text)]
        public int? Column { get; set; }

                

        /// <summary>
        /// Получает или задает признак сопоставления колонки со свойством.
        /// </summary>
        [ListView("Мэппинг свойства по колонке")]
        [DetailView("Мэппинг свойства по колонке")]
        [DefaultValue(false)]
        public bool IsColumnMap { get; set; }

        /// <summary>
        /// Получает или задает дефолтное значение.
        /// </summary>
        [ListView("Значение")]
        [DetailView("Значение")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Value { get; set; }

        /// <summary>
        /// Получает или задает наименование типа.
        /// </summary>
        [ListView("Тип")]
        [DetailView("Тип")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeName { get; set; }

        /// <summary>
        /// Получает или задает ИД шаблона.
        /// </summary>
        [SystemProperty]
        public int? ExportTemplateID { get; set; }

        /// <summary>
        /// Получает или задает шаблон.
        /// </summary>
        [ListView("Шаблон")]
        [DetailView("Шаблон")]
        //[PropertyDataType(PropertyDataType.Text)]
        public ExportTemplate ExportTemplate { get; set; }

    }
}
