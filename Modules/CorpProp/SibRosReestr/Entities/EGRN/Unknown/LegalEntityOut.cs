using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Юридическое лицо, орган власти
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LegalEntityOut")]
    public class LegalEntityOut
    {

        /// <summary>
        /// Тип юридического лица
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public Dict Type { get; set; }
        /// <summary>
        /// Юридическое лицо, орган власти
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("entity")]
        public EntityOut Entity { get; set; }
        /// <summary>
        /// Контактная информация
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("contacts")]
        public Contacts Contacts { get; set; }
    }
}
