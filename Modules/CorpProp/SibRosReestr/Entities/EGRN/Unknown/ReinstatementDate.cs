using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Восстановление права на основании судебного акта
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ReinstatementDate")]
    public class ReinstatementDate
    {

        /// <summary>
        /// Дата ранее произведенной государственной регистрации права
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("prev_registration_date")]
        public System.DateTime Prev_registration_date { get; set; }
    }
}
