using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о праве (бесхозяйное имущество)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("OwnerlessRightRecordOut")]
    public class OwnerlessRightRecordOut 
    {
        public OwnerlessRightRecordOut() { }

        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("record_info")]
        public RegistrationDate Record_info { get; set; }
        /// <summary>
        /// Общие сведения о правах на бесхозяйное имущество
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ownerless_right_data")]
        public OwnerlessRight Ownerless_right_data { get; set; }
    }
}
