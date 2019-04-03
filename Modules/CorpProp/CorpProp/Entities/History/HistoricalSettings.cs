using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.History
{
    /// <summary>
    /// Настройка исторических свойст объектов Системы.
    /// </summary>
    public class HistoricalSettings : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса HistoricalSettings.
        /// </summary>
        public HistoricalSettings():base()
        {

        }
        /// <summary>
        /// Получае или задает наименование типа, по которому будет вестись историчность.
        /// </summary>
        /// <remarks>
        /// Наименование без указания пространства имен.
        /// </remarks>
        [ListView]
        [DetailView("Тип объекта")]
        [PropertyDataType("ObjectTypeName")]
        public string TypeName { get; set; }

        /// <summary>
        /// Получает или задает наименования свойств, разделенных ';'.
        /// </summary>
        /// <remarks>
        /// При изменении этих свойств будет создана историчная запись.
        /// </remarks>
        [ListView]
        [DetailView("Свойства")]       
        public string Propertys { get; set; }

        
    }

  
}
