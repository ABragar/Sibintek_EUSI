using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Срок, на который установлено ограничение права и обременение объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("PeriodInfoType")]
    public class PeriodInfoType
    {

        /// <summary>
        /// Дата начала действия
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "start_date")]
        public System.DateTime Start_date { get; set; }
        /// <summary>
        /// Дата прекращения действия
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "end_date")]
        public System.DateTime End_date { get; set; }
        /// <summary>
        /// Срок действия ограничения/обременения (Продолжительность)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("deal_validity_time")]
        public string Deal_validity_time { get; set; }
    }
}
