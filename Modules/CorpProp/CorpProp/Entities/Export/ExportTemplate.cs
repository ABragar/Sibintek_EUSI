using Base;
using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CorpProp.Entities.Export
{

    /// <summary>
    /// Представляет шаблон экспорта.
    /// </summary>
    public class ExportTemplate : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ExportTemplate.
        /// </summary>
        public ExportTemplate(): base()
        {
            Items = new List<ExportTemplateItem>();
        }

        /// <summary>
        /// Получает или задает наименование шаблона.
        /// </summary>
        [ListView("Наименование шаблона")]
        [DetailView("Наименование шаблона", Order =1, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }


        /// <summary>
        /// Получает или задает Системный код.
        /// </summary>
        [ListView("Системный код")]
        [DetailView("Системный код", Order = 2, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Code { get; set; }


        /// <summary>
        /// Получает или задает мнемонику объекта.
        /// </summary>            
        [ListView("Тип объекта")]
        [DetailView("Тип объекта", Order = 3)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Получает или задает наименование экспортируемого файла.
        /// </summary>
        [ListView("Наименование файла")]
        [DetailView("Наименование файла", Order = 4)]
        [PropertyDataType(PropertyDataType.Text)]
        public string FileName { get; set; }


        /// <summary>
        /// Получает или задает номер строки.
        /// </summary>
        [ListView("Номер строки начала данных")]
        [DetailView("Номер строки начала данных", Order = 5)]
        public int? StartRow { get; set; }

        /// <summary>
        /// Получает или задает номер колонки.
        /// </summary>
        [ListView("Номер колонки начала данных")]
        [DetailView("Номер колонки начала данных", Order = 6)]
        [PropertyDataType(PropertyDataType.Text)]
        public int? StartColumn { get; set; }

       

        /// <summary>
        /// Получает или задает ИД файла.
        /// </summary>
        [SystemProperty]
        public int? FileID { get; set; }


        /// <summary>
        /// Получает или задает файл.
        /// </summary>
        [ListView("Файл")]
        [DetailView("Файл", Order = 7)]
        [ForeignKey("FileID")]       
        public virtual FileDB File { get; set; }

        /// <summary>
        /// Получает или задает кол-ию строк шаблона.
        /// </summary>
        [ListView("Строки")]
        [DetailView("Строки", Order = 9)]
        [XmlArray("Items")]
        [XmlArrayItem("ExportTemplateItem")]
        public virtual List<ExportTemplateItem> Items { get; set; }
        
        public Dictionary<int, string> GetColumnsMap()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            var list = Items.Where(w => w.Column != null && w.IsColumnMap).DefaultIfEmpty().OrderBy(ord => ord.Column);

            foreach (var item in list)
            {
                if (!dict.ContainsKey(item.Column.Value))
                    dict.Add(item.Column.Value, item.Value);
            }

            return dict;
        }

    }
}
