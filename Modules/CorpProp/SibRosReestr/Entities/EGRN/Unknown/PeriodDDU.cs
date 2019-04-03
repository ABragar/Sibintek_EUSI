using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Срок для ипотеки земельного участка, на котором осуществляется строительство многоквартирного дома и (или) иного объекта недвижимого имущества, возникшей на основании закона
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("PeriodDDU")]
    public class PeriodDDU
    {

        /// <summary>
        /// Дата регистрации первого ДДУ
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "first_ddu_date")]
        public System.DateTime First_ddu_date { get; set; }
        /// <summary>
        /// Срок передачи застройщиком объекта
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("transfer_deadline")]
        public string Transfer_deadline { get; set; }
    }
}
