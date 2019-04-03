using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Российское юридическое лицо
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ResidentOut")]
    public class ResidentOut
    {

        /// <summary>
        /// Организационно-правовая форма
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("incorporation_form")]
        public Dict Incorporation_form { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public string Name { get; set; }
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
