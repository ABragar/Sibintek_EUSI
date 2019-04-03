using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Инвестиционное товарищество
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Partnership")]
    public class Partnership
    {
        public Partnership()
        {
            Partnership_participants = new List<PartnershipParticipant>();
        }
        /// <summary>
        /// Участники договора инвестиционного товарищества
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("partnership_participant", IsNullable = false, ElementName = "partnership_participants")]
        [System.Xml.Serialization.XmlArray("partnership_participants")]
        [System.Xml.Serialization.XmlArrayItem("partnership_participant", Type = typeof(PartnershipParticipant), IsNullable = false)]
        public List<PartnershipParticipant> Partnership_participants { get; set; }
    }
}
