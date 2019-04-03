using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Участники долевого строительства по договорам участия в долевом строительстве
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("EquityParticipantsInfo")]
    public class EquityParticipantsInfo
    {

        /// <summary>
        /// Участники долевого строительства
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("equity_participants")]
        public string Equity_participants { get; set; }
    }
}
