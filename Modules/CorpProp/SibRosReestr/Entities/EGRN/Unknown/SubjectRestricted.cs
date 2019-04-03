using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Лицо, в пользу которого установлены ограничения права и обременения объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SubjectRestricted")]
    public class SubjectRestricted
    {

        [System.Xml.Serialization.XmlElementAttribute("another", typeof(AnotherRestricted))]
        [System.Xml.Serialization.XmlElementAttribute("individual", typeof(IndividualOut))]
        [System.Xml.Serialization.XmlElementAttribute("legal_entity", typeof(LegalEntityOut))]
        [System.Xml.Serialization.XmlElementAttribute("public_formation", typeof(PublicFormations))]
        [System.Xml.Serialization.XmlElementAttribute("public_servitude", typeof(PublicServitude))]
        [System.Xml.Serialization.XmlElementAttribute("undefined", typeof(Undefined))]
        public object Item { get; set; }
    }
}
