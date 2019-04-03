
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Общие сведения о правах на бесхозяйное имущество (Номер регистрации и наименование органа)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("OwnerlessRight")]
    public class OwnerlessRight 
    {
        public OwnerlessRight() { }
        /// <summary>
        /// Номер регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ownerless_right_number")]
        public string Ownerless_right_number { get; set; }
        /// <summary>
        /// Наименование органа местного самоуправления (органа государственной власти - для городов федерального значения Москвы, Санкт-Петербурга, Севастополя), представившего заявление о постановке на учет данного объекта недвижимости в качестве бесхозяйного
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("authority_name")]
        public string Authority_name { get; set; }
    }
}
