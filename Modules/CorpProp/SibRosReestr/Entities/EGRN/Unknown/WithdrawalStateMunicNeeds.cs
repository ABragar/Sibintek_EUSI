using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о наличии решения об изъятии
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("WithdrawalStateMunicNeeds")]
    public class WithdrawalStateMunicNeeds
    {

        /// <summary>
        /// Реквизиты решения об изъятии
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("decision_attribute")]
        public DocRequisiteMain Decision_attribute { get; set; }
    }
}
