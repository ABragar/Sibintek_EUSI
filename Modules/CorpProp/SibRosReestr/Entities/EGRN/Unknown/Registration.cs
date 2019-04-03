using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Номер и дата регистрации
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Registration")]
    public class Registration
    {

        /// <summary>
        /// Номер регистрации/реестровой записи
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("restriction_encumbrance_number")]
        public RestrictRecordNumber Restriction_encumbrance_number { get; set; }
        /// <summary>
        /// Дата регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "registration_date")]
        public System.DateTime Registration_date { get; set; }
    }
}
