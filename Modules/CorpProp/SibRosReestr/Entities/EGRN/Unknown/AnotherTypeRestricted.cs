using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Тип иного субъекта
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AnotherTypeRestricted")]
    public class AnotherTypeRestricted
    {

        [System.Xml.Serialization.XmlElementAttribute("aparthouse_owners", typeof(AparthouseOwners))]
        [System.Xml.Serialization.XmlElementAttribute("bonds_holders", typeof(BondsHoldersOut))]
        [System.Xml.Serialization.XmlElementAttribute("certificates_holders", typeof(CertificatesHoldersOut))]
        [System.Xml.Serialization.XmlElementAttribute("equity_participants_info", typeof(EquityParticipantsInfo))]
        [System.Xml.Serialization.XmlElementAttribute("investment_unit_owner", typeof(InvestmentUnitOwnerOut))]
        [System.Xml.Serialization.XmlElementAttribute("not_equity_participants_info", typeof(NotEquityParticipantsInfo))]
        [System.Xml.Serialization.XmlElementAttribute("other", typeof(OtherSubject))]
        [System.Xml.Serialization.XmlElementAttribute("partnership", typeof(Partnership))]
        public object Item { get; set; }
    }
}
