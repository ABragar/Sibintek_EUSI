using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о правообладателе
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RightHolderOut")]
    public class RightHolderOut
    {

        [System.Xml.Serialization.XmlElementAttribute("another", typeof(AnotherRight))]
        [System.Xml.Serialization.XmlElementAttribute("individual", typeof(IndividualOut))]
        [System.Xml.Serialization.XmlElementAttribute("legal_entity", typeof(LegalEntityOut))]
        [System.Xml.Serialization.XmlElementAttribute("public_formation", typeof(PublicFormations))]
        public object Item { get; set; }
    }
}
