using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Владельцы инвестиционных паев
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("InvestmentUnitOwnerOut")]
    public class InvestmentUnitOwnerOut
    {

        /// <summary>
        /// Название (индивидуальное обозначение), идентифицирующее паевой инвестиционный фонд
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("investment_unit_name")]
        public string Investment_unit_name { get; set; }
    }
}
