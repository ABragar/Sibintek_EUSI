using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Даты государственной регистрации (постановки/снятия с учета (регистрации))
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RecordInfo")]
    public class RecordInfo
    {

        /// <summary>
        /// Дата постановки на учет/ регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("registration_date")]
        public System.DateTime Registration_date { get; set; }
        /// <summary>
        /// Дата снятия с учета/регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cancel_date")]
        public System.DateTime Cancel_date { get; set; }
    }

}
