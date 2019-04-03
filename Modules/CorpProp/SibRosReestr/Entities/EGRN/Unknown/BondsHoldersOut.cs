using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Владельцы облигаций
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("BondsHoldersOut")]
    public class BondsHoldersOut
    {

        /// <summary>
        /// Государственный регистрационный номер выпуска облигаций
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("bonds_number")]
        public string Bonds_number { get; set; }
        /// <summary>
        /// Дата государственной регистрации номера выпуска облигаций
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "issue_date")]
        public System.DateTime Issue_date { get; set; }
    }
}
