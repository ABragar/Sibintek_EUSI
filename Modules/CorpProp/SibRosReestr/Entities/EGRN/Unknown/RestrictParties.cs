using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о лицах, в пользу которых установлены ограничения права и обременения объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RestrictParties")]
    public class RestrictParties
    {
        public RestrictParties()
        {
            Restricted_rights_parties = new List<RestrictedRightsPartyOut>();
        }

        /// <summary>
        /// Сведения о лицах, в пользу которых установлены ограничения права и обременения объекта недвижимости
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("restricted_rights_party", IsNullable = false, ElementName = "restricted_rights_parties")]
        [System.Xml.Serialization.XmlArray("restricted_rights_parties")]
        [System.Xml.Serialization.XmlArrayItem("restricted_rights_party", Type = typeof(RestrictedRightsPartyOut), IsNullable = false)]
        public List<RestrictedRightsPartyOut> Restricted_rights_parties { get; set; }
    }
}
