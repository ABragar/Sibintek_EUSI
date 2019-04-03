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
    [System.Xml.Serialization.XmlRootAttribute("PeriodTypeLand")]
    public class PeriodTypeLand
    {

        [System.Xml.Serialization.XmlElementAttribute("period_ddu", typeof(PeriodDDU))]
        [System.Xml.Serialization.XmlElementAttribute("period_info", typeof(PeriodInfoType))]
        public object Item { get; set; }
    }
}
