using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Адрес (фиас)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AddressMain")]
    public class AddressMain
    {

        /// <summary>
        /// Адрес (по справочнику ФИАС)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address_fias")]
        public Address Address_fias { get; set; }
        /// <summary>
        /// Неформализованное описание
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("note")]
        public string Note { get; set; }
        /// <summary>
        /// Адрес в соответствии с ФИАС (Текст)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("readable_address")]
        public string Readable_address { get; set; }
    }
}
