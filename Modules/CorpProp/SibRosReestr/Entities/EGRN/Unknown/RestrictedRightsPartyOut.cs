using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о лице, в пользу которого установлены ограничения права и обременения объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RestrictedRightsPartyOut")]
    public class RestrictedRightsPartyOut
    {

        /// <summary>
        /// Тип лица, в пользу которых установлены ограничения права и обременения объекта недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public Dict Type { get; set; }
        /// <summary>
        /// Лицо, в пользу которого установлены ограничения права и обременения объекта недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("subject")]
        public SubjectRestricted Subject { get; set; }
    }
}
