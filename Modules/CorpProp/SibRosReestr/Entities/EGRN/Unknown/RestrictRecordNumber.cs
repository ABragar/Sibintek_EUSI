using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Номер регистрации/реестровой записи об ограничении/обременении
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RestrictRecordNumber")]
    public class RestrictRecordNumber
    {

        /// <summary>
        /// Номер реестровой записи
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("number")]
        public string Number { get; set; }
        /// <summary>
        /// Номер регистрации ограничения права или обременения ОН
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("right_number")]
        public string Right_number { get; set; }
    }
}
