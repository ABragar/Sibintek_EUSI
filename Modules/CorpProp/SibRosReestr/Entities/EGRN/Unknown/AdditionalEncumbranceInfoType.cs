using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Дополнительная информация в зависимости от вида зарегистрированного ограничения права или обременения объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AdditionalEncumbranceInfoType")]
    public class AdditionalEncumbranceInfoType
    {

        /// <summary>
        /// Сервитут
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("servitude")]
        public ServitudeType Servitude { get; set; }
    }
}
