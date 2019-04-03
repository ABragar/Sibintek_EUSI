
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Дата государственной регистрации
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RegistrationDate")]
    public class RegistrationDate 
    {
        public RegistrationDate() { }
        /// <summary>
        /// Дата регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("registration_date")]
        public System.DateTime Registration_date { get; set; }
    }
}
