using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Гражданство
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Citizenship")]
    public class Citizenship
    {

        [System.Xml.Serialization.XmlElementAttribute("no_citizenship_person", typeof(NoCitizenshipPerson))]
        [System.Xml.Serialization.XmlElementAttribute("person_citizenship_country", typeof(PersonCitizenshipCountry))]
        public object Item { get; set; }
    }
}
