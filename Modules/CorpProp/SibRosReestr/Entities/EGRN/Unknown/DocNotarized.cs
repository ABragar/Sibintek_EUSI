using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Нотариальное удостоверение документа
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DocNotarized")]
    public class DocNotarized
    {
        public DocNotarized() { }
        /// <summary>
        /// Дата нотариального удостоверения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "notarize_date")]
        public System.DateTime Notarize_date { get; set; }
        /// <summary>
        /// Фамилия и инициалы нотариуса
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("notary_name")]
        public string Notary_name { get; set; }
        /// <summary>
        /// Номер в реестре регистрации нотариальных действий
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("notary_action_num")]
        public string Notary_action_num { get; set; }
    }
}
