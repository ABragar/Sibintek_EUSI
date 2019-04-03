using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Attributes
{
    
    /// <summary>
    /// Представляет атрибут, указывающий отображаемый формат и/или маску для виджета Kendo при отображении свойства в таблицах или в редакторах.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class SibDisplayFormatAttribute : Attribute
    {
        public string DisplayFormat { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса.
        /// </summary>
        /// <param name="format"></param>
        public SibDisplayFormatAttribute(string format)
        {
            DisplayFormat = format;
        }
    }
}
