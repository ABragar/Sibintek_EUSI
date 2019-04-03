using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Срок, на который установлено ограничение прав и обременение объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("PeriodType")]
    public class PeriodType
    {

        /// <summary>
        /// Срок, на который установлено ограничение права и обременение объекта недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("period_info")]
        public PeriodInfoType Period_info { get; set; }
    }
}
