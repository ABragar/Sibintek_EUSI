using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд (cрок снятия с ГКУ 3 года)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DateRemovedCadAccount")]
    public class DateRemovedCadAccount
    {

        /// <summary>
        /// Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд (реквизиты решения)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("withdrawal_state_munic_needs")]
        public WithdrawalStateMunicNeeds Withdrawal_state_munic_needs { get; set; }
    }
}
