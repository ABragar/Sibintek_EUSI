
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Общие сведения о сделке (вид сделки)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DealDataType")]
    public class DealDataType 
    {
        public DealDataType() { }
        /// <summary>
        /// Вид сделки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("deal_type")]
        public Dict Deal_type { get; set; }
    }
}
