using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Участник договора инвестиционного товарищества
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("PartnershipParticipant")]
    public class PartnershipParticipant
    {

        /// <summary>
        /// Юридическое лицо (российское, иностранное  юридическое лицо)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("legal_entity")]
        public LegalEntityUL Legal_entity { get; set; }
        /// <summary>
        /// Контактная информация
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("contacts")]
        public Contacts Contacts { get; set; }
    }
}
