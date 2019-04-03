using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Орган государственной власти, орган местного самоуправления, иной государственный орган
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("GovementEntity")]
    public class GovementEntity
    {

        /// <summary>
        /// Полное наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("full_name")]
        public string Full_name { get; set; }
        /// <summary>
        /// ИНН
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("inn")]
        public string Inn { get; set; }
        /// <summary>
        /// ОГРН
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ogrn")]
        public string Ogrn { get; set; }
    }
}
